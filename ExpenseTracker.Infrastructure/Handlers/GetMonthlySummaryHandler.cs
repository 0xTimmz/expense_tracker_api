using System;
using System.Collections.Generic;
using System.Text;

using ExpenseTracker.Application.Queries;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Handlers
{
    public class MonthlySummaryDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }

    public class GetMonthlySummaryHandler
    {
        private readonly AppDbContext _context;

        public GetMonthlySummaryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MonthlySummaryDto>> Handle(GetMonthlySummaryQuery query)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.ExpenseDate.Month == query.Month && e.ExpenseDate.Year == query.Year)
                .GroupBy(e => e.Category.Name)
                .Select(g => new MonthlySummaryDto
                {
                    Category = g.Key,
                    Total = g.Sum(e => e.Amount)
                })
                .ToListAsync();
        }
    }
}
