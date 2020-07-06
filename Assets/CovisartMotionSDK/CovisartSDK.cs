using SimulatorBackgroundWorkerService.CommonClasses;
using CovisartCommunicationSDK;
using UnityEngine;
using System.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace CovisartMotionSDK
{
    public class CovisartSDK : MonoBehaviour
    {
        public GameObject Aircraft;
        private static CommunicationSDK _communication;
        private AxisData _axisData;
        private SimulatorCommandData _commandData;
        public bool IsDataTransferStarted = false;
        private Thread controlThread;
        private CommandData state;

        private GameObject textObj;
        private Text buttonText;

        // Temp varible
        private bool tempOpenConnection = false;

        void Awake()
        {
            _commandData = new SimulatorCommandData();
            state = new CommandData();
        }

        private void setButtonText(int buttonNumber, string text)
        {
            // Get child button gameobject then get grandchild gameobject text.
            textObj = this.gameObject.transform.GetChild(buttonNumber).GetChild(0).gameObject;
            buttonText = textObj.GetComponent<Text>();

            buttonText.text = text;
        }

        private void ControlTread(Func<byte[]> command, string log)
        {
            if (!(controlThread?.IsAlive ?? false))
            {
                controlThread = new Thread(() => SendData(command()));
                controlThread.Start();
                Debug.Log(log);
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
        }
        private void ControlTread(Action command)
        {
            if (!(controlThread?.IsAlive ?? false))
            {
                controlThread = new Thread(() => command());
                controlThread.Start();
            }
            else
            {
                Debug.LogError("Thread is busy");
            }
        }

        public void OpenConnection()
        {
            // toggle connection off and on button
            if (!tempOpenConnection)
            {
                ControlTread(_commandData.OpenConnection, "Opened connection");
                tempOpenConnection = true;
                // OnStateUpdate();
                // change button text close connection
                setButtonText(0, "Close Connection");
                // make power X and Y button active ...
            }
            else
            {
                ControlTread(_commandData.CloseConnection, "Closed conncetion");
                tempOpenConnection = false;
                // OnStateUpdate();
                // change button text open connection
                setButtonText(0, "Connect");
                // make power X and Y button pasive ...
            }
        }

        public void PowerMotors()
        {
            if (state.EngineXPowerState && state.EngineYPowerState && tempOpenConnection)
            {
                ControlTread(_commandData.PowerOff, "Motors powered off");
                OnStateUpdate();
                // make calibrate X and Y buttons pasive
                // change button text power on
                setButtonText(1, "Power");
                // toggle powerMotors on and change button text
            }
            else if (tempOpenConnection)
            {
                ControlTread(_commandData.PowerOn, "Motors Powered");
                OnStateUpdate();
                // make calibrate X and Y buttons active
                // change button text power off
                setButtonText(1, "Power Off");
                // toggle powerMotors off and button text
            }
            else
            {
                Debug.LogError("Can not power motors");
            }
        }

        public void PowerAxisX()
        {
            if (state.EngineXPowerState && tempOpenConnection)
            {
                ControlTread(_commandData.PowerOffX, "Motors X powered off");
                OnStateUpdate();
                // make calibrate X button pasive
                // change button text "power X on"
                // toggle powerMotors X on
            }
            else if (tempOpenConnection)
            {
                ControlTread(_commandData.PowerOnX, "X Powered");
                OnStateUpdate();
                // make calibrate X button active
                // change button text "power X off"
                // toggle powerMotors X off
            }
            else
            {
                Debug.LogError("Can not power X motors");
            }
        }

        public void PowerAxisY()
        {
            if (state.EngineYPowerState && tempOpenConnection)
            {
                ControlTread(_commandData.PowerOffY, "Motors Y powered off");
                OnStateUpdate();
                // make calibrate Y button pasive
                // change button text "power Y on"
                // toggle powerMotors text on
            }
            else if (tempOpenConnection)
            {
                ControlTread(_commandData.PowerOnY, "Y Powered");
                OnStateUpdate();
                // make calibrate Y button 
                // change button text "power Y off"
                // toggle powerMotors text Y off
            }
            else
            {
                Debug.LogError("Can not power Y motors");
            }
        }

        public void CalibrateAxisX()
        {
            if (tempOpenConnection && state.EngineXPowerState)
            {
                ControlTread(_commandData.CalibrateX, "X Calibrated");
                OnStateUpdate();
                // make ... active
                // toggle Calibrate X on and off
            }
            else
            {
                Debug.LogError("Can not cannot calibrate Axis X");
            }
        }

        public void CalibrateAxisY()
        {
            if (tempOpenConnection && state.EngineYPowerState)
            {
                ControlTread(_commandData.CalibrateY, "Y Calibrated");
                OnStateUpdate();
                // make ... active
                // toggle Calibrate Y on and off
            }
            else
            {
                Debug.LogError("Can not cannot calibrate Axis Y");
            }
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

            // Printing info
            Type t = state.GetType();
            FieldInfo[] fields = t.GetFields();

            foreach (var field in fields)
            {
                Debug.Log(field.Name + " " + field.FieldType.Name + " " + field.GetValue(state));
            }
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
            else
                state.returnValue.ToString();
        }

        void Update()
        {
            if (!IsDataTransferStarted) return;
            var x = Aircraft.transform.eulerAngles.x.ToString();
            var y = Aircraft.transform.eulerAngles.y.ToString();
            SendOfData(x, y);
        }

        public void OnStateUpdate()
        {
            string json = (SendData(_commandData.GetState()));
            state = JsonUtility.FromJson<CommandData>(json);
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