using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Framework
{
    /// <summary>
    /// Interaction logic for UserControlCodeSettings.xaml
    /// </summary>
    public partial class UserControlCodeSettings : UserControl, INotifyPropertyChanged
    {
        #region Property Change

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        public UserControlCodeSettings()
        {
            InitializeComponent();
            PasswordRight = false;
            _indexWrite = 0;
            Values = new int[_passwordLenght];
            Code = new int[_passwordLenght];
            for (int i = 0; i < _passwordLenght; i++)
                Code[i] = i;
            Reset();
        }

        public int[] Code { get; set; }

        public bool PasswordRight {
            get { return _passwordRight; }
            set
            {
                if (_passwordRight != value)
                {
                    _passwordRight = value;
                    if (value)
                        Visibility =  Visibility.Collapsed;
                    else
                        Visibility = Visibility.Visible;
                    OnPropertyChanged(nameof(PasswordRight));
                }
            }
        }
        private bool _passwordRight;

        public int[] Values { get; set; }

        private int _indexWrite;

        private const int _passwordLenght = 4;

        public void Reset()
        {
            for (int i = 0; i < _passwordLenght; i++)
                Values[i] = -1;
            PasswordRight = false;
        }

        private void ButtonRed_Click(object sender, RoutedEventArgs e)
        {
            Values[_indexWrite] = 0;
            if (++_indexWrite == _passwordLenght)
                _indexWrite = 0;
        }

        private void ButtonGreen_Click(object sender, RoutedEventArgs e)
        {
            Values[_indexWrite] = 1;
            if (++_indexWrite == _passwordLenght)
                _indexWrite = 0;
        }

        private void ButtonBlue_Click(object sender, RoutedEventArgs e)
        {
            Values[_indexWrite] = 2;
            if (++_indexWrite == _passwordLenght)
                _indexWrite = 0;
        }

        private void ButtonOrange_Click(object sender, RoutedEventArgs e)
        {
            Values[_indexWrite] = 3;
            if (++_indexWrite == _passwordLenght)
                _indexWrite = 0;
            CheckPassword();
        }

        private void CheckPassword()
        {
            bool check = true;
            int i = _indexWrite;
            for (int j = 0; j < _passwordLenght; j++, i++)
            {
                if (i == _passwordLenght)
                    i = 0;
                if (Code[j] != Values[i])
                    check = false;
            }
            PasswordRight = check;
        }
    }
}
