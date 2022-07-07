namespace FaceIO.Domain.PersonImage.Entities
{
    using Common.Entities;
    using Person.Entities;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PersonImage : Entity
    {
        public int PersonFk { get; protected internal set; }

        [ForeignKey(nameof(PersonFk))]
        public Person Person { get; protected internal set; } = null!;
    }
}