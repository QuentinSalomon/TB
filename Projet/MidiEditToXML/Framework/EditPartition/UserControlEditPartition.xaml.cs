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
using Sanford.Multimedia.Midi;
using System.Windows.Forms;

namespace Framework
{
    /// <summary>
    /// Interaction logic for UserControlEditPartition.xaml
    /// </summary>
    public partial class UserControlEditPartition : System.Windows.Controls.UserControl
    {
        Brush[] ColorChannel = {Brushes.Blue, Brushes.Red, Brushes.Green, Brushes.Yellow, Brushes.Violet, Brushes.Turquoise, Brushes.Black};

        const int rectangleNoteSize = 10;
        const int octaveSize = 12;

        public UserControlEditPartition()
        {
            InitializeComponent();

            for (int k = 0; k < CanvasNotes.Height / rectangleNoteSize; k++)
            {
                Line l = new Line();
                //rect.Y1 = 0;
                //rect.Y2 = 0;
                l.X2 = 100000;//double.MaxValue;
                l.StrokeThickness = k % octaveSize == 0 ? 2 : 1;
                l.Stroke = k % octaveSize == 0 ? Brushes.ForestGreen : Brushes.LightBlue;
                l.StrokeDashArray = new DoubleCollection(new double[] { 4, 3 });
                l.Visibility = Visibility.Visible;
                CanvasNotes.Children.Add(l);
                Canvas.SetLeft(l, 0);
                Canvas.SetBottom(l, k * rectangleNoteSize);
            }
        }

        public PartitionMidi CurrentPartition { get; set; }

        public Note CurrentNote { get; set; }

        public void ShowPartition()
        {
            ReleaseDrawPartition();

            int i = 0, maxTick = 0;
            foreach (Channel ch in CurrentPartition.Channels)
            {
                i++;
                foreach (Note note in ch.Notes)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = rectangleNoteSize;
                    rect.Height = rectangleNoteSize;
                    rect.Fill = ColorChannel[i];
                    rect.DataContext = note;
                    rect.Visibility = Visibility.Visible;
                    CanvasNotes.Children.Add(rect);
                    Canvas.SetLeft(rect, note.Tick / 10);
                    Canvas.SetBottom(rect, (note.Octave * octaveSize + note.High) * rectangleNoteSize);
                    rect.MouseLeftButtonDown += R_MouseLeftButtonDownRectangle;

                    maxTick = maxTick < note.Tick ? note.Tick : maxTick;
                }
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
        }

        private void R_MouseLeftButtonDownRectangle(object sender, MouseButtonEventArgs e)
        {
            CurrentNote = (sender as Rectangle).DataContext as Note;
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender == ScrollViewer1)
            {
                ScrollViewer2.ScrollToVerticalOffset(e.VerticalOffset);
                ScrollViewer2.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
            else
            {
                ScrollViewer1.ScrollToVerticalOffset(e.VerticalOffset);
                ScrollViewer1.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }
    }
}
