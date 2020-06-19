using System;
using System.Collections;
using System.Collections.Generic;
using SimulatorBackgroundWorkerService.CommonClasses;
using UnityEngine;

namespace CovisartMotionSDK
{
    public class AircraftData : MonoBehaviour
    {
        public GameObject jet;
        public double axisX;
        public double axisY;
        public double axisZ;
        public double axisW;
        public Vector3 eulerAngle;
        public Vector3 position;


        private SimulatorCommandData commandaData;
        void Awake()
        {
            commandaData = new SimulatorCommandData();
        }
        void Update()
        {
            axisX = jet.transform.eulerAngles.x ;
            axisY = jet.transform.eulerAngles.y ;
            axisZ = jet.transform.eulerAngles.z ;
            axisW = jet.transform.rotation.w ;
            eulerAngle = jet.transform.eulerAngles;
            position = jet.transform.position;
        }
        public void StartDataTransfer()
        {
            SendData(commandaData.OpenConnection());
            SendData(commandaData.PowerOn());
            SendData(commandaData.EnableExactPositonX());
            SendData(commandaData.EnableExactPositonY());
            var state = SendData(commandaData.GetState());
            Debug.Log(state);
        }
        private static  string SendData(byte[] bits)
        {
            return MyTcpClient.Connect("127.0.0.1", bits);
        }
    }
}

