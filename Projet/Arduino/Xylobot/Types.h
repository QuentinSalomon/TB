#ifndef Types_h
#define Types_h

#include <arduino.h>

#define RECEIVE_TYPE_SIZE 7
#define SEND_TYPE_SIZE 4

enum ReceiveTypeMessage { ReceiveType_Tempo, ReceiveType_Notes, ReceiveType_Start, ReceiveType_Stop, ReceiveType_Pause, ReceiveType_SpeedFactor, ReceiveType_HitTimeKey };

enum SendTypeMessage { SendType_Ok, SendType_TooManyData, SendType_ErrorStartByte,  SendType_ErrorType, SendType_OtherError};


#endif
