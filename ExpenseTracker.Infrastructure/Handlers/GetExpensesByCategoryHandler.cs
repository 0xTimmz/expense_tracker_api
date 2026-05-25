using System;
using System.Collections.Generic;
using System.Text;

using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Queries;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Handlers
{
    public class GetExpensesByCategoryHandler
    {
        private readonly AppDbContext _context;

        public GetExpensesByCategoryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExpenseDto>> Handle(GetExpensesByCategoryQuery query)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.CategoryId == query.CategoryId)
                .Select(e => new ExpenseDto
                {
                    Id = e.Id,
                    Amount = e.Amount,
                    Category = e.Category.Name,
                    ExpenseDate = e.ExpenseDate,
                    Description = e.Description,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();
        }
    }
}
