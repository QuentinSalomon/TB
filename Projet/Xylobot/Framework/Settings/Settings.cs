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
    [IntlConceptName("Framework.Settings.Name", "Settings")]
    [ConceptSmallImage(typeof(Settings), "/Images/Settings32x32.png")]
    [ConceptLargeImage(typeof(Settings), "/Images/Settings64x64.png")]
    public class Settings :ConceptComponent
    {
        public Settings()
        {
            for (int i = 0; i < Xylobot.numberKeysXylophone; i++)
            {
                Key tmp = new Key(Xylobot.defaultTimeHitKey);
                tmp.Name = "key" + i.ToString();
                Keys.Add(tmp);
            }
        }

        #region Propriétés

        [ConceptSerialized]
        [ConceptViewVisible]
        [IntlConceptName("Framework.Settings.DefaultPathLoadFile", "DefaultPathLoadFile")]
        public string DefaultPathLoadFile
        {
            get { return _defaultPathLoadFile; }
            set
            {
                if (_defaultPathLoadFile != value)
                {
                    _defaultPathLoadFile = value;
                    DoPropertyChanged(DefaultPathLoadFilePropertyName);
                }
            }
        }
        private string _defaultPathLoadFile;
        public const string DefaultPathLoadFilePropertyName = "DefaultPathLoadFile";

        [ConceptSerialized]
        [ConceptViewVisible]
        [IntlConceptName("Framework.Settings.PathSaveFile", "PathSaveFile")]
        public string PathSaveFile
        {
            get { return _pathSaveFile; }
            set
            {
                if (_pathSaveFile != value)
                {
                    _pathSaveFile = value;
                    DoPropertyChanged(PathSaveFilePropertyName);
                }
            }
        }
        private string _pathSaveFile;
        public const string PathSaveFilePropertyName = "PathSaveFile";

        [ConceptSerialized]
        [ConceptAutoCreate]
        [IntlConceptName("Common.PartitionXylo.Keys", "Keys")]
        public StaticListKey Keys { get; protected set; }

        #endregion

        #region WPF command

        public WpfCommand CommandPathSaveFile
        {
            get
            {
                if (_commandPathSaveFile == null)
                {
                    _commandPathSaveFile = new WpfCommand();
                    _commandPathSaveFile.Executed += (sender, e) =>
                    {
                        System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
                        // Do not allow the user to create new files via the FolderBrowserDialog.
                        dlg.ShowNewFolderButton = false;
                        
                        dlg.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            PathSaveFile = dlg.SelectedPath;
                        }
                    };

                    _commandPathSaveFile.CanExecuteChecking += (sender, e) =>
                    {
                        e.CanExecute = true;
                    };
                }
                return _commandPathSaveFile;
            }
        }
        private WpfCommand _commandPathSaveFile;

        public WpfCommand CommandPathLoadFile
        {
            get
            {
                if (_commandPathLoadFile == null)
                {
                    _commandPathLoadFile = new WpfCommand();
                    _commandPathLoadFile.Executed += (sender, e) =>
                    {
                        System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
                        // Do not allow the user to create new files via the FolderBrowserDialog.
                        dlg.ShowNewFolderButton = false;

                        // Default to the My Documents folder.
                        //dlg.RootFolder = Environment.SpecialFolder.Personal;
                        dlg.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            DefaultPathLoadFile = dlg.SelectedPath;
                        }
                    };

                    _commandPathLoadFile.CanExecuteChecking += (sender, e) =>
                    {
                        e.CanExecute = true;
                    };
                }
                return _commandPathLoadFile;
            }
        }
        private WpfCommand _commandPathLoadFile;

        #endregion
    }

    [IntlConceptName("Framework.Key.Name", "Key")]
    [ConceptSmallImage(typeof(Key), "/Images/Keys32x32.png")]
    [ConceptLargeImage(typeof(Key), "/Images/Keys64x64.png")]
    public class Key : ConceptComponent
    {
        public Key()
        {
        }

        public Key(double value)
        {
            HitTime = value;
        }

        [ConceptSerialized]
        [ConceptViewVisible]
        [IntlConceptName("Framework.DoubleConcept.HitTime", "HitTime")]
        public double HitTime
        {
            get { return _hitTime; }
            set
            {
                if (_hitTime != value)
                {
                    _hitTime = value;
                    DoPropertyChanged(HitTimePropertyName);
                }
            }
        }
        private double _hitTime;
        public const string HitTimePropertyName = "HitTime";
    }

    [IntlConceptName("Framework.Key.Name", "Key")]
    [ConceptSmallImage(typeof(StaticListKey), "/Images/Keys32x32.png")]
    [ConceptLargeImage(typeof(StaticListKey), "/Images/Keys64x64.png")]
    public class StaticListKey : ConceptStaticList<Key>
    { }
}
