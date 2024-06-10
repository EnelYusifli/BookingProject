using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.MessageCommands.MessageReplyCommands;

public class MessageReplyCommandRequest:IRequest<MessageReplyCommandResponse>
{
    public int Id { get; set; }
    [Required]
    [MaxLength(1000)]
    public string Reply { get; set; }
}
