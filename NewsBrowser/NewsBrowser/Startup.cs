using Backend.Helpers;
using Backend.Models;
using Backend.Models.Semantic;
using Backend.Repositories.Abstract;
using Backend.Repositories.Concrete;
using Backend.Services.Abstract;
using Backend.Services.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;
using System;

namespace NewsBrowser
{
    public class Startup
    {
        // private string DefaultGraphPath = @"computer-science.graphml";
        private string DefaultGraphPath = @"dbpedia-cat-graph-broader-only-top-3.graphml";
        // private string DefaultGraphPath = @"dbpedia-graph-categories-only-broader-v2.graphml";

        private string LablesDocFreqPath = @"dbpedia-cat-graph-top-3-labels-doc-freq.txt";
        private string DefaultNodeIdPropName = "label";

        private string DefaultEdgePropName = "label";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();

            var esClientSettings = new ConnectionSettings(new Uri(Configuration["Elasticsearch:Url"]))
                                .DefaultIndex(Configuration["Elasticsearch:Index"])
                                .ThrowExceptions();

            var esClient = new ElasticClient(esClientSettings);

            var requestExistIndex = new IndexExistsRequest("subscriptions");

            if (!esClient.Indices.Exists(requestExistIndex).Exists)
            {
                var settings = new IndexSettings { NumberOfReplicas = 0, NumberOfShards = 1 };
                var indexConfig = new IndexState { Settings = settings };
                esClient.Indices.Create("subscriptions", c => c
                    .InitializeUsing(indexConfig)
                    .Map<Subscriber>(mp => mp.AutoMap()));
            }

            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfig>();

            var semanticGraph = new SemanticGraph(DefaultGraphPath, DefaultNodeIdPropName, 
                    DefaultEdgePropName, lablesDocFreqPath: LablesDocFreqPath);

            services.AddSingleton<IElasticClient>(esClient);
            services.AddSingleton(emailConfig);
            services.AddSingleton(semanticGraph);

            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsService, NewsService>();

            services.AddScoped<ISubscribeRepository, SubscribeRepository>();
            services.AddScoped<ISubscribeService, SubscribeService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISemanticService, SemanticService>();

            // In production, the Angular files will be served from this directory

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
