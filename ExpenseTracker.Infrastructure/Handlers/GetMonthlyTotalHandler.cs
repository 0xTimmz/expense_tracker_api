using System;
using System.Collections.Generic;
using System.Text;

using ExpenseTracker.Application.Queries;
using ExpenseTracker.Infrastructure.Data;

namespace ExpenseTracker.Infrastructure.Handlers
{
    public class GetMonthlyTotalHandler
    {
        private readonly AppDbContext _context;

        public GetMonthlyTotalHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> Handle(GetMonthlyTotalQuery query)
        {
            return await Task.FromResult(
                _context.Expenses
                    .Where(e => e.ExpenseDate.Month == query.Month && e.ExpenseDate.Year == query.Year)
                    .Sum(e => e.Amount)
            );
        }
    }
}
