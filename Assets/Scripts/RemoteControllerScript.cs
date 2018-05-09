﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Threading;
using System;
using System.IO.Ports;
using System.IO;
using System.Linq;

public class RemoteControllerScript : MonoBehaviour {
	public GameObject target;
    public AudioMixer audioMixer;
    public String PORT;
    public RailSystem rail;
	public float distanceToMove = 5;
    public float noiseThreshold = 0.01f;
    public float factor = 30;

	private SerialPort stream;
    private float normalisationFactor = 1.0f / 32768.0f;
    private float angleX = 0;
    private float angleY = 0;
    private float angleZ = 0;
	private bool buttonOn = false;
    private float volume = 0;

    private volatile bool shouldLog = true;
    private Queue<Vector3> rotationQueue = new Queue<Vector3>();
    private Thread datalogger;

    void Start() {
        try {
            stream = new SerialPort(PORT, 9600, Parity.None, 8, StopBits.One);
            stream.ReadTimeout = 1;
            stream.WriteTimeout = 1;
            stream.DtrEnable = false;
            stream.Open();
			Debug.Log("Arduino connected on " + PORT);

            datalogger = new Thread(LogDataAsync);
            datalogger.Start();

        }
		catch {
            // disable the script if Arduino not found
            Debug.Log("Could not connect to Arduino on port" + PORT);
            this.enabled = false;
        }
    }

    void OnApplicationQuit() {
        stream.Close();
        shouldLog = false;
    }

    void Update() {
		if (!stream.IsOpen) return;
		
		try {
			string input = stream.ReadLine();
			string[] rawData = input.Split(';');

            float gx = Int32.Parse(rawData[3]) * normalisationFactor;
            float gy = Int32.Parse(rawData[4]) * normalisationFactor;
            float gz = Int32.Parse(rawData[5]) * normalisationFactor;

			// Prevent minor noise -  if the absolute value of the normalized gyro-data is less than 0.025f then don't add anything
            if (Mathf.Abs(gx) < noiseThreshold) gx = 0f;
            if (Mathf.Abs(gy) < noiseThreshold) gy = 0f;
            if (Mathf.Abs(gz) < noiseThreshold) gz = 0f;

			angleX += gx;
            angleY += gy;
            angleZ += gz;
            
            volume += -gz * 5;

            // get button state
			buttonOn = rawData[6] == "1";

			if (buttonOn) rail.enabled = false;
			else rail.enabled = true;
		}
		catch (System.IO.IOException ioe) {
			Debug.Log("IOException: " + ioe.Message);
		}
		catch {}

        if (Input.GetKeyDown(KeyCode.Escape) && stream.IsOpen) {
            stream.Close();
        }

        if (volume > 15) volume = 15;
        else if (volume < -10) volume = -10;
        audioMixer.SetFloat("Volume", volume);

		stream.BaseStream.Flush();
		stream.DiscardInBuffer();
    }

    void LogDataAsync() {
        string path = "remotecontroller-" + PORT + ".csv";
        using (StreamWriter dataFile = new StreamWriter(path)) {
            // write header
            dataFile.WriteLine("x,y,z");
            dataFile.Flush();

            while (shouldLog) {
                if (rotationQueue.Count > 0) {
                    Vector3 rotation = rotationQueue.Dequeue();
                    string rotationToString = rotation.x + "," + rotation.y + "," + rotation.z;
					dataFile.WriteLine(rotationToString);
                    dataFile.Flush();
                }
            }
        }
    }
}