using Cars.BLL.Helpers;
using Cars.API.Helpers;
using Cars.BLL.Validators;
using Cars.DAL.DbContext;
using Cars.DAL.Models;
using Cars.COMMON.Constants;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Cars.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSeq(Configuration.GetSection("Seq"));
            });

            services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);

            services.AddDbContext<CarDbContext>(options =>
                                                   options/*.UseLazyLoadingProxies()*/
                                                          .UseSqlServer(Configuration.GetConnectionString("DefaultDatabase")));
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<CarDbContext>().AddDefaultTokenProviders();



            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                    {
                        options.SaveToken = true;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidAudience = Configuration["JWT:ValidAudience"],
                            ValidIssuer = Configuration["JWT:ValidIssuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                context.Token = context.Request.Cookies["jwt-token"];
                                return Task.CompletedTask;
                            }
                        };
                    })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
            
            services.AddAuthorization(o => o.DefaultPolicy = new AuthorizationPolicyBuilder(
                                      CookieAuthenticationDefaults.AuthenticationScheme,
                                      JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());

            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                .AddFluentValidation(s =>
                {
                    s.RegisterValidatorsFromAssemblyContaining<CarImportDTOValidator>();
                    s.RegisterValidatorsFromAssemblyContaining<CarUpdateDTOValidator>();
                    s.RegisterValidatorsFromAssemblyContaining<SortAndPageCarModelValidator>();
                    s.RegisterValidatorsFromAssemblyContaining<SortAndPageUserModelValidator>();
                    s.RegisterValidatorsFromAssemblyContaining<LoginVmValidator>();
                    s.RegisterValidatorsFromAssemblyContaining<ChangePasswordVmValidator>();
                    s.ValidatorOptions.CascadeMode = CascadeMode.Stop;
                });
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = AppConstants.ApiAttributes.NAME, Version = AppConstants.ApiAttributes.VERSION_NUMBER });
                c.EnableAnnotations();
            });

            services.AddHttpClient();

            services.AddMemoryCache();

            services.AddCarServices();

            services.AddStackExchangeRedisCache(options =>
            {
#if DEBUG
                options.Configuration = Configuration["Redis:ConnectionStringDebug"];
#else
                options.Configuration = Configuration["Redis:ConnectionStringRelease"];
#endif
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            MappingHelper.AutoMapperInit();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    AppConstants.ApiAttributes.NAME + " " + AppConstants.ApiAttributes.VERSION_NUMBER));
            }

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
