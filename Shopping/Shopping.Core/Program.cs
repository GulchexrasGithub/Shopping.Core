// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Shopping.Core
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();


        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>());
        }
    }
}