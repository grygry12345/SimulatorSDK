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
        private Thread thread = new Thread(() => IdleThread());

        void Awake()
        {
            _commandData = new SimulatorCommandData();
        }

        private static void IdleThread()
        {

        }

        public void OpenConnection()
        {
            if (!thread.IsAlive) 
            {
                thread = new Thread(() => _commandData.OpenConnection());
                thread.Start();
                Debug.Log("Connection started");
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
        }
        public void PowerMotors()
        {
            if (!thread.IsAlive)
            {
                thread = new Thread(() => _commandData.PowerOn());
                thread.Start();
                Debug.Log("Motors powered");
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
        }

        public void PowerAxisX()
        {
            if (!thread.IsAlive)
            {
                thread = new Thread(() => _commandData.PowerOnX());
                thread.Start();
                Debug.Log("Axis X Powered");
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
        }

        public void PowerAxisY()
        {

            if (!thread.IsAlive)
            {
                thread = new Thread(() => _commandData.PowerOnY());
                thread.Start();
                Debug.Log("Axis Y Powered");
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
        }

        public void CalibrateAxisX()
        {
            if (!thread.IsAlive)
            {
                thread = new Thread(() => _commandData.CalibrateX());
                thread.Start();
                Debug.Log("Axis X calibrated");
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
        }

        public void CalibrateAxisY()
        {
            if (!thread.IsAlive)
            {
                thread = new Thread(() => _commandData.CalibrateY());
                thread.Start();
                Debug.Log("Axis Y calibrated");
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
        }

        public void ResetError()
        {

            if (!thread.IsAlive)
            {
                thread = new Thread(() => _commandData.ResetError());
                thread.Start();
                Debug.Log("Reset Error.");
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
        }

        public void ResetErrorX()
        {
            if (!thread.IsAlive)
            {
                thread = new Thread(() => _commandData.ResetErrorX());
                thread.Start();
                Debug.Log("Reset Error X.");
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
        }

        public void ResetErrorY()
        {
            if (!thread.IsAlive)
            {
                thread = new Thread(() => _commandData.ResetErrorY());
                thread.Start();
                Debug.Log("Reset Error Y.");
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
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
            if (!thread.IsAlive)
            {
                thread = new Thread(() => StartExactPositionThread());
                thread.Start();
            }
            else
            {
                Debug.LogError("Thread is busy");
            };
        }

        public void StartDataListener()
        {
            if (!thread.IsAlive)
            {
                thread = new Thread(() => _commandData.StartArmaThread());
                thread.Start();
                Debug.Log("Data litener started.");
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
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
            if (!thread.IsAlive)
            {
                thread = new Thread(() => StartDataTransferThread());
                thread.Start();
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
        }

        private void StopDataTransferThread()
        {
            if(_communication == null)
                _communication = new CommunicationSDK();
            var state = _communication.StopCommunication();
            if(state.hasError)
                Debug.LogError(state.errorMessage);
        }

        public void StopDataTrensfer()
        {
            if (!thread.IsAlive)
            {
                thread = new Thread(() => StopDataTransferThread());
                thread.Start();
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
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

