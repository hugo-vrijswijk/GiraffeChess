﻿using System;
using GiraffeChess.DomainService;
using GiraffeChess.DomainService.Service;
using GiraffeChess.Infrastructure.Entities;
using GiraffeChess.Infrastructure.Mapper;
using GiraffeChess.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace GiraffeChess.WebAPI
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
            var dbConn = Environment.GetEnvironmentVariable("DB_CONN") ??
                         Configuration.GetConnectionString("DbConn");
            services.AddDbContext<ChessContext>(options => options.UseSqlServer(dbConn),
                ServiceLifetime.Transient, ServiceLifetime.Singleton);

            services.AddTransient<BoardMapper>();
            services.AddTransient<BoardTileMapper>();
            services.AddTransient<ChessPieceMapper>();

            services.AddTransient<IBoardRepository, BoardRepository>();
            services.AddTransient<IBoardService, BoardService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var context = app.ApplicationServices.GetService<ChessContext>())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
                
            app.UseMvc();
        }
    }
}
