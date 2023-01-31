namespace FaceIO.Domain.Face.Entities
{
    using Common.Entities;
    using Person.Entities;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Face : Entity
    {
        public int PersonFk { get; protected internal set; }

        [ForeignKey(nameof(PersonFk))]
        public Person Person { get; protected internal set; } = null!;

        public string FileName { get; protected internal set; } = string.Empty;

        public void MarkAsDeleted()
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not delete face, face is already deleted.");
            }

            DeletedOn = DateTime.UtcNow;
        }

        public static class Factory
        {
            public static Face Create(string fileName, int personId)
            {
                return new Face
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