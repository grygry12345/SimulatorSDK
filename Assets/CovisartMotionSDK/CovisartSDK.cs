using SimulatorBackgroundWorkerService.CommonClasses;
using CovisartCommunicationSDK;
using UnityEngine;
using System.Threading;

namespace CovisartMotionSDK
{
    public class CovisartSDK : MonoBehaviour
    {
        public GameObject Aircraft;
        private static CommunicationSDK _communication;
        private AxisData _axisData;
        private SimulatorCommandData _commandData;
        public bool IsDataTransferStarted = false;
        void Awake()
        {
            _commandData = new SimulatorCommandData();
        }
        public void OpenConnection()
        {
            Thread thread = new Thread(() => _commandData.OpenConnection());
            
            thread.Start();
            Debug.Log("Connection started");
        }
        public void PowerMotors()
        {
            Thread thread = new Thread(() => _commandData.PowerOn());
            thread.Start();
            
            Debug.Log("Motors Powered");
        }

        public void PowerAxisX()
        {
            Thread thread = new Thread(() => _commandData.PowerOnX());
            thread.Start();
            Debug.Log("Axis X Powered");
        }

        public void PowerAxisY()
        {
            Thread thread = new Thread(() => _commandData.PowerOnY());
            thread.Start();
            Debug.Log("Axis Y Powered");
        }

        public void CalibrateAxisX()
        {
            Thread thread = new Thread(() => _commandData.CalibrateX());
            thread.Start();
            Debug.Log("X Axis calibrated");
        }

        public void CalibrateAxisY()
        {
            Thread thread = new Thread(() => _commandData.CalibrateY());
            thread.Start();
            Debug.Log("Y Axis calibrated");
        }

        public void ResetError()
        {
            Thread thread = new Thread(() => _commandData.ResetError());
            thread.Start();
            Debug.Log("Reset Error.");
        }

        public void ResetErrorX()
        {
            Thread thread = new Thread(() => _commandData.ResetErrorX());
            thread.Start();
            Debug.Log("Reset X error");
        }

        public void ResetErrorY()
        {
            Thread thread = new Thread(() => _commandData.ResetErrorY());
            thread.Start();
            Debug.Log("Reset Y error");
        }

        private void StartExactPositionThread()
        {
            _commandData.EnableExactPositonX();
            _commandData.EnableExactPositonY();
            var state = (_commandData.GetState());
            Debug.Log(state);
        }

        public void StartExactPosition()
        {
            Thread thread = new Thread(() => StartExactPositionThread());
            thread.Start();
        }

        public void StartDataListener()
        {
            Thread thread = new Thread(() => _commandData.StartArmaThread());
            thread.Start();
        }

        public void StartDataTransfer()
        {
            _communication = new CommunicationSDK();
            var state = _communication.StartCommunication();
            if(state.hasError)
                Debug.Log(state.errorMessage);
            else
            {
                IsDataTransferStarted = true;
            }
        }

        private void StopDataTransferThread()
        {
            if(_communication == null)
                _communication = new CommunicationSDK();
            var state = _communication.StopCommunication();
            if(state.hasError)
                Debug.Log(state.errorMessage);
        }

        public void StopDataTrensfer()
        {
            Thread thread = new Thread(() => StopDataTransferThread());
            thread.Start();
        }

        private void SendOfData(string axisX, string axisY)
        {
            _axisData = new AxisData {AxisX = axisX, AxisY = axisY};
            var state = _communication.SendData(_axisData);
            if(state.hasError)
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

        //private static  string SendData(byte[] bits)
        //{
        //    return MyTcpClient.Connect("127.0.0.1", bits);
        //}
    }
}

