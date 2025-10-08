using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentScoreTracker.API.Models
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Student")]
        public int? StudentId { get; set; }

        public Student? Student { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [Range(0, 100)]
        public int Marks { get; set; }


        [Required]
        public DateTime Date { get; set; }
    }
}
