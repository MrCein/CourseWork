using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WpfAppСourseWork.Model
{
    public class UserLoginDataModel: INotifyPropertyChanged
    {
        private string _userName;
        public string UserName { get => _userName; set { if (_userName != value) { _userName = value; OnPropertyChanged(_userName); } } }

        private string _passwordHash;
        public string PasswordHash { get => _passwordHash; set { _passwordHash = value; } }

        private string _salt;
        public string Salt { get => _salt; set {  _salt = value; } }

        private string _email;
        public string Email { get => _email; set { if (_email != value) { _email = value; OnPropertyChanged(_email); } } }      

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static explicit operator List<object>(UserLoginDataModel? v)
        {
            throw new NotImplementedException();
        }
    }
}
