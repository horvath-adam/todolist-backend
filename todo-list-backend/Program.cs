using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using todo_list_backend.Data;

namespace todo_list_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = CreateHostBuilder(args);
                var host = builder.Build();
                using (var scope = host.Services.CreateScope())
                {
                    var todoListDbContext = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();
                    todoListDbContext.Database.Migrate();
                }

                host.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine($"CONSOLE: {e.Message} - {e.StackTrace}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
