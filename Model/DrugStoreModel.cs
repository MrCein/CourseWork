using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace WpfAppСourseWork.Model
{
    public class DrugStoreModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public DrugStoreModel() { SelectedType = new ObservableCollection<string> {
            "Адренолітичні засоби", "Адреноміметичні засоби", "Адсорбуючі засоби", "Азотисті іприти", "Аналептичні засоби", "Анальгезуючі засоби", "Анестезуючі засоби",
            "Анорексигенні засоби", "Антацидні засоби", "Антиангінальні засоби", "Антидепресанти", "Антидоти (протиотрути)", "Антикоагулянти", "Антиметаболіти", "Антисептичні засоби",
            "Вітрогінні засоби", "Гангліоблокуючі засоби", "Гіпотензивні засоби", "Гормональні препарати", "Десенсибілізуючі засоби", "Жарознижувальні засоби", "Жовчогінні засоби",
            "Імунодепресанти", "Інтерферони", "Кортикостероїди", "Курареподібні засоби (міорелаксанти периферичної дії)", "Маткові засоби", "Міорелаксанти", "Міотичні засоби",
            "Мочегінні засоби (діуретики)", "Нейролептичні засоби", "Обволікаючі засоби", "Відхаркувальні засоби", "Протизапальні засоби", "Протисудомні засоби",
            "Психостимулюючі засоби", "Серцеві глікозиди", "Симпатолітичні засоби", "Проносні засоби", "Снодійні засоби", "Суднорозширювальні засоби", "Судинозвужувальні засоби",
            "Спазмолітичні засоби", "Сульфаніламіди", "Транквілізатори", "Хіміотерапевтичні засоби", "Холінолітичні засоби", "Холіноміметичні засоби", "Цефалоспорини", "Цитостатичні засоби"
        }; }

        public string this[string columnName] => throw new NotImplementedException();

        private string? _name;
        public string? Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string? _type;
        public string? Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged(nameof(Type)); }
        }

        private int _quantityInPack;
        public int QuantityInPack
        {
            get { return _quantityInPack; }
            set { _quantityInPack = value; OnPropertyChanged(nameof(QuantityInPack)); }
        }

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged(nameof(Price)); }
        }

        private int _quantityInStock;
        public int QuantityInStock
        {
            get { return _quantityInStock; }
            set { _quantityInStock = value; OnPropertyChanged(nameof(QuantityInStock)); }
        }

        private DateTime _manufactureDate = DateTime.Now;
        public DateTime? ManufactureDate
        {
            get { return _manufactureDate; }
            set { _manufactureDate = (DateTime)value; OnPropertyChanged(nameof(ManufactureDate)); }
        }

        private DateTime _expiryDate = DateTime.Now;
        public DateTime? ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = (DateTime)value; OnPropertyChanged(nameof(ExpiryDate)); }
        }

        private ObservableCollection<string> _selectedType;
        public ObservableCollection<string> SelectedType
        {
            get => _selectedType;
            set { _selectedType = value; OnPropertyChanged(nameof(SelectedType)); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public string Error
        {
            get
            {
                StringBuilder error = new StringBuilder();
                ValidateProperty(nameof(Name), error);
                ValidateProperty(nameof(Type), error);
                ValidateProperty(nameof(QuantityInPack), error);
                ValidateProperty(nameof(Price), error);
                ValidateProperty(nameof(QuantityInStock), error);
                ValidateProperty(nameof(ManufactureDate), error);
                ValidateProperty(nameof(ExpiryDate), error);

                return error.ToString();
            }
        } // Отримання помилки для всіх властивостей

        private void ValidateProperty(string propertyName, StringBuilder error)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    if (!IsValidField(Name))
                    {
                        error.AppendLine($"{TextLog.getWrongLogFieldLog("''Назва''")}\n");
                    }
                    break;
                case nameof(Type):
                    if (!IsValidField(Type))
                        error.AppendLine($"{TextLog.getWrongLogFieldLog("''Тип''")}\n");
                    break;
                case nameof(QuantityInPack):
                    if (!IsDigitValid(QuantityInPack))
                        error.AppendLine($"{TextLog.ErrorCount("''Кількість одиниць в упаковці''")}\n");
                    break;
                case nameof(Price):
                    if (!IsCorerctPrice(Price))
                        error.AppendLine($"{TextLog.ErrorCount("''Ціна''")}\n");
                    break;
                case nameof(QuantityInStock):
                    if (!IsDigitValid(QuantityInStock))
                        error.AppendLine($"{TextLog.ErrorCount("''Кілкість товару на складі''")}\n");
                    break;
                case nameof(ManufactureDate):
                    if (ManufactureDate.HasValue && ManufactureDate.Value > DateTime.Now)
                        error.AppendLine($"{TextLog.ErrorDate}\n");
                    break;
                case nameof(ExpiryDate):
                    if (ExpiryDate.HasValue && ExpiryDate.Value <= ManufactureDate.Value)
                        error.AppendLine($"{TextLog.ErrorTerm}\n");
                    break;

            }
        }

        private bool IsValidField(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            return true;
        }

        private bool IsDigitValid(int value)
        {
            if (value == 0 || value < 0)
                return false;
            return true;
        }

        private bool IsCorerctPrice(decimal value)
        {
            if (value is (decimal)0.00 or < 0)
                return false;
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
