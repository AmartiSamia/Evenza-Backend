using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Project.API.Services
{
    
    public class PasswordService : IPasswordService
    {
        public List<string> ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                errors.Add("Password must be at least 8 characters long");

            if (!Regex.IsMatch(password, "[A-Z]"))
                errors.Add("Password must contain at least one uppercase letter");

            if (!Regex.IsMatch(password, "[a-z]"))
                errors.Add("Password must contain at least one lowercase letter");

            if (!Regex.IsMatch(password, "[0-9]"))
                errors.Add("Password must contain at least one number");

            if (!Regex.IsMatch(password, "[!@#$%^&*]"))
                errors.Add("Password must contain at least one special character (!@#$%^&*)");

            return errors;
        }

        public bool IsPasswordValid(string password)
        {
            return ValidatePassword(password).Count == 0;
        }
    }
}