#include "stdafx.h"
#include "CppUnitTest.h"
#include "CircularBuffer.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace UnitTest1
{		
	TEST_CLASS(UnitTest1)
	{
	public:
		
		TEST_METHOD(TestMethod1)
		{
			CircularBuffer c;
			int val;
			Assert::AreEqual(c.SizeAvailble(), BUFFER_SIZE);
			Assert::IsFalse(c.Read(&val));
			Assert::IsTrue(c.Write(2));
			Assert::IsTrue(c.Read(&val));
			Assert::AreEqual(val, 2);
			for (int i = 0; i < BUFFER_SIZE; i++)
				Assert::IsTrue(c.Write(i));
			Assert::AreEqual(c.SizeAvailble(), 0);
			for (int i = 0; i < BUFFER_SIZE; i++) {
				Assert::IsTrue(c.Read(&val));
				Assert::AreEqual(val, i);
			}
			Assert::IsFalse(c.Read(&val));
			Assert::AreEqual(c.SizeAvailble(), BUFFER_SIZE);
		}

	};
}