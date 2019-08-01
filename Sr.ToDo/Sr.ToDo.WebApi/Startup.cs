using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Sr.ToDo.Core.Contracts;
using Sr.ToDo.Core.Repositories;

namespace Sr.ToDo.WebApi
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
			services.AddDbContext<Core.Dal.SrToDoContext>
				(options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));

			services.AddControllers();

			//services.AddMvc()
			//.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sr.ToDo API", Version = "v1" });

				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});

			// DI repose
			services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
			services.AddScoped(typeof(IToDoRepository), typeof(ToDoRepository));
			services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
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
				// The default HSTS value is 30 days. You may want to change this for production
				// scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseStaticFiles();

			// Runs matching. An endpoint is selected and set on the HttpContext if a match is found.
			app.UseRouting();

			// Middleware
			app.UseCors();
			app.UseHttpsRedirection();
			app.UseAuthorization();

			// Core 3.0 executes the endpoint that was selected by routing. https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-core-3-0-preview-4/
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				//endpoints.MapRazorPages();
			});

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger
			// JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sr.ToDo V1");
				c.RoutePrefix = string.Empty;
			});
		}
	}
}