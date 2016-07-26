using Concept.Model.Wpf;
using Framework;
using System.Windows;
using System;

namespace Xylobot
{
    public class Bootstrapper : ConceptBootstrapper
    {
        protected override Uri SplashScreenImageUri
        {
            get
            {
                return new Uri(@"/Framework;component/Images/Xylophone2.jpg", UriKind.RelativeOrAbsolute);
            }
        }

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
