using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfAppСourseWork.ViewModel;

namespace WpfAppСourseWork.View
{
    public partial class LoginForm : Window
    {
        private LoginViewModel loginViewModel = new LoginViewModel();
        public LoginForm()
        {
            InitializeComponent();
            DataContext = loginViewModel;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            loginViewModel.PasswordTextBox = PasswordBox.Password;
            string title = "Авторізація";
            if (!string.IsNullOrWhiteSpace(loginViewModel.Error)) FormsCallHandler.ShowMessage(loginViewModel.Error, title);
            else if (loginViewModel.Authentication(UserEmailTextBox.Text, PasswordBox.Password)) 
                { 
                    //FormsCallHandler.ShowMessage(loginViewModel.User, title);
                    FormsCallHandler.ShowMessage(TextLog.SuccessLogin + loginViewModel.User, title, false);
                    DialogResult = true;
                    Close();
                }
                else FormsCallHandler.ShowMessage(TextLog.ErrorLogin, title);
        }

        private void SugnUp(object sender, MouseButtonEventArgs e)
        {
            RegitrationForm startRegister = new RegitrationForm();
            FormsCallHandler.SetCenterPositionAndOpen(startRegister);
            Close();
        }

        private void Color_Dark(object sender, MouseEventArgs e)
        {
            LoginButton.Foreground = Brushes.GreenYellow;
        }

        private void Color_White(object sender, MouseEventArgs e)
        {
            LoginButton.Foreground = Brushes.White;
        }

        private void Color_Light(object sender, MouseEventArgs e)
        {
            SignUpLabel.Foreground = Brushes.Silver;
        }

        private void Color_Gray(object sender, MouseEventArgs e)
        {
            SignUpLabel.Foreground= Brushes.Gray;
        }
    }
}
