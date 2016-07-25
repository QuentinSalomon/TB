#include "I2CXylo.h"

const int LightRegister = 4, LightBit = 0x80;

I2CXylo::I2CXylo()
{
  
}

void I2CXylo::Init()
{
  Wire.begin();
  SetIoDirOutput(32);
  SetIoDirOutput(33);
  SetIoDirOutput(34);

  GraduateLight(true);
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
      Wire.write(_registerPushValues[i] | (i==LightRegister ? (_isLightOn ? LightBit : 0) : 0));
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
    Wire.write(_registerReleaseValues[i] | (i==LightRegister ? (_isLightOn ? LightBit : 0) : 0));
    Wire.endTransmission();
    _registerReleaseValues[i] = 0;
  }  
}

void I2CXylo::SetLight(bool on)
{
  _isLightOn = on;
  Wire.beginTransmission(_registers[LightRegister].address);
  Wire.write(_registers[LightRegister].registerAddress);
  Wire.write(_registerPushValues[LightRegister] | (_isLightOn ? LightBit : 0));
  Wire.endTransmission();
}

void I2CXylo::GraduateLight(bool on)
{
  if(on ? !_isLightOn : _isLightOn)
    for (int i = 0; i < 100; i++)
    {
      long p2 = 40 * i;
      long p1 = 4000L - p2;
      SetLight(!on);
      delayMicroseconds(p1);
      SetLight(on);
      delayMicroseconds(p2);
    }
}

