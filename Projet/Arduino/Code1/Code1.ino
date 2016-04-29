#include "Notes.h"
#include "Types.h"

#define START_BYTE 255
#define HEAD_MSG_SIZE 4

Note notes[256];
int idxWrite, idxRead;
byte tmp, numMessage,oldNumMessage;
SendTypeMessage MessageSendType;

//void NotesMsg();
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
  Serial.begin(28800);
//  while (!Serial) {
//    ; // wait for serial port to connect. Needed for native USB
//  }
}

void loop() {
  if (Serial.available() > 0) {
    //ReadMessage(); 
    pinMode(13, OUTPUT);
    test();
    
  }
}

void test() {
  digitalWrite(13, HIGH);
  if(Serial.read() == START_BYTE){
    if(Serial.read() == START_BYTE)
      Serial.write(10);
    else
      Serial.write(3);
  }
  else
    Serial.write(0);
}

void ReadMessage()
{
  if (Serial.read() == START_BYTE) //check si on lit bien depuis le début du message
  {
    oldNumMessage = numMessage;
    numMessage = Serial.read();
    if (oldNumMessage != numMessage) //Check la réemission
    {
      tmp = Serial.read();
      if(tmp < SEND_TYPE_SIZE) //Vérification que le type existe
      {
        switch (tmp){
          case ReceiveType_Tempo : //tempo
            break;
          case ReceiveType_Notes : //Notes
            break;
          case ReceiveType_Start : //Start
            break;
          case ReceiveType_Stop : //Stop
            break;
          case ReceiveType_Pause : //Pause
            break;
          default:
            break;
        }
        MessageSendType = SendType_Ok;
      }else
        MessageSendType = SendType_ErrorType;
    }else //Si on reçoit 2 fois le même message on transmet la même réponse qu'avant
      MessageSendType = MessageSendType;
  }else
    MessageSendType = SendType_ErrorStartByte;

  AnswerMessage(MessageSendType);
}
void AnswerMessage(byte type)
{
  byte msg[HEAD_MSG_SIZE];
  msg[0] = START_BYTE;
  msg[1] = numMessage;
  msg[2] = type;
  msg[3] = idxWrite - idxRead; //TODO : à changer
}

