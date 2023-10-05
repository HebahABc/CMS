using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using System.Diagnostics;

namespace Crito.Services;

public class MailService : IDisposable
{
    private string _from;
    private string _smtp;
    private int _port;
    private string _username;
    private string _password;
    private SmtpClient smtpClient;

    public MailService(string from, string smtp, int port, string username, string password )
    {
        _from = from;
        _smtp = smtp;
        _port = port;
        _username = username;
        _password = password;
        smtpClient = new SmtpClient();
    }


    public async Task SendAsync (string to, string subject, string body)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            await smtpClient.ConnectAsync(_smtp, _port, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(_username, _password);

            var result = await smtpClient.SendAsync(email);
        }
        catch (Exception ex) {
            Debug.WriteLine(ex.Message);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) 
        {
            smtpClient.DisconnectAsync(true).ConfigureAwait(false);
        }
    }
}
