#include "CircularBuffer.h"



CircularBuffer::CircularBuffer()
{
	_iWrite = 0;
	_iRead = 0;
	_full = false;
}

CircularBuffer::~CircularBuffer()
{
}

bool CircularBuffer::Write(int value)
{
	if (!_full) {
		_buffer[_iWrite++] = value;
		if (_iWrite == BUFFER_SIZE)
			_iWrite = 0;
		if (_iWrite == _iRead)
			_full = true;
		return true;
	}
	else
		return false;
}
bool CircularBuffer::Read(int* value)
{
	if (SizeAvailable() != BUFFER_SIZE) {
		_full = false;
		*value = _buffer[_iRead++];
		if (_iRead == BUFFER_SIZE)
			_iRead = 0;
		return true;
	}
	else
		return false;
}

int CircularBuffer::SizeAvailable()
{
	if (!_full)
		return _iWrite >= _iRead ? BUFFER_SIZE - (_iWrite - _iRead) : _iRead - _iWrite;
	else
		return 0;
}

void CircularBuffer::Clear()
{
	for (int i = 0; i<BUFFER_SIZE; i++)
		_buffer[i] = NULL;
	_iWrite = 0;
	_iRead = 0;
	_full = false;
}

bool CircularBuffer::Current(int* value)
{
	if (SizeAvailable() != BUFFER_SIZE) {
		*value = _buffer[_iRead];
		return true;
	}
	else
		return false;
}