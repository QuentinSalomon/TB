#ifndef Notes_h
#define Notes_h

#include <arduino.h>

#define NOTE_SIZE 6

struct Tone
{
  byte registerIndex, mask;
};

class Note
{
  public :
  
    Note(byte p, uint32_t t);
    Note();
    
    byte GetPitch();
    void SetPitch(byte p);
    uint32_t GetTick();
    void SetTick(uint32_t t);
    double GetIntensity();
    void SetIntensity(double i);
  private :
    byte _pitch;
    uint32_t _tick;
    double _intensity;
};

struct KeyXylophone
{
  double hitTime;
  unsigned long timePushed; // temps au quel la note à été poussée
  double intensity;
  bool pushed;
};

#endif
