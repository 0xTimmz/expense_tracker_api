using System;
using System.Collections.Generic;
using System.Text;

using ExpenseTracker.Application.Commands;
using ExpenseTracker.Infrastructure.Data;

namespace ExpenseTracker.Infrastructure.Handlers
{
    public class DeleteExpenseHandler
    {
        private readonly AppDbContext _context;

        public DeleteExpenseHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteExpenseCommand command)
        {
            var expense = await _context.Expenses.FindAsync(command.Id);

            if (expense == null) return false;

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
