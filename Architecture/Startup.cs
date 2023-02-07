using Architecture.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Architecture.Core.Services.Members;
using Architecture.DAL.Repository.Members;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Architecture.Middleware;
using Architecture.Service;
using Architecture.Services;
using Swashbuckle.AspNetCore.Filters;
using Architecture.Controllers;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using Architecture.Core.Services.Files;
using Architecture.DAL.Repository.File;

namespace Architecture
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
            services.AddControllersWithViews();

            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connection));

            services.AddScoped<IMembersService, MembersService>();
            services.AddScoped<IMembersRepository, MembersRepository>();

            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFileRepository, FileRepository>();

            services.AddSwaggerExamplesFromAssemblyOf<TokenModel>();
            services.AddSwaggerExamplesFromAssemblyOf<TokenExampleResponce>();

            // Begin Swagger Configuration
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Architecture api", Version = "v1", Description = "My first Open Api"});

                c.ExampleFilters();

                // в настройках проекта, в отладке, необходимо включить авто генерацию кода в xml
                // что бы работало описание тегов sammary и т.д.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "MyApi.xml");
                c.IncludeXmlComments(filePath);

                // START Add Swagger authentication by JWT Token
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

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
                        new string[] { }
                    }
                });
                // END Swagger authentication by JWT Token
            });
            #endregion
            // End Swagger Configuration

            // автоматически сканируется сборка и подключает конфигурационные файлы автомаппера
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ApiMappingProfile>();
            });

            // JWT Start
            #region Authentication
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])) //Configuration["JwtToken:SecretKey"]
                };
            });
            #endregion
            // JWT End

            // JWT Start
            services.AddTransient<IUserService, UserService>();
            // JWT End

            //services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, MyAuthenticationHandler>("MYSCHEMA", null, null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // For upload files path UploadController
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
            //    RequestPath = new PathString("/Resources")
            //});

            // JWT Start
            app.UseMiddleware<JWTMiddleware>();
            // JWT END

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
                endpoints.MapSwagger();
            });

            //if (env.IsEnvironment("Development"))
            //{
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "Architecture api v1");
                });
            //}
        }
    }
}
