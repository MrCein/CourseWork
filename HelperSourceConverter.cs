using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Collections.Generic;
using WpfAppСourseWork.Model;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using WpfAppСourseWork.View;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WpfAppСourseWork
{
    public class HelperSourceConverter
    {
    }

    public class InverseVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool intValue && intValue == true) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class ZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool intValue && intValue == false)  ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ZeroInvertToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is int intValue && intValue == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ZeroInvertToEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is int intValue && intValue == 0) ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CurrentDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Всі вхідні значення ігноруються, оскільки ми визначаємо початкову дату як поточну
            return DateTime.Now;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Ми не використовуємо зворотнє перетворення в цьому випадку
            return Binding.DoNothing;
        }
    }

    public static class FormsCallHandler
    {
        public static void ShowMessage(string message, string title = null, bool error = true)
        {
            ErrorMessageModel errorMessageModel = new ErrorMessageModel(message, title, error);
            ErrorMessage errorMessage = new ErrorMessage(errorMessageModel);
            errorMessage.ShowDialog();
            errorMessage.Close();
        }

        public static void SetCenterPositionAndOpen(Window window)
        {
            //window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        public static object FindUser(string userEmail)
        {
            try
            {
                List<UserLoginDataModel> loginDataList = FileHelper.LoadUserLoginData();
                return (loginDataList != null && loginDataList.Count > 0) ? loginDataList.Where(user => user.Email == userEmail).ToList() : null;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "Помилка при пошуку користувача:");
                return null;
            }
        }


    }

    public static class ValidateData
    {
        // Check field formd

        public static bool IsNotEmpty(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                // Спробуємо створити об'єкт MailAddress
                MailAddress mailAddress = new MailAddress(email);
                return true; // Якщо створено без виключень, то адреса валідна
            }
            catch (FormatException)
            {
                return false; // Виняток FormatException вказує на невірний формат електронної адреси
            }
        }

        public static bool ContainsLetterAndDigit(string input)
        {
            // Регулярний вираз для перевірки наявності літери та цифри
            Regex regex = new Regex("(?=.*[a-zA-Z])(?=.*[0-9])");
            // Якщо вхідний рядок пустий, то не треба превіряти.
            return (string.IsNullOrWhiteSpace(input)) ? false : regex.IsMatch(input);
        }

        public static bool IsEnglishLettersAndDigitsOnly(string input)
        {
            // Використовуємо регулярний вираз для перевірки на наявність тільки англійських літер та цифр
            Regex regex = new Regex("^[a-zA-Z]+[a-zA-Z0-9.@]*$");
            // Якщо вхідний рядок пустий, то не треба превіряти
            return (string.IsNullOrWhiteSpace(input)) ? false : regex.IsMatch(input);
        }
    }

    public static class TextLog
    {
        public static string ErrorName = "Ім'я повинно вміщувати букви або букви та цифри, але не меньш двох символів!";
        public static string ErrorEmail = "Некоректний email!";
        public static string ErrorPassword = "Некоректний пароль!\nПароль повинен обов'язково вміщувати англійскі літери та хочаб одну цифру.\nДовжина паролю шість сімволів та білше!";
        public static string ErrorLogin = "Помилка! Невірний Email або пароль";
        public static string ErrorConfirm = "Пароль та підтверження паролю не збігаються!";
        public static string ErrorIsExistEmail = "Не можливо зареєструвати, користувач з таким email вже яснує!";
        public static string ErrorIsExistName = "Таке ім'я вже існує! Оберіть будь ласка інше!";
        public static string ErrorDate = "Дата виробництва не може бути в майбутьному!";
        public static string ErrorTerm = "Термін сберігання не може бути меньш ніж дата виробництва!";
        public static string ErrorCount(string input) => $"Поле {input} не може бути нуль, або нижче нуля!";
        public static string getEmptyLogFieldLog(string input) => $"Поле {input} не повинно бути пусте!";
        public static string getWrongLogFieldLog(string input) => $"Поле {input} пусте, або заповнено невірно!";
        public static string SuccessLogin = "Вітаємо! Успішна аторізація!";
        public static string SuccessRegistration = "Вітаємо! Успішна  реєстрація!\nПерезайдіть у систему!";
    }

    public static class FileHelper
    {
        private static readonly string FilePath = "userdata.dat";
        public static List<UserLoginDataModel> LoadUserLoginData()
        {
            try
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    if (fs.Length > 0)
                    {
                        return (List<UserLoginDataModel>)formatter.Deserialize(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return new List<UserLoginDataModel>();
        }

        public static bool SaveUser(UserLoginDataModel user)
        {
            List<UserLoginDataModel> loginDataList = LoadUserLoginData();
            loginDataList.Add(user);
            return SaveUserLoginData(loginDataList);
        }

        public static bool SaveUserLoginData(List<UserLoginDataModel> loginDataList)
        {
            try
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, loginDataList);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }

        }
    }
}
