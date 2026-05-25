using System;
using System.Collections.Generic;
using System.Text;

using ExpenseTracker.Application.Commands;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Handlers
{
    public class UpdateExpenseHandler
    {
        private readonly AppDbContext _context;

        public UpdateExpenseHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateExpenseCommand command)
        {
            var expense = await _context.Expenses.FindAsync(command.Id);

            if (expense == null) return false;

            expense.Amount = command.Expense.Amount;
            expense.CategoryId = command.Expense.CategoryId;
            expense.ExpenseDate = command.Expense.ExpenseDate;
            expense.Description = command.Expense.Description;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
