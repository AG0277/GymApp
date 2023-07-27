using System.ComponentModel.DataAnnotations;

namespace GymApp.Models
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public ICollection<Meal> Meal { get; set; }

    }
}
