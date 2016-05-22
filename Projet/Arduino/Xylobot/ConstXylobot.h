#ifndef ConstXylobot_h
#define ConstXylobot_h

#include "Notes.h"

#define START_BYTE 255
#define HEAD_SEND_MSG_SIZE 4
#define HEAD_RECEIVE_MSG_SIZE 5
#define BAUD_RATE_SERIAL 57600

#define NOTE_COUNT_XYLO 37

#define TIME_HIT_MS 7
#define TIMER_MS 200             //Tempo   //TODO: 50ms
#define TIMER_US 200000          // 500mS set timer duration in microseconds

#define TICK_INC 12

/*I2C*/
const Tone toneTab[NOTE_COUNT_XYLO] = {
  {0,128}/*Do5*/, {3,1}/*DO#5*/, {0,64}/*Ré5*/, {3,2}/*Ré#5*/, {0,32}/*Mi5*/, {0,16}/*Fa5*/, {3,4}/*Fa#5*/, 
  {0,8}/*Sol5*/, {3,8}/*Sol#5*/ , {0,4}/*La5*/, {3,16}/*La#5*/, {0,2}/*Si5*/,
  
  {0,1}/*Do6*/, {3,32}/*DO#6*/, {1,128}/*Ré6*/, {3,64}/*Ré#6*/, {1,64}/*Mi6*/, {1,32}/*Fa6*/, {3,128}/*Fa#6*/, 
  {1,16}/*Sol6*/, {4,1}/*Sol#6*/ , {1,8}/*La6*/, {4,2}/*La#6*/, {1,4}/*Si6*/,  

  {2,128}/*Do7*/, {4,4}/*DO#7*/, {2,64}/*Ré7*/, {4,8}/*Ré#7*/, {2,32}/*Mi7*/, {2,16}/*Fa7*/, {4,16}/*Fa#7*/, 
  {2,8}/*Sol7*/, {4,32}/*Sol#7*/ , {2,4}/*La7*/, {4,64}/*La#7*/, {2,2}/*Si7*/,

  {2,1}/*Do8*/
  };



#endif
