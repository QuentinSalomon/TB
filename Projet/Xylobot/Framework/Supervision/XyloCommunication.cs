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
    
    public enum ReceiveTypeMessage : byte { Ok, TooManyData, ErrorStartByte, ErrorType, ErrorMsgArduino/*Message reçu par l'arduino mauvais*/ }

    public class XyloCommunication
    {
        #region const
        
        const byte StartByte = 255;
        // TODO : Ajuster la vitesse
        const Int32 BaudRate = 57600, SizeHeadMessage = 5, TimeOut = 3000;
        const int SizeOctave = 12, StartOctaveXylophone = 5;
        #endregion

        #region Constructor
        
        public XyloCommunication()
        {
            _numMessage = 0;
            // TODO : rendre configurable
            PortName = "COM5";
            _serialPort = new SerialPort();
            _serialPort.PortName = SetPortName();
            _serialPort.BaudRate = BaudRate;
            _serialPort.ReadTimeout = TimeOut;
            _serialPort.WriteTimeout = TimeOut;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Parity = Parity.None;
            _serialPort.Handshake = Handshake.None;
        }

        #endregion

        #region Propriétés

        public byte ArduinoNoteSizeAvaible { get; private set; }
        public UInt32 ArduinoCurrentTick { get; private set; }

        // TODO : a mettre dans un fichier de configuration
        public string PortName { get; set; }

        #endregion

        #region Methods

        public void Init()
        {
            try
            {
                _serialPort.Open();
                _serialPort.DiscardOutBuffer();
                //Stop la communication i2c et récupère la place dans le buffer circulaire de l'arduino
                SendMessage(SendTypeMessage.Stop);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ReceiveTypeMessage Read()
        {
            int tmp;
            byte[] tick = new byte[sizeof(UInt32)];
            try
            {
                tmp = _serialPort.ReadByte();
                if (tmp == StartByte)
                {
                    tmp = _serialPort.ReadByte();
                    if (tmp == _numMessage)
                    {
                        tmp = _serialPort.ReadByte();
                        ArduinoNoteSizeAvaible = (byte)_serialPort.ReadByte();
                        for(int i=0;i< sizeof(UInt32);i++)
                            tick[i] = (byte)_serialPort.ReadByte();
                        ArduinoCurrentTick = BitConverter.ToUInt32(tick, 0);
                        switch ((ReceiveTypeMessage)tmp)
                        {
                            case ReceiveTypeMessage.Ok:
                                return ReceiveTypeMessage.Ok;
                            case ReceiveTypeMessage.TooManyData:
                                return ReceiveTypeMessage.TooManyData;
                            case ReceiveTypeMessage.ErrorStartByte:
                                return ReceiveTypeMessage.ErrorStartByte;
                            case ReceiveTypeMessage.ErrorType:
                                return ReceiveTypeMessage.ErrorType;
                            default:
                                return ReceiveTypeMessage.ErrorMsgArduino;
                        }
                    } else
                        return ReceiveTypeMessage.ErrorMsgArduino; //Renvoyer le message précedent
                } else
                    return ReceiveTypeMessage.ErrorMsgArduino; //Renvoyer le message précedent

            }
            catch (TimeoutException e)
            {
                throw e;
            }
        }

        public void SendNotes(List<Note> notes)
        {
            ushort noteSize = (sizeof(UInt32) + 2*sizeof(byte)); //envoie le tick (UINT32), la hauteur(byte) et l'intensité(byte)
            if (notes.Count > ArduinoNoteSizeAvaible)
                throw new Exception("trop de notes");
            try
            {
                do
                {
                    int i = 0;
                    byte[] msg = new byte[SizeHeadMessage + notes.Count * noteSize];
                    ushort dataSize = (ushort)(notes.Count * noteSize);
                    byte[] headMsg = HeaderMessage(dataSize, (byte)SendTypeMessage.Notes);
                    for (i = 0; i < SizeHeadMessage; i++)
                        msg[i] = headMsg[i];
                    foreach (Note note in notes)
                    {
                        msg[i++] = (byte)(note.High + (note.Octave - StartOctaveXylophone) * SizeOctave);
                        foreach (byte data in BitConverter.GetBytes(note.Tick))
                            msg[i++] = data;
                        msg[i++] = note.Intensity;
                    }
                    //Envoie
                    _serialPort.DiscardOutBuffer();
                    _serialPort.Write(msg, 0, msg.Length);

                } while (Read() != ReceiveTypeMessage.Ok);
            }
            catch (TimeoutException e)
            {
                throw e;
            }
            _numMessage++;
        }

        public void SendDatas(SendTypeMessage typeMessage, List<byte> datas)
        {
            try
            {
                do
                {
                    int i = 0;
                    byte[] msg = new byte[SizeHeadMessage + datas.Count];
                    ushort dataSize = (ushort)(datas.Count);
                    byte[] headMsg = HeaderMessage(dataSize, (byte)typeMessage);
                    for (i = 0; i < SizeHeadMessage; i++)
                        msg[i] = headMsg[i];
                    foreach (byte data in datas)
                        msg[i++] = data;
                    //Envoie
                    _serialPort.DiscardOutBuffer();
                    _serialPort.Write(msg, 0, msg.Length);

                } while (Read() != ReceiveTypeMessage.Ok);
            }
            catch (TimeoutException e)
            {
                throw e;
            }
            _numMessage++;
        }

        public void SendMessage(SendTypeMessage typeMessage)
        {
            byte[] msg = HeaderMessage(0, (byte)typeMessage);
            try
            {
                do {
                    _serialPort.DiscardOutBuffer();
                    _serialPort.DiscardInBuffer();
                    _serialPort.Write(msg, 0, msg.Length);
                } while (Read() != ReceiveTypeMessage.Ok);
            }
            catch (TimeoutException e)
            {
                throw e;
            }

            _numMessage++;
        }

        public byte[] HeaderMessage(ushort dataSize, byte type)
        {
            byte[] header = new byte[SizeHeadMessage];
            header[0] = StartByte;
            header[1] = _numMessage;
            header[2] = type;
            header[3] = BitConverter.GetBytes(dataSize)[0];
            header[4] = BitConverter.GetBytes(dataSize)[1];
            return header;
        }

        // Display Port values and prompt user to enter a port.
        public static string SetPortName()
        {
            string portName = "COM5";
            //string defaultPortName = "COM5";
            //WindowSelectUsbPort windowUsbPort = new WindowSelectUsbPort();
            //windowUsbPort.Execute(ref portName, defaultPortName);
            return portName;
        }

        #endregion

        #region Private

        private SerialPort _serialPort;
        private byte _numMessage;

        #endregion
    }
}
