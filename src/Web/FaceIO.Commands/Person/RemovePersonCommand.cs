namespace FaceIO.Commands.Person
{
    using Domain.Person.Entities;
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Domain.Person.Repositories;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RemovePersonCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid PersonUid { get; }

        public RemovePersonCommand(Guid customerUid, Guid personUid)
        {
            CustomerUid = customerUid;
            PersonUid = personUid;
        }
    }

    public class RemovePersonCommandHandler : IRequestHandler<RemovePersonCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly IPersonsRepository _personsRepository;

        public RemovePersonCommandHandler(IFaceIODbContext dbContext, IPersonsRepository personsRepository)
        {
            _dbContext = dbContext;
            _personsRepository = personsRepository;
        }

        public async Task<Unit> Handle(RemovePersonCommand request, CancellationToken cancellationToken)
        {
            Person person = await _personsRepository.GetPersonAsync(request.CustomerUid, request.PersonUid);

            person.MarkAsDeleted();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
