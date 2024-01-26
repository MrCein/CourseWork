using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using WpfAppСourseWork.ViewModel;

namespace WpfAppСourseWork.Model
{
    public class AuthenticationService : IAuthenticationService
    {
        private List<UserLoginDataModel> users; // Фіктивна база даних користувачів
        private readonly string filePath = "users.json";

        private string _foundUserName;
        public string FoundUserName => _foundUserName;

        public AuthenticationService()
        {
            // Ініціалізація фіктивної бази даних користувачів
            users = LoadUsersFromFile(filePath);
        }

        public bool IsAuthenticateUser(string email, string password)
        {
            UserLoginDataModel user = GetUserEmail(email).FirstOrDefault();
            _foundUserName = (user != null) ? user.UserName : "";
            return ((user != null) && (VerifyPassword(password, user.PasswordHash, user.Salt)));
        }

        public List<UserLoginDataModel> GetUserEmail(string email)
        {
            return users.Where(user => user.Email == email).ToList();
        }

        public List<UserLoginDataModel> GetUserName(string name)
        {
            return users.Where(user => user.UserName == name).ToList();
        }

        public bool AddNewUser(RegisterViewModel model)
        {
            UserLoginDataModel newUser = CreateUser(model);
            users.Add(newUser);
            return SaveUsersToFile();
        }

        private List<UserLoginDataModel> LoadUsersFromFile(string filePath)
        {
            try
            {
                // Зчитування даних з файлу та ініціалізація списку користувачів
                string json = File.ReadAllText(filePath);
                List<UserLoginDataModel> loadedUsers = JsonSerializer.Deserialize<List<UserLoginDataModel>>(json);

                return loadedUsers ?? new List<UserLoginDataModel>();
            }
            catch (Exception)
            {
                // Обробка помилки при зчитуванні файлу
                return new List<UserLoginDataModel>();
            }
        }

        private bool SaveUsersToFile()
        {
            try
            {
                // Збереження даних користувачів у файл JSON з використанням StreamWriter
                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                    sw.Write(json);
                    sw.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string EncodeBase64(byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        private string DecodeBase64(string encodedString)
        {
            byte[] bytes = Convert.FromBase64String(encodedString);
            return Encoding.UTF8.GetString(bytes);
        }

        private UserLoginDataModel CreateUser(RegisterViewModel model)
        {
            string salt = GenerateSalt(); //Base64
            string passwordHash = ComputeHash(model.PasswordForm, salt); //Base64

            return new UserLoginDataModel
            {
                UserName = model.UserNameForm,
                Email = model.EmailForm,
                Salt = salt,
                PasswordHash = passwordHash
            };
        }

        private bool VerifyPassword(string plainPassword, string storedHash, string storedSalt)
        {
            string inputHash = ComputeHash(plainPassword, storedSalt);
            return storedHash.Equals(inputHash, StringComparison.Ordinal);
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];

            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(saltBytes);
            }

            return EncodeBase64(saltBytes);
        }

        private string ComputeHash(string input, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            using (var sha256 = new SHA256Managed())
            {
                byte[] saltedPassword = Encoding.UTF8.GetBytes(input).Concat(saltBytes).ToArray();
                byte[] hashBytes = sha256.ComputeHash(saltedPassword);

                return EncodeBase64(hashBytes);
            }
        }

        public string ConvertSecureStringToString(SecureString secureString)
        {
            IntPtr valuePtr = IntPtr.Zero;

            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        public bool AuthenticateUser(SecureString password)
        {
            throw new NotImplementedException();
        }
    }
}
