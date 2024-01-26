using System.ComponentModel;
using System.Text;
using WpfAppСourseWork.Model;

namespace WpfAppСourseWork.ViewModel
{
    public class LoginViewModel: INotifyPropertyChanged, IDataErrorInfo
    {

        AuthenticationService authenticationService = new AuthenticationService();

        private string _user;
        public string User { get { return _user; } set { _user = value; OnPropertyChanged(User); } }

        private string _userEmailTextBox;
        public string UserEmailTextBox { get { return _userEmailTextBox; } set { if (_userEmailTextBox != value) { _userEmailTextBox = value; OnPropertyChanged(UserEmailTextBox); } } }

        private string _passwordTextBox;

        public string PasswordTextBox { get { return _passwordTextBox; } set { if (_passwordTextBox != value) { _passwordTextBox = value; OnPropertyChanged(PasswordTextBox); } } }

        public bool Authentication(string email, string password)
        {
            User = authenticationService.FoundUserName;
            return authenticationService.IsAuthenticateUser(email, password);
        }

        public string this[string columnName] {
            get
            {
                if (columnName == "UserNameTexbox" && !IsValidEmail()) return TextLog.ErrorEmail;

                if (columnName == "PasswordTextbox" && !ValidateData.IsEnglishLettersAndDigitsOnly(PasswordTextBox) && PasswordTextBox.Length < 6) return TextLog.ErrorPassword;

                return null; // Валідація пройшла успішно
            }
        }

        public string Error
        {
            get
            {
                StringBuilder error = new StringBuilder();
                ValidateProperty(nameof(UserEmailTextBox), error);
                ValidateProperty(nameof(PasswordTextBox), error);

                return error.ToString();
            }
        }

        private void ValidateProperty(string propertyName, StringBuilder error)
        {
            switch (propertyName)
            {
                case nameof(UserEmailTextBox):
                    if (!IsValidEmail())
                    {
                        error.AppendLine($"{TextLog.ErrorEmail}\n");
                    }
                    break;
                case nameof(PasswordTextBox):
                    if (!ValidateData.IsNotEmpty(PasswordTextBox) || !ValidateData.IsEnglishLettersAndDigitsOnly(PasswordTextBox) || PasswordTextBox.Length < 6)
                        error.AppendLine($"{TextLog.ErrorPassword}\n");
                    break;

            }
        }

        private bool IsValidEmail()
        {
            return ValidateData.IsNotEmpty(UserEmailTextBox) && ValidateData.IsValidEmail(UserEmailTextBox);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
