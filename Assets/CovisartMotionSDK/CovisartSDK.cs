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
using SimulatorBackgroundWorkerService.Enums;
using System.Net.Sockets;
using System.Net;
using System.Text;
// using UnityEngine.UIElements;

namespace CovisartMotionSDK
{
    public class CovisartSDK : MonoBehaviour
    {
        private static CommunicationSDK _communication;
        private AxisData _axisData;
        private SimulatorCommandData _commandData;
        private ProgressState<bool> progressState;
        private bool toggleExactPosition;
        
        public bool IsDataTransferStarted = false;
        public GameObject Aircraft;
        public AircraftData AircraftData;
        private Thread controlThread;
        private CommandData state;
        private Text buttonText;
        private Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private static IPAddress serverAddr = IPAddress.Parse("127.0.0.1");
        private IPEndPoint endPoint = new IPEndPoint(serverAddr, 555);

        void Awake()
        {
            toggleExactPosition = false;
            _commandData = new SimulatorCommandData();
            state = new CommandData();
            OnStateUpdate();
            progressState = new ProgressState<bool>();
            
        }

        private void SetButtonText(int buttonNumber, string text)
        {
            // Get child button gameobject then get grandchild gameobject text.
            var textObj = this.gameObject.transform.GetChild(buttonNumber).GetChild(0).gameObject;
            buttonText = textObj.GetComponent<Text>();

            buttonText.text = text;
        }

