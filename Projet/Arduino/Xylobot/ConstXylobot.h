#ifndef ConstXylobot_h
#define ConstXylobot_h

#include "Notes.h"

#define START_BYTE 255
#define HEAD_SEND_MSG_SIZE 8
#define HEAD_RECEIVE_MSG_SIZE 5
#define BAUD_RATE_SERIAL 57600

#define NOTE_COUNT_XYLO 37

#define DEFAULT_TIME_HIT_US 9.5*1000
#define TEMPO 4000        //Temps en us entre chaque tick

/*I2C*/
extern const Tone toneTab[NOTE_COUNT_XYLO];
//extern const int timeHitNote[NOTE_COUNT_XYLO];

enum NotesXylo{
  Do5,DoDiese5,Re5,ReDiese5,Mi5,Fa5,FaDiese5,Sol5,SolDiese5,La5,LaDiese5,Si5,
  Do6,DoDiese6,Re6,ReDiese6,Mi6,Fa6,FaDiese6,Sol6,SolDiese6,La6,LaDiese6,Si6,
  Do7,DoDiese7,Re7,ReDiese7,Mi7,Fa7,FaDiese7,Sol7,SolDiese7,La7,LaDiese7,Si7,
  Do8};



#endif
