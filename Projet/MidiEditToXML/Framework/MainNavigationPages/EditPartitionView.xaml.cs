using System;
using System.Collections.Generic;
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
    public partial class EditPartitionView : UserControl
    {
        Brush[] ColorChannel = { Brushes.Blue, Brushes.Red, Brushes.Green, Brushes.Yellow, Brushes.Violet, Brushes.Turquoise, Brushes.Orange };

        const int rectangleNoteSize = 15;

        public EditPartitionView()
        {
            InitializeComponent();

            for (int k = 0; k < CanvasNotes.Height / rectangleNoteSize; k++)
            {
                Line l = new Line();
                l.X2 = 100000;//double.MaxValue;
                l.StrokeThickness = k % NotesConvert.octaveSize == 0 ? 2 : 1;
                l.Stroke = k % NotesConvert.octaveSize == 0 ? Brushes.ForestGreen : Brushes.LightBlue;
                l.StrokeDashArray = new DoubleCollection(new double[] { 4, 3 });
                l.Visibility = Visibility.Visible;
                CanvasNotes.Children.Add(l);
                Canvas.SetLeft(l, 0);
                Canvas.SetBottom(l, k * rectangleNoteSize - (k % NotesConvert.octaveSize == 0 ? 1 : 0.5));

                Rectangle r = new Rectangle();
                r.Height = rectangleNoteSize;
                r.Width = 100;
                r.Fill = Brushes.White;
                r.Stroke = Brushes.Black;
                r.StrokeThickness = 0.3;
                r.Visibility = Visibility.Visible;
                CanvasNotesHigh.Children.Add(r);
                Canvas.SetLeft(r, 0);
                Canvas.SetBottom(r, k * rectangleNoteSize);

                TextBlock txtB = new TextBlock();
                txtB.Text = NotesConvert.tabNote[k % NotesConvert.octaveSize];
                txtB.Height = rectangleNoteSize;
                txtB.FontSize = rectangleNoteSize-1;
                txtB.Foreground = Brushes.Black;
                txtB.Visibility = Visibility.Visible;
                CanvasNotesHigh.Children.Add(txtB);
                Canvas.SetRight(txtB, 0);
                Canvas.SetBottom(txtB, k * rectangleNoteSize);
            }
        }

        public PartitionMidi CurrentPartition { get; set; }

        public Note CurrentNote { get; set; }

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
                rectChannel.Fill = ColorChannel[i];
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
