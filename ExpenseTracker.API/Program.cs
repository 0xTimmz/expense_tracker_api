using ExpenseTracker.Infrastructure.Data;
using ExpenseTracker.Infrastructure.Handlers;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExpenseTracker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/expensetracker.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
                ));

            // ======================================
            // register handlers
            // ======================================
            builder.Services.AddScoped<CreateExpenseHandler>();
            builder.Services.AddScoped<UpdateExpenseHandler>();
            builder.Services.AddScoped<DeleteExpenseHandler>();
            builder.Services.AddScoped<GetAllExpensesHandler>();
            builder.Services.AddScoped<GetExpenseByIdHandler>();
            builder.Services.AddScoped<GetExpensesByCategoryHandler>();
            builder.Services.AddScoped<GetExpensesByDateRangeHandler>();
            builder.Services.AddScoped<GetMonthlySummaryHandler>();
            builder.Services.AddScoped<GetMonthlyTotalHandler>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}