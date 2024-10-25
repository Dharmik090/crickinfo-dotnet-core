//using ASPCoreEFSimple.Models;
using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.Interface;
using crickinfo_mvc_ef_core.Models.SQL;
using Microsoft.AspNetCore.Authentication.Cookies;





//using crickinfo_mvc_ef_core.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
//using crickinfo_mvc_ef_core.Data; // Add this line

//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreEFSimple
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
			services.AddScoped<IUserRepo, SQLUserRepo>();
			services.AddScoped<ITournamentRepo, SQLTournamentRepo>();
            services.AddScoped<ITeamsRepo, SQLTeamsRepo>();
			services.AddScoped<IUnitOfWork, SQLUnitOfWorkRepo>();
			services.AddScoped<IMatchesRepo, SQLMatchesRepo>();
			services.AddScoped<IPointsTableRepo, SQLPointsTableRepo>();
            services.AddDbContext<CrickInfoContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            

			services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
            });

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
			}
			app.UseStaticFiles();

			app.UseRouting();

            app.UseSession();

			app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
