namespace FaceIO.Domain.PersonInGroup.Entities
{
    using Common.Entities;
    using Group.Entities;
    using Person.Entities;
    using System.ComponentModel.DataAnnotations.Schema;

    internal class PersonInGroup : Entity
    {
        public int GroupFk { get; set; }

        [ForeignKey(nameof(GroupFk))]
        public Group Group { get; set; } = null!;

        public int PersonFk { get; set; }

        [ForeignKey(nameof(PersonFk))]
        public Person Person { get; set; } = null!;
    }
}