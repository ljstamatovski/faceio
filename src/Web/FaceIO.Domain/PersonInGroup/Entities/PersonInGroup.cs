namespace FaceIO.Domain.PersonInGroup.Entities
{
    using Common.Entities;
    using Group.Entities;
    using Person.Entities;
    using System.ComponentModel.DataAnnotations.Schema;

    internal class PersonInGroup : Entity
    {
        public int GroupFk { get; protected internal set; }

        [ForeignKey(nameof(GroupFk))]
        public Group Group { get; protected internal set; } = null!;

        public int PersonFk { get; protected internal set; }

        [ForeignKey(nameof(PersonFk))]
        public Person Person { get; protected internal set; } = null!;
    }
}