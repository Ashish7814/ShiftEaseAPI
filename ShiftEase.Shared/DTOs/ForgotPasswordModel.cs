using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Shared.DTOs
{
    public class ForgotPasswordModel
    {
        public string Email { get; set; }
    }

    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Token { get; set; }        // encoded token from email link
        public string NewPassword { get; set; }
    }
}
