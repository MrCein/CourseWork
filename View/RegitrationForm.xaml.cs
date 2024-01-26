using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WpfAppСourseWork.ViewModel;

namespace WpfAppСourseWork.View
{
    public partial class RegitrationForm : Window
    {
        private RegisterViewModel registrationViewModel = new RegisterViewModel();
        public RegitrationForm()
        {
            InitializeComponent();
            DataContext = registrationViewModel;
        }

        private void SaveDataUser(object sender, RoutedEventArgs e)
        {
            registrationViewModel.PasswordForm = PasswordTextBox.Password;
            registrationViewModel.ConfirmPasswordForm = ConfirmPasswordTextBox.Password;
            string title = "Реєстрація";

            if (!string.IsNullOrEmpty(registrationViewModel.Error)) FormsCallHandler.ShowMessage(registrationViewModel.Error, title);
            else if (registrationViewModel.IsSaveUser())
            {
                FormsCallHandler.ShowMessage(TextLog.SuccessRegistration, title, false);
                DialogResult = true;
                registrationViewModel = null;
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
        }

        private void Color_Green(object sender, MouseEventArgs e)
        {
            BtnReg.Foreground = Brushes.GreenYellow;
        }

        private void Color_White(object sender, MouseEventArgs e)
        {
            BtnReg.Foreground = Brushes.White;
        }
    }
}
