using Concept.Model.Wpf;
using Concept.Utils.Wpf;
using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MidiEditToXML
{
    public class Bootstrapper : ConceptBootstrapper
    {
        protected override void Load()
        {
            base.Load();
            FrameworkController.Instance.Load();
        }

        protected override void Unload()
        {
            base.Unload();
            FrameworkController.Instance.Unload();
        }

        protected override Window CreateMainWindow()
        {
            return new MainWindow() { DataContext = new MainViewModel() };
        }
    }
}
