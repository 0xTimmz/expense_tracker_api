using System;
using System.Collections.Generic;
using System.Text;

using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Queries;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Handlers
{
    public class GetExpenseByIdHandler
    {
        private readonly AppDbContext _context;

        public GetExpenseByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ExpenseDto?> Handle(GetExpenseByIdQuery query)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.Id == query.Id)
                .Select(e => new ExpenseDto
                {
                    Id = e.Id,
                    Amount = e.Amount,
                    Category = e.Category.Name,
                    ExpenseDate = e.ExpenseDate,
                    Description = e.Description,
                    CreatedAt = e.CreatedAt
                })
                .FirstOrDefaultAsync();
        }
    }
}