        private void SetButtonColor(int buttonNumber, Color buttonColor)
        {
            var buttonObj = gameObject.transform.GetChild(buttonNumber).gameObject;
            var buttonColors = buttonObj.GetComponent<Button>().colors;
            buttonColors.normalColor = buttonColor;
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

        private void ControlTread(Func<EngineType ,byte[]> command,EngineType v, string log)
        {
            if (!(controlThread?.IsAlive ?? false))
            {
                controlThread = new Thread(() => SendData(command(v)));
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
            if (!state.ConnectionState)
            {
                ControlTread(_commandData.OpenConnection, "Opened connection");
                OnStateUpdate();
                // change button text close connection
                SetButtonText(0, "Disconnect");
                // make power X and Y button active ...
            }
            else
            {
                ControlTread(_commandData.CloseConnection, "Closed conncetion");
                OnStateUpdate();
                // change button text open connection
                SetButtonText(0, "Connect");
                SetButtonText(1, "Power");
                // make power X and Y button pasive ...
            }
        }

        public void PowerMotors()
        {
            if (state.EngineXPowerState && state.EngineYPowerState && state.ConnectionState && !progressState.hasError)
            {
                ControlTread(_commandData.PowerOff, "Motors powered off");
                OnStateUpdate();
                // make calibrate X and Y buttons pasive

                // change button text power on and change color
                SetButtonText(1, "Power");
                // toggle powerMotors on and change button text
            }
            else if (state.ConnectionState && !progressState.hasError)
            {
                ControlTread(_commandData.PowerOn, "Motors Powered");
                OnStateUpdate();
                // make calibrate X and Y buttons active
                
                // change button text power off and change color
                SetButtonText(1, "Power Off");
                // toggle powerMotors off and button text
            }
            else
            {
                Debug.LogError("Can not power motors");
            }
        }

        public void PowerAxisX()
        {
            if (state.EngineXPowerState && state.ConnectionState && !progressState.hasError)
            {
                ControlTread(_commandData.PowerOffX, "Motors X powered off");
                OnStateUpdate();
                // make calibrate X button pasive
                // change button text "power X on"

            }
            else if (state.ConnectionState && !progressState.hasError)
            {
                ControlTread(_commandData.PowerOnX, "X Powered");
                OnStateUpdate();
                // make calibrate X button active
                // change button text "power X off"

            }
            else
            {
                Debug.LogError("Can not power X motors");
            }
        }

        public void PowerAxisY()
        {
            if (state.EngineYPowerState && state.ConnectionState && !progressState.hasError)
            {
                ControlTread(_commandData.PowerOffY, "Motors Y powered off");
                OnStateUpdate();
                // make calibrate Y button pasive
                // change button text "power Y on"
                // toggle powerMotors text on
            }
            else if (state.ConnectionState && !progressState.hasError)
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
            if (state.EngineXPowerState && !state.EngineXEnableExactPositionState && !progressState.hasError && !state.EngineXCalibrationState && state.ConnectionState)
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
            if (state.EngineYPowerState && !state.EngineYEnableExactPositionState && !progressState.hasError && !state.EngineYCalibrationState && state.ConnectionState)
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
            if (state.ConnectionState && progressState.hasError)
            {
                ControlTread(_commandData.ResetError, "Error Reset");
                OnStateUpdate();
            }
            else
            {
                Debug.LogError("Progress state has no error");
            }
        }

        public void ResetErrorX()
        {
            if (state.ConnectionState && progressState.hasError)
            {
                ControlTread(_commandData.ResetErrorX, "Error X Reset");
                OnStateUpdate();
            }
            else
            {
                Debug.LogError("Progress state has no error");
            }
        }

        public void ResetErrorY()
        {
            if (state.ConnectionState && progressState.hasError)
            {
                ControlTread(_commandData.ResetErrorY, "Error Y Reset");
                OnStateUpdate();
            }
            else
            {
                Debug.LogError("Progress state has no error");
            }
        }

        private void StartExactPositionThread()
        {
            // Enable exactposition
            if (state.EngineXPowerState && state.EngineYPowerState && 
                !state.EngineXEnableExactPositionState && !state.EngineYEnableExactPositionState && 
                !progressState.hasError && state.EngineXCalibrationState && state.EngineYCalibrationState && state.ConnectionState)
            {
                SendData(_commandData.EnableExactPositonX());
                SendData(_commandData.EnableExactPositonY());
                OnStateUpdate();
                
                Debug.Log("Exact Posiiton Enabled");
                PrintStateInfo();
            }
            // disable exact position
            else if (state.EngineXPowerState && state.EngineYPowerState &&
                state.EngineXEnableExactPositionState && state.EngineYEnableExactPositionState &&
                !progressState.hasError && state.ConnectionState)
            {
                SendData(_commandData.DisableExactPositionX());
                SendData(_commandData.DisableExactPositionY());
                OnStateUpdate();

                Debug.Log("Exact Posiiton Disabled");
                PrintStateInfo();
            }
            else
            {
                Debug.LogError("Invalid");
            }
        }

        public void StartCovisartUDP()
        {
            ControlTread(_commandData.StartCovisartUdpServer, "Covisart UDP started");
            OnStateUpdate();
        }

        public void StopCovisartUDP()
        {
            ControlTread(_commandData.StopCovisartUdpServer, "Covisart UDP stopped");
            OnStateUpdate();
        }

        public void StartExactPosition()
        {
            
            ControlTread(StartExactPositionThread);

            // Toggle button text
            if (toggleExactPosition == false)
            {
                SetButtonText(6, "DisableExactPosiiton");
                toggleExactPosition = true;
            } 
            else
            {
                SetButtonText(6, "StartExactPosition");
                toggleExactPosition = false;
            }
        }

        public void StartDataListener()
        {
            if (state.EngineXCalibrationState && state.EngineYCalibrationState && !state.EngineXEnableExactPositionState && !state.EngineYEnableExactPositionState && !state.EngineXErrorState && !state.EngineYErrorState)
            {
                ControlTread(_commandData.StartArmaThread, "Data listener started.");
                OnStateUpdate();
            }
        }

        private void StartDataTransferThread()
        {
            _communication = new CommunicationSDK();
            progressState = _communication.StartCommunication();
            if (progressState.hasError)
                Debug.LogError(progressState.errorMessage);
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
            progressState = _communication.StopCommunication();
            if (progressState.hasError)
                Debug.LogError(progressState.errorMessage);
            else
            {
                IsDataTransferStarted = false;
            }
        }

        public void StopDataTrensfer()
        {
            ControlTread(StopDataTransferThread);
        }

        public void ManuelControlUp()
        {
            ControlTread(_commandData.ManuelControlUp, EngineType.X, "Motors X up");
            ControlTread(_commandData.ManuelControlUp, EngineType.Y, "Motors Y up");
            OnStateUpdate();
        }

        public void ManuelControlDown()
        {
            ControlTread(_commandData.ManuelControlDown, EngineType.X, "Motors X Down");
            ControlTread(_commandData.ManuelControlDown, EngineType.Y, "Motors Y Down");
            OnStateUpdate();
        }

        private void SendOfData(string axisX, string axisY)
        {
            _axisData = new AxisData { AxisX = axisX, AxisY = axisY };
            progressState = _communication.SendData(_axisData);
            if (progressState.hasError)
                Debug.Log(progressState.errorMessage);
        }

        void Update()
        {
            //if (!IsDataTransferStarted) return;
            //var x = Aircraft.transform.eulerAngles.x.ToString();
            //var y = Aircraft.transform.eulerAngles.y.ToString();
            //SendOfData(x, y);

            // Send aircraft data every frame
            if (state.CovisartUdpServerState)
            {
                SendUDPData(AircraftData);
            }
        }

        // Updates Json
        public void OnStateUpdate()
        {
            string json = (SendData(_commandData.GetState()));
            state = JsonUtility.FromJson<CommandData>(json);
        }

        public void PrintStateInfo()
        {
            Type t = state.GetType();
            FieldInfo[] fields = t.GetFields();

            foreach (var field in fields)
            {
                Debug.Log(field.Name + " " + field.FieldType.Name + " " + field.GetValue(state));
            }
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
        
        private void SendUDPData(AircraftData aircraftData)
        {
            string text = aircraftData.axisX.ToString() + "*";
            text += aircraftData.axisY.ToString() + "*";
            text += aircraftData.axisZ.ToString() + "*";
            text += aircraftData.eulerAngle.x.ToString() + "*";
            text += aircraftData.eulerAngle.y.ToString() + "*";
            text += aircraftData.eulerAngle.z.ToString() + "*";

            byte[] send_buffer = Encoding.ASCII.GetBytes(text);

            sock.SendTo(send_buffer, endPoint);
        }
    }
}