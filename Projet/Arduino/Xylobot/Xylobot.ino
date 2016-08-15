#include "Notes.h"
#include "Types.h"
#include "CircularBuffer.h"
#include "ConstXylobot.h"
#include "I2CXylo.h"
//#include <Wire.h>

/*Serial*/
int modeSerial = 0, numMessage = -1,oldNumMessage,idxReadHeadMessage = 0;;
SendTypeMessage msgSendType;
byte headMessage[HEAD_RECEIVE_MSG_SIZE];

/*I2C*/
I2CXylo i2cXylo;
unsigned long currentTick=0xFFFFFFFF, startTime=0;
bool play=false;
KeyXylophone keysXylophone[NOTE_COUNT_XYLO] = {0};
int debug=1;

/*Global*/
CircularBuffer bufferNotes;
double tempoFactor = 1;
int tempo = 4000;


/*I2C*/
void ReleasePush();
void Push();

/*Serial*/
void NotesMsg(uint16_t dataSize);
void StartMsg(uint16_t dataSize);
void StopMsg(uint16_t dataSize);
void PauseMsg(uint16_t dataSize);
void TempoMsg(uint16_t dataSize);
void SpeedFactorMsg(uint16_t dataSize);
void TimeHitKeyMsg(uint16_t dataSize);

bool ReadHeadMessage();
bool CheckMessage();
void ReadDataMessage();
void ResponseMessage(byte type);

void DebugBlink(byte status)
{
  for(;;)
  {
    for (byte i = 0; i < status; i++)
    {
      digitalWrite(13, 1);
      delay(100);
      digitalWrite(13, 0);
      delay(100);
    }
    delay(1000);
  }
}
/****************************************ARDUINO FONCTIONS****************************************/
void setup() {
  pinMode(13, OUTPUT);
  for(int i=0; i<NOTE_COUNT_XYLO; i++)
    keysXylophone[i].hitTime = DEFAULT_TIME_HIT_US;
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
  if(play)
    Push();
  else
    startTime = micros(); //Actualise le temps si on est en pause
  ReleasePush();
  
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
      while(Serial.available()) //Vide le Buffer d'entrée
        Serial.read();
      ResponseMessage(msgSendType);
      modeSerial = 0;
      break;
      
    default:
      break;
  }
}

/****************************************FONCTIONS I2C****************************************/
void ReleasePush()
{
  bool needRelease = false;
  for(int i=0;i<NOTE_COUNT_XYLO;i++) //Check toutes les notes poussées et les préparent pour le release si besoin
    if(keysXylophone[i].pushed)
      if((micros() >= keysXylophone[i].timePushed ? micros() - keysXylophone[i].timePushed : 0xFFFFFFFF - keysXylophone[i].timePushed + micros()) > keysXylophone[i].hitTime + keysXylophone[i].intensity){
        i2cXylo.PrepareRelease(toneTab[i]);
        keysXylophone[i].pushed = false;
        needRelease = true;
      }
  if(needRelease)
    i2cXylo.ApplyRelease();
}

void Push()
{
  Note currentNote;
  if(bufferNotes.Current(&currentNote)){ //Check que les notes soient déjà arrivée
    //Si le tick de la note est plus petit que le tick courant ==> nouvelle partition (==> tick = 0)
    if(currentNote.GetTick() < currentTick){
      currentTick = 0;
      startTime = micros();
    } 
    while(currentNote.GetTick() == currentTick)
    {
      bufferNotes.Consume(&currentNote); //si le tick est celui de la note courante, on consume la note
      byte pitch = currentNote.GetPitch();
      i2cXylo.PreparePush(toneTab[pitch]);
      
      keysXylophone[pitch].pushed = true;
      keysXylophone[pitch].timePushed = micros();
      keysXylophone[pitch].intensity = currentNote.GetIntensity();
      if(!bufferNotes.Current(&currentNote)) //Actualise la note courante, s'il y en a plus on quitte la boucle
        break;
/****************************DEBUG***************************************/
//      if(currentTick + 10000 < currentNote.GetTick()){
//        currentTick = currentNote.GetTick() - 100;
//        digitalWrite(13, HIGH);
//      }
/**************************END DEBUG*************************************/
    }
    i2cXylo.ApplyPush();

    // TODO a simplifier par : micros() - startTime
    if((micros() >= startTime ? micros() - startTime : 0xFFFFFFFF - startTime + micros()) > tempo/tempoFactor ){
      currentTick++;
      startTime = micros();        
    }
  }
  else
    startTime = micros();
}



/****************************************FONCTIONS SERIAL****************************************/
bool ReadHeadMessage() {
    while(Serial.available()){
      headMessage[idxReadHeadMessage++] = Serial.read();
      if(idxReadHeadMessage==HEAD_RECEIVE_MSG_SIZE){
        idxReadHeadMessage = 0;
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
    case ReceiveType_Tempo : //Tempo
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
    case ReceiveType_SpeedFactor : //Music speed
      SpeedFactorMsg(dataSize);
      break;
    case ReceiveType_KeyHitTime : //Key hit time
      TimeHitKeyMsg(dataSize);
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
        tmpTick |= (((uint32_t)noteBytes[j+1]) << (j*8));
//      if(tmpTick > 100000000)
//        DebugBlink(3);
      tmpNote.SetPitch(noteBytes[0]);
      tmpNote.SetTick(tmpTick);
      tmpNote.SetIntensity((double)noteBytes[5]*4000/128); //Intensité changeant de 0 à 4 ms le temps de frappe
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
  i2cXylo.GraduateLight(true);
  msgSendType = SendType_Ok;
}
void StopMsg(uint16_t dataSize)
{
  play=false;
  bufferNotes.Clear();
  currentTick = 0;
  i2cXylo.GraduateLight(false);
  msgSendType = SendType_Ok;
}
void PauseMsg(uint16_t dataSize)
{
  play=false;
  msgSendType = SendType_Ok;
}
void TempoMsg(uint16_t dataSize)
{
  while(1)
    if(Serial.available() == dataSize)
      break;
  tempo = Serial.read() + (Serial.read() << 8); 
  msgSendType = SendType_Ok;
}
void SpeedFactorMsg(uint16_t dataSize)
{
  while(1)
    if(Serial.available() == dataSize)
      break;
  tempoFactor = Serial.read() + (double)Serial.read()/100; 
  msgSendType = SendType_Ok;
}

void TimeHitKeyMsg(uint16_t dataSize)
{
  int idxNote;
  while(1)
    if(Serial.available() == dataSize)
      break;
  idxNote = Serial.read();
  keysXylophone[idxNote].hitTime = (Serial.read() + (double)Serial.read()/100)*1000;
  msgSendType = SendType_Ok;
}

void ResponseMessage(byte type)
{
  byte msg[HEAD_SEND_MSG_SIZE];
  msg[0] = START_BYTE;
  msg[1] = numMessage;
  msg[2] = type;
  msg[3] = bufferNotes.SizeAvailable();
  for(int i=0;i<4;i++)
    msg[i+4] = (byte)(currentTick>>i*8);
  Serial.write(msg, HEAD_SEND_MSG_SIZE);   
}

