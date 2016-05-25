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
int count = 0, idxTime = 0, tick = 0;

/*I2C*/
I2CXylo i2cXylo;

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

/****************************************TIMER INTERRUPT****************************************/
//#define TIMER_US 1000000                            // 0.5mS set timer duration in microseconds 

volatile bool in_long_isr = false;                 // True if in long interrupt
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
  /* INIT INTERRUPT */
//  noInterrupts();
//  Timer1.initialize(TIMER_US);                  // Initialise timer 1
//  //démarrage décalé
//  Timer1.attachInterrupt(timerIsr);             // attach the ISR routine here
//  interrupts();
}

void loop() {
  if(idxTime*TIMER_MS < millis())
  {
    if(idxTime == 5){
      for(int i=0;i<NOTE_COUNT_XYLO;i++){
        i2cXylo.PreparePush(toneTab[i]);
        digitalWrite(13, HIGH);
        i2cXylo.ApplyPush();
        delay(TIME_HIT_MS);
        i2cXylo.ReleasePush();
        delay(15);
      }
      //digitalWrite(13, LOW);
    }
    idxTime++;
  }else{
    switch (modeSerial)
    {
      case 0:   //entête du message
          if(ReadHeadMessage())
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
}

/****************************************FONCTIONS INTERRUPT****************************************/

void timerIsr()
{
  static int a=0;
//  if(syncInterrupt){
    if(a++ == 5){
      i2cXylo.PreparePush(toneTab[Fa5]);
      digitalWrite(13, HIGH);
      i2cXylo.ApplyPush();
      delay(TIME_HIT_MS);
      i2cXylo.ReleasePush();
      digitalWrite(13, LOW);
    }
//  }
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
        if(bufferNotes.SizeAvailable() > (headMessage[3]|headMessage[4])/NOTE_SIZE)
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
      k -= NOTE_SIZE;
  }
  msgSendType = SendType_Ok;
}
void StartMsg(uint16_t dataSize)
{
  interrupts();
  msgSendType = SendType_Ok;
}
void StopMsg(uint16_t dataSize)
{
  noInterrupts();
  bufferNotes.Clear();
  msgSendType = SendType_Ok;
}
void PauseMsg(uint16_t dataSize)
{
  noInterrupts();
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

