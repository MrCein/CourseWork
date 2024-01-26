using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using WpfAppСourseWork.Model;

namespace WpfAppСourseWork.ViewModel
{
    public class RegisterViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        AuthenticationService authenticationService = new AuthenticationService();

        private string _userNameForm;
        public string UserNameForm { get => _userNameForm; set { _userNameForm = value; OnPropertyChanged(UserNameForm); } }
        private string _email;
        public string EmailForm { get => _email; set { _email = value; OnPropertyChanged(EmailForm); } }
        private string _password;
        public string PasswordForm { get => _password; set { _password = value; } }
        private string _confirmpassword;
        public string ConfirmPasswordForm { get => _confirmpassword; set { _confirmpassword = value; } }

        public string Error
        {
            get
            {
                StringBuilder error = new StringBuilder();
                ValidateProperty(nameof(UserNameForm), error);
                ValidateProperty(nameof(EmailForm), error);
                ValidateProperty(nameof(PasswordForm), error);
                ValidateProperty(nameof(ConfirmPasswordForm), error);

                return error.ToString();
            }
        }

        private void ValidateProperty(string propertyName, StringBuilder error)
        {
            switch (propertyName)
            {
                case nameof(UserNameForm):
                    if (!ValidateData.IsNotEmpty(UserNameForm))
                        error.AppendLine($"{TextLog.ErrorName}\n");
                    if (!NotExistName(UserNameForm))
                        error.AppendLine($"{TextLog.ErrorIsExistName}\n");
                    break;
                case nameof(EmailForm):
                    if (!IsValidEmail())
                        error.AppendLine($"{TextLog.ErrorEmail}\n");
                    if (!NotExistEmail())
                        error.AppendLine($"{TextLog.ErrorIsExistEmail}\n");
                    break;
                case nameof(PasswordForm):
                    if (!ValidateData.IsNotEmpty(PasswordForm) || !ValidateData.IsEnglishLettersAndDigitsOnly(PasswordForm) || PasswordForm.Length < 6)
                        error.AppendLine($"{TextLog.ErrorPassword}\n");
                    break;
                case nameof(ConfirmPasswordForm):
                    if (ConfirmPasswordForm != PasswordForm)
                        error.AppendLine($"{TextLog.ErrorConfirm}\n");
                    break;
            }
        }

        private bool IsValidEmail()
        {
            return ValidateData.IsNotEmpty(EmailForm) && ValidateData.IsValidEmail(EmailForm);
        }

        private bool NotExistEmail()
        {
            return authenticationService.GetUserEmail(EmailForm).Count == 0;
        }

        public bool IsSaveUser()
        {
            return authenticationService.AddNewUser(this);
        }

        public bool NotExistName(string name)
        {
            return authenticationService.GetUserName(name).Count == 0;
        }

        public string this[string columnName] => throw new NotImplementedException();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
