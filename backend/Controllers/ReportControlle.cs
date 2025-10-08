using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentScoreTracker.API.Data;
using StudentScoreTracker.API.Models;

namespace StudentScoreTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/reports/students?class=1st§ion=A
        [HttpGet("students")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents(
            [FromQuery(Name = "class")] string classFilter,
            [FromQuery] string section)
        {
            if (string.IsNullOrEmpty(classFilter) || string.IsNullOrEmpty(section))
                return BadRequest("Class and Section are required");

            var students = await _context.Students
                .Where(s => s.Class == classFilter && s.Section == section)
                .Select(s => new { s.Id, s.Name })
                .ToListAsync();

            return Ok(students);
        }

        // GET: api/reports?student=John&class=1st§ion=A&startDate=2025-01-01&endDate=2025-10-06
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetReport(
            [FromQuery] string student,
            [FromQuery(Name = "class")] string classFilter,
            [FromQuery] string section,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var studentEntity = await _context.Students
                .FirstOrDefaultAsync(s => s.Name == student && s.Class == classFilter && s.Section == section);

            if (studentEntity == null) return NotFound("Student not found");

            var subjects = new[] { "English", "Math", "Science" };
            var report = new List<object>();

            foreach (var subj in subjects)
            {
                // Attendance
                var attendanceRecords = _context.Attendance
                    .Where(a => a.StudentId == studentEntity.Id &&
                                a.Date >= (startDate ?? DateTime.MinValue) &&
                                a.Date <= (endDate ?? DateTime.MaxValue));

                var totalDays = await attendanceRecords.CountAsync();
                var presentDays = await attendanceRecords.CountAsync(a => a.Status == "Present");
                var attendancePercent = totalDays > 0 ? ((double)presentDays / totalDays * 100).ToString("0") + "%" : "0%";

                // Feedback
                var feedbackText = await _context.Feedbacks
                    .Where(f => f.StudentId == studentEntity.Id &&
                                f.Subject == subj &&
                                f.Date >= (startDate ?? DateTime.MinValue) &&
                                f.Date <= (endDate ?? DateTime.MaxValue))
                    .OrderByDescending(f => f.Date)
                    .Select(f => f.FeedbackText)
                    .FirstOrDefaultAsync() ?? "";

                // Mock grade (replace with actual Grade entity if exists)
                var grade = "A";

                report.Add(new { subject = subj, grade, attendance = attendancePercent, feedback = feedbackText });
            }

            return Ok(report);
        }

        // POST: api/reports/feedback
        [HttpPost("feedback")]
        public async Task<IActionResult> SaveFeedback([FromBody] FeedbackDto dto)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Name == dto.Student);
            if (student == null) return NotFound("Student not found");

            var feedback = new Feedback
            {
                StudentId = student.Id,
                Subject = dto.Subject,
                FeedbackText = dto.Feedback,
                Date = DateTime.UtcNow
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Feedback saved successfully" });
        }
    }

    // DTO for saving feedback
    public class FeedbackDto
    {
        public string Student { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Feedback { get; set; } = string.Empty;
    }
}
