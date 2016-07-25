#ifndef Mcp23017_h
#define Mcp23017_h

#include <Wire.h>
#include <arduino.h>
#include "Notes.h"

#define REGISTER_COUNT 5

struct Mcp23017Register
{
  byte address, registerAddress;
};

class I2CXylo
{
  public :
    I2CXylo();

    void Init();
    void PreparePush(const Tone t);
    void ApplyPush();
    void PrepareRelease(const Tone t);
    void ApplyRelease();
    void SetLight(bool on);
    void GraduateLight(bool on);
  private :
    void SetIoDirOutput(byte address);
    const Mcp23017Register _registers[REGISTER_COUNT] = {{33,0x12}, {34,0x12}, {32,0x12}, {33,0x13}, {32,0x13}};
    byte _registerPushValues[REGISTER_COUNT] = {0};
    byte _registerPushedValues[REGISTER_COUNT] = {0};
    byte _registerReleaseValues[REGISTER_COUNT] = {0};
    bool _isLightOn = false;
};

#endif
