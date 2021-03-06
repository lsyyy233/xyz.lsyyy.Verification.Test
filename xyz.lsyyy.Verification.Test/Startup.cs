using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Serialization;
using xyz.lsyyy.Verification.Extension;
using static System.String;

namespace xyz.lsyyy.Verification.Test
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<MyDbContext>(options =>
			{
				options
					.UseLazyLoadingProxies()
					.UseMySql(Configuration.GetConnectionString("DefaultDataBase"));
			});
			services.AddControllers()
				.AddNewtonsoftJson(option =>
				{
					option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				});
			services.AddVerificationService((s, o) =>
			{
				o.Address = new Uri(Configuration.GetValue<string>("VerificationServer"));
			});
			services.AddLogging();
			services.AddRouting(x =>
			{
				x.LowercaseQueryStrings = true;
				x.LowercaseUrls = true;
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler(handler =>
				{
					handler.Run(async context =>
					{
						context.Response.StatusCode = 500;
						await context.Response.WriteAsync("Unexpected Error");
					});
				});
			}
			app.UseStaticFiles();
			app.UseRouting();
			app.UseVerificationMiddleware((context, serviceProvider) =>
				{
					//context.Session.GetString("Token")
					if (context.Request.Headers.TryGetValue("Token", out StringValues token))
					{
						return token.FirstOrDefault();
					}
					return Empty;
				}
			);
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
