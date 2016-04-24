using Concept.Model;
using Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;

namespace Framework
{
    enum TypeMessage : byte { Tempo, Notes, Start, Stop, Pause }

    public class SerialUsb
    {
        private const byte _startByte = 255;
        private const Int32 _baudRate = 9600, _sizeHeadMessage = 4, _timeOut = 500;
        private const string _portName = "COM1";
        private SerialPort _usb;
        private byte _numMessage;
        Thread t;
        private Xylobot _xylo;

        public SerialUsb(Xylobot xylo)
        {
            _numMessage = 0;
            _usb = new SerialPort(_portName, _baudRate);
            _usb.ReadTimeout = _timeOut;
            _usb.WriteTimeout = _timeOut;

            _xylo = xylo;

            t = new Thread(test);
            
            _usb.Open();
            t.Start();
        }

        #region Propriétés

        public byte ArduinoSizeAvaible { get; set; }

        #endregion

        #region Methods

        public void test()
        {
            int tmp;
            byte[] msg = new byte[2];
            msg[0] = Byte.MaxValue;
            msg[1] = 13;
            _usb.Write(msg, 0, 2);
            if (_usb.ReadByte() != Byte.MaxValue)
            {
                tmp = _usb.ReadByte();

                Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
                {
                    _xylo.Buffer = tmp.ToString();
                }));
            }

        }

        public void Read()
        {
            List<byte> msg = new List<byte>();
            try
            {
                msg.Add((byte)_usb.ReadByte());     //byte start
                msg.Add((byte)_usb.ReadByte());     //byte n° message
                msg.Add((byte)_usb.ReadByte());     //Type message
                msg.Add((byte)_usb.ReadByte());     //Size avaible
            }
            catch (TimeoutException e)
            {
                throw e;
            }
        }

        public void SendNotes(List<Note> notes)
        {
            int noteSize = (sizeof(Int32) + sizeof(byte));
            if (notes.Count>byte.MaxValue/noteSize)
                throw new Exception("trop de notes");
            int i=0;
            byte[] msg = new byte[_sizeHeadMessage + notes.Count * noteSize];
            byte[] dataSize = BitConverter.GetBytes(notes.Count * noteSize);
            msg[0] = _startByte;
            msg[1] = _numMessage++;
            msg[2] = (byte)TypeMessage.Notes;
            msg[3] = dataSize[0];
            msg[4] = dataSize[1];
            i = 5;
            foreach (Note note in notes)
            {
                msg[i++] = note.Pitch;
                foreach (byte data in BitConverter.GetBytes(note.Tick))
                    msg[i++] = data;
            }
            //Envoie
            try
            {
                _usb.Write(msg, 0, msg.Length);
            }
            catch (TimeoutException e)
            {
                ;
            }
        }

        #endregion
    }
}
