using MediatR;

namespace Application.User
{
    public class CreateUserCommand : IRequest<bool>
    {
        public Domain.User User { get; set; }
    }
   
}
