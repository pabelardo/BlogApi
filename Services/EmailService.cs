using System.Net;
using System.Net.Mail;

namespace BlogApi.Services;

public class EmailService
{
    public bool Send(
        string toName,
        string toEmail,
        string subject,
        string body,
        string fromName = "Dev da InnoLevels",
        string fromEmail = "pedro.abelardo@innolevels.com.br")
    {
        var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port)
        {
            Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password), //Credenciais de rede
            DeliveryMethod = SmtpDeliveryMethod.Network, //Passamos o método de entrega que é via SMTP
            EnableSsl = true //Estamos utilizando porta segura (587). Se marcar como false, quer dizer que utilizaremos porta 25 e irá falhar.
        };

        var mail = new MailMessage
        {
            From = new MailAddress(fromEmail, fromName)
        };

        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true; //Podemos mandar no corpo da mensagem um <h1> ou qualquer sintaxe html

        try
        {
            smtpClient.Send(mail);
            return true;
        }
        catch 
        {
            return false;
        }
    }
}