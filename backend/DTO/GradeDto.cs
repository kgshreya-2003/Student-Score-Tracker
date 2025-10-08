namespace StudentScoreTracker.API.DTOs
{
    public class GradeDto
    {
        public int? Id { get; set; }
        public int? StudentId { get; set; }
        public string? Student { get; set; } // name
        public string Subject { get; set; } = string.Empty;
        public int Marks { get; set; }
        public string Date { get; set; } = string.Empty;
     

    }
}
