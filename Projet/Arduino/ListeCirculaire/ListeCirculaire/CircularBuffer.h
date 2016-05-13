#include <cstdlib>


#define BUFFER_SIZE 5

class CircularBuffer
{
public:
	CircularBuffer();
	~CircularBuffer();

	bool Write(int value);
	bool Read(int* value);
	int SizeAvailable();
	void Clear();
private:
	int _buffer[BUFFER_SIZE], _iWrite, _iRead;
	bool _full;
};