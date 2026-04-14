using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Shared.DTOs;
using Shared.Enums;

namespace BAL.Validation
{
    public class UserValidation
    {
        /// <summary>
        /// Validates user registration data
        /// </summary>
        public static bool ValidateUserRegistration(UserDto user, out List<string> errors)
        {
            errors = new List<string>();

            // Check if user is null
            if (user == null)
            {
                errors.Add("User cannot be null");
                return false;
            }

            // Validate Name
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                errors.Add("Name is required");
            }
            else if (user.Name.Length < 2)
            {
                errors.Add("Name must be at least 2 characters long");
            }
            else if (user.Name.Length > 100)
            {
                errors.Add("Name cannot exceed 100 characters");
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                errors.Add("Email is required");
            }
            else if (!IsValidEmail(user.Email))
            {
                errors.Add("Email format is invalid");
            }
            else if (user.Email.Length > 100)
            {
                errors.Add("Email cannot exceed 100 characters");
            }

            // Validate Password
            
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                errors.Add("Password is required");
            }
            else if (!IsValidPassword(user.Password))
            {
                errors.Add("Password must be at least 6 characters long and must contain one uppercase, one special character, one number");
            }

            // Validate Role
            if (!IsValidUserRole(user.Role))
            {
                errors.Add("Invalid user role");
            }

            return errors.Count == 0;
        }

        /// <summary>
        /// Validates user login credentials
        /// </summary>
        public static bool ValidateLogin(string email, string password, out List<string> errors)
        {
            errors = new List<string>();

            // Validate Email
            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Email is required");
            }
            else if (!IsValidEmail(email))
            {
                errors.Add("Email format is invalid");
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password is required");
            }
            else if (!IsValidPassword(password))
            {
                errors.Add("Password must be at least 6 characters long and must contain one uppercase, one special character, one number");
            }

            return errors.Count == 0;
        }

        /// <summary>
        /// Validates email format using regex
        /// </summary>
        /// 
        public static bool IsValidPassword(string password)
        {
            try
            {
                string pattern  = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
                return Regex.IsMatch(password, pattern);
            }
            catch
            {
                return false;
            }
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                // Standard email validation regex
                string pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the user role is valid
        /// </summary>
        public static bool IsValidUserRole(UserRole role)
        {
            return Enum.IsDefined(typeof(UserRole), role);
        }

        /// <summary>
        /// Gets list of valid user roles
        /// </summary>
        public static List<string> GetValidUserRoles()
        {
            var roles = new List<string>();
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                roles.Add(role.ToString());
            }
            return roles;
        }

        /// <summary>
        /// Validates user profile update
        /// </summary>
        public static bool ValidateUserUpdate(UserDto user, out List<string> errors)
        {
            errors = new List<string>();

            if (user == null)
            {
                errors.Add("User cannot be null");
                return false;
            }

            // Only validate fields that are provided
            if (!string.IsNullOrWhiteSpace(user.Name))
            {
                if (user.Name.Length < 2)
                {
                    errors.Add("Name must be at least 2 characters long");
                }
                else if (user.Name.Length > 100)
                {
                    errors.Add("Name cannot exceed 100 characters");
                }
            }

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                if (!IsValidEmail(user.Email))
                {
                    errors.Add("Email format is invalid");
                }
                else if (user.Email.Length > 100)
                {
                    errors.Add("Email cannot exceed 100 characters");
                }
            }

            return errors.Count == 0;

        }
    }
}
