#include <IRremote.hpp>
#define IR_RECEIVE_PIN 2
#define IR_SEND_PIN 4
#define DEFAULT_REPEATS 2
#define ADDRESS 0xEF00

//#define DISABLE_CODE_FOR_RECEIVER // Saves 450 bytes program memory and 269 bytes RAM if receiving functions are not used.

void setup() {
  Serial.begin(115200);
  Serial.println("Starting...");

  IrSender.begin(IR_SEND_PIN);  
  IrReceiver.begin(IR_RECEIVE_PIN);
}

void loop() {
  receive();
  if(Serial.available() <= 0) return;

  uint8_t command = Serial.read();
  sendCommand(command);
  //Serial.println(command);
}

void sendCommand(uint8_t command){
  IrSender.sendNEC(ADDRESS, command, DEFAULT_REPEATS);
}

void receive(){
  if (IrReceiver.decode()) {
    IrReceiver.printIRResultShort(&Serial, true); 
    IrReceiver.printIRSendUsage(&Serial);
    Serial.println();
    IrReceiver.resume();
  }
}