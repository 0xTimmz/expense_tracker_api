using System;
using System.Collections.Generic;
using System.Text;

using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Commands
{
    public class UpdateExpenseCommand
    {
        public int Id { get; set; }
        public UpdateExpenseDto Expense { get; set; } = null!;
    }
}
