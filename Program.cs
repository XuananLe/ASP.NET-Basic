using BookService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
namespace BookService;

public abstract class Program{
    public static void Main(string []args)
    {
        var builder = WebApplication.CreateBuilder();
        
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // Add the database exception filter
        builder.Services.AddControllersWithViews();



        // add DI to create the EF Instance and SQL Server connection
        builder.Services.AddDbContext<SchoolContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        var services = app.Services; 
        try
        {
            var scope = app.Services.CreateScope(); // Create a new scope to retrieve scoped services
            var context = scope.ServiceProvider.GetService<SchoolContext>();
            context?.Database.EnsureCreated();
            if (context != null) DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
        
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}