using BusinessLayer.Models.User;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtilities.Data
{
    public static class TestDataHelperUser
    {
        public static User GetUser1()
        {
            return new User()
            {
                Id = "1",
                Name = "Pavel Novák",
                UserName = "bookworm",
                Email = "pavel.novak@seznam.cz",
                IsAdministrator = false,
            };
        }

        public static User GetUser2()
        {
            return new User()
            {
                Id = "2",
                Name = "Karolína Svobodová",
                UserName = "kaja2000",
                Email = "karolina.svobodova@email.cz",
                IsAdministrator = false,
            };
        }

        public static User GetUser3()
        {
            return new User()
            {
                Id = "3",
                Name = "Prokop Dlouhý",
                UserName = "pageturner",
                Email = "prokop.dlouhy@gmail.com",
                IsAdministrator = false,
            };
        }

        public static UserModel GetUserModel1()
        {
            return new UserModel()
            {
                Id = "1",
                Name = "Pavel Novák",
                UserName = "bookworm",
                Email = "pavel.novak@seznam.cz"
            };
        }

        public static UserModel GetUserModel2()
        {
            return new UserModel()
            {
                Id = "2",
                Name = "Karolína Svobodová",
                UserName = "kaja2000",
                Email = "karolina.svobodova@email.cz",
            };
        }

        public static UserModel GetUserModel3()
        {
            return new UserModel()
            {
                Id = "3",
                Name = "Prokop Dlouhý",
                UserName = "pageturner",
                Email = "prokop.dlouhy@gmail.com"
            };
        }

        public static CreateUserModel GetCreateUser1()
        {
            return new CreateUserModel()
            {
                Name = "Pavel Novák",
                UserName = "bookworm",
                Email = "pavel.novak@seznam.cz",
                IsAdministrator = false,
            };
        }

        public static CreateUserModel GetCreateUser2()
        {
            return new CreateUserModel()
            {
                Name = "Karolína Svobodová",
                UserName = "kaja2000",
                Email = "karolina.svobodova@email.cz",
                IsAdministrator = false,
            };
        }

        public static CreateUserModel GetCreateUser3()
        {
            return new CreateUserModel()
            {
                Name = "Prokop Dlouhý",
                UserName = "pageturner",
                Email = "prokop.dlouhy@gmail.com",
                IsAdministrator = false,
            };
        }
    }
}
