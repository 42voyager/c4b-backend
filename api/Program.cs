using System;
using backend.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend
{
    public class Program
    {
        /// <summary>
        /// Este metódo <c>Main</c> inicia toda aplicação.
        /// </summary>
        /// <param name="args">Argumentos recebidos na hora da execução.</param>
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            CreateDbIfNotExists(host);
            host.Run();
        }

        /// <summary>
        /// Este metódo <c>CreateDbIfNotExists</c> cria o banco de dados caso ele não exista.
        /// </summary>
        /// <param name="host">Instância do IHost.</param>
        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SellerContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, new Models.Errors().DbCreationError.Description);
                }
            }
        }

        /// <summary>
        /// Este metódo <c>CreateHostBuilder</c> cria o builder host ao inicar o programa.
        /// </summary>
        /// <param name="args">Args recebidos no main.</param>
        /// <returns>A instância do IHostBuilder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
