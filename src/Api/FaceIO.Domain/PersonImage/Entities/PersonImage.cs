namespace FaceIO.Domain.PersonImage.Entities
{
    using Common.Entities;
    using Person.Entities;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PersonImage : Entity
    {
        public int PersonFk { get; protected internal set; }

        [ForeignKey(nameof(PersonFk))]
        public Person Person { get; protected internal set; } = null!;

        public string FileName { get; protected internal set; } = string.Empty;

        public void MarkAsDeleted()
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not delete person image, person image is already deleted.");
            }

            DeletedOn = DateTime.UtcNow;
        }

        public static class Factory
        {
            public static PersonImage Create(string fileName, int personId)
            {
                return new PersonImage
                {
                    Uid = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow,
                    FileName = fileName,
                    PersonFk = personId
                };
            }
        }
    }
}