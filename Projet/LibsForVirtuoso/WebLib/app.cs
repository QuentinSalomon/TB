using System.Diagnostics;

namespace Material
{
    class Chrono
    {
        public Chrono()
        {
            Name = "Chrono";
            timer = new Stopwatch();
        }
        public void Start()
        {
            timer.Start();
            _running = true;
        }
        public void Stop()
        {
            timer.Stop();
            _running = false;
        }
       public void Reset()
        {
            timer.Reset();
            _running = false;
        }

        public bool Running
        {
            get
            {
                return _running;
            }
            set
            {
                if (value)
                {
                    _running = true;
                    Start();
                }
                else
                {
                    _running = false;
                    Stop();
                }
            }
        }

        public string Name { get; set; }
        public int millisecondes { get { return timer.Elapsed.Milliseconds; } private set {; } }
        public int secondes { get { return timer.Elapsed.Seconds; } private set {; } }

        public int minutes { get { return timer.Elapsed.Minutes; } private set {; } }

        private Stopwatch timer;
        private bool _running;
    }
}
