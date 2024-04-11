using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagmentAPI.Model;

namespace StudentManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StudentDetailsController : ControllerBase
    {
        private readonly StudentDetailContext _context;

        public StudentDetailsController(StudentDetailContext context)
        {
            _context = context;
        }

        // GET: api/StudentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDetail>>> GetstudentDetails()
        {
            var userid = User.Claims.First(c => c.Type == "UserID").Value;
            return await _context.studentDetails.Where(c=>c.UserId == userid).ToListAsync();
        }

        // GET: api/StudentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDetail>> GetStudentDetail(int id)
        {
            var studentDetail = await _context.studentDetails.FindAsync(id);

            if (studentDetail == null)
            {
                return NotFound();
            }

            return studentDetail;
        }

        // PUT: api/StudentDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentDetail(int id, StudentDetail studentDetail)
        {
            if (id != studentDetail.StdId)
            {
                return BadRequest();
            }

            _context.Entry(studentDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StudentDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<StudentDetail>> PostStudentDetail(StudentDetail studentDetail)
        {
            var userid = User.Claims.First(c => c.Type == "UserID").Value;
            studentDetail.UserId = userid;
            _context.studentDetails.Add(studentDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentDetail", new { id = studentDetail.StdId }, studentDetail);
        }

        // DELETE: api/StudentDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StudentDetail>> DeleteStudentDetail(int id)
        {
            var studentDetail = await _context.studentDetails.FindAsync(id);
            if (studentDetail == null)
            {
                return NotFound();
            }

            _context.studentDetails.Remove(studentDetail);
            await _context.SaveChangesAsync();

            return studentDetail;
        }

        private bool StudentDetailExists(int id)
        {
            return _context.studentDetails.Any(e => e.StdId == id);
        }
    }
}
