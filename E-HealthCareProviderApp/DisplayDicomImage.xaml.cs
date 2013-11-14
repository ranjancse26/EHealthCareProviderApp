using Dicom;
using Dicom.Imaging;
using MahApps.Metro.Controls;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace E_HealthCareProviderApp
{
    /// <summary>
    /// Interaction logic for DisplayDicomImage.xaml
    /// </summary>
    public partial class DisplayDicomImage : MetroWindow
    {
        private readonly System.Drawing.Image Image;
        private UIElement last;

        public DisplayDicomImage(System.Drawing.Image dicomImage)
        {
            InitializeComponent();
            this.Image = dicomImage;
        }

        private void WindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            canvas.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(image_ManipulationStarting);
            canvas.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(image_ManipulationDelta);
            canvas.ManipulationInertiaStarting += new EventHandler<ManipulationInertiaStartingEventArgs>(canvas_ManipulationInertiaStarting);
    
            ThreadPool.QueueUserWorkItem(delegate(object s)
            {
                Dispatcher.Invoke((Action)(()=> {
                    DisplayImage();
                }));
            });

            double height = SystemParameters.WorkArea.Height;
            double width = SystemParameters.WorkArea.Width;

            dicomImage.Margin = new Thickness(width/2 - 300, 30, 0, 0);
         }
        
        protected void DisplayImage()
        {
            try
            {
                System.Drawing.Image bmp = Image;
                MemoryStream strm = new MemoryStream();
                bmp.Save(strm, System.Drawing.Imaging.ImageFormat.Bmp);

                strm.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.EndInit();

                dicomImage.Source = bi;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Problem in displaying Dicom Image. More details - {0}", e.Message));
            }
        }
        
        void canvas_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 DIPS per inch / 1000ms^2)
            e.TranslationBehavior = new InertiaTranslationBehavior()
            {
                InitialVelocity = e.InitialVelocities.LinearVelocity,
                DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0)
            };

            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 DIPS per inch / (1000ms^2)
            e.ExpansionBehavior = new InertiaExpansionBehavior()
            {
                InitialVelocity = e.InitialVelocities.ExpansionVelocity,
                DesiredDeceleration = 0.1 * 96 / 1000.0 * 1000.0
            };

            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior = new InertiaRotationBehavior()
            {
                InitialVelocity = e.InitialVelocities.AngularVelocity,
                DesiredDeceleration = 720 / (1000.0 * 1000.0)
            };
            e.Handled = true;
        }

        void image_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;
            if (uie != null)
            {
                if (last != null) Canvas.SetZIndex(last, 0);
                Canvas.SetZIndex(uie, 2);
                last = uie;
            }

            e.ManipulationContainer = canvas;
            e.Handled = true;
        }

        void image_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            var element = e.Source as FrameworkElement;
            if (element != null)
            {
                var deltaManipulation = e.DeltaManipulation;
                var matrix = ((MatrixTransform)element.RenderTransform).Matrix;
                System.Windows.Point center = new System.Windows.Point(element.ActualWidth / 2, element.ActualHeight / 2);
                center = matrix.Transform(center);
                matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
                matrix.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);
                matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);

                ((MatrixTransform)element.RenderTransform).Matrix = matrix;
                e.Handled = true;

                if (e.IsInertial)
                {
                    Rect containingRect = new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);
                    Rect shapeBounds = element.RenderTransform.TransformBounds(new Rect(element.RenderSize));
                    if (e.IsInertial && !containingRect.Contains(shapeBounds))
                    {
                        e.ReportBoundaryFeedback(e.DeltaManipulation);
                        e.Complete();
                    }
                }
            }
        }
    }
}
