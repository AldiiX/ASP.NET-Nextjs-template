using System.Collections.Generic;
using dotenv.net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace aspnext.Server;



public static class Program {

    public static ILogger Logger { get; private set; } = null!;
    public static WebApplication Application { get; private set; } = null!;
    public static IDictionary<string, string> ENV { get; private set; } = DotEnv.Read();



    #if DEBUG
        public static readonly bool DevelopmentMode = true;
    #else
        public static readonly bool DevelopmentMode = false;
    #endif


    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddOpenApi();
        builder.Services.AddHttpClient();

        Application = builder.Build();

        Application.UseDefaultFiles();
        Application.MapStaticAssets();

        // Configure the HTTP request pipeline.
        if (Application.Environment.IsDevelopment()) Application.MapOpenApi();


        //app.UseHttpsRedirection();

        Application.UseAuthorization();
        Application.UseCors();

        // pridani X-Powered-By
        Application.Use(async (context, next) => {
            context.Response.Headers.Append("X-Powered-By", "ASP.NET");
            await next.Invoke();
        });


        Application.MapControllers();

        //app.MapFallbackToFile("/index.html");

        Logger = Application.Logger;


        Application.Run();
    }
}