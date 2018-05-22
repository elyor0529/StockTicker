using System;
using StockTicker.Lib.Common.Memberships;

namespace StockTicker.Soap.Models
{
    [Serializable]
    public class UserModel
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public MemberRole Role { get; set; }


        public UserModel()
        {

        }

        public UserModel(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public UserModel(string fullName, string userName, string password)
        {
            FullName = fullName;
            UserName = userName;
            Password = password;
        }

    }
}