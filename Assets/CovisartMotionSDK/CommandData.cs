using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CommandData : MonoBehaviour
{
    /// <summary>
    /// Command Datas
    /// </summary>
    public string EngineXErrorState { get; set; }
    public string EngineYErrorState { get; set; }
    public string EngineXPowerState { get; set; }
    public string EngineYPowerState { get; set; }
    public string XplaneThreadState { get; set; }
    public string EngineXEnableExactPositionState { get; set; }
    public string EngineYEnableExactPositionState { get; set; }
    public int CurrentX { get; set; }
    public int CurrentY { get; set; }
    public string ServiceRestartRequired { get; set; }
    public string EngineXCalibrationState { get; set; }
    public string EngineYCalibrationState { get; set; }
    public string ConnectionState { get; set; }
    public string ArmaThreadState { get; set; }
    public int ArmaThreadX { get; set; }
    public int ArmaThreadY { get; set; }
    public int XplaneThreadX { get; set; }
    public int XplaneThreadY { get; set; }
    public string CovisartUdpServerState { get; set; }
    public int CovisartUdpServerX { get; set; }
    public int CovisartUdpServerY { get; set; }
    public int ActPositionX { get; set; }
    public int ActPositionY { get; set; }
    public int ModuloActPosX { get; set; }
    public int ModuloActPosY { get; set; }
    public int ActVelocityX { get; set; }
    public int ActVelocityY { get; set; }
    public int PosDifferenceX { get; set; }
    public int PosDifferenceY { get; set; }
    public int ActAccelerationX { get; set; }
    public int ActAccelerationY { get; set; }
    public string OperationalX { get; set; }
    public string OperationalY { get; set; }
}
