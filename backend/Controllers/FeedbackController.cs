using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentScoreTracker.API.Data;
using StudentScoreTracker.API.Models;

namespace StudentScoreTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Feedback
        [HttpPost]
        public async Task<IActionResult> AddFeedbacks([FromBody] List<Feedback> feedbacks)
        {
            if (feedbacks == null || !feedbacks.Any())
                return BadRequest(new { message = "No feedbacks provided." });

            var allErrors = new List<string>();

            foreach (var feedback in feedbacks)
            {
                // Validate model attributes (e.g., [Required])
                var context = new ValidationContext(feedback, null, null);
                var results = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(feedback, context, results, true);

                if (!isValid)
                {
                    allErrors.AddRange(results.Select(r => r.ErrorMessage));
                }

                // Check StudentId > 0
                if (feedback.StudentId <= 0)
                {
                    allErrors.Add($"StudentId must be greater than zero. Invalid value: {feedback.StudentId}");
                    continue; // no point checking DB if invalid Id
                }

                // Check student exists in DB
                bool studentExists = await _context.Students.AnyAsync(s => s.Id == feedback.StudentId);
                if (!studentExists)
                {
                    allErrors.Add($"Invalid StudentId: {feedback.StudentId}");
                }

                // Optional: validate Date is not default
                if (feedback.Date == default)
                {
                    allErrors.Add($"Invalid Date for StudentId: {feedback.StudentId}");
                }
            }

            if (allErrors.Any())
            {
                return BadRequest(new { message = "Validation failed", errors = allErrors });
            }

            try
            {
                _context.Feedbacks.AddRange(feedbacks);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Feedback saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error saving feedback.", error = ex.Message });
            }
        }
    }
}
