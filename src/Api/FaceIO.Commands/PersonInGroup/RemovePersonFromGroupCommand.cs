namespace FaceIO.Commands.PersonInGroup
{
    using Amazon.Rekognition;
    using Amazon.Rekognition.Model;
    using Contracts.Common.Database.Context;
    using Domain.PersonInGroup.Entities;
    using Domain.PersonInGroup.Repositories;
    using FaceIO.Domain.Location.Entities;
    using FaceIO.Domain.Location.Repositories;
    using FaceIO.Domain.Person.Repositories;
    using FaceIO.Domain.PersonAccessToLocation.Entities;
    using MediatR;
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class RemovePersonFromGroupCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public Guid PersonUid { get; }

        public RemovePersonFromGroupCommand(Guid customerUid, Guid groupUid, Guid personUid)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
            PersonUid = personUid;
        }
    }

    public class RemovePersonFromGroupCommandHandler : IRequestHandler<RemovePersonFromGroupCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly IAmazonRekognition _awsRekognition;
        private readonly IPersonsRepository _personsRepository;
        private readonly IPersonsInGroupsRepository _personsInGroupsRepository;

        public RemovePersonFromGroupCommandHandler(
            IFaceIODbContext dbContext,
            IAmazonRekognition awsRekognition,
            IPersonsRepository personsRepository,
            IPersonsInGroupsRepository personsInGroupsRepository)
        {
            _dbContext = dbContext;
            _awsRekognition = awsRekognition;
            _personsRepository = personsRepository;
            _personsInGroupsRepository = personsInGroupsRepository;
        }

        public async Task<Unit> Handle(RemovePersonFromGroupCommand command, CancellationToken cancellationToken)
        {
            PersonInGroup personInGroup = await _personsInGroupsRepository.GetPersonInGroupAsync(
                customerUid: command.CustomerUid,
                groupUid: command.GroupUid,
                personUid: command.PersonUid);

            personInGroup.MarkAsDeleted();

            IReadOnlyList<PersonAccessToLocation> personAccessToLocations = await _personsRepository.GetPersonAccessToLocationsAsync(customerUid: command.CustomerUid,
                                                                                                                                     groupUid: command.GroupUid,
                                                                                                                                     personUid: command.PersonUid);

            foreach (PersonAccessToLocation personAccessToLocation in personAccessToLocations)
            {
                personAccessToLocation.MarkAsDeleted();

                if (personAccessToLocation.FaceId != null)
                {
                    var deleteFacesRequest = new DeleteFacesRequest
                    {
                        CollectionId = personAccessToLocation.GroupAccessToLocation.Location.CollectionId,
                        FaceIds = new List<string> { personAccessToLocation.FaceId }
                    };

                    DeleteFacesResponse response = await _awsRekognition.DeleteFacesAsync(deleteFacesRequest, cancellationToken);

                    if (response.HttpStatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception();
                    }
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}