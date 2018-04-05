using UnityEngine;
using System.Collections;
using System;
using System.IO.Ports;
using System.Linq;


public class MPUscript_alt : MonoBehaviour
{

    SerialPort stream;
    public Transform target; // is the gameobject to

    float acc_normalizer_factor = 0.00025f;
    float gyro_normalizer_factor = 1.0f / 32768.0f;   // 32768 is max value captured during test on imu

    float curr_angle_x = 0;
    float curr_angle_y = 0;
    float curr_angle_z = 0;

    float curr_offset_x = 0;
    float curr_offset_y = 0;
    float curr_offset_z = 0;

    // Increase the speed/influence rotation
    public float factor = 7;


    public bool enableRotation;
    public bool enableTranslation;


    void Start()
    {
        //Open the port for the datastream. Adjust "COM4" as needed, leave the rest.
        //Make sure your API compatibility level is set to NET 2.0, and not NET 2.0 Subset 
            // Edit -> Project Settings -> Player -> API Compatibility Level*  down at the right side window.
        stream = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
        stream.ReadTimeout = 1;
        stream.WriteTimeout = 1;
        stream.DtrEnable = false;
        stream.Open();
    }

    void Update()
    {
        string dataString = "null received";

        if (stream.IsOpen)
        {
            try
            {
                dataString = stream.ReadLine();
                Debug.Log("DATA_ : " + dataString);
            }
            catch (System.IO.IOException ioe)
            {
                Debug.Log("IOException: " + ioe.Message);
            }

        }
        else
            dataString = "NOT OPEN";
        Debug.Log("DATA_ : " + dataString);

        if (!dataString.Equals("NOT OPEN"))
        {
            // Received datastring looks like "accx;accy;accz;gyrox;gyroy;gyroz"
            //Below splits the string into a string array everytime the character ; appears
            char splitChar = ';';
            string[] dataRaw = dataString.Split(splitChar);

            // Normalized ACCELEROMETER data (can be enabled with "Enable translation")
            float ax = Int32.Parse(dataRaw[0]) * acc_normalizer_factor;
            float ay = Int32.Parse(dataRaw[1]) * acc_normalizer_factor;
            float az = Int32.Parse(dataRaw[2]) * acc_normalizer_factor;

            // Normalized GYROSCOPE data (currently what we use)
            float gx = Int32.Parse(dataRaw[3]) * gyro_normalizer_factor;
            float gy = Int32.Parse(dataRaw[4]) * gyro_normalizer_factor;
            float gz = Int32.Parse(dataRaw[5]) * gyro_normalizer_factor;

            // Prevent drift? Not sure. Only applicable for accelerometer
            if (Mathf.Abs(ax) - 1 < 0) ax = 0;
            if (Mathf.Abs(ay) - 1 < 0) ay = 0;
            if (Mathf.Abs(az) - 1 < 0) az = 0;


            curr_offset_x += ax;
            curr_offset_y += ay;
            curr_offset_z += 0; // The IMU module have value of z axis of 16600 caused by gravity


            // Prevent minor noise -  if the absolute value of the normalized gyro-data is less than 0.025f then don't add anything
            if (Mathf.Abs(gx) < 0.025f) gx = 0f;
            if (Mathf.Abs(gy) < 0.025f) gy = 0f;
            if (Mathf.Abs(gz) < 0.025f) gz = 0f;

            //Add normalized angles to new angle
            curr_angle_x += gx;
            curr_angle_y += gy;
            curr_angle_z += gz;

            if (enableTranslation) target.transform.position = new Vector3(curr_offset_x, curr_offset_z, curr_offset_y);
            if (enableRotation) target.transform.rotation = Quaternion.Euler(curr_angle_x * factor, -curr_angle_z * factor, curr_angle_y * factor);

            stream.BaseStream.Flush();
            stream.DiscardInBuffer();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && stream.IsOpen)
            stream.Close();
    }

}