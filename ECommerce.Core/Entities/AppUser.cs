using System.ComponentModel.DataAnnotations;
namespace ECommerce.Core.Entities
{
    public class AppUser : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string? UserName { get; set; } 
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public Guid UserGuid { get; set; } = Guid.NewGuid();

        public List<Adress?> Adresses { get; set; }
    }
}
