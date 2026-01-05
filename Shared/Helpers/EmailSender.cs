using SendGrid;
using SendGrid.Helpers.Mail;

namespace Shared.Helpers;

public class EmailSender : IEmailSender
{
    public async Task SendResetLinkAsync(string toEmail, string resetUrl)
    {
        var apiKey = "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";
        var client = new SendGridClient(apiKey);

        var from = new EmailAddress("nakopia8080@gmail.com", "Auth.Template");
        var subject = "Reset Your Password";
        var to = new EmailAddress(toEmail);

        var plainTextContent = $"Click the following link to reset your password: {resetUrl}";
        var htmlContent = $@"
        <html>
            <body style='font-family: Arial, sans-serif;'>
                <h3>Reset Your Password</h3>
                <p>Hello,</p>
                <p>Click the link below to reset your password:</p>
                <p>
                    <a href='{resetUrl}' 
                       style='background-color:#007bff;color:white;
                              padding:10px 15px;text-decoration:none;
                              border-radius:5px;display:inline-block;'>
                        Reset Password
                    </a>
                </p>
                <p>If you didn’t request this, you can ignore this email.</p>
                <p>– The ReservoApp Team</p>
            </body>
        </html>";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);
    }

}

public interface IEmailSender
{
    Task SendResetLinkAsync(string toEmail, string resetUrl);
}