using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CommandData
{
    /// <summary>
    /// Command Datas
    /// </summary>
    public bool EngineXErrorState;
    public bool EngineYErrorState;
    public bool EngineXPowerState;
    public bool EngineYPowerState;
    public bool XplaneThreadState;
    public bool EngineXEnableExactPositionState;
    public bool EngineYEnableExactPositionState;
    public double CurrentX;
    public double CurrentY;
    public bool ServiceRestartRequired;
    public bool EngineXCalibrationState;
    public bool EngineYCalibrationState;
    public bool ConnectionState;
    public bool ArmaThreadState;
    public double ArmaThreadX;
    public double ArmaThreadY;
    public double XplaneThreadX;
    public double XplaneThreadY;
    public bool CovisartUdpServerState;
    public double CovisartUdpServerX;
    public double CovisartUdpServerY;
    public double ActPositionX;
    public double ActPositionY;
    public double ModuloActPosX;
    public double ModuloActPosY;
    public double ActVelocityX;
    public double ActVelocityY;
    public double PosDifferenceX;
    public double PosDifferenceY;
    public double ActAccelerationX;
    public double ActAccelerationY;
    public bool OperationalX;
    public bool OperationalY;
}
