#ifndef Notes_h
#define Notes_h

class Note
{
  public :
    Note(byte p, long t);
    
    byte GetPitch();
    //void SetPitch(byte p);
    long GetTick();
    //void SetTick(long t);
  private :
    byte _pitch;
    long _tick;
};

#endif
