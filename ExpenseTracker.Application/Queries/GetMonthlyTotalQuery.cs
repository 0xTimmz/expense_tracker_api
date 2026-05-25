using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Queries
{
    public class GetMonthlyTotalQuery
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
