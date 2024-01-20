using System.IO.Ports;
using NAudio.Wave;
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

var rainbow = new RandomInfinteIterator<byte>(Rainbow);

var capture = new WasapiLoopbackCapture();
var beatDetector = new BeatDetector();
capture.DataAvailable += OnDataAvailable;
beatDetector.OnBeatDetected += ()=>{
    rainbow.MoveNext();
    controller.SendCommand(rainbow.Current);
    //Console.WriteLine("Beat");
};
capture.StartRecording();

controller.SendCommand(CasaluxColumn.Commands.TOGGLE_ON_OFF);

Console.ReadKey();

controller.SendCommand(CasaluxColumn.Commands.TOGGLE_ON_OFF);
capture.StopRecording();
port.Close();
port.Dispose();

void OnDataAvailable(object? sender, WaveInEventArgs e){
    var buffer = ConvertToFloat(e.Buffer, e.BytesRecorded);
    beatDetector.ProcessData(buffer);

    static float[] ConvertToFloat(byte[] buffer, int bytesRecorded){
        var samplesRecorded = bytesRecorded / 4; // 32-bit float samples
        var floatBuffer = new float[samplesRecorded];
        for (int index = 0, floatIndex = 0; index < bytesRecorded; index += 4, floatIndex++)
        {
            floatBuffer[floatIndex] = BitConverter.ToSingle(buffer, index);
        }
        return floatBuffer;
    }
}