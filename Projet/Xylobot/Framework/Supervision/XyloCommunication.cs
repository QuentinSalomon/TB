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
    public enum SendTypeMessage : byte { Tempo, Notes, Start, Stop, Pause }
    
    public enum ReceiveTypeMessage : byte { Ok, TooManyData, ErrorStartByte, ErrorType }

    public class XyloCommunication
    {
        #region const
        
        const byte StartByte = 255;
        // TODO : Ajuster la vitesse
        const Int32 BaudRate = 57600, SizeHeadMessage = 5, TimeOut = 3000;
        const int ReceiveTypeSize = 4;
        #endregion

        // TODO : changer de place le thread t
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
            // TO DO: Régler comme il faut
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Parity = Parity.None;
            _serialPort.Handshake = Handshake.None;


            _xylo = xylo;

            t = new Thread(test);

            _serialPort.Open();
            _serialPort.DiscardOutBuffer();
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
            List<Note> listeTest = new List<Note>();
            //listeTest.Add(new Note(100, 1000));
            //listeTest.Add(new Note(80, 1100));
            SendNotes(listeTest);
            Read();
        }

        public void Read()
        {
            int tmp;
            try
            {
                tmp = _serialPort.ReadByte();
                if (tmp == StartByte)
                {
                    tmp = _serialPort.ReadByte();
                    if (tmp == _numMessage-1)
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

                            _xylo.Test = tmp.ToString();
                        }
                        else
                            _xylo.Test = "Mauvais Type"; //Renvoyer le message précedent
                    } else
                        _xylo.Test = "Mauvais numéro de message"; //Renvoyer le message précedent
                } else
                    _xylo.Test = "Mauvais Startbyte"; //Renvoyer le message précedent

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
            for (i = 0; i < SizeHeadMessage; i++)
                msg[i] = headMsg[i];
            foreach (Note note in notes)
            {
                msg[i++] = (byte)(note.High * note.Octave);
                foreach (byte data in BitConverter.GetBytes(note.Tick))
                    msg[i++] = data;
            }
            //Envoie
            try
            {
                //_serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                _serialPort.Write(msg, 0, msg.Length);
            }
            catch (TimeoutException e)
            {
                ;
            }
        }

        public void SendMessage(SendTypeMessage typeMessage)
        {
            byte[] msg = HeaderMessage(0, (byte)typeMessage);

            try
            {
                _serialPort.DiscardOutBuffer();
                _serialPort.DiscardInBuffer();
                _serialPort.Write(msg, 0, msg.Length);
            }
            catch (TimeoutException e)
            {
                ;
            }
        }

        public byte[] HeaderMessage(ushort dataSize, byte type)
        {
            byte[] header = new byte[SizeHeadMessage];
            header[0] = StartByte;
            header[1] = _numMessage++;
            header[2] = type;
            header[3] = BitConverter.GetBytes(dataSize)[0];
            header[4] = BitConverter.GetBytes(dataSize)[1];
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
