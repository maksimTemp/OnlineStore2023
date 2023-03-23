using OnlineStore2023.DataContext;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.Utilities;
using System;
using System.Linq;

namespace OnlineStore2023.Managers
{
    public class AccountManager
    {
        public static Guid? LoggedId { get; private set; }
        public static string UserType { get; private set; }

        public static bool Login(string userLogin, string password)
        {
            bool output = false;

            if (userLogin == null || userLogin == string.Empty || password == null || password == string.Empty)
                return false;

            string passwordHash = string.Empty;
            string passwordSalt = string.Empty;
            try
            {
                OnlineShop2022Context dataContext = new();
                passwordHash = dataContext.Users.Where(x => x.UserLogin == userLogin).Select(x => x.UserPasswordHash).FirstOrDefault();
                passwordSalt = dataContext.Users.Where(x => x.UserLogin == userLogin).Select(x => x.UserPasswordSalt).FirstOrDefault();
            }
            catch (Exception e)
            {
                StandardMessages.Error("При входе в систему произошла ошибка!");
            }

            if (passwordHash == null) return false;
            if (CheckPasswords(password, passwordHash, passwordSalt))
                output = true;

            if (output)
            {
                try
                {
                    OnlineShop2022Context dataContext = new();
                    LoggedId = dataContext.Users.Where(x => x.UserLogin == userLogin).Select(x => x.UserId).FirstOrDefault();
                    UserType = dataContext.Users.Where(x => x.UserLogin == userLogin).Select(x => x.UserType).FirstOrDefault();
                }
                catch (Exception e)
                {
                    StandardMessages.Error("При входе в систему произошла ошибка!");
                }
            }
            return output;
        }

        public static bool Register(string login, string email, string password, string firstname, string lastname, out string message)
        {
            try
            {
                OnlineShop2022Context dataContext = new();

                var saltGenerator = new PasswordSaltGenerator();
                var newPasswordSalt = saltGenerator.GetRandomString(10);
                var newPasswordHash = new PasswordHashGenerator();

                UsersDatum curUserData = new()
                {
                    UserFirstName = firstname,
                    UserLastName = lastname
                };

                Customer customer = new()
                {
                    CustomerEmailAdress = email
                };

                var user = new User(
                    login,
                    newPasswordHash.GenerateHash(password, newPasswordSalt),
                    newPasswordSalt,
                    "simple_user",
                    true,
                    curUserData,
                    customer);

                dataContext.Users.Add(user);
                dataContext.SaveChanges();
            }
            catch (Exception e)
            {
                StandardMessages.Error("Во время регистрации произошла ошибка!");
                message = "Произошла ошибка";
                return false;
            }

            message = "Регистрация прошла успешно!";
            return true;
        }

        public static void Logout()
        {
            LoggedId = null;
        }

        private static bool CheckPasswords(string password, string passwordHash, string passwordSalt)
        {
            var newPasswordHash = new PasswordHashGenerator();
            var verPassword = newPasswordHash.GenerateHash(password, passwordSalt);
            return (verPassword == passwordHash);
        }

        public static bool ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                OnlineShop2022Context dataContext = new();
                PasswordHashGenerator passwordHashGenerator = new();
                var user = dataContext.Users.Single(x => x.UserId == LoggedId);
                if (!CheckPasswords(oldPassword, user.UserPasswordHash, user.UserPasswordSalt))
                    return false;
                user.UserPasswordHash = passwordHashGenerator.GenerateHash(newPassword, user.UserPasswordSalt);
                dataContext.Users.Update(user);
                dataContext.SaveChanges();
            }
            catch (Exception e)
            {
                StandardMessages.Error("Произошла ошибка при смене пароля!");
                return false;
            }

            return true;
        }

        public static bool ChangePassword(string newPassword, Guid userID)
        {
            try
            {
                OnlineShop2022Context dataContext = new();
                PasswordHashGenerator passwordHashGenerator = new();
                var user = dataContext.Users.FirstOrDefault(x => x.UserId == userID);
                user.UserPasswordHash = passwordHashGenerator.GenerateHash(newPassword, user.UserPasswordSalt);
                dataContext.Users.Update(user);
                dataContext.SaveChanges();
            }
            catch (Exception e)
            {
                string mess = "Произошла ошибка при смене пароля!\n";
                StandardMessages.Error(mess + e.Message);
                return false;
            }

            return true;
        }
    }
}
