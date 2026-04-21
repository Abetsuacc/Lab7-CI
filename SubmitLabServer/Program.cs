using SubmitLabServer.Components;
using SubmitLabServer.Services;

namespace SubmitLabServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            // injection
            builder.Services.AddSingleton<ILibraryService, LibraryService>();
            var app = builder.Build();

            // this is wheer cvs loads up
            var libraryService = app.Services.GetRequiredService<ILibraryService>();
            libraryService.ReadBooks();
            libraryService.ReadUsers();

            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
              
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
