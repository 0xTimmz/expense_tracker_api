using ExpenseTracker.Application.Commands;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Queries;
using ExpenseTracker.Infrastructure.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [ApiController]
    [Route("api/v1/expenses")]
    public class ExpensesController : ControllerBase
    {
        private readonly CreateExpenseHandler _createHandler;
        private readonly UpdateExpenseHandler _updateHandler;
        private readonly DeleteExpenseHandler _deleteHandler;
        private readonly GetAllExpensesHandler _getAllHandler;
        private readonly GetExpenseByIdHandler _getByIdHandler;
        private readonly GetExpensesByCategoryHandler _getByCategoryHandler;
        private readonly GetExpensesByDateRangeHandler _getByDateRangeHandler;
        private readonly ILogger<ExpensesController> _logger;

        public ExpensesController(
            CreateExpenseHandler createHandler,
            UpdateExpenseHandler updateHandler,
            DeleteExpenseHandler deleteHandler,
            GetAllExpensesHandler getAllHandler,
            GetExpenseByIdHandler getByIdHandler,
            GetExpensesByCategoryHandler getByCategoryHandler,
            GetExpensesByDateRangeHandler getByDateRangeHandler,
            ILogger<ExpensesController> logger)
        {
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            _deleteHandler = deleteHandler;
            _getAllHandler = getAllHandler;
            _getByIdHandler = getByIdHandler;
            _getByCategoryHandler = getByCategoryHandler;
            _getByDateRangeHandler = getByDateRangeHandler;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExpenseDto dto)
        {
            _logger.LogInformation("[Command] CreateExpense called at {Time}", DateTime.UtcNow);

            if (dto.Amount <= 0)
            {
                _logger.LogWarning("[Validation] CreateExpense failed - Amount <= 0");
                return BadRequest(new { error = "Amount must be greater than 0", field = "Amount" });
            }

            if (dto.ExpenseDate > DateTime.UtcNow)
            {
                _logger.LogWarning("[Validation] CreateExpense failed - future date");
                return BadRequest(new { error = "Expense date cannot be in the future", field = "ExpenseDate" });
            }

            try
            {
                var command = new CreateExpenseCommand { Expense = dto };
                var id = await _createHandler.Handle(command);
                _logger.LogInformation("[Command] CreateExpense succeeded - ID: {Id}", id);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Exception] CreateExpense failed unexpectedly");
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExpenseDto dto)
        {
            _logger.LogInformation("[Command] UpdateExpense called for ID: {Id} at {Time}", id, DateTime.UtcNow);

            if (dto.Amount <= 0)
            {
                _logger.LogWarning("[Validation] UpdateExpense failed - Amount <= 0");
                return BadRequest(new { error = "Amount must be greater than 0", field = "Amount" });
            }

            if (dto.ExpenseDate > DateTime.UtcNow)
            {
                _logger.LogWarning("[Validation] UpdateExpense failed - future date");
                return BadRequest(new { error = "Expense date cannot be in the future", field = "ExpenseDate" });
            }

            try
            {
                var command = new UpdateExpenseCommand { Id = id, Expense = dto };
                var result = await _updateHandler.Handle(command);

                if (!result)
                {
                    _logger.LogWarning("[Command] UpdateExpense - ID: {Id} not found", id);
                    return NotFound(new { error = "Expense not found", field = "id" });
                }

                _logger.LogInformation("[Command] UpdateExpense succeeded for ID: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Exception] UpdateExpense failed unexpectedly");
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("[Command] DeleteExpense called for ID: {Id} at {Time}", id, DateTime.UtcNow);

            try
            {
                var command = new DeleteExpenseCommand { Id = id };
                var result = await _deleteHandler.Handle(command);

                if (!result)
                {
                    _logger.LogWarning("[Command] DeleteExpense - ID: {Id} not found", id);
                    return NotFound(new { error = "Expense not found", field = "id" });
                }

                _logger.LogInformation("[Command] DeleteExpense succeeded for ID: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Exception] DeleteExpense failed unexpectedly");
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("[Query] GetAllExpenses called at {Time}", DateTime.UtcNow);
            var result = await _getAllHandler.Handle(new GetAllExpensesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("[Query] GetExpenseById called for ID: {Id}", id);
            var result = await _getByIdHandler.Handle(new GetExpenseByIdQuery { Id = id });

            if (result == null)
            {
                _logger.LogWarning("[Query] GetExpenseById - ID: {Id} not found", id);
                return NotFound(new { error = "Expense not found", field = "id" });
            }

            return Ok(result);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            _logger.LogInformation("[Query] GetExpensesByCategory called for CategoryId: {CategoryId}", categoryId);
            var result = await _getByCategoryHandler.Handle(new GetExpensesByCategoryQuery { CategoryId = categoryId });
            return Ok(result);
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            _logger.LogInformation("[Query] GetExpensesByDateRange called from {Start} to {End}", startDate, endDate);
            var result = await _getByDateRangeHandler.Handle(new GetExpensesByDateRangeQuery
            {
                StartDate = startDate,
                EndDate = endDate
            });
            return Ok(result);
        }
    }
}