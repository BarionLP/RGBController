using System.IO.Ports;

using var port = new SerialPort("COM3"){
    BaudRate = 115200,
    Handshake = Handshake.None,
    DataBits = 8,
    Parity = Parity.None,
    StopBits = StopBits.One,
};

IEnumerable<IRCommand> Rainbow = [
    IRCommand.Red,
    IRCommand.RedOrange,
    IRCommand.Orange,
    IRCommand.OrangeYellow,
    IRCommand.Yellow,
    IRCommand.Green,
    IRCommand.GreenCyan,
    IRCommand.Cyan,
    IRCommand.CyanBlue,
    IRCommand.LightBlue,

    //blueish colors interfere with ir
    //IRCommand.Blue, 
    //IRCommand.BluePurple, 
    //IRCommand.Purple, 
    //IRCommand.PurpleMagenta, 
    //IRCommand.Magenta,
];


port.DataReceived += (sender, e) =>{
    Console.WriteLine(port.ReadLine());
};

port.Open();

port.Write([(byte)IRCommand.ToggleOnOff], 0, 1);

while(true){
    foreach(var command in Rainbow){
        port.Write([(byte)command], 0, 1);
        await Task.Delay(1000);
        if(Console.KeyAvailable) break;
    }
    if(Console.KeyAvailable) break;
}

port.Close();

enum IRCommand{
    Brighter            = 0x0,
    Dimmer              = 0x1,
    Switch              = 0x2,
    ToggleOnOff         = 0x3,
    CoolerWhite         = 0x7,
    WarmerWhite         = 0xB,
    IncreaseColorTime   = 0xF,
    DecreaseColorTime   = 0x13,
    Mode                = 0x17,

    Red         = 0x4,
    RedOrange   = 0x8,
    Orange      = 0xC,
    OrangeYellow= 0x10,
    Yellow      = 0x14,
    Green       = 0x5,
    GreenCyan   = 0x9,
    Cyan        = 0xD,
    CyanBlue    = 0x11,
    LightBlue   = 0x15,
    Blue        = 0x6,
    BluePurple  = 0xA,
    Purple      = 0xE,
    PurpleMagenta= 0x12,
    Magenta     = 0x16,
}