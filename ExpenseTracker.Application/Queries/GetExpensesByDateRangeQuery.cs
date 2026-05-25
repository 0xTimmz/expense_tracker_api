using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Queries
{
    public class GetExpensesByDateRangeQuery
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
