using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentScoreTracker.API.Data;
using StudentScoreTracker.API.Models;

namespace StudentScoreTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Attendance/students?class=10th&section=A
        [HttpGet("students")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents([FromQuery] string? @class, [FromQuery] string? section)
        {
            if (_context.Students == null)
                return NotFound("Students table not found in the database.");

            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(@class))
                query = query.Where(s => s.Class == @class);

            if (!string.IsNullOrEmpty(section))
                query = query.Where(s => s.Section == section);

            var students = await query.ToListAsync();

            return Ok(students);
        }

        // ✅ POST: api/Attendance/mark
        [HttpPost("mark")]
        public async Task<IActionResult> MarkAttendance([FromBody] List<Attendance> records)
        {
            if (records == null || records.Count == 0)
                return BadRequest("No attendance records provided.");

            foreach (var record in records)
            {
                var exists = await _context.Attendance
                    .FirstOrDefaultAsync(a => a.StudentId == record.StudentId && a.Date.Date == record.Date.Date);

                if (exists == null)
                {
                    _context.Attendance.Add(record);
                }
                else
                {
                    exists.IsPresent = record.IsPresent;
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Attendance saved successfully!");
        }

        // ✅ GET: api/Attendance/summary
        [HttpGet("summary")]
        public async Task<ActionResult<IEnumerable<object>>> GetAttendanceSummary()
        {
            var summary = await _context.Attendance
                .Include(a => a.Student)
                .GroupBy(a => new { a.StudentId, a.Student.Name })
                .Select(g => new
                {
                    StudentName = g.Key.Name,
                    PresentDays = g.Count(a => a.IsPresent),
                    TotalDays = g.Count(),
                    AttendancePercentage = Math.Round((double)g.Count(a => a.IsPresent) / g.Count() * 100, 2)
                })
                .ToListAsync();

            return Ok(summary);
        }
    }
}
