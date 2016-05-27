#include "Notes.h"
#include "Types.h"
#include "CircularBuffer.h"
#include "ConstXylobot.h"
#include "I2CXylo.h"
#include <TimerOne.h>
#include <MsTimer2.h>
//#include <Wire.h>

/*Serial*/
int numMessage = -1,oldNumMessage;
SendTypeMessage msgSendType;
//Variable pour la lecture de l'en-tête du message
byte headMessage[HEAD_RECEIVE_MSG_SIZE];
int count = 0;

/*I2C*/
I2CXylo i2cXylo;
uint32_t tick=0, idxTime = 0;
bool play=false;

int modeSerial = 0; //Lecture = 0, traitement = 1 et 2, écriture = 3
CircularBuffer bufferNotes;

void NotesMsg(uint16_t dataSize);
void StartMsg(uint16_t dataSize);
void StopMsg(uint16_t dataSize);
void PauseMsg(uint16_t dataSize);
void TempoMsg(uint16_t dataSize);

bool ReadHeadMessage();
bool CheckMessage();
void ReadDataMessage();
void ResponseMessage(byte type);

/****************************************ARDUINO FONCTIONS****************************************/
void setup() {
  pinMode(13, OUTPUT);
  /* INIT SERIAL COM */
  // open the serial port:
  //Serial.setTimeout(500);
  Serial.flush();
  Serial.begin(BAUD_RATE_SERIAL);
  while(Serial.available());
  /* INIT I2C */
  i2cXylo.Init();
}

void loop() {
  /*****I2C*****/
  if(idxTime*TIMER_MS < millis())
  {
    if(play){
      Note currentNote;
      while(1)
      {
        if(! bufferNotes.Current(&currentNote))
          break;
        //Si le tick de la note est plus petit que le tick courant ==> nouvelle partition (==> tick = 0)
        if(currentNote.GetTick() < tick){
          digitalWrite(13, HIGH);
          tick = 0;
        }
          
        if(currentNote.GetTick() == tick){
          bufferNotes.Consume(&currentNote); //si le tick est celui de la note courante, on consume la note
          i2cXylo.PreparePush(toneTab[currentNote.GetPitch()]);
        }
        else
          break;
      }
      i2cXylo.ApplyPush();
      delay(TIME_HIT_MS);
      i2cXylo.ReleasePush();
      
      tick += TICK_INC;
    }

    idxTime++;
  }
  /*****USB*****/
  switch (modeSerial)
  {
    case 0:   //entête du message
        if(ReadHeadMessage()) //Attend de recevoir tout l'entête du message avant de continuer
          modeSerial = 1;
      break;
      
    case 1:   //Validité du message? et lecture des données
      if(CheckMessage())
        modeSerial = 2;
      else
        modeSerial = 3;
      break;

    case 2:
      ReadDataMessage();
      modeSerial = 3;
      break;
      
    case 3:
      while(Serial.available())
        Serial.read();
      ResponseMessage(msgSendType);
      modeSerial = 0;
      break;
    default:
      break;
  }
}

/****************************************FONCTIONS SERIAL****************************************/
bool ReadHeadMessage() {
    while(Serial.available()){
      headMessage[count++] = Serial.read();
      if(count==HEAD_RECEIVE_MSG_SIZE){
        count = 0;
        return true; 
      }
    }
    return false;
}

bool CheckMessage() {
  uint32_t dataSize;
  /*Start byte correcte?*/
  if(headMessage[0] == START_BYTE){
    /*Pas de réemission?*/
    oldNumMessage = numMessage;
    numMessage = headMessage[1];
    if(oldNumMessage != numMessage){
      /*Type de message existant?*/
      if(headMessage[2] < RECEIVE_TYPE_SIZE)
      {
        /*place pour toutes les données reçues?*/
        if(bufferNotes.SizeAvailable() >= (headMessage[3]|(headMessage[4]<<8))/NOTE_SIZE)
          return true;
        else
          msgSendType = SendType_TooManyData;
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
  uint32_t tmpTick;
  int j, k;

  if(dataSize%NOTE_SIZE != 0){
    msgSendType = SendType_OtherError;
    return;
  }

  for(k=0; k<dataSize; k+=NOTE_SIZE){
    //Lit une note
    if(Serial.available() >= NOTE_SIZE){
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
    }
    else
      k -= NOTE_SIZE; //soustrait l'additoin de la boucle for car les données n'ont pas encre été reçues
  }
  msgSendType = SendType_Ok;
}
void StartMsg(uint16_t dataSize)
{
  play = true;
  msgSendType = SendType_Ok;
}
void StopMsg(uint16_t dataSize)
{
  play=false;
  bufferNotes.Clear();
  msgSendType = SendType_Ok;
}
void PauseMsg(uint16_t dataSize)
{
  play=false;
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

