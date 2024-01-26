using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfAppСourseWork.Model;
using WpfAppСourseWork.View;
using WpfAppСourseWork.ViewModel;

namespace WpfAppСourseWork
{
    public partial class MainWindow : Window
    {
        private DrugStoreViewModel _viewModel;
        public MainWindow()
        {
            var loginForm = new LoginForm();
            if (loginForm.ShowDialog() != true)
            {
                // Користувач вибрав вийти або іншу дію, виходимо з програми
                Close();
                return;
            }
            InitializeComponent();
            _viewModel = new DrugStoreViewModel();
            DataContext = _viewModel;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.RemovedItems.OfType<DrugStoreModel>().Concat(e.AddedItems.OfType<DrugStoreModel>()))
            {
                item.IsSelected = e.AddedItems.Contains(item);

                if (e.AddedItems.Contains(item))
                {
                    _viewModel.SelectedDrugModels.Add(item);
                }
                else
                {
                    _viewModel.SelectedDrugModels.Remove(item);
                }
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.RemovedItems.OfType<DrugStoreModel>().Concat(e.AddedItems.OfType<DrugStoreModel>()))
            {
                item.IsSelected = e.AddedItems.Contains(item);

                if (e.AddedItems.Contains(item))
                {
                    _viewModel.SelectedDrugModels.Add(item);
                }
                else
                {
                    _viewModel.SelectedDrugModels.Remove(item);
                }
            }
        }

        private void Set_Filter(object sender, RoutedEventArgs e)
        {
            DataGridList.ItemsSource = _viewModel.FilteredDrugModels;
        }

        private void Clear_Filter(object sender, RoutedEventArgs e)
        {
            _viewModel.FilteredDrugModels.Clear();
            DataGridList.ItemsSource = _viewModel.Drugs;
        }
    }
}
