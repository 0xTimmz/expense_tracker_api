using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.DTOs
{
    public class CreateExpenseDto
    {
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? Description { get; set; }
    }
}
