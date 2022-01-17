using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using MongoOdataApi.Models;
using MongoOdataApi.Settings;

namespace MongoOdataApi
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
            services.Configure<MongoDbSettings>(
                Configuration.GetSection(nameof(MongoDbSettings)));

            services.AddSingleton<IMongoDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddControllers().AddOData(opt =>
            {
                //opt.AddRouteComponents("v{version}", edmModel);
                opt.Select().Filter().Count().OrderBy().AddRouteComponents("odata", GetEmdModel());
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MongoOdataApi", Version = "v1" });
            });
        }

        private static IEdmModel GetEmdModel()
        {
            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Pessoa>("Pessoas");
            modelBuilder.EnableLowerCamelCase();
            return modelBuilder.GetEdmModel();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MongoOdataApi v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
