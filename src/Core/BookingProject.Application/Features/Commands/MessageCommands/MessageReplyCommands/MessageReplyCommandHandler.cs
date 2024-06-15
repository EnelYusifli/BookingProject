using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNet.Identity;

namespace BookingProject.Application.Features.Commands.MessageCommands.MessageReplyCommands;

public class MessageReplyCommandHandler : IRequestHandler<MessageReplyCommandRequest, MessageReplyCommandResponse>
{
    private readonly IMessageRepository _repository;
    private readonly IEmailService _emailService;

    public MessageReplyCommandHandler(IMessageRepository repository,IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }
    public async Task<MessageReplyCommandResponse> Handle(MessageReplyCommandRequest request, CancellationToken cancellationToken)
    {
       var message=await  _repository.GetByIdAsync(request.Id);
        if (message is null) throw new NotFoundException("Message not found");
        string subject = "Reply to your message";
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email", "messagereply.html");
        string html = File.ReadAllText(filePath);
        html = html.Replace("{{message}}", message.MessageText);
        html = html.Replace("{{reply}}",request.Reply);
        await _emailService.SendEmail(message.Email, subject, html);
        message.IsReplied = true;
        await _repository.CommitAsync();
        return new MessageReplyCommandResponse();

    }
}
