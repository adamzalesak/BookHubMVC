using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class User : IdentityUser
    {
        public String Name { get; set; }
        public bool IsAdministrator { get; set; } = false;
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
