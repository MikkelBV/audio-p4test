using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.IO.Ports;
using System.IO;
using System.Linq;


public class Gyrosensor : MonoBehaviour {
    SerialPort stream;
    public GameObject target;
    float acc_normalizer_factor = 0.00025f;
    //float gyro_normalizer_factor = 1.0f / 32768.0f;   // 32768 is max value captured during test on imu
    float gyro_normalizer_factor = 1.0f / 32768.0f;
    public float noise_threshold = 0.01f;
    public float DistanceToMoveOnButtonpress;

    float curr_angle_x = 0;
    float curr_angle_y = 0;
    float curr_angle_z = 0;

    float curr_offset_x = 0;
    float curr_offset_y = 0;
    float curr_offset_z = 0;

    public float factor = 13;
    public bool enableRotation;
    public bool EnableMouseControlsForController = false;
    public String port;
    private Queue<Vector3> rotationQueue = new Queue<Vector3>();
    private Thread datalogger;

    private volatile bool shouldLog = true;

    void Start() {
        try {
            /*
             * Open the port for the datastream. Adjust "COM4" as needed in the inspector if needed.
             * Make sure your API compatibility level is set to NET 2.0, and not NET 2.0 Subset 
             * Edit -> Project Settings -> Player -> API Compatibility Level*  down at the right side window.
             * 
             * new SerialPort(port, baudrate, Parity, databits, stopbits)
             * port = COM port; baudrate = amount of bits transferred per second; Parity = Error checking (default none); Databits = expected int32 bits to receive; Stopbits = amount of stopbits
             */
            stream = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
            stream.ReadTimeout = 1;
            stream.WriteTimeout = 1;
            stream.DtrEnable = false;
            stream.Open();

            datalogger = new Thread(LogDataAsync);
            datalogger.Start();
        } catch {
            // disable the script if Arduino not found
            Debug.Log("Could not connect to Arduino on port " + port);
            this.enabled = false;

            if (EnableMouseControlsForController) EnableMouseControls();
        }
    }

    void OnApplicationQuit() {
        stream.Close();
        shouldLog = false;
    }

    void Update() {
        string dataString = "0;0;0;0;0;0;-1";

        if (stream.IsOpen) {
            try {
                dataString = stream.ReadLine();
            } catch (System.IO.IOException ioe) {
                Debug.Log("IOException: " + ioe.Message);
                dataString = null;
            } catch {
                dataString = null;
            }
        } else {
            dataString = null;
        }

        if (dataString != null) {
            // Received datastring looks like "accx;accy;accz;gyrox;gyroy;gyroz"
            // Below splits the string into a string array everytime the character ; appears
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
            curr_offset_z += az; // The IMU module have value of z axis of 16600 caused by gravity


            // Prevent minor noise -  if the absolute value of the normalized gyro-data is less than 0.025f then don't add anything
            if (Mathf.Abs(gx) < noise_threshold) gx = 0f;
            if (Mathf.Abs(gy) < noise_threshold) gy = 0f;
            if (Mathf.Abs(gz) < noise_threshold) gz = 0f;

            //Add normalized angles to new angle
            curr_angle_x += gx;
            curr_angle_y += gy;
            curr_angle_z += gz;

            if (enableRotation) {
                Vector3 newRotation;

                if (port == "COM3") {
                    newRotation = new Vector3(curr_angle_x * factor, -curr_angle_z * factor, 0);
                } else {
                    newRotation = new Vector3(0, -curr_angle_z * factor, 0);
                    bool buttonPressed = dataRaw[6] == "1";
                    bool notPressed = dataRaw[6] == "0";

                    if (buttonPressed) {
                        transform.localPosition = new Vector3(0, 0, DistanceToMoveOnButtonpress);
                    } else if (notPressed) {
                        transform.localPosition = Vector3.zero;
                    }
                }

                target.transform.localRotation = Quaternion.Euler(newRotation);
                rotationQueue.Enqueue(newRotation);
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                target.transform.rotation = Quaternion.Euler(Vector3.zero);
            }

            stream.BaseStream.Flush();
            stream.DiscardInBuffer();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && stream.IsOpen) {
            stream.Close();
        }
    }

    void EnableMouseControls() {
        GetComponent<MouseController>().enabled = true;
    }

    void LogDataAsync() {
        string path = string.Format("log-{0}.csv", port);
        using (StreamWriter dataFile = new StreamWriter(path)) {
            // write header
            dataFile.WriteLine("x,y,z");
            dataFile.Flush();

            while (shouldLog) {
                if (rotationQueue.Count > 0) {
                    Vector3 rotation = rotationQueue.Dequeue();
                    string rotationToString = string.Format("{0},{1},{2}", rotation.x, rotation.y, rotation.z);
                    dataFile.WriteLine(rotationToString);
                    dataFile.Flush();
                }
            }
        }
    }
}   