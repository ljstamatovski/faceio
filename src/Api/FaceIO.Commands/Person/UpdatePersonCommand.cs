namespace FaceIO.Commands.Person
{
    using Contracts.Common.Database.Context;
    using Domain.Person.Entities;
    using Domain.Person.Repositories;
    using MediatR;
    using System;

    public class UpdatePersonCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid PersonUid { get; }

        public string Name { get; }

        public string? Email { get; }

        public string? Phone { get; }

        public UpdatePersonCommand(Guid customerUid, Guid personUid, string name, string? email, string? phone)
        {
            CustomerUid = customerUid;
            PersonUid = personUid;
            Name = name;
            Email = email;
            Phone = phone;
        }
    }

    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly IPersonsRepository _personsRepository;

        public UpdatePersonCommandHandler(IFaceIODbContext dbContext, IPersonsRepository personsRepository)
        {
            _dbContext = dbContext;
            _personsRepository = personsRepository;
        }

        public async Task<Unit> Handle(UpdatePersonCommand command, CancellationToken cancellationToken)
        {
            Person person = await _personsRepository.GetPersonAsync(command.CustomerUid, command.PersonUid);

            person.SetName(command.Name)
                .SetEmail(command.Email)
                .SetPhone(command.Phone);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
