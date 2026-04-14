using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shared.Seeding
{
    public class EmailSender
    {
        /// <summary>
        /// Validates if the email format is correct
        /// </summary>
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Error: Email cannot be null or empty");
                return false;
            }

            try
            {
                // Basic email validation using regex
                string emailPattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
                if (!Regex.IsMatch(email, emailPattern))
                {
                    Console.WriteLine($"Error: Invalid email format: {email}");
                    return false;
                }

                // Additional validation using MailAddress class
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch (FormatException)
            {
                Console.WriteLine($"Error: Invalid email address format: {email}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating email: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> SendEmailAsync(string toEmail)
        {
            // Validate email before attempting to send
            if (!IsValidEmail(toEmail))
            {
                return false;
            }

            string subject = "Successful Registration";
            string body = "Your registration is successful! You are welcome the Abhay AirLines All the best for your journey";
            
            MailMessage message = null;
            SmtpClient smtp = null;
            
            try
            {
                string fromEmail = "abhaychauhan5051a@gmail.com";
                string password = "guwv rqqw trnu sggx";

                message = new MailMessage();
                message.From = new MailAddress(fromEmail);
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = body;

                smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                smtp.EnableSsl = true;
                smtp.Timeout = 10000;

                await smtp.SendMailAsync(message);

                Console.WriteLine($"Email sent successfully to {toEmail}!");
                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP Error: {ex.Message}");
                Console.WriteLine($"Status Code: {ex.StatusCode}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                Console.WriteLine("\nPossible causes:");
                Console.WriteLine("- Gmail app-specific password is incorrect");
                Console.WriteLine("- 2-Factor Authentication not enabled");
                Console.WriteLine("- Network connectivity issue");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
            finally
            {
                message?.Dispose();
                smtp?.Dispose();
            }
        }
    }

}
