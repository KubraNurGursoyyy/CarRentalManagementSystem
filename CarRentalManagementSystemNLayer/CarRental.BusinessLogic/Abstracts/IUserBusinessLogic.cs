using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.BusinessLogic.Abstracts
{
    public interface IUserBusinessLogic 
    {
        bool LogIn(string Email, string Password);
    }
}
