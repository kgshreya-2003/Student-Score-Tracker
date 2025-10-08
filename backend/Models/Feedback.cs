using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using StudentScoreTracker.API.Models;

public class Feedback
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [ForeignKey("StudentId")]
    [JsonIgnore] // prevents sending full Student object
    public Student Student { get; set; }

    [Required]
    public string Feedbacks { get; set; }  // must match exactly

    [Required]
    public DateTime Date { get; set; } // must be a valid DateTime
    public string Subject { get; internal set; }
    public string FeedbackText { get; internal set; }
}