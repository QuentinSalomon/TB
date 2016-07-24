using Concept.Model.Wpf;
using Framework;
using System.Windows;

namespace Xylobot
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

        //protected override void LoadCustomSkins()
        //{
        //    base.LoadCustomSkins();
        //    LoadSkin();
        //}

        //private static void LoadSkin()
        //{
        //    ConceptStyle.AddSkin("ConceptSkin.Chrome", "Flat Blue");

        //    ConceptStyle.ApplySkin("ConceptSkin.Chrome");
        //}
    }
}
