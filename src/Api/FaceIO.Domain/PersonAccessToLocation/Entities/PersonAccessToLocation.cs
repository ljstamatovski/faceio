namespace FaceIO.Domain.PersonAccessToLocation.Entities
{
    using FaceIO.Domain.Common.Entities;
    using FaceIO.Domain.GroupAccessToLocation.Entities;
    using FaceIO.Domain.Person.Entities;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PersonAccessToLocation : Entity
    {
        public int PersonFk { get; protected internal set; }

        [ForeignKey(nameof(PersonFk))]
        public Person Person { get; protected internal set; } = null!;

        public int GroupAccessToLocationFk { get; protected internal set; }

        [ForeignKey(nameof(GroupAccessToLocationFk))]
        public GroupAccessToLocation GroupAccessToLocation { get; protected internal set; } = null!;

        public string? FaceId { get; protected internal set; }

        public void MarkAsDeleted()
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not delete person access to location, person access to location is already deleted.");
            }

            DeletedOn = DateTime.UtcNow;
        }

        public static class Factory
        {
            public static PersonAccessToLocation Create(Person person, GroupAccessToLocation groupAccessToLocation, string? faceId)
            {
                return new PersonAccessToLocation
                {
                    Uid = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow,
                    FaceId = faceId,
                    Person = person,
                    PersonFk = person.Id,
                    GroupAccessToLocation = groupAccessToLocation,
                    GroupAccessToLocationFk = groupAccessToLocation.Id
                };
            }
        }
    }
}
