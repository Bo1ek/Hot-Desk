using FluentEmail.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SoftwareMind.WebAPI.Services;

public class EmailService : IEmailService
{
    private readonly IFluentEmailFactory _fluentEmailFactory;

    public EmailService(IFluentEmailFactory fluentEmailFactory)
    {
        _fluentEmailFactory = fluentEmailFactory ?? throw new ArgumentNullException(nameof(fluentEmailFactory));
    }

    public async Task Send(EmailMessageModel emailMessageModel)
    {
        if (emailMessageModel == null)
        {
            throw new ArgumentNullException(nameof(emailMessageModel));
        }

        await _fluentEmailFactory.Create().To(emailMessageModel.ToAddress)
            .Subject(emailMessageModel.Subject)
            .Body(emailMessageModel.Body, true)
            .SendAsync();
    }
}

public interface IEmailService
{
    /// <summary>
    /// Send an email.
    /// </summary>
    /// <param name="emailMessage">Message object to be sent</param>
    Task Send(EmailMessageModel emailMessage);
}

public class EmailMessageModel
{
    public string ToAddress { get; set; }
    public string Subject { get; set; }
    public string? Body { get; set; }
    public string? AttachmentPath { get; set; }
    public EmailMessageModel(string toAddress, string subject, string? body = "")
    {
        ToAddress = toAddress;
        Subject = subject;
        Body = body;
    }
}

public class EmailSender : IEmailSender
{
    private readonly IEmailService _emailService;

    public EmailSender(IEmailService emailService)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        EmailMessageModel emailMessage = new(email, subject, htmlMessage);

        await _emailService.Send(emailMessage);
    }
}
