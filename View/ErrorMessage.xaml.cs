using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAppСourseWork.Model;

namespace WpfAppСourseWork.View
{
    public partial class ErrorMessage : Window
    {
        public ErrorMessage(ErrorMessageModel _errorMessageModel)
        {
            InitializeComponent();
            DataContext = _errorMessageModel;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
