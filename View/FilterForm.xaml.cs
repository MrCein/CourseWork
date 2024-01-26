using System.Windows;
using WpfAppСourseWork.Model;

namespace WpfAppСourseWork.View
{
    public partial class FilterForm : Window
    {
        public FilterForm(FilterModel filterModel)
        {
            InitializeComponent();
            DataContext = filterModel;
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }

}
