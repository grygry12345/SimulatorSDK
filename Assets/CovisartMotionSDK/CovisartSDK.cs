using SimulatorBackgroundWorkerService.CommonClasses;
using CovisartCommunicationSDK;
using UnityEngine;
using System.Threading;
using System;
using System.Runtime.CompilerServices;

namespace CovisartMotionSDK
{
    public class CovisartSDK : MonoBehaviour
    {
        public GameObject Aircraft;
        private static CommunicationSDK _communication;
        private AxisData _axisData;
        private SimulatorCommandData _commandData;
        public bool IsDataTransferStarted = false;
        private Thread thread;
        private CommandData state;

        void Awake()
        {
            _commandData = new SimulatorCommandData();
        }

        private void ControlTread(Func<byte[]> command, string log)
        {
            if (!(thread?.IsAlive ?? false))
            {
                thread = new Thread(() => SendData(command()));
                thread.Start();
                Debug.Log(log);
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
        }
        private void ControlTread(Action command)
        {
            if (!(thread?.IsAlive ?? false))
            {
                thread = new Thread(() => command());
                thread.Start();
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
        }

        public void OpenConnection()
        {
            ControlTread(_commandData.OpenConnection, "Opened connection");
        }

        public void PowerMotors()
        {
            ControlTread(_commandData.PowerOn, "Motors Powered");
        }

        public void PowerAxisX()
        {
            ControlTread(_commandData.PowerOnX, "X Powered");
        }

        public void PowerAxisY()
        {
            ControlTread(_commandData.PowerOnY, "Y Powered");
        }

        public void CalibrateAxisX()
        {
            ControlTread(_commandData.CalibrateX, "X Calibrated");
        }

        public void CalibrateAxisY()
        {
            ControlTread(_commandData.CalibrateY, "Y Calibrated");
        }

        public void ResetError()
        {
            ControlTread(_commandData.ResetError, "Error Reset");
        }

        public void ResetErrorX()
        {
            ControlTread(_commandData.ResetErrorX, "Error X Reset");
        }

        public void ResetErrorY()
        {
            ControlTread(_commandData.ResetErrorY, "Error Y Reset");
        }

        private void StartExactPositionThread()
        {
            _commandData.EnableExactPositonX();
            _commandData.EnableExactPositonY();
            string json = (SendData(_commandData.GetState()));

            state = new CommandData();
            // it will be chacked
            JsonUtility.FromJsonOverwrite(json, state);
            Debug.Log(this.state);
        }

        public void StartExactPosition()
        {
            ControlTread(StartExactPositionThread);
        }

        public void StartDataListener()
        {
            ControlTread(_commandData.StartArmaThread, "Data listener started.");
        }

        private void StartDataTransferThread()
        {
            _communication = new CommunicationSDK();
            var state = _communication.StartCommunication();
            if (state.hasError)
                Debug.LogError(state.errorMessage);
            else
            {
                IsDataTransferStarted = true;
            }
        }

        public void StartDataTransfer()
        {
            ControlTread(StartDataTransferThread);
        }

        private void StopDataTransferThread()
        {
            if (_communication == null)
                _communication = new CommunicationSDK();
            var state = _communication.StopCommunication();
            if (state.hasError)
                Debug.LogError(state.errorMessage);
        }

        public void StopDataTrensfer()
        {
            ControlTread(StopDataTransferThread);
        }

        private void SendOfData(string axisX, string axisY)
        {
            _axisData = new AxisData { AxisX = axisX, AxisY = axisY };
            var state = _communication.SendData(_axisData);
            if (state.hasError)
                Debug.Log(state.errorMessage);
        }

        void Update()
        {
            if (!IsDataTransferStarted) return;
            var x = Aircraft.transform.eulerAngles.x.ToString();
            var y = Aircraft.transform.eulerAngles.y.ToString();
            SendOfData(x, y);
        }

        /*void FixedUpdate()
        {
            var state = SendData(_commandData.GetState());
            Debug.Log(state);
        }*/

        private static string SendData(byte[] bits)
        {
            return MyTcpClient.Connect("127.0.0.1", bits);
        }
    }
}