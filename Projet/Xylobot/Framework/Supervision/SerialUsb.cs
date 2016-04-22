using Concept.Model;
using Common;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    enum TypeMessage : byte { Tempo, Notes, Start, Stop, Pause }
    enum Days : byte { Sat = 1, Sun, Mon, Tue, Wed, Thu, Fri };

    public class SerialUsb
    {
        private const byte _startByte = 255;
        private const Int32 _baudRate = 230400, _sizeHeadMessage = 4, _timeOut = 500;
        private const string _portName = "COM1";
        private SerialPort _usb;
        private byte _numMessage;

        public SerialUsb()
        {
            _numMessage = 0;
            _usb = new SerialPort(_portName, _baudRate);
            _usb.ReadTimeout = _timeOut;
            _usb.WriteTimeout = _timeOut;
            _usb.Open();
        }

        #region

        public byte ArduinoSizeAvaible { get; set; }

        #endregion

        #region Methods

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
            byte[] msg = new byte[_sizeHeadMessage + notes.Count];
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
