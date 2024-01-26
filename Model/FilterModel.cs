using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppСourseWork.Model
{
    public class FilterModel: INotifyPropertyChanged
    {
        private string _name;
        public string? Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }
        private string _type;
        public string? Type { get => _type; set { _type = value; OnPropertyChanged(nameof(Type)); } }
        private DateTime? _manufacturedate;
        public DateTime? ManufactureDate { get => _manufacturedate; set { _manufacturedate = (DateTime)value; OnPropertyChanged(nameof(ManufactureDate)); } }
        private DateTime? _expiredate;
        public DateTime? ExpiryDate { get => _expiredate; set { _expiredate = (DateTime)value; OnPropertyChanged(nameof(ExpiryDate)); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
