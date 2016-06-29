﻿using Common;
using Concept.Model;
using Concept.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    [IntlConceptName("Framework.Xylobot.Name", "Xylobot")]
    [ConceptSmallImage(typeof(Playlist), "/Images/Xylophone32x32.png")]
    [ConceptLargeImage(typeof(Playlist), "/Images/Xylophone64x64.png")]
    public class Xylobot : ConceptComponent
    {
        #region Constructor

        public Xylobot()
        {
            XyloCommunication = new XyloCommunication();
            IsInit = false;
            AbortInit = false;
        }

        #endregion

        public static readonly int octaveSize = 12, StartOctaveXylophone = 5;

        #region Propriétés

        public bool IsInit { get; set; }

        public bool AbortInit { private get; set; }

        public UInt32 ArduinoCurrentTick { get { return XyloCommunication.ArduinoCurrentTick; } }
        public byte ArduinoNoteSizeAvaible { get { return XyloCommunication.ArduinoNoteSizeAvaible; } }

        [ConceptViewVisible(false)]
        public XyloCommunication XyloCommunication { get; set; }

        #endregion

        #region Methods

        public void Init()
        {
            while (!IsInit && !AbortInit)
            {
                try
                {
                    XyloCommunication.Init();
                    IsInit = true;
                }
                catch (Exception)
                {
                    //IsInit = false;
                    continue;
                }
            }
        }

        public void Start()
        {
            XyloCommunication.SendMessage(SendTypeMessage.Start);
        }

        public void Stop()
        {
            XyloCommunication.SendMessage(SendTypeMessage.Stop);
        }

        public void Pause()
        {
            XyloCommunication.SendMessage(SendTypeMessage.Pause);
        }

        public void SendNotes(List<Note> notes)
        {
            if (notes.Count > ArduinoNoteSizeAvaible)
                throw new Exception("trop de notes");

            List<byte> datas = new List<byte>();
            foreach (Note note in notes)
            {
                datas.Add((byte)(note.High + (note.Octave - StartOctaveXylophone) * octaveSize));
                foreach (byte data in BitConverter.GetBytes(note.Tick))
                    datas.Add(data);
                datas.Add(note.Intensity);
            }

            XyloCommunication.SendDatas(SendTypeMessage.Notes, datas);
        }

        public void SendTempo(UInt16 tempo)
        {
            List<byte> datas = new List<byte>();
            foreach (byte data in BitConverter.GetBytes(tempo))
                datas.Add(data);

            XyloCommunication.SendDatas(SendTypeMessage.Tempo, datas);
        }

        #endregion
    }
}
