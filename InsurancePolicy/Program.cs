using InsurancePolicy.Data;
using InsurancePolicy.Exceptions;
using InsurancePolicy.Mappers;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using InsurancePolicy.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

namespace InsurancePolicy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("connstring"));
                options.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("*");
                });
            });


            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddTransient<IRoleService, RoleService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IAdminService, AdminService>();
            builder.Services.AddTransient<IAgentService, AgentService>();
            builder.Services.AddTransient<ICustomerService, CustomerService>();
            builder.Services.AddTransient<IDocumentService, DocumentService>();
            builder.Services.AddTransient<IEmployeeService, EmployeeService>();
            builder.Services.AddTransient<IInsurancePlanService, InsurancePlanService>();
            builder.Services.AddTransient<IInsuranceSchemeService, InsuranceSchemeService>();
            builder.Services.AddTransient<IPolicyService, PolicyService>();
            builder.Services.AddTransient<IPaymentService,PaymentService>();    
            builder.Services.AddTransient<IClaimService, ClaimService>();
            builder.Services.AddTransient<IInsuranceSettingsService, InsuranceSettingsService>();
            builder.Services.AddTransient<ITaxSettingsService, TaxSettingsService>();
            builder.Services.AddTransient<INomineeService,NomineeService>();
            builder.Services.AddTransient<IInstallmentService, InstallmentService>();
            builder.Services.AddTransient<IPaymentService, PaymentService>();
            builder.Services.AddTransient<IWithdrawalRequestService, WithdrawalRequestService>();
            builder.Services.AddTransient<ICommissionService, CommissionService>();
            builder.Services.AddTransient<ICustomerQueryService, CustomerQueryService>();
            builder.Services.AddSingleton<EmailService>();

            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

 

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                   .GetBytes(builder.Configuration.GetSection("AppSettings:Key").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            builder.Services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                .WriteTo.File("Logs/app-log.txt", rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true, shared: true).CreateLogger();

            builder.Services.AddExceptionHandler<AppExceptionHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler(_ => { });
            app.UseHttpsRedirection();
            app.UseCors("AllowAngularApp");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
