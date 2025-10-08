using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentScoreTracker.API.Data;
using StudentScoreTracker.API.Models;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/students?class=1&section=A&stream=A
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents(
            [FromQuery(Name = "class")] string? classFilter,
            [FromQuery] string? section,
            [FromQuery] string? stream)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(classFilter))
                query = query.Where(s => s.Class == classFilter);

            if (!string.IsNullOrEmpty(section))
                query = query.Where(s => s.Section == section);

            if (!string.IsNullOrEmpty(stream))
                query = query.Where(s => s.Class.EndsWith(stream));

            return await query.ToListAsync();
        }

        // ✅ GET by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            return student;
        }

        // ✅ POST: Add student
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // ✅ PUT: Update student
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
                return BadRequest();

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // ✅ DELETE student
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
