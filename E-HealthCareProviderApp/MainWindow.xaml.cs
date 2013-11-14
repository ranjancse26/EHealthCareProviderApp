using E_HealthCareProviderApp.ViewModels;
using EHealthCareDataAccess;
using MahApps.Metro.Controls;
using SmartLoginOverlayDemo2.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace E_HealthCareProviderApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public LoginViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();

            this.ViewModel = new LoginViewModel();
            this.DataContext = this.ViewModel;
        }

        private void WindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
    
        }

        private void AppointmentClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (cmbPatients.SelectedIndex > -1)
            {
                var appointmentDataRepository = new AppointmentDataRepository();

                var appointmentInfo = new AppointmentInfo();
                appointmentInfo.PatientId = int.Parse(cmbPatients.SelectedValue.ToString());
                appointmentInfo.UniqueIdentifier = appointmentDataRepository.GetPatientUniqueIdentifier(appointmentInfo.PatientId);
                appointmentInfo.ShowDialog();
            }
        }

        private void LoadPatients(object sender, System.Windows.RoutedEventArgs e)
        {
            var patientViewModel = new List<PatientViewModel>();
            var patientDataRepository = new PatientDataRepository();
            var patients = patientDataRepository.GetAllPatients();

            foreach (var patient in patients)
            {
                patientViewModel.Add(new PatientViewModel
                {
                    FullName = patient.FirstName + " " + patient.LastName,
                    Id = patient.PatientId
                });
            }

            cmbPatients.ItemsSource = patientViewModel;
            cmbPatients.DisplayMemberPath = "FullName";
            cmbPatients.SelectedValuePath = "Id";
      }

        private void UploadPatientDicomClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (cmbPatients.SelectedValue == null) return;

            var patientDataRepository = new PatientDataRepository();
            var appointmentInfo = new AppointmentInfo();

            var manageDicom = new ManagePatientDicom();
            manageDicom.PatientId = int.Parse(cmbPatients.SelectedValue.ToString());
            manageDicom.UniqueIdentifier = patientDataRepository.GetPatientUniqueIdentifier(int.Parse(cmbPatients.SelectedValue.ToString()));
            manageDicom.ShowDialog();
        }

        private void DicomViewerClick(object sender, System.Windows.RoutedEventArgs e)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\DicomViewer\\DicomViewer.exe";
            Process.Start(path);
        }
    }
}
