using SimulatorBackgroundWorkerService.CommonClasses;
using CovisartCommunicationSDK;
using UnityEngine;

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
            SendData(_commandData.OpenConnection());
            Debug.Log("Connection started");
        }
        public void PowerMotors()
        {
            SendData(_commandData.PowerOn());
            Debug.Log("Motors Powered");
        }

        public void StartExactPosition()
        {
            SendData(_commandData.EnableExactPositonX());
            SendData(_commandData.EnableExactPositonY());
            var state = SendData(_commandData.GetState());
            Debug.Log(state);
        }

        public void StartDataListener()
        {
            SendData(_commandData.StartArmaThread());
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

        public void StopDataTransfer()
        {
            if(_communication == null)
                _communication = new CommunicationSDK();
            var state = _communication.StopCommunication();
            if(state.hasError)
                Debug.Log(state.errorMessage);
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

        private static  string SendData(byte[] bits)
        {
            return MyTcpClient.Connect("127.0.0.1", bits);
        }
    }
}

