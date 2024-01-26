using System.Windows;
using System.Windows.Input;
using WpfAppСourseWork.Model;

namespace WpfAppСourseWork.View
{
    public partial class AddEditDrugWindow : Window
    {
        public DrugStoreModel Drug;
        
        public AddEditDrugWindow(DrugStoreModel _existingDrug = null)
        {
            InitializeComponent();
            if (_existingDrug != null)
            {
                Drug = _existingDrug;
                Title = "Редагування позиції";
                Submit.Content = "Зберегти";
            } else
            {
                Drug = new DrugStoreModel();
                Title = "Додання позиції";
                Submit.Content = "Додати";
            }
            DataContext = Drug;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Drug.Error))
            {
                DialogResult = true;
                Close();
            }
            else MessageBox.Show($"Форма заповнена невірно!\n\n{Drug.Error}", Title);

        }

        private void TextBox_PreviewDigitInput(object sender, TextCompositionEventArgs e)
        {
            // Перевірка, чи введений символ є цифрою
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true; // Відхилити введення символу, якщо це не цифра
            }
        }

        private void TextBox_PreviewDecimalInput(object sender, TextCompositionEventArgs e)
        {
            string input = e.Text;

            // Використовуємо регулярний вираз для перевірки, чи є введений символ цифрою або точкою
            //if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"[\d.]"))
            //{
            //    e.Handled = true; // Відхиляємо введення, якщо це не цифра або точка
            //}

            // Дозволяємо тільки цифри та крапку
            if (!char.IsDigit(e.Text, 0) && e.Text != ".")
            {
                e.Handled = true;
            }

            // Перевірка наявності точки лише один раз
            //var textBox = sender as TextBox;
            //if (textBox != null && input == "." && textBox.Text.Contains("."))
            //{
            //    e.Handled = true; // Відхиляємо введення, якщо точка вже присутня в тексті
            //}
        }

        private void TextBox_PreviewCharInput(object sender, TextCompositionEventArgs e)
        {
            // Перевірка, чи введено тільки букви
            if (!char.IsLetter(e.Text, 0))
            {
                e.Handled = true; // Відхилити введення, якщо вміст не є літерою
            }
        }
    }
}
