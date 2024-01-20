using System.IO.Ports;

namespace RGBController.Core;

public sealed class IRController{
    private readonly SerialPort _port;

    public IRController(SerialPort port){
        _port = port;
        _port.Open();
    }

    public void SendCommand(byte command){
        _port.Write([command], 0, 1);
    }
}