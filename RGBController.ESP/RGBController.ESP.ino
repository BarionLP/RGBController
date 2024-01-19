#include <IRremote.hpp>
#define IR_RECEIVE_PIN 2
#define IR_SEND_PIN 4
#define DEFAULT_REPEATS 3

//#define DISABLE_CODE_FOR_RECEIVER // Saves 450 bytes program memory and 269 bytes RAM if receiving functions are not used.

void setup() {
  Serial.begin(115200);
  Serial.println("Starting...");

  IrSender.begin(IR_SEND_PIN);
  //delay(100);
  IrSender.sendNEC(0xEF00, 0x3, DEFAULT_REPEATS);
  IrSender.sendNEC(0xEF00, 0x2, DEFAULT_REPEATS);
  
  IrReceiver.begin(IR_RECEIVE_PIN);
}

void loop() {
  if (IrReceiver.decode()) {
      //Serial.println(IrReceiver.decodedIRData.decodedRawData, HEX); // Print "old" raw data

      IrReceiver.printIRResultShort(&Serial, true); // Print complete received data in one line
      IrReceiver.printIRSendUsage(&Serial);   // Print the statement required to send this data
      Serial.println();
      IrReceiver.resume();
  }

  delay(100);
}
