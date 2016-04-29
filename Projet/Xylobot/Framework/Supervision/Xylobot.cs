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
        [ConceptDefaultValue("1.0.0")]
        [IntlConceptName("Framework.Xylbot.Buffer", "Buffer Usb")]
        public string Buffer
        {
            get { return _buffer; }
            set
            {
                if (_buffer != value)
                {
                    _buffer = value;
                    DoPropertyChanged(BufferPropertyName);
                }
            }
        }
        private string _buffer;
        public const string BufferPropertyName = "Buffer";

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

        [ConceptViewVisible(false)]
        public XyloCommunication XyloCommunication { get; set; }



        #endregion

        #region Wpf Commands
        // TODO : delete
        public WpfCommand CommandGoto
        {
            get
            {
                if (_commandGoto == null)
                {
                    _commandGoto = new WpfCommand();
                    _commandGoto.Executed += (sender, e) =>
                    {
                        //RobotLink.Reference.AxisXLink.Reference.Position = PositionLink.Reference.X;
                        //RobotLink.Reference.AxisYLink.Reference.Position = PositionLink.Reference.Y;
                        //RobotLink.Reference.AxisZLink.Reference.Position = PositionLink.Reference.Z;
                    };

                    _commandGoto.CanExecuteChecking += (sender, e) =>
                    {
                        //e.CanExecute = PositionLink.ValidReference && PositionLink.Reference != null &&
                        //    RobotLink.ValidReference && RobotLink.Reference != null;
                    };
                }
                return _commandGoto;
            }
        }
        private WpfCommand _commandGoto;

        #endregion
    }
}
