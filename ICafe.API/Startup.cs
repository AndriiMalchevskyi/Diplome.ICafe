using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ICafe.Application.Interfaces;
using ICafe.Application;
using ICafe.Persistence.Data;
using ICafe.Domain.Entities;
using ICafe.Persistence;
using ICafe.Application.Helpers;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using ICafe.Persistence.Helpers;
using ICafe.API.Helpers;

namespace ICafe.API
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
            services.AddDbContext<ICafeContext>(x =>
                x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, Role>(cft =>
            {
                cft.User.RequireUniqueEmail = false;
            }).AddEntityFrameworkStores<ICafeContext>()
            .AddDefaultTokenProviders();

            //JwtBearerDefaults.AuthenticationScheme
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value))
                    };
                });
            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<ICafeContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            //services.AddAuthorization();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRoles", policy => policy.RequireRole("admin", "sysadmin", "root"));
                options.AddPolicy("RequireWorkersRoles", policy => policy.RequireRole("waiter", "cook"));
            });


            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builderCors =>
                {
                    builderCors.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
                });
            });
            services.AddTransient<Seed>();

            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            //Mapper.Reset();
            services.AddAutoMapper(typeof(AutoMapperProfiles));
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<User>, UserRepository>();
            //services.AddScoped<IEstateAgentsRepository, EstateAgentsRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            seeder.SeedUsers();
            app.UseHttpsRedirection();
            app.UseCors("EnableCORS");
            //app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseMvc();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
