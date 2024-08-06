namespace ECommerceProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // In Startup.cs
            public void ConfigureServices(IServiceCollection services)
            {
                // Other configurations...

                services.AddIdentityServer()
                    .AddInMemoryClients(Config.Clients)
                    .AddInMemoryApiResources(Config.ApiResources)
                    .AddInMemoryApiScopes(Config.ApiScopes)
                    .AddInMemoryIdentityResources(Config.IdentityResources)
                    .AddTestUsers(Config.TestUsers)
                    .AddDeveloperSigningCredential();

                services.AddAuthentication();
            }

            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseHttpsRedirection();
                app.UseRouting();
                app.UseIdentityServer();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }

        }
    }
}