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
			Assert::AreEqual(c.SizeAvailable(), BUFFER_SIZE);
			Assert::IsFalse(c.Read(&val));
			Assert::IsTrue(c.Write(2));
			Assert::IsTrue(c.Read(&val));
			Assert::AreEqual(val, 2);
			for (int i = 0; i < BUFFER_SIZE; i++)
				Assert::IsTrue(c.Write(i));
			Assert::AreEqual(c.SizeAvailable(), 0);
			Assert::IsFalse(c.Write(1));
			for (int i = 0; i < BUFFER_SIZE; i++) {
				Assert::IsTrue(c.Read(&val));
				Assert::AreEqual(val, i);
			}
			Assert::IsFalse(c.Read(&val));
			Assert::AreEqual(c.SizeAvailable(), BUFFER_SIZE);
			for (int i = 0; i < BUFFER_SIZE; i++)
				Assert::IsTrue(c.Write(i));
			Assert::AreEqual(c.SizeAvailable(), 0);

			c.Clear();

			Assert::AreEqual(c.SizeAvailable(), BUFFER_SIZE);
			Assert::IsFalse(c.Read(&val));

			for (int i = 0; i < BUFFER_SIZE; i++)
				Assert::IsTrue(c.Write(i));
			Assert::AreEqual(c.SizeAvailable(), 0);
			Assert::IsFalse(c.Write(1));
			for (int i = 0; i < BUFFER_SIZE; i++) {
				Assert::IsTrue(c.Current(&val));
				Assert::AreEqual(val, i);
				Assert::IsTrue(c.Read(&val));
				Assert::AreEqual(val, i);
			}
		}

	};
}