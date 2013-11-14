using Dicom;
using Dicom.Imaging;
using EHealthCareDataAccess;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace E_HealthCareProviderApp
{
    /// <summary>
    /// Interaction logic for ManagePatientDicom.xaml
    /// </summary>
    public partial class ManagePatientDicom : MetroWindow
    {
        DicomFile file = null;
               
        public ManagePatientDicom()
        {
            InitializeComponent();
        }

        public int PatientId { get; set; }

        public Guid UniqueIdentifier { get; set; }
        
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            LoadDicom();
        }

        private void LoadDicom()
        {
            try
            {
                var dicomRepository = new DicomDataRepository(UniqueIdentifier);
                var patientDicom = dicomRepository.GetAllDicom(this.PatientId);
                listView.ItemsSource = patientDicom;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void OpenFile(string fileName)
        {
            try
            {
                file = DicomFile.Open(fileName);
            }
            catch (DicomFileException ex)
            {
                file = ex.File;
            }
       }

        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "DICOM Files (*.dcm;*.dic)|*.dcm;*.dic|All Files (*.*)|*.*";

            ofd.ShowDialog();

            if (ofd.FileName == "") return;

            txtFilePath.Text = ofd.FileName;
            OpenFile(ofd.FileName);
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        private void UploadButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dicomRepository = new DicomDataRepository(UniqueIdentifier);
                var dicom = new EHealthCareDataAccess.Dicom();
                var image = new DicomImage(file.Dataset);
                dicom.DateTime = DateTime.Now;
                dicom.Dicom1 = ImageToByteArray(image.RenderImage());
                dicom.PatientId = this.PatientId;
                dicom.UniqueIdentifier = this.UniqueIdentifier;
                dicom.Subject = txtSubject.Text.Trim();
                dicom.ProviderId = E_HealthCareProviderApp.Properties.Settings.Default.ProviderId;
                dicomRepository.SaveDicom(dicom);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // Re Load List View
            LoadDicom();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            var id = int.Parse(((Button)sender).CommandParameter.ToString());
            var dicomRepository = new DicomDataRepository(UniqueIdentifier);
            dicomRepository.DeleteDicom(this.PatientId, id);
            LoadDicom();
        }

        public System.Drawing.Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        private void ViewDicomButtonClick(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).CommandParameter == null) return;

            var id = int.Parse(((Button)sender).CommandParameter.ToString());
            var dicomRepository = new DicomDataRepository(UniqueIdentifier);

            try
            {
                var dicom = dicomRepository.GetDicomById(this.PatientId, id);
                var displayDicomImage = new DisplayDicomImage(ByteArrayToImage(dicom.Dicom1));
                displayDicomImage.ShowDialog();
            }
            catch(Exception ex){
                MessageBox.Show(ex.ToString());
                return;
            }
        }
    }
}
