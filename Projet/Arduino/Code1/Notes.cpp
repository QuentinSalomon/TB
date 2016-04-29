#include "Notes.h"

Note::Note()
{

}

Note::Note(byte p, long t)
{
  _pitch = p;
  _tick = t;
}

byte Note::GetPitch()
{
  return _pitch;
}

long Note::GetTick()
{
  return _tick;
}

