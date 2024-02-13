using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MovieAPI.Middlewares;
using MovieAPI.Models;

namespace MovieAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<MovieDbContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("*")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Inside ConfigureServices method
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie API", Version = "v1" });

                // Define the security scheme
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                // Add the JWT bearer token parameter to the Authorization header
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            // Register the data seeding service
            builder.Services.AddScoped<DataSeeder>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Seed the data when the application starts
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dataSeeder = services.GetRequiredService<DataSeeder>();
                dataSeeder.Seed();
            }

            app.UseCors("AllowSpecificOrigin");

            // Apply API token middleware with hard-coded token
            app.UseApiTokenMiddleware("your-hard-coded-api-token");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}
