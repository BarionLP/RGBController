using System.IO.Ports;
using Ametrin.Utils;
using RGBController.Core;

var port = new SerialPort("COM3"){
    BaudRate = 115200,
    Handshake = Handshake.None,
    DataBits = 8,
    Parity = Parity.None,
    StopBits = StopBits.One,
};

port.DataReceived += (sender, e) =>{
    Console.WriteLine(port.ReadLine());
};

var controller = new IRController(port);

List<byte> Rainbow = [
    CasaluxColumn.Colors.RED,
    CasaluxColumn.Colors.RED_ORANGE,
    CasaluxColumn.Colors.ORANGE,
    CasaluxColumn.Colors.ORANGE_YELLOW,
    CasaluxColumn.Colors.YELLOW,
    CasaluxColumn.Colors.GREEN,
    CasaluxColumn.Colors.GREEN_CYAN,
    CasaluxColumn.Colors.CYAN,
    CasaluxColumn.Colors.CYAN_BLUE,
    CasaluxColumn.Colors.LIGHT_BLUE,
    CasaluxColumn.Colors.BLUE,
    CasaluxColumn.Colors.BLUE_PURPLE, 
    CasaluxColumn.Colors.PURPLE, 
    CasaluxColumn.Colors.PURPLE_MAGENTA, 
    CasaluxColumn.Colors.MAGENTA,
];

controller.SendCommand(CasaluxColumn.Commands.TOGGLE_ON_OFF);

while(true){
    if(Console.KeyAvailable) break;
    controller.SendCommand(Rainbow.GetRandomElement());
    await Task.Delay(561);
}

controller.SendCommand(CasaluxColumn.Commands.TOGGLE_ON_OFF);

port.Close();
port.Dispose();