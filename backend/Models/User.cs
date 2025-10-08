using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentScoreTracker.API.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }  // ✅ FIXED: Changed from int to string

        public ICollection<Grade>? Grades { get; set; }
    }
}
