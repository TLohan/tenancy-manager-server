using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using tenancy_manager_server.Entities;
using tenancy_manager_server.Models;
using tenancy_manager_server.Services;

namespace tenancy_manager_server
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                      builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                  .Build());
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(opts =>
            {
                opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddDbContext<AppDbContext>(options =>
                      options.UseSqlServer("Server=tcp:tenancymgmtserver.database.windows.net,1433;Initial Catalog=tenancymgmtdb;Persist Security Info=False;User ID=serveradmin;Password=engue4Va;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
            services.BuildServiceProvider().GetService<AppDbContext>().Database.Migrate();
            services.AddScoped<IRepository, Repository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<House, HouseDto>();
                cfg.CreateMap<HouseDto, House>();
                cfg.CreateMap<HouseForCreationDto, House>();

                cfg.CreateMap<Flat, FlatDto>();
                cfg.CreateMap<FlatDto, Flat>();
                cfg.CreateMap<FlatForCreationDto, Flat>();

                cfg.CreateMap<Lease, LeaseDto>();
                cfg.CreateMap<LeaseDto, Lease>();
                cfg.CreateMap<LeaseForCreationDto, Lease>();
                cfg.CreateMap<Lease, LeaseForUpdateDto>();
                cfg.CreateMap<LeaseForUpdateDto, Lease>();

                cfg.CreateMap<Tenant, TenantDto>();
                cfg.CreateMap<TenantDto, Tenant>();
                cfg.CreateMap<TenantForCreationDto, Tenant>();
                cfg.CreateMap<TenantForUpdateDto, Tenant>();

                cfg.CreateMap<Payment, PaymentDto>();
                cfg.CreateMap<PaymentDto, Payment>();
                cfg.CreateMap<PaymentForUpdateDto, Payment>();

                cfg.CreateMap<RentReview, RentReviewDto>();
                cfg.CreateMap<RentReviewDto, RentReview>();
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
