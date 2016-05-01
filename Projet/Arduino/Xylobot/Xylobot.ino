#include "Notes.h"
#include "Types.h"
#include "CircularBuffer.h"

#define START_BYTE 255
#define HEAD_MSG_SIZE 4

Note notes[256];
int idxWrite, idxRead;
byte numMessage,oldNumMessage;
SendTypeMessage messageSendType;
CircularBuffer bufferNotes;

void NotesMsg(uint16_t dataSize);
void StartMsg(uint16_t dataSize);
void StopMsg(uint16_t dataSize);
void PauseMsg(uint16_t dataSize);
void TempoMsg(uint16_t dataSize);

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
  byte tmp;
  uint16_t dataSize;
  if (Serial.read() == START_BYTE) //check si on lit bien depuis le début du message
  {
    oldNumMessage = numMessage;
    numMessage = Serial.read();
    if (oldNumMessage != numMessage) //Check la réemission
    {
      tmp = Serial.read();
      dataSize = Serial.read();
      dataSize |= (Serial.read() << 8);
      if(tmp < SEND_TYPE_SIZE) //Vérification que le type existe
      {
        switch (tmp){
          case ReceiveType_Tempo : //tempo
            TempoMsg(dataSize);
            return;
            break;
          case ReceiveType_Notes : //Notes
            NotesMsg(dataSize);
            return;
            break;
          case ReceiveType_Start : //Start
            StartMsg(dataSize);
            return;
            break;
          case ReceiveType_Stop : //Stop
            StopMsg(dataSize);
            return;
            break;
          case ReceiveType_Pause : //Pause
            PauseMsg(dataSize);
            break;
          default:
            break;
        }
      }else
        messageSendType = SendType_ErrorType;
    }else //Si on reçoit 2 fois le même message on transmet la même réponse qu'avant
      messageSendType = messageSendType;
  }else
    messageSendType = SendType_ErrorStartByte;

  AnswerMessage(messageSendType);
}
void AnswerMessage(byte type)
{
  byte msg[HEAD_MSG_SIZE];
  msg[0] = START_BYTE;
  msg[1] = numMessage;
  msg[2] = type;
  msg[3] = bufferNotes.SizeAvailble();
  Serial.write(msg, 4);
}

void NotesMsg(uint16_t dataSize)
{
  AnswerMessage(SendType_Ok);
}
void StartMsg(uint16_t dataSize)
{
  AnswerMessage(SendType_Ok);
}
void StopMsg(uint16_t dataSize)
{
  AnswerMessage(SendType_Ok);
}
void PauseMsg(uint16_t dataSize)
{
  AnswerMessage(SendType_Ok);
}
void TempoMsg(uint16_t dataSize)
{
  AnswerMessage(SendType_Ok);
}

