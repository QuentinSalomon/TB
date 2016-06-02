#include "I2CXylo.h"

I2CXylo::I2CXylo()
{
  
}

void I2CXylo::Init()
{
  Wire.begin();
  SetIoDirOutput(32);
  SetIoDirOutput(33);
  SetIoDirOutput(34);
}

void I2CXylo::SetIoDirOutput(byte address)
{
  Wire.beginTransmission(address);
  Wire.write(0);
  Wire.write(0);
  Wire.endTransmission();
  Wire.beginTransmission(address);
  Wire.write(1);
  Wire.write(0);
  Wire.endTransmission();
  
}

void I2CXylo::PreparePush(const Tone t)
{
  _registerPushValues[t.registerIndex] |= t.mask;
}

void I2CXylo::ApplyPush()
{
  for(int i=0; i < REGISTER_COUNT; i++)
  {
    _registerPushedValues[i] |= _registerPushValues[i];
    if(_registerPushValues[i] != 0)
    {
      Wire.beginTransmission(_registers[i].address);
      Wire.write(_registers[i].registerAddress);
      Wire.write(_registerPushValues[i] | (i==4 ? 0x80 : 0));
      Wire.endTransmission();
      _registerPushValues[i] = 0;
    }
  }
}

void I2CXylo::PrepareRelease(const Tone t)
{
  _registerReleaseValues[t.registerIndex] = (_registerPushedValues[t.registerIndex] ^ t.mask);
  _registerPushedValues[t.registerIndex] = _registerReleaseValues[t.registerIndex];
}

void I2CXylo::ApplyRelease()
{
  for(int i=0; i < REGISTER_COUNT; i++)
  {
    Wire.beginTransmission(_registers[i].address);
    Wire.write(_registers[i].registerAddress);
    Wire.write(_registerReleaseValues[i] | (i==4 ? 0x80 : 0));
    Wire.endTransmission();
    _registerReleaseValues[i] = 0;
  }  
}

