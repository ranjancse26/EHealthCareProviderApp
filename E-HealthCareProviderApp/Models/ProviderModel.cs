using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_HealthCareProviderApp.Models
{
    public class ProviderModel : IDataErrorInfo 
    {
        public long PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public System.DateTime DOB { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "FirstName")
                {
                    if (string.IsNullOrEmpty(FirstName))
                        result = "Please enter a First Name";
                }
                if (columnName == "LastName")
                {
                    if (string.IsNullOrEmpty(LastName))
                        result = "Please enter a Last Name";
                }
                if (columnName == "PhoneNumber")
                {
                    if (string.IsNullOrEmpty(PhoneNumber))
                        result = "Please enter a Phone Number";
                }
                if (columnName == "EmailAddress")
                {
                    if (string.IsNullOrEmpty(EmailAddress))
                        result = "Please enter a EmailAddress";
                }
                if (columnName == "Address")
                {
                    if (string.IsNullOrEmpty(Address))
                        result = "Please enter a Address";
                }
                if (columnName == "UserName")
                {
                    if (string.IsNullOrEmpty(UserName))
                        result = "Please enter a UserName";
                }
                if (columnName == "Password")
                {
                    if (string.IsNullOrEmpty(Password))
                        result = "Please enter a Password";
                }
                return result;
            }
        }
    }
}
