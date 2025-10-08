using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentScoreTracker.API.Data;
using StudentScoreTracker.API.Models;

namespace StudentScoreTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GradesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Grades
        [HttpGet]
        public async Task<IActionResult> GetAllGrades()
        {
            var grades = await _context.Grades
                .Include(g => g.Student)
                .Select(g => new
                {
                    g.Id,
                    StudentName = g.Student != null ? g.Student.Name : "Unknown",
                    g.Subject,
                    g.Marks,
                    g.Date,
                    GradeLetter = GetGradeLetter(g.Marks),
                    g.StudentId
                })
                .ToListAsync();

            return Ok(grades);
        }

        // ✅ GET: api/Grades/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGradeById(int id)
        {
            var grade = await _context.Grades
                .Include(g => g.Student)
                .Where(g => g.Id == id)
                .Select(g => new
                {
                    g.Id,
                    StudentName = g.Student != null ? g.Student.Name : "Unknown",
                    g.Subject,
                    g.Marks,
                    g.Date,
                    GradeLetter = GetGradeLetter(g.Marks),
                    g.StudentId
                })
                .FirstOrDefaultAsync();

            if (grade == null)
                return NotFound(new { message = "Grade not found" });

            return Ok(grade);
        }

        // ✅ GET: api/Grades/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetGradesByStudent(int studentId)
        {
            var studentExists = await _context.Students.AnyAsync(s => s.Id == studentId);
            if (!studentExists)
                return BadRequest(new { message = $"Invalid Student ID: {studentId}" });

            var grades = await _context.Grades
                .Where(g => g.StudentId == studentId)
                .Select(g => new
                {
                    g.Id,
                    g.Subject,
                    g.Marks,
                    g.Date,
                    GradeLetter = GetGradeLetter(g.Marks)
                })
                .ToListAsync();

            var avgMarks = grades.Any() ? grades.Average(g => g.Marks) : 0;
            var overallGrade = GetGradeLetter((int)avgMarks);

            return Ok(new
            {
                StudentId = studentId,
                AverageMarks = avgMarks,
                OverallGrade = overallGrade,
                Subjects = grades
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade([FromBody] Grade grade)
        {
            if (grade == null)
                return BadRequest(new { message = "Grade data is null" });

            var studentExists = await _context.Students.AnyAsync(s => s.Id == grade.StudentId);
            if (!studentExists)
                return BadRequest(new { message = $"Invalid Student ID: {grade.StudentId}" });

            try
            {
                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Grade added successfully", grade });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error saving grade", error = ex.Message });
            }
        }


        // ✅ PUT: api/Grades/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrade(int id, [FromBody] Grade grade)
        {
            if (id != grade.Id)
                return BadRequest(new { message = "ID mismatch" });

            var existingGrade = await _context.Grades.FindAsync(id);
            if (existingGrade == null)
                return NotFound(new { message = "Grade not found" });

            existingGrade.Subject = grade.Subject;
            existingGrade.Marks = grade.Marks;
            existingGrade.Date = grade.Date;
            existingGrade.StudentId = grade.StudentId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Grade updated successfully",
                    GradeLetter = GetGradeLetter(existingGrade.Marks)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating grade", error = ex.Message });
            }
        }

        // ✅ DELETE: api/Grades/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
                return NotFound(new { message = "Grade not found" });

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Grade deleted successfully" });
        }

        // 🎯 Helper function to calculate grade
        private string GetGradeLetter(int marks)
        {
            if (marks >= 90) return "A+";
            if (marks >= 80) return "A";
            if (marks >= 70) return "B";
            if (marks >= 60) return "C";
            if (marks >= 50) return "D";
            return "F";
        }
    }
}
