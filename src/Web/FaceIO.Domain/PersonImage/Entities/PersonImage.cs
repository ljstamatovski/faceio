namespace FaceIO.Domain.PersonImage.Entities
{
    using Common.Entities;
    using Person.Entities;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PersonImage : Entity
    {
        public int PersonFk { get; set; }

        [ForeignKey(nameof(PersonFk))]
        public Person Person { get; set; } = null!;

        public string FileName { get; set; } = string.Empty;

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