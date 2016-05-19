#ifndef CircularBuffer_h
#define CircularBuffer_h

#include <arduino.h>
#include "Notes.h"

#define BUFFER_SIZE 100   //Max 256 (1 byte)

class CircularBuffer
{
public:
	CircularBuffer();
	~CircularBuffer();
  
  bool Current(Note* value);
	bool Write(Note value);
	bool Consume(Note* value);
	byte SizeAvailable();
  void Clear();
private:
	Note _buffer[BUFFER_SIZE];
	int _iWrite, _iRead;
	bool _full;
};

#endif
