#include "Notes.h"

Note::Note()
{

}

Note::Note(byte p, uint32_t t)
{
  _pitch = p;
  _tick = t;
}

byte Note::GetPitch()
{
  return _pitch;
}

uint32_t Note::GetTick()
{
  return _tick;
}

void Note::SetPitch(byte p)
{
  _pitch = p;
}

void Note::SetTick(uint32_t t)
{
  _tick = t;
}

double Note::GetIntensity()
{
  return _intensity;
}

void Note::SetIntensity(double i)
{
  _intensity = i;
}

