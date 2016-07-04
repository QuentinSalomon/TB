using Common;
using Concept.Model;
using Concept.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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

        public static readonly int octaveSize = 12, startOctaveXylophone = 5, numberKeysXylophone = 37;
        public static readonly double defaultTimeHitKey = 9.5;

        #region Propriétés

        public bool IsInit { get; set; }

        public bool AbortInit { private get; set; }

        public UInt32 ArduinoCurrentTick { get { return XyloCommunication.ArduinoCurrentTick; } }
        public byte ArduinoNoteSizeAvaible { get { return XyloCommunication.ArduinoNoteSizeAvaible; } }

        [ConceptViewVisible(false)]
        public XyloCommunication XyloCommunication { get; set; }

        #endregion

        #region Methods

        public bool Init()
        {
            int i = 0;
            Application.Current.Dispatcher.Invoke(new Action(() => AbortInit = XyloCommunication.SetPortName() != true));
            while (!IsInit && !AbortInit && i++ <= 10)
            {
                try
                {
                    XyloCommunication.Init();
                    IsInit = true;
                }
                catch (Exception)
                {
                    continue;
                }

                Thread.Sleep(1000);
            }
            return IsInit;
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
                datas.Add((byte)(note.High + (note.Octave - startOctaveXylophone) * octaveSize));
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

        public void SendSpeedFactor(double speedFactor)
        {
            List<byte> datas = new List<byte>();
            int tmpSpeedFactor = (int)(speedFactor * 100);
            datas.Add((byte)(tmpSpeedFactor / 100));
            datas.Add((byte)(tmpSpeedFactor % 100));

            XyloCommunication.SendDatas(SendTypeMessage.SpeedFactor, datas);
        }

        public void SendKeyHitTime(int index, double hitTime)
        {
            List<byte> datas = new List<byte>();
            int tmpHitTime = (int)(hitTime * 100);
            datas.Add((byte)index);
            datas.Add((byte)(tmpHitTime / 100));
            datas.Add((byte)(tmpHitTime % 100));

            XyloCommunication.SendDatas(SendTypeMessage.KeyHitTime, datas);
        }

        #endregion
    }
}
