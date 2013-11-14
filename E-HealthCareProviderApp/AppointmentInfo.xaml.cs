using EHealthCareDataAccess;
using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace E_HealthCareProviderApp
{
    /// <summary>
    /// Interaction logic for WeightInfo.xaml
    /// </summary>
    public partial class AppointmentInfo : MetroWindow
    {
        public AppointmentInfo()
        {
            InitializeComponent();
        }

        public int PatientId { get; set; }

        public Guid UniqueIdentifier { get; set; }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            LoadAppointments();
        }
        
        private void LoadAppointments()
        {
            try
            {
                var appointmentDataRepository = new AppointmentDataRepository(UniqueIdentifier);
                listViewAppointments.ItemsSource = appointmentDataRepository.GetAllAppointmentData(PatientId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            var id = int.Parse(((System.Windows.Controls.Button)sender).CommandParameter.ToString());
            UpdateStatus(id, "Confirmed");
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            var id = int.Parse(((System.Windows.Controls.Button)sender).CommandParameter.ToString());
            UpdateStatus(id, "Canceled");
        }

        private void UpdateStatus(int id, string status)
        {
            try
            {
                var appointmentDataRepository = new AppointmentDataRepository(UniqueIdentifier);
                appointmentDataRepository.UpdateAppointmentStatus(id, PatientId, status);

                // Reload appointments
                LoadAppointments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
