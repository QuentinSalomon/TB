#ifndef Types_h
#define Types_h

#include <arduino.h>

#define RECEIVE_TYPE_SIZE 7
#define SEND_TYPE_SIZE 4

enum ReceiveHeadMessage { ReceiveHeadMessage_Startbyte = 0, ReceiveHeadMessage_Number, ReceiveHeadMessage_Type, ReceiveHeadMessage_DataSize1, ReceiveHeadMessage_DataSize2 };

enum ReceiveTypeMessage { ReceiveType_Tempo, ReceiveType_Notes, ReceiveType_Start, ReceiveType_Stop, ReceiveType_Pause, ReceiveType_SpeedFactor, ReceiveType_KeyHitTime };

enum SendTypeMessage { SendType_Ok, SendType_TooManyData, SendType_ErrorStartByte,  SendType_ErrorType, SendType_OtherError};


#endif
