using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using backend.Interfaces;
using backend.Services;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "backend", Version = "v1" });
            });
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IEmailService<Customer>, EmailService<Customer>>();
            services.AddScoped<IEmailService<Feedback>, EmailService<Feedback>>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IFeedbackStarService, FeedbackStarService>();
            services.AddScoped<IRecaptchaService, RecaptchaService>();
            services.AddScoped<IBankInfoService, BankInfoService>();
			services.AddScoped<ICreditService, CreditService>();
			services.AddScoped<IContractService, ContractService>();
			services.AddScoped<ICreatePdfService, CreatePdfService>();

            services.AddHttpClient<IRecaptchaService,RecaptchaService>();

            services.AddDbContext<SellerContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("SellerContext")));
            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "backend v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
