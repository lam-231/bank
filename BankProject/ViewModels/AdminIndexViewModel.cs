using BankProject.Models;

namespace BankProject.ViewModels
{
    public class AdminIndexViewModel
    {
        public List<User> AllUsers { get; set; }

        public decimal ATMCash { get; set; }
    }
}
