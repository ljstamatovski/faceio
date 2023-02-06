namespace FaceIO.Commands.Location
{
    using Amazon.Rekognition;
    using Amazon.Rekognition.Model;
    using Contracts.Common.Database.Context;
    using Domain.Customer.Entities;
    using Domain.Customer.Repositories;
    using Domain.Location.Entities;
    using MediatR;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddLocationCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public string Name { get; }

        public string? Description { get; }

        public AddLocationCommand(Guid customerUid, string name, string? description)
        {
            Name = name;
            Description = description;
            CustomerUid = customerUid;
        }
    }

    public class AddLocationCommandHandler : IRequestHandler<AddLocationCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly ICustomersRepository _customersRepository;
        private readonly IAmazonRekognition _awsRekognition;

        public AddLocationCommandHandler(
            IFaceIODbContext dbContext,
            ICustomersRepository customersRepository,
            IAmazonRekognition awsRekognition)
        {
            _dbContext = dbContext;
            _customersRepository = customersRepository;
            _awsRekognition = awsRekognition;
        }

        public async Task<Unit> Handle(AddLocationCommand request, CancellationToken cancellationToken)
        {
            Customer customer = await _customersRepository.GetCustomerAsync(request.CustomerUid);

            Location location = Location.Factory.Create(name: request.Name,
                                                        description: request.Description,
                                                        customer: customer);

            _dbContext.Set<Location>().Add(location);

            var createCollectionRequest = new CreateCollectionRequest()
            {
                CollectionId = location.CollectionId
            };

            CreateCollectionResponse response = await _awsRekognition.CreateCollectionAsync(createCollectionRequest, cancellationToken);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                //
            }


            return Unit.Value;
        }
    }
}