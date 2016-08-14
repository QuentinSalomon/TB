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
    public partial class EditPartitionView : UserControl, INotifyPropertyChanged
    {
        Brush[] ColorChannel = { Brushes.Blue, Brushes.Red, Brushes.Green, Brushes.Yellow, Brushes.Violet, Brushes.Turquoise, Brushes.Orange };

        const int rectangleNoteSize = 15;

        public EditPartitionView()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(() =>
            {
                int normalNoteCount = 0;
                for (int k = 0; k < CanvasNotes.Height / rectangleNoteSize; k++)
                {
                    int idxNote = k % NotesConvert.octaveSize;

                    Line l = new Line();
                    l.X2 = 100000;//double.MaxValue;
                    l.StrokeThickness = idxNote == 0 ? 2 : 1;
                    l.Stroke = idxNote == 0 ? Brushes.ForestGreen : Brushes.LightBlue;
                    l.StrokeDashArray = new DoubleCollection(new double[] { 4, 3 });
                    l.Visibility = Visibility.Visible;
                    CanvasNotes.Children.Add(l);
                    Canvas.SetLeft(l, 0);
                    Canvas.SetBottom(l, k * rectangleNoteSize - (idxNote == 0 ? 1 : 0.5));

                    bool blackNote = false;

                    for (int i = 0; i < NotesConvert.idxBlackNote.Length; i++)
                        if (idxNote == NotesConvert.idxBlackNote[i])
                            blackNote = true;

                    Rectangle r = new Rectangle();
                    r.Height = rectangleNoteSize * (blackNote ? 1 : (double)12/7);
                    r.Width = 100 * (blackNote ? 0.6 : 1);
                    r.Fill = blackNote ? Brushes.Black : Brushes.White;
                    r.Stroke = Brushes.Black;
                    r.StrokeThickness = 0.3;
                    r.Visibility = Visibility.Visible;
                    CanvasNotesHigh.Children.Add(r);
                    Canvas.SetLeft(r, 0);
                    if (blackNote)
                    {
                        Canvas.SetBottom(r, k * rectangleNoteSize);
                        Canvas.SetZIndex(r, 3);
                    }
                    else {
                        Canvas.SetBottom(r, normalNoteCount * rectangleNoteSize * 12/7);
                        Canvas.SetZIndex(r, 2);
                    }

                    TextBlock txtB = new TextBlock();
                    txtB.Text = NotesConvert.tabNote[idxNote];
                    if(idxNote == 0)
                    {
                        txtB.Text += ' ' + (k / NotesConvert.octaveSize).ToString();
                        txtB.FontWeight = FontWeights.Bold;
                    }
                    txtB.Height = rectangleNoteSize;
                    txtB.FontSize = rectangleNoteSize - 1;
                    txtB.VerticalAlignment = VerticalAlignment.Center;
                    txtB.Foreground = blackNote ? Brushes.White : Brushes.Black;
                    txtB.Visibility = Visibility.Visible;
                    CanvasNotesHigh.Children.Add(txtB);
                    Canvas.SetZIndex(txtB, 3);
                    if (blackNote)
                    {
                        Canvas.SetLeft(txtB, 0);
                        Canvas.SetBottom(txtB, k * rectangleNoteSize);
                    }
                    else
                    {
                        Canvas.SetRight(txtB, 0);
                        Canvas.SetBottom(txtB, normalNoteCount * rectangleNoteSize * 12/7);
                    }

                    normalNoteCount += (blackNote ? 0 : 1);
                }
                for (int k = 0; k < CanvasNotes.Height / rectangleNoteSize / NotesConvert.octaveSize; k++)
                {

                }
            }), null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void DoPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public PartitionMidi CurrentPartition { get; set; }

        public double KeyWidth { get { return columnKeys.ActualWidth; } set { columnKeys.Width = new GridLength(value); } }

        public int OctaveNumber {
            get { return (int)((CanvasNotes.ActualHeight / rectangleNoteSize - 1) / NotesConvert.octaveSize); }
            set {
                CanvasNotes.Height = (value * NotesConvert.octaveSize + 1) * rectangleNoteSize;
                CanvasNotesHigh.Height = (value * NotesConvert.octaveSize + 1) * rectangleNoteSize;
            }
        }

        public int StartOctave { get; set; }

        public Note CurrentNote
        {
            get { return _currentNote; }
            set
            {
                if (_currentNote != value)
                {
                    _currentNote = value;
                    DoPropertyChanged(CurrentNotePropertyName);
                }
            }
        }
        private Note _currentNote;
        public const string CurrentNotePropertyName = "CurrentNote";

        public Channel CurrentChannel { get; set; }

        public void ShowPartition()
        {
            ReleaseDrawPartition();

            int i = 0, maxTick = 0;
            foreach (Channel ch in CurrentPartition.Channels)
            {
                Rectangle rectChannel = new Rectangle();
                rectChannel.Width = rectangleNoteSize;
                rectChannel.Height = rectangleNoteSize;
                rectChannel.Fill = ColorChannel[i%7];
                rectChannel.DataContext = ch;
                rectChannel.Visibility = Visibility.Visible;
                rectChannel.HorizontalAlignment = HorizontalAlignment.Center;
                rectChannel.Margin = new Thickness(0, 2, 0, 3);
                rectChannel.MouseLeftButtonDown += R_MouseLeftButtonDownRectangleChannel;
                StackPanelChannelsColor.Children.Add(rectChannel);

                TextBlock txtB = new TextBlock();
                txtB.Text = "Channel" + i.ToString();
                txtB.Height = rectangleNoteSize+5;
                txtB.VerticalAlignment = VerticalAlignment.Center;
                StackPanelChannelsName.Children.Add(txtB);

                foreach (Note note in ch.Notes)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = rectangleNoteSize;
                    rect.Height = rectangleNoteSize;
                    rect.Fill = ColorChannel[i];
                    rect.Stroke = Brushes.Black;
                    rect.StrokeThickness = 0.3;
                    rect.DataContext = note;
                    rect.Visibility = Visibility.Visible;
                    CanvasNotes.Children.Add(rect);
                    Canvas.SetLeft(rect, note.Tick / 10);
                    Canvas.SetBottom(rect, (note.Octave * NotesConvert.octaveSize + note.High) * rectangleNoteSize);
                    rect.MouseLeftButtonDown += R_MouseLeftButtonDownRectangleNote;

                    maxTick = maxTick < note.Tick ? note.Tick : maxTick;
                }
                i++;
            }
            CanvasNotes.Width = maxTick / 10 + 50;
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
            StackPanelChannelsColor.Children.Clear();
            StackPanelChannelsName.Children.Clear();
        }

        private void R_MouseLeftButtonDownRectangleNote(object sender, MouseButtonEventArgs e)
        {
            CurrentNote = (sender as Rectangle).DataContext as Note;
        }

        private void R_MouseLeftButtonDownRectangleChannel(object sender, MouseButtonEventArgs e)
        {

        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender == ScrollViewer1)
            {
                ScrollViewer2.ScrollToVerticalOffset(e.VerticalOffset);
                ScrollViewer2.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
            if (sender == ScrollViewer2)
            {
                ScrollViewer1.ScrollToVerticalOffset(e.VerticalOffset);
                ScrollViewer1.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }
    }
}
