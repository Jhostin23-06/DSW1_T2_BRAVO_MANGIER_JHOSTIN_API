using Library.Application.DTOs.Loan;
using Library.Application.Interfaces.Loan;
using Library.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        // GET: api/loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetLoans()
        {
            try
            {
                var loans = await _loanService.GetAllLoansAsync();
                return Ok(loans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // GET: api/loans/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetActiveLoans()
        {
            try
            {
                var loans = await _loanService.GetActiveLoansAsync();
                return Ok(loans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // GET: api/loans/student/{studentName}
        [HttpGet("student/{studentName}")]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetLoansByStudent(string studentName)
        {
            try
            {
                var loans = await _loanService.GetLoansByStudentAsync(studentName);
                return Ok(loans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // GET: api/loans/book/{bookId}
        [HttpGet("book/{bookId}")]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetLoansByBook(int bookId)
        {
            try
            {
                var loans = await _loanService.GetLoansByBookAsync(bookId);
                return Ok(loans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // GET: api/loans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDto>> GetLoan(int id)
        {
            try
            {
                var loan = await _loanService.GetLoanByIdAsync(id);

                if (loan == null)
                {
                    return NotFound($"Préstamo con ID {id} no encontrado");
                }

                return Ok(loan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // POST: api/loans
        [HttpPost]
        public async Task<ActionResult<LoanDto>> CreateLoan([FromBody] CreateLoanDto createLoanDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var loan = await _loanService.CreateLoanAsync(createLoanDto);
                return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loan);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // PUT: api/loans/5/return
        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnLoan(int id)
        {
            try
            {
                var loanExists = await _loanService.LoanExistsAsync(id);
                if (!loanExists)
                {
                    return NotFound($"Préstamo con ID {id} no encontrado");
                }

                var returnedLoan = await _loanService.ReturnLoanAsync(id);
                return Ok(returnedLoan);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // DELETE: api/loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            try
            {
                var result = await _loanService.DeleteLoanAsync(id);

                if (!result)
                {
                    return NotFound($"Préstamo con ID {id} no encontrado");
                }

                return NoContent();
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

    }
}
