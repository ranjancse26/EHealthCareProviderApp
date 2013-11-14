using MahApps.Metro.Controls;
using System.Windows;

namespace E_HealthCareProviderApp
{
    /// <summary>
    /// Interaction logic for E_HealthCareProviderApp.xaml
    /// </summary>
    public partial class ProviderRegistrationOrLogon : MetroWindow
    {
        public ProviderRegistrationOrLogon()
        {
            InitializeComponent();
        }

        private void ProviderRegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var providerRegistration = new ProviderRegistration();
            var status = providerRegistration.ShowDialog();
            this.Close();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var mainWindow = new MainWindow();
            var status = mainWindow.ShowDialog();
            this.Close();
        }
    }
}
