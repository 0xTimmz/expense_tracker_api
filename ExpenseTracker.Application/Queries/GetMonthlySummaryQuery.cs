using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Queries
{
    public class GetMonthlySummaryQuery
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
