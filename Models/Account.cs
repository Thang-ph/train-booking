using System;
using System.Collections.Generic;

namespace PRN211_Project_Group_4.Models
{
    public partial class Account
    {
        public Account()
        {
            Bookings = new HashSet<Booking>();
        }

        private static bool isLoggedIn { get; set; } = false;
        private static bool isAdmin { get; set; } = false;


        public int AccountId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? Role { get; set; }

        public bool GetAccountStatus => isLoggedIn;
        public bool GetRoleStatus => isAdmin;

        public void Login()
        {
            isLoggedIn = true;
        }
        public void Logout()
        {
            isLoggedIn = false;
        }

        public virtual ICollection<Booking> Bookings { get; set; }


        public void checkRole(int? role)
        {
            if (role == 1)  isAdmin = true;
            else  isAdmin = false;
        }
    }
}
