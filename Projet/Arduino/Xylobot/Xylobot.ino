#include "Notes.h"
#include "Types.h"
#include "CircularBuffer.h"

#define START_BYTE 255
#define HEAD_SEND_MSG_SIZE 4
#define HEAD_RECEIVE_MSG_SIZE 5

int idxWrite, idxRead;
int numMessage,oldNumMessage;
SendTypeMessage msgSendType;
CircularBuffer bufferNotes;

byte headMessage[HEAD_RECEIVE_MSG_SIZE];
Note note;

int modeSerial; //Lecture = 0, traitement = 1, écriture = 2
Note n;
  

void NotesMsg(uint16_t dataSize);
void StartMsg(uint16_t dataSize);
void StopMsg(uint16_t dataSize);
void PauseMsg(uint16_t dataSize);
void TempoMsg(uint16_t dataSize);

void ReadMessageTest();
void ReadMessage();

bool ReadHeadMessage();
bool CheckMessage();
void ReadDataMessage();
void ResponseMessage(byte type);

void setup() {
  
  pinMode(13, OUTPUT);
  digitalWrite(13, LOW);
  modeSerial = 0;
  // open the serial port:
  numMessage =-1;
  //Serial.setTimeout(500);
  Serial.begin(19200);
}

void loop() {
//  switch (modeSerial)
//  {
//    case 0:   //entête du message
      if (Serial.available() > 0)
        if(ReadHeadMessage())
          modeSerial = 1;
//      break;
//    case 1:   //Validité du message? et lecture des données
//      if(CheckMessage())
//        void ReadDataMessage();
//      break;
//    case 2:
//      ResponseMessage(msgSendType);
//      digitalWrite(13, HIGH);
//      break;
//    default:
//      break;
//  }
}

/****************************************FONCTIONS****************************************/
bool ReadHeadMessage() {
    static byte i;
    if(i<HEAD_RECEIVE_MSG_SIZE){
      while(Serial.available()){
        headMessage[i++] = Serial.read();
        if(i==HEAD_RECEIVE_MSG_SIZE){
          //if((headMessage[0] == 255) && (headMessage[1] == 0) && (headMessage[2] == 1) && (headMessage[3] == 5) && (headMessage[4] == 0))
          digitalWrite(13, HIGH);
          i = 0;
          return true;
        }
      }
      return false;
    }
    else{
      if((headMessage[0] == 255) && (headMessage[1] == 0) && (headMessage[2] == 1) && (headMessage[3] == 5) && (headMessage[4] == 0))
        digitalWrite(13, HIGH);
      i = 0;
      return true;
    }
}

bool CheckMessage() {
  uint32_t dataSize;
  if(headMessage[0] == 255){
    oldNumMessage = numMessage;
    numMessage = headMessage[1];
    if(oldNumMessage != numMessage){
        if(headMessage[2] < RECEIVE_TYPE_SIZE)
        {
          return true;
        }else
          msgSendType = SendType_ErrorType;
    }else
      msgSendType = msgSendType;
  }else
    msgSendType = SendType_ErrorStartByte;
  return false;
}

void ReadDataMessage() {
  uint32_t dataSize;
  dataSize = headMessage[3];
  dataSize |= headMessage[4] << 8;
  switch (headMessage[2]){
    case ReceiveType_Tempo : //tempo
      TempoMsg(dataSize);
      break;
    case ReceiveType_Notes : //Notes
      NotesMsg(dataSize);
      break;
    case ReceiveType_Start : //Start
      StartMsg(dataSize);
      break;
    case ReceiveType_Stop : //Stop
      StopMsg(dataSize);
      break;
    case ReceiveType_Pause : //Pause
      PauseMsg(dataSize);
      break;
    default:
      break;
  }
}

