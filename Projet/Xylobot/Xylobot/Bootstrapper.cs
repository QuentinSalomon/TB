using Concept.Model.Wpf;
using Framework;
using System.Windows;
using System;

namespace Xylobot
{
    public class Bootstrapper : ConceptBootstrapper
    {
        public Bootstrapper(Window splashScreen) : base(splashScreen)
        {

        }

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
