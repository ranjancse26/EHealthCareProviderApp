using EHealthCareDataAccess;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace E_HealthCareProviderApp
{
    /// <summary>
    /// Interaction logic for PatientRegistration.xaml
    /// Validation - http://codeblitz.wordpress.com/2009/05/12/wpf-validation-summary-control/
    /// </summary>
    public partial class ProviderRegistration : MetroWindow
    {
        private int noOfErrorsOnScreen = 0;

        public ProviderRegistration()
        {
            InitializeComponent();
            this.ShowMaxRestoreButton = false;
            this.ShowMinButton = false;
            rdMale.IsChecked = true;
            errorLabel.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                noOfErrorsOnScreen++;
            else
                noOfErrorsOnScreen--;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            dtPicker.SelectedDate = DateTime.Now;
        }

        private void RegisterProviderButtonClick(object sender, RoutedEventArgs e)
        {
            if (noOfErrorsOnScreen > 0)
            {
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                return;
            }

            bool isMale = false;
            try
            {
                if (rdMale.IsChecked != null && bool.Parse(rdMale.IsChecked.ToString()))
                    isMale = true;
                var providerDataRepository = new ProviderDataRepository();
                providerDataRepository.SaveProviderRecord(new Provider
                {
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    MiddleName = txtMiddleName.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Gender = isMale,
                    DOB = dtPicker.SelectedDate.Value, 
                    EmailAddress = txtEmailAddress.Text.Trim(),
                    UserName = txtUserName.Text.Trim(),
                    Password = EncryptDecrypt.EncryptData(passWordBox.Password),
                    PhoneNumber = txtPhoneNumber.Text.Trim()
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            this.Hide();
            var mainWindow = new MainWindow();
            mainWindow.ShowDialog();
            this.Close();
        }
    }
}
