#include "Mcp23017.h"

Mcp23017::Mcp23017()
{
  
}

void Mcp23017::Init()
{
  Wire.begin();
  SetIoDirOutput(32);
  SetIoDirOutput(33);
  SetIoDirOutput(34);
}

void Mcp23017::SetIoDirOutput(byte address)
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

void Mcp23017::PreparePush(const Tone& t)
{
  _registerValues[t.registerIndex] |= t.mask;
}

void Mcp23017::ApplyPush()
{
  for(int i=0; i < REGISTER_COUNT; i++)
  {
    if(_registerValues[i] != 0)
    {
      Wire.beginTransmission(_registers[i].address);
      Wire.write(_registers[i].registerAddress);
      Wire.write(_registerValues[i] | (i==4 ? 0x80 : 0));
      Wire.endTransmission();
    }
  }
}

void Mcp23017::ReleasePush()
{
  for(int i=0; i < REGISTER_COUNT; i++)
  {
    if(_registerValues[i] != 0)
    {
      Wire.beginTransmission(_registers[i].address);
      Wire.write(_registers[i].registerAddress);
      Wire.write(0 | (i==4 ? 0x80 : 0));
      Wire.endTransmission();
      _registerValues[i] = 0;
    }
  }  
}

