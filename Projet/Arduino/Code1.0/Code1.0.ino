#include "Notes.h"
#include "Types.h"

#define startByte 255

//Note notes[256];
int idxWrite, idxRead;
byte tmp, numMessage,oldNumMessage;
byte dataSize[2];

void NotesReceive();
//void StartMsg();
//void StopMsg();
//void PauseMsg();
//void TempoMsg();

void ReadMessage();
void AnswerMessage(byte type);

void setup() {
  // open the serial port:
//  idxWrite = 0;
//  idxRead = 0;
  //Serial.setTimeout(500);
  Serial.begin(9600);
//  while (!Serial) {
//    ; // wait for serial port to connect. Needed for native USB
//  }
}

void loop() {
  if (Serial.available() > 0) {
    //ReadMessage(); 
  }
}

void test() {
  if (Serial.read() == startByte)
  {
    tmp = Serial.read();
    Serial.write(startByte);
    Serial.write(tmp);
  }
  else
  {
    Serial.write(startByte);
    Serial.write(0);
  }
}

void ReadMessage()
{
  if (Serial.read() == startByte) //check si on lit bien depuis le début du message
  {
    oldNumMessage = numMessage;
    numMessage = Serial.read();
    if (oldNumMessage != numMessage){ //Check la réemission
      switch (Serial.read()){
//        case ((byte)ReceiveTypes.Tempo) : //tempo
//          break;
//        case ((byte)ReceiveTypes.Notes) : //Notes
//          break;
//        case ((byte)ReceiveTypes.Start) : //Start
//          break;
//        case ((byte)ReceiveTypes.Stop) : //Stop
//          break;
//        case ((byte)ReceiveTypes.Pause) : //Pause
//          break;
//        default:
//          break;
      }
    }
  }else
    AnswerMessage(3); //Type erreur byte start
}
void AnswerMessage(byte type)
{
  
}

