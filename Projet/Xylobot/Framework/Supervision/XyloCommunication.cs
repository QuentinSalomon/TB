﻿using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace Framework
{
    public enum SendTypeMessage : byte { Tempo, Notes, Start, Stop, Pause, SpeedFactor, KeyHitTime }
    
    public enum ReceiveTypeMessage : byte { Ok, TooManyData, ErrorStartByte, ErrorType, ErrorMsgArduino/*Message reçu par l'arduino mauvais*/ }

    public class XyloCommunication
    {
        #region const
        
        const byte StartByte = 255;
        const Int32 BaudRate = 115200, SizeHeadMessage = 5, TimeOut = 3000;
        const int SizeOctave = 12, StartOctaveXylophone = 5;

        #endregion

        #region Constructor
        
        public XyloCommunication()
        {
            _numMessage = 0;
            _portName = _defaultPortName;
            _serialPort = new SerialPort();
            _serialPort.PortName = _portName;
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

        #endregion

        #region Methods

        public void Init()
        {
            try
            {
                _serialPort.Open();
                _serialPort.DiscardOutBuffer();
                //Stop la communication i2c et récupère la place dans le buffer circulaire de l'Arduino
                SendMessage(SendTypeMessage.Stop);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private ReceiveTypeMessage Read()
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

        public void SendDatas(SendTypeMessage typeMessage, List<byte> datas)
        {
            try
            {
                int countSend = 0;
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

                    if (countSend++ > 9)
                        throw new Exception("Send-receive message fail too many times.");

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
                int countSend = 0;
                do
                {
                    _serialPort.DiscardOutBuffer();
                    _serialPort.DiscardInBuffer();
                    _serialPort.Write(msg, 0, msg.Length);

                    if (countSend++ > 9)
                        throw new Exception("Send-receive message fail too many times.");
                } while (Read() != ReceiveTypeMessage.Ok);
            }
            catch (TimeoutException e)
            {
                throw e;
            }

            _numMessage++;
        }

        private byte[] HeaderMessage(ushort dataSize, byte type)
        {
            byte[] header = new byte[SizeHeadMessage];
            header[0] = StartByte;
            header[1] = _numMessage;
            header[2] = type;
            header[3] = BitConverter.GetBytes(dataSize)[0];
            header[4] = BitConverter.GetBytes(dataSize)[1];
            return header;
        }

        public bool? SetPortName()
        {
            WindowSelectUsbPort windowUsbPort = new WindowSelectUsbPort();
            windowUsbPort.Execute(ref _portName, _defaultPortName);
            if (windowUsbPort.DialogResult == true)
                _serialPort.PortName = _portName;
            return windowUsbPort.DialogResult;
        }   

        #endregion

        #region Private

        private SerialPort _serialPort;
        private string _portName;
        private const string _defaultPortName = "COM5";
        private byte _numMessage;

        #endregion
    }
}
