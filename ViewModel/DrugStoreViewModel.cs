using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using WpfAppСourseWork.Model;
using WpfAppСourseWork.Commands;
using System.Windows;
using System.Xml.Linq;
using WpfAppСourseWork.View;

namespace WpfAppСourseWork.ViewModel
{
    class DrugStoreViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DrugStoreModel> _drugs;
        private DrugStoreModel _selectedDrug;
        private ObservableCollection<DrugStoreModel> _selectedDrugModels;
        private ObservableCollection<DrugStoreModel> _filteredDrugModels;
        private string filePath = "DrugStore.xml";
        public ObservableCollection<DrugStoreModel> Drugs
        {
            get { return _drugs; }
            set { _drugs = value; OnPropertyChanged(nameof(Drugs)); }
        }
        
        public ObservableCollection<DrugStoreModel> FilteredDrugModels
        {
            get { return _filteredDrugModels; }
            set { _filteredDrugModels = value; OnPropertyChanged(nameof(FilteredDrugModels)); }
        }

        public DrugStoreModel SelectedDrug
        {
            get { return _selectedDrug; }
            set { if (_selectedDrug != value) { _selectedDrug = value; OnPropertyChanged(nameof(SelectedDrug)); } }
        }

        public ObservableCollection<DrugStoreModel> SelectedDrugModels
        {
            get { return _selectedDrugModels; }
            set { _selectedDrugModels = value; OnPropertyChanged(nameof(SelectedDrugModels)); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FilterCommand { get; }

        public DrugStoreViewModel()
        {
            // Ініціалізуйте колекцію ліків з файлу XML тут

            Drugs = new ObservableCollection<DrugStoreModel>();
            SelectedDrugModels = new ObservableCollection<DrugStoreModel>();
            FilteredDrugModels = new ObservableCollection<DrugStoreModel>();
            AddCommand = new RelayCommand(AddDrug);
            EditCommand = new RelayCommand(EditDrug, CanEditOrDeleteDrug);
            DeleteCommand = new RelayCommand(DeleteDrug, CanEditOrDeleteDrug);
            FilterCommand = new RelayCommand(FilterDrug);

            LoadDataFromXml();
        }

        private void LoadDataFromXml()
        {
            try
            {
                 // Шлях до файлу
                if (System.IO.File.Exists(filePath))
                {
                    var doc = XDocument.Load(filePath);
                    foreach (var element in doc.Descendants("Drug"))
                    {
                        var drug = new DrugStoreModel
                        {
                            Name = element.Element("Name").Value,
                            Type = element.Element("Type").Value,
                            QuantityInPack = Convert.ToInt32(element.Element("QuantityInPack").Value),
                            Price = Convert.ToInt32(element.Element("Price").Value),
                            QuantityInStock = Convert.ToInt32(element.Element("QuantityInStock").Value),
                            ManufactureDate = DateTime.Parse(element.Element("ManufactureDate").Value),
                            ExpiryDate = DateTime.Parse(element.Element("ExpiryDate").Value)
                        };
                        Drugs.Add(drug);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні даних: {ex.Message}");
            }
        }

        private void SaveDataToXml()
        {
            try
            {
                var doc = new XDocument();
                var root = new XElement("Drugs");

                foreach (var drug in Drugs)
                {
                    var drugElement = new XElement("Drug",
                        new XElement("Name", drug.Name),
                        new XElement("Type", drug.Type),
                        new XElement("QuantityInPack", drug.QuantityInPack),
                        new XElement("Price", drug.Price),
                        new XElement("QuantityInStock", drug.QuantityInStock),
                        new XElement("ManufactureDate", drug.ManufactureDate),
                        new XElement("ExpiryDate", drug.ExpiryDate)
                    );
                    root.Add(drugElement);
                }

                doc.Add(root);
                doc.Save(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при збереженні даних: {ex.Message}");
            }
        }

        public void AddDrug()
        {
            var addDrugWindow = new AddEditDrugWindow();
            addDrugWindow.Topmost = true;
            if (addDrugWindow.ShowDialog() == true)
            {
                Drugs.Add(addDrugWindow.Drug);
                SaveDataToXml();
            }
        }

        public void EditDrug()
        {
            var selectedDrugs = _drugs.Where(s => s.IsSelected).ToList();

            if (selectedDrugs.Count > 1) FormsCallHandler.ShowMessage($"Ви хочете редагувати {selectedDrugs.Count} позиції! Оберіть одну!","Редагування");
            else
            {
                var editDrugWindow = new AddEditDrugWindow(SelectedDrug);
                editDrugWindow.Topmost = true;
                if (editDrugWindow.ShowDialog() == true)
                {
                    SaveDataToXml();
                }
            }
            selectedDrugs.Clear();
            SelectedDrugModels.Clear();
            SelectedDrug = null;
            _drugs.Where(s => s.IsSelected).ToList().ForEach(drug => drug.IsSelected = false);
        }

        private void DeleteDrug()
        {
            var selectedDrugs = _drugs.Where(s => s.IsSelected).ToList();

            if (selectedDrugs.Any() && MessageBox.Show($"Ви впевнені, що хочете видалити {selectedDrugs.Count} позицію(ції)?", "Підтвердження видалення", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (var selectedDrug in selectedDrugs)
                {
                    Drugs.Remove(selectedDrug);
                }

                SaveDataToXml();
            }
            selectedDrugs.Clear();
            SelectedDrugModels.Clear();
            SelectedDrug = null;
            _drugs.Where(s => s.IsSelected).ToList().ForEach(drug => drug.IsSelected = false);
        }

        private void FilterDrug()
        {            
            FilterModel filterModel = new FilterModel();
            var filterForm = new FilterForm(filterModel);
            filterForm.Topmost = true;
            if (filterForm.ShowDialog() == true)
            {
                foreach (var drug in Drugs
                .Where(drug =>
                    (string.IsNullOrWhiteSpace(filterModel.Name) || !string.IsNullOrWhiteSpace(drug.Name) && drug.Name.Contains(filterModel.Name)) &&
                    (string.IsNullOrWhiteSpace(filterModel.Type) || !string.IsNullOrWhiteSpace(drug.Type) && drug.Type.Contains(filterModel.Type)) &&
                    (!filterModel.ManufactureDate.HasValue || drug.ManufactureDate >= filterModel.ManufactureDate) &&
                    (!filterModel.ExpiryDate.HasValue || drug.ExpiryDate <= filterModel.ExpiryDate)
                ))
                {
                    FilteredDrugModels.Add(drug); // Додаємо відфільтровані моделі до колекції
                }

            }
            
        }

        private bool CanEditOrDeleteDrug()
        {
            return SelectedDrug != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
