using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Web.Models.Account
{
    public class RegisterConfirmationModel
    {
        public string Email { get; set; }
        public bool DisplayConfirmAccountLink { get; set; }
        public string EmailConfirmationUrl { get; set; }
        public string ReturnUrl { get; set; }

        public RegisterConfirmationModel()
        {
            
        }
    }
}
