using System.Text;
using E_voting.DB_CONTEXT;
using E_voting.JWT;
using E_voting.Model;
using E_voting.Repository;
using E_voting.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace E_voting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext
            builder.Services.AddDbContext<EVotingDbContext>(options =>
            {
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
                options.UseMySql(builder.Configuration.GetConnectionString("AppDatabase"), serverVersion);
            });

            // Repositories
            builder.Services.AddTransient<ICommonRepository<Admin>, CommonRepository<Admin>>();
            builder.Services.AddTransient<ICommonRepository<Candidate>, CommonRepository<Candidate>>();
            builder.Services.AddTransient<ICommonRepository<Election>, CommonRepository<Election>>();
            builder.Services.AddTransient<ICommonRepository<Vote>, CommonRepository<Vote>>();
            builder.Services.AddTransient<ICommonRepository<Voter>, CommonRepository<Voter>>();
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IElectionRepository, ElectionRepository>();
            builder.Services.AddScoped<IElectionService, ElectionService>();
            builder.Services.AddScoped<IVoterService, VoterService>();
            builder.Services.AddScoped<IVoterRepository, VoterRepository>();
            builder.Services.AddScoped<ICandidateService, CandidateService>();
            builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
            builder.Services.AddScoped<IVoteRepository, VoteRepository>();



            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();


            builder.Services.Configure<JwtSettings>(
            builder.Configuration.GetSection("JwtSettings"));
            builder.Services.AddSingleton<TokenGenerator>();
            // ✅ JWT Authentication
            var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            builder.Services.AddAuthorization();

            // ✅ Swagger with JWT support
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Voting API", Version = "v1" });

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.\r\n\r\n" +
                                  "Enter 'Bearer' [space] and then your token.\r\n\r\nExample: \"Bearer abc123\""
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            var app = builder.Build();

            // Swagger UI setup
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Voting API v1");
                    c.RoutePrefix = "swagger";
                });
            }

            app.MapGet("/", () => "Welcome to the E-Voting API!");

            app.UseHttpsRedirection();

           
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Serve Angular static files
            app.UseDefaultFiles(); 
            app.UseStaticFiles();
            app.MapFallbackToFile("index.html");


            app.Run();
        }
    }
}
