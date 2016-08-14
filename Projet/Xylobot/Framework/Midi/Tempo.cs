using Concept.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class Tempo : ConceptComponent
    {
        #region Constructeur

        public Tempo() { }
        public Tempo(int tick, double tempo)
        {
            Tick = tick;
            Value = tempo;
        }

        #endregion

        public int Tick
        {
            get { return _tick; }
            set
            {
                if (_tick != value)
                {
                    _tick = value;
                    DoPropertyChanged(TickPropertyName);
                }
            }
        }
        private int _tick;
        public const string TickPropertyName = "Tick";

        public double Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    DoPropertyChanged(ValuePropertyName);
                }
            }
        }
        private double _value;
        public const string ValuePropertyName = "Value";
    }

    public class StaticListTempo : ConceptStaticList<Tempo>
    { }
}
