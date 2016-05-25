#ifndef ConstXylobot_h
#define ConstXylobot_h

#include "Notes.h"

#define START_BYTE 255
#define HEAD_SEND_MSG_SIZE 4
#define HEAD_RECEIVE_MSG_SIZE 5
#define BAUD_RATE_SERIAL 57600

#define NOTE_COUNT_XYLO 37

#define TIME_HIT_MS 9
#define TIMER_MS 500             //Tempo   //TODO: 50ms
#define TIMER_US 200000          // 500mS set timer duration in microseconds

#define TICK_INC 12              //Incrément du 'tick' pour les notes (=diff. de tick entre 2 notes)

/*I2C*/
extern const Tone toneTab[NOTE_COUNT_XYLO];

enum NotesXylo{
  Do5,DoDiese5,Re5,ReDiese5,Mi5,Fa5,FaDiese5,Sol5,SolDiese5,La5,LaDiese5,Si5,
  Do6,DoDiese6,Re6,ReDiese6,Mi6,Fa6,FaDiese6,Sol6,SolDiese6,La6,LaDiese6,Si6,
  Do7,DoDiese7,Re7,ReDiese7,Mi7,Fa7,FaDiese7,Sol7,SolDiese7,La7,LaDiese7,Si7,
  Do8};



#endif