void NotesMsg(uint16_t dataSize)
{
  static byte i;
  Note tmpNote;
  byte noteBytes[NOTE_SIZE] = {0};
  uint32_t tmpTick, j, k;

  for(k=0; k<dataSize; k+=NOTE_SIZE){
    //Lit une note
    while(Serial.available()){
      noteBytes[i++] = Serial.read();
      if(i == NOTE_SIZE){
        i = 0;
        break;
      }
    }
    tmpTick = 0;
    for(j=0;j<4;j++)
      tmpTick |= (noteBytes[j+1] << (j*8));  
    tmpNote.SetPitch(noteBytes[0]);
    tmpNote.SetTick(tmpTick);
    bufferNotes.Write(tmpNote);
    msgSendType = SendType_Ok;
  }
}
void StartMsg(uint16_t dataSize)
{
  msgSendType = SendType_Ok;
}
void StopMsg(uint16_t dataSize)
{
  msgSendType = SendType_Ok;
}
void PauseMsg(uint16_t dataSize)
{
  msgSendType = SendType_Ok;
}
void TempoMsg(uint16_t dataSize)
{
  msgSendType = SendType_Ok;
}

void ResponseMessage(byte type)
{
  byte msg[HEAD_SEND_MSG_SIZE];
  msg[0] = START_BYTE;
  msg[1] = numMessage;
  msg[2] = type;
  msg[3] = bufferNotes.SizeAvailable();
  Serial.write(msg, 4);   
}













void ReadMessageTest()
{
  byte head[HEAD_RECEIVE_MSG_SIZE];
  int i;
  uint16_t dataSize;
  //Lecture de l'entête du message
  while(Serial.available()){
    head[i++] = Serial.read();
    if(i==HEAD_RECEIVE_MSG_SIZE)
      break;
  }

  if(head[0] == 255 && head[1] == 0 && head[2] == 1 && head[3] == 5 && head[4] == 0)
    digitalWrite(13, HIGH);

  ResponseMessage(SendType_Ok);
  
//  if(head[0] == 255){
//    
//    oldNumMessage = numMessage;
//    numMessage = head[1];
//    if(oldNumMessage != numMessage){
//      dataSize = head[3];
//      dataSize |= head[4];
//    
//      switch (head[2]){
//        case ReceiveType_Tempo : //tempo
//          TempoMsg(dataSize);
//          return;
//          break;
//        case ReceiveType_Notes : //Notes
//          digitalWrite(13, HIGH);
//          NotesMsg(dataSize);
//          return;
//          break;
//        case ReceiveType_Start : //Start
//          StartMsg(dataSize);
//          return;
//          break;
//        case ReceiveType_Stop : //Stop
//          StopMsg(dataSize);
//          return;
//          break;
//        case ReceiveType_Pause : //Pause
//          PauseMsg(dataSize);
//          break;
//        default:
//          msgSendType = SendType_ErrorType;
//          break;
//      }
//    }else
//      msgSendType = msgSendType;
//  }else
//    msgSendType = SendType_ErrorStartByte;
//    
//  ResponseMessage(msgSendType);
}

void ReadMessage()
{
  byte tmp;
  uint16_t dataSize;
  tmp = Serial.read();
  if (tmp == START_BYTE) //check si on lit bien depuis le début du message
  {
    oldNumMessage = numMessage;
    numMessage = Serial.read();
    if (oldNumMessage != numMessage) //Check la réemission
    {
      tmp = Serial.read();
      dataSize = Serial.read();
      dataSize |= (Serial.read() << 8);
      switch (tmp){
        case ReceiveType_Tempo : //tempo
          //digitalWrite(13, HIGH);
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
          msgSendType = SendType_ErrorType;
          break;
      }       
    }else //Si on reçoit 2 fois le même message on transmet la même réponse qu'avant
      msgSendType = msgSendType;
  }else
    msgSendType = SendType_ErrorStartByte;

  ResponseMessage(msgSendType);
}

//void NotesMsg(uint16_t dataSize)
//{
//  Note tmpNote;
//  uint16_t i;
//  uint32_t tmpTick;
//  
//  for(i=0;i<dataSize;i+=NOTE_SIZE)
//  {
//    tmpNote.SetPitch(Serial.read());
//    tmpTick = Serial.read();
//    tmpTick |= (Serial.read() << 8);
//    tmpTick |= (Serial.read() << 16);
//    tmpTick |= (Serial.read() << 24);
//    tmpNote.SetTick(tmpTick);
//    bufferNotes.Write(tmpNote);
//  }
//  msgSendType = SendType_Ok;
//}


