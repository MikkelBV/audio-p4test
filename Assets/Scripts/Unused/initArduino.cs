using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class initArduino : MonoBehaviour
{

    public float speed = 20;
    private float amountToMove;

    static SerialPort sp = new SerialPort("COM4", 9600);


    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 1;
    }


    void Update()
    {
        amountToMove = speed * Time.deltaTime;

        if (sp.IsOpen)
        {
            try
            {
                moveObject(sp.ReadByte());
                print(sp.ReadByte());
            }
            catch (System.Exception)
            {

            }
        }

    }

    void moveObject(int direction)
    {
        if (direction == 1)
        {
            transform.Translate(Vector3.left * amountToMove, Space.World);
        }
        if (direction == 2)
        {
            transform.Translate(Vector3.right * amountToMove, Space.World);
        }
    }
}
