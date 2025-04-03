using System.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ICE3.Services;

public class EmailSender : IEmailSender
{
    private readonly string _sendGridApiKey;

    public EmailSender(IConfiguration configuration)
    {
        _sendGridApiKey = configuration["SendGrid:ApiKey"];
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("omar.j.black@gmail.com",
                "Majestic Solutions"); // Verified email in SendGrid
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject,
                "Welcome to Majestic Solutions", message);
            
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Body.ReadAsStringAsync();
                Console.WriteLine($"An error occurred while sending email: {errorMessage}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while sending email: {ex.Message}");
        }
    }
}