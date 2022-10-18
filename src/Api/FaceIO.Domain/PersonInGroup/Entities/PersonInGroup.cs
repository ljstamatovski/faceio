namespace FaceIO.Domain.PersonInGroup.Entities
{
    using Common.Entities;
    using Group.Entities;
    using Person.Entities;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public sealed class PersonInGroup : Entity
    {
        public int GroupFk { get; set; }

        [ForeignKey(nameof(GroupFk))]
        public Group Group { get; set; } = null!;

        public int PersonFk { get; set; }

        [ForeignKey(nameof(PersonFk))]
        public Person Person { get; set; } = null!;

        public void MarkAsDeleted()
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not delete person in group, person in group is already deleted.");
            }

            DeletedOn = DateTime.UtcNow;
        }

        public static class Factory
        {
            public static PersonInGroup Create(int groupId, int personId)
            {
                return new PersonInGroup
                {
                    Uid = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow,
                    GroupFk = groupId,
                    PersonFk = personId
                };
            }
        }
    }
}