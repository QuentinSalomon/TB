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
    enum SendTypeMessage : byte { Tempo, Notes, Start, Stop, Pause }
    
    enum ReceiveTypeMessage : byte { Ok, TooManyData, ErrorStartByte, ErrorType }

    public class XyloCommunication
    {
        #region const

        // TODO : variables privées à la fin
        const byte StartByte = 255;
        // TODO : Ajuster la vitesse
        const Int32 BaudRate = 28800, SizeHeadMessage = 5, TimeOut = 2000;
        const int ReceiveTypeSize = 4;
        #endregion

        // TODO : delete t
        Thread t;

        #region Constructor

        // TODO : dépendance incorrecte xylo
        public XyloCommunication(Xylobot xylo)
        {
            _numMessage = 0;
            PortName = "COM5";
            _serialPort = new SerialPort(PortName, BaudRate);
            _serialPort.ReadTimeout = TimeOut;
            _serialPort.WriteTimeout = TimeOut;

            _xylo = xylo;

            t = new Thread(test);

            _serialPort.Open();
            t.Start();
        }

        #endregion

        #region Propriétés

        public byte ArduinoSizeAvaible { get; set; }

        // TODO : a mettre dans un fichier de configuration
        public string PortName { get; set; }

        #endregion

        #region Methods

        public void test()
        {
            byte[] a = new byte[1];
            a[0] = StartByte;
            _serialPort.Write(a, 0, 1);
            _xylo.Test = _serialPort.ReadByte().ToString(); 
        }

        public void Read()
        {
            List<byte> msg = new List<byte>();
            int tmp;
            try
            {
                tmp = _serialPort.ReadByte();
                if (tmp == StartByte)
                {
                    tmp = _serialPort.ReadByte();
                    if (tmp == _numMessage)
                    {
                        tmp = _serialPort.ReadByte();
                        if (tmp < ReceiveTypeSize)
                        {
                            tmp = _serialPort.ReadByte();
                            if (tmp >= 0)
                                ArduinoSizeAvaible = (byte)tmp;
                            switch ((ReceiveTypeMessage)tmp)
                            {
                                case ReceiveTypeMessage.Ok:
                                    break;
                                case ReceiveTypeMessage.TooManyData:
                                    break;
                                case ReceiveTypeMessage.ErrorStartByte:
                                    break;
                                case ReceiveTypeMessage.ErrorType:
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                            ; //Renvoyer le message précedent
                    } else
                        ; //Renvoyer le message précedent
                } else
                    ; //Renvoyer le message précedent

            }
            catch (TimeoutException e)
            {
                throw e;
            }
        }

        public void SendNotes(List<Note> notes)
        {
            int noteSize = (sizeof(UInt32) + sizeof(byte));
            if (notes.Count>byte.MaxValue/noteSize)
                throw new Exception("trop de notes");

            int i=0;
            byte[] msg = new byte[SizeHeadMessage + notes.Count * noteSize];
            ushort dataSize = BitConverter.ToUInt16(BitConverter.GetBytes(notes.Count * noteSize), 0);
            byte[] headMsg = HeaderMessage(dataSize, (byte)SendTypeMessage.Notes);
            //byte[] dataSize = BitConverter.GetBytes(notes.Count * noteSize);
            //msg[0] = StartByte;
            //msg[1] = _numMessage;
            //msg[2] = (byte)SendTypeMessage.Notes;
            //msg[3] = dataSize[0];
            //msg[4] = dataSize[1];

            i = SizeHeadMessage;
            foreach (Note note in notes)
            {
                msg[i++] = note.Pitch;
                foreach (byte data in BitConverter.GetBytes(note.Tick))
                    msg[i++] = data;
            }
            //Envoie
            try
            {
                _serialPort.Write(msg, 0, msg.Length);
            }
            catch (TimeoutException e)
            {
                ;
            }
        }

        public byte[] HeaderMessage(ushort size, byte type)
        {
            byte[] header = new byte[SizeHeadMessage];
            header[0] = StartByte;
            header[1] = _numMessage;
            header[2] = type;
            header[3] = BitConverter.GetBytes(size)[0];
            header[4] = BitConverter.GetBytes(size)[1];
            return header;
        }

        #endregion

        #region Private

        private SerialPort _serialPort;
        private byte _numMessage;
        // TODO : delete xylo
        private Xylobot _xylo;

        #endregion
    }
}
