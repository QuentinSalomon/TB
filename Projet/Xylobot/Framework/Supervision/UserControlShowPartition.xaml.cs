using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UserControlEditPartition.xaml
    /// </summary>
    public partial class UserControlShowPartition : UserControl
    {
        Brush[] ColorChannel = { Brushes.Blue, Brushes.Red, Brushes.Green, Brushes.Yellow, Brushes.Violet, Brushes.Turquoise, Brushes.Orange };
        public static readonly int[] idxBlackNote = new int[] 
        {1,3,6,8,10};
        public static readonly string[] tabNote = new string[]
        { "DO", "DO#", "RE", "RE#", "MI", "FA", "FA#", "SOL", "SOL#", "LA", "LA#", "SI" };
        double rectangleNoteSize = 15;
        double factorSpaceNote = 2;

        public double KeyWidth { get { return columnKeys.ActualWidth; } set { columnKeys.Width = new GridLength(value); } }

        public int OctaveNumber { get; set; }

        #region Public Methods

        public UserControlShowPartition()
        {
            InitializeComponent();
        }

        public void InitKeys()
        {
            const int whiteKeysPerOctave = 7;
            int whiteNoteCount = 0;
            int numberKey = (int)(CanvasNotes.Height / rectangleNoteSize);

            CanvasKeys.Width = KeyWidth;

            for (int k = 0; k < numberKey; k++)
            {
                int idxNote = k % Xylobot.octaveSize;
                bool blackNote = false;

                for (int i = 0; i < idxBlackNote.Length; i++)
                    if (idxNote == idxBlackNote[i])
                        blackNote = true;

                Rectangle r = new Rectangle();
                r.Stroke = Brushes.Black;
                r.StrokeThickness = 0.3;
                r.Visibility = Visibility.Visible;
                r.Name = "Key" + k.ToString();
                if (blackNote)
                {
                    r.Width = KeyWidth * 0.6;
                    r.Height = rectangleNoteSize;
                    r.Fill = Brushes.Black;
                    CanvasKeys.Children.Add(r);
                    Canvas.SetLeft(r, 0);
                    Canvas.SetBottom(r, k * rectangleNoteSize);
                    Canvas.SetZIndex(r, 3);
                }
                else {
                    r.Width = KeyWidth;
                    r.Height = rectangleNoteSize * (double)Xylobot.octaveSize / whiteKeysPerOctave;
                    r.Fill = Brushes.White;
                    CanvasKeys.Children.Add(r);
                    Canvas.SetLeft(r, 0);
                    Canvas.SetBottom(r, whiteNoteCount * rectangleNoteSize * Xylobot.octaveSize / whiteKeysPerOctave);
                    Canvas.SetZIndex(r, 2);
                }

                TextBlock txtB = new TextBlock();
                txtB.Text = tabNote[idxNote];
                if (idxNote == 0)
                {
                    txtB.Text += ' ' + (k / Xylobot.octaveSize + Xylobot.startOctaveXylophone).ToString();
                    txtB.FontWeight = FontWeights.Bold;
                }
                txtB.Height = rectangleNoteSize;
                txtB.FontSize = rectangleNoteSize * 0.9;
                txtB.VerticalAlignment = VerticalAlignment.Center;
                txtB.Visibility = Visibility.Visible;
                CanvasKeys.Children.Add(txtB);
                Canvas.SetZIndex(txtB, 3);
                if (blackNote)
                {
                    txtB.Foreground = Brushes.White;
                    Canvas.SetLeft(txtB, 5);
                    Canvas.SetBottom(txtB, k * rectangleNoteSize + 2);
                }
                else
                {
                    txtB.Foreground = Brushes.Black;
                    Canvas.SetRight(txtB, 5);
                    Canvas.SetBottom(txtB, whiteNoteCount * rectangleNoteSize * Xylobot.octaveSize / whiteKeysPerOctave + 2);
                }

                whiteNoteCount += (blackNote ? 0 : 1);
                if (k == numberKey - 1)
                    r.Height = rectangleNoteSize;
            }
        }

        public void ClearKeys()
        {
            List<UIElement> listToClear = new List<UIElement>();
            foreach (UIElement o in CanvasKeys.Children)
                listToClear.Add(o);
            for (int i = 0; i < listToClear.Count; i++)
                CanvasKeys.Children.Remove(listToClear[i]);
        }

        public void InitNotesView()
        {
            for (int k = 0; k < OctaveNumber+1; k++)
            {
                Line lineOctave = new Line();
                lineOctave.X2 = 100000;//double.MaxValue;
                lineOctave.StrokeThickness = 2;
                lineOctave.Stroke = Brushes.Blue;
                lineOctave.StrokeDashArray = new DoubleCollection(new double[] { 4, 3 });
                lineOctave.Visibility = Visibility.Visible;
                CanvasNotes.Children.Add(lineOctave);
                Canvas.SetLeft(lineOctave, 0);
                Canvas.SetBottom(lineOctave, k * Xylobot.octaveSize * rectangleNoteSize - 1);
            }

            Binding bindingHeihtLine = new Binding();
            Binding bnd = new Binding("ActualHeight") { ElementName = "ScrollViewerNotes" };
            LineRed.X1 = ScrollViewerNotes.ActualWidth * 0.8;
            LineRed.X2 = ScrollViewerNotes.ActualWidth * 0.8;
            LineRed.Y1 = 0;
            BindingOperations.SetBinding(LineRed, Line.Y2Property, bnd);
        }

        public void ShowPartition()
        {
            ReleaseDrawPartition();

            if ((DataContext as Sequencer).CurrentPartition != null)
            {
                int maxTick = 0;
                foreach (Note note in (DataContext as Sequencer).CurrentPartition.Notes)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = rectangleNoteSize;
                    rect.Height = rectangleNoteSize;
                    rect.Fill = Brushes.ForestGreen;
                    rect.Stroke = Brushes.Black;
                    rect.StrokeThickness = 0.3;
                    rect.DataContext = note;
                    rect.Visibility = Visibility.Visible;
                    CanvasNotes.Children.Add(rect);
                    Canvas.SetLeft(rect, note.Tick / factorSpaceNote + LineRed.X1);
                    Canvas.SetBottom(rect, ((note.Octave - Xylobot.startOctaveXylophone) * Xylobot.octaveSize + note.High) * rectangleNoteSize);

                    maxTick = maxTick < note.Tick ? note.Tick : maxTick;
                }
                CanvasNotes.Width = maxTick / factorSpaceNote + 50;
            }
        }

        public void ReleaseDrawPartition()
        {
            List<Rectangle> ChildsToRemove = new List<Rectangle>();
            foreach (var o in CanvasNotes.Children)
            {
                if (o is Rectangle)
                    ChildsToRemove.Add((Rectangle)o);
            }
            for (int i = 0; i < ChildsToRemove.Count; i++)
            {
                CanvasNotes.Children.Remove(ChildsToRemove[i]);
            }
        }

        public void ScrollPartition()
        {
            ScrollViewerNotes.ScrollToHorizontalOffset(((Sequencer)DataContext).PartitionProgress * CanvasNotes.ActualWidth);
        }

        #endregion

        #region private Methods

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender == ScrollViewerKeys)
            {
                ScrollViewerNotes.ScrollToVerticalOffset(e.VerticalOffset);
                ScrollViewerNotes.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
            if (sender == ScrollViewerNotes)
            {
                ScrollViewerKeys.ScrollToVerticalOffset(e.VerticalOffset);
                ScrollViewerKeys.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rectangleNoteSize = ScrollViewerKeys.ActualHeight / Xylobot.numberKeysXylophone;
            CanvasNotes.Height = ScrollViewerKeys.ActualHeight;
            CanvasKeys.Height = ScrollViewerKeys.ActualHeight;

            ClearKeys();
            InitKeys();

            InitNotesView();

            ((Sequencer)DataContext).PropertyChanged += DataContextPropertyChangedEventHandler;
        }
        
        private void DataContextPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Sequencer.CurrentPartitionPropertyName)
                ShowPartition();
            else if (e.PropertyName == Sequencer.PartitionProgressPropertyName)
                ScrollPartition();
        }

        #endregion
    }
}
