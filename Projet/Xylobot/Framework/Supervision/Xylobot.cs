using Common;
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
            XyloCommunication = new XyloCommunication(this);
        }

        #endregion

        #region Propriétés



        [ConceptViewVisible(true)]
        [ConceptDefaultValue("...")]
        [IntlConceptName("Framework.Xylbot.Test", "Test")]
        public string Test
        {
            get { return _test; }
            set
            {
                if (_test != value)
                {
                    _test = value;
                    DoPropertyChanged(TestPropertyName);
                }
            }
        }
        private string _test;
        public const string TestPropertyName = "Test";

        public bool IsInit { get; set; }

        public UInt32 ArduinoCurrentTick { get { return XyloCommunication.ArduinoCurrentTick; } }
        public byte ArduinoNoteSizeAvaible { get { return XyloCommunication.ArduinoNoteSizeAvaible; } }

        [ConceptViewVisible(false)]
        public XyloCommunication XyloCommunication { get; set; }

        #endregion

        #region Methods

        public void Init()
        {
            XyloCommunication.Init();
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
            XyloCommunication.SendNotes(notes);
        }

        #endregion
    }
}
