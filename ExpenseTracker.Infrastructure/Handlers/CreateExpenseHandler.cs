using System;
using System.Collections.Generic;
using System.Text;

using ExpenseTracker.Application.Commands;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Data;

namespace ExpenseTracker.Infrastructure.Handlers
{
    public class CreateExpenseHandler
    {
        private readonly AppDbContext _context;

        public CreateExpenseHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateExpenseCommand command)
        {
            var expense = new Expense
            {
                Amount = command.Expense.Amount,
                CategoryId = command.Expense.CategoryId,
                ExpenseDate = command.Expense.ExpenseDate,
                Description = command.Expense.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return expense.Id;
        }
    }
}
