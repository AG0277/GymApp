using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymApp.Models
{
    public class AppUser : IdentityUser
    {

        [ForeignKey("Adress")]
        public int AdressId { get; set; }
        public ICollection<Meal> Meal { get; set; }

    }
}
