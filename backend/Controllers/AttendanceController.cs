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

        // ✅ Get all attendance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAttendance()
        {
            var records = await _context.Attendance
                .Include(a => a.Student)
                .Select(a => new
                {
                    a.Id,
                    a.StudentId,
                    StudentName = a.Student.Name,
                    a.Date,
                    a.Status
                })
                .ToListAsync();

            return Ok(records);
        }

        // ✅ Add attendance
        [HttpPost]
        public async Task<IActionResult> AddAttendance([FromBody] Attendance attendance)
        {
            if (attendance == null)
                return BadRequest(new { message = "Attendance data is null" });

            var studentExists = await _context.Students.AnyAsync(s => s.Id == attendance.StudentId);
            if (!studentExists)
                return BadRequest(new { message = $"Invalid Student ID: {attendance.StudentId}" });

            try
            {
                _context.Attendance.Add(attendance);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Attendance saved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error saving attendance", error = ex.Message });
            }
        }

        // ✅ Update attendance
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendance(int id, [FromBody] Attendance updated)
        {
            if (id != updated.Id)
                return BadRequest(new { message = "ID mismatch" });

            var existing = await _context.Attendance.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Attendance record not found" });

            existing.Status = updated.Status;
            existing.Date = updated.Date;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Attendance updated successfully" });
        }

        // ✅ Delete attendance
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            var record = await _context.Attendance.FindAsync(id);
            if (record == null)
                return NotFound(new { message = "Record not found" });

            _context.Attendance.Remove(record);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Attendance deleted successfully" });
        }
    }
}
