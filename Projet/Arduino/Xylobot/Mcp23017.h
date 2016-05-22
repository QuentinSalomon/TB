#ifndef Mcp23017_h
#define Mcp23017_h

#include <Wire.h>
#include <arduino.h>
#include "Notes.h"

struct Mcp23017Register
{
  byte address, registerAddress;
};

#define REGISTER_COUNT 5

class Mcp23017
{
  public :
    Mcp23017();

    void Init();
    void PreparePush(const Tone& tone);
    void ApplyPush();
    void ReleasePush();
  private :
    void SetIoDirOutput(byte address);
    
    const Mcp23017Register _registers[REGISTER_COUNT] = {{33,0x12}, {34,0x12}, {32,0x12}, {33,0x13}, {32,0x13}};
    byte _registerValues[5] = {0};
};

#endif
