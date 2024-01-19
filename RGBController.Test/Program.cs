using System.IO.Ports;

using var port = new SerialPort("COM3"){
    BaudRate = 115200,
    Handshake = Handshake.None,
};

port.DataReceived += (sender, e) =>{
    Console.WriteLine(port.ReadLine());
};

Console.ReadKey();