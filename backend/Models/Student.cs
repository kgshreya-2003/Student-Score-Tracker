using System.ComponentModel.DataAnnotations;

namespace StudentScoreTracker.API.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Class { get; set; }

        public int RollNo { get; set; }

        public string Section { get; set; } 

        // Foreign Key to Teacher (who created/added the student)
        public int CreatedByTeacherId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<Attendance> Attendance { get; set; } = new List<Attendance>();

    }

}
