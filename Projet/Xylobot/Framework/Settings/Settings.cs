using Concept.Model;
using Concept.Utils.Wpf;
using System;

namespace Framework
{
    [IntlConceptName("Framework.Settings.Name", "Settings")]
    public class Settings : ConceptComponent
    {
        public Settings()
        {
            //Initialisation des 37 touches
            for (int i = 0; i < Xylobot.numberKeysXylophone; i++)
            {
                Key tmp = new Key(Xylobot.defaultTimeHitKey);
                tmp.Name = "key" + i.ToString();
                Keys.Add(tmp);
            }
            NeedSaved = false;
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
                    if(_defaultPathLoadFile != null)
                        NeedSaved = true;
                    _defaultPathLoadFile = value;
                    DoPropertyChanged(DefaultPathLoadFilePropertyName);
                }
            }
        }
        private string _defaultPathLoadFile;
        public const string DefaultPathLoadFilePropertyName = "DefaultPathLoadFile";


        [ConceptSerialized]
        [ConceptAutoCreate]
        [IntlConceptName("Framework.Settings.Keys", "Keys")]
        public StaticListKey Keys { get; protected set; }

        public void ChangeKey(int index, double hitTime)
        {
            Keys[index].HitTime = hitTime;
            NeedSaved = true;
        }

        public bool NeedSaved
        {
            get { return _needSaved; }
            set
            {
                if (_needSaved != value)
                {
                    _needSaved = value;
                    DoPropertyChanged(NeedSavedPropertyName);
                }
            }
        }
        private bool _needSaved;
        public const string NeedSavedPropertyName = "NeedSaved";

        #endregion

        #region WPF command

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
