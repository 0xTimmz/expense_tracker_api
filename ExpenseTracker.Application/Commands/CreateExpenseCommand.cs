using System;
using System.Collections.Generic;
using System.Text;
using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Commands
{
    public class CreateExpenseCommand
    {
        public CreateExpenseDto Expense { get; set; } = null!;
    }
}
