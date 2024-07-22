using Microsoft.EntityFrameworkCore;
using Serilog;
using Services.Infrastructure.Data;

namespace Services.API.Extentions;

public static class MigrationManager
{
    private static int _retryForAvailability = 0;

    public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            try
            {
                scope.ServiceProvider.GetRequiredService<ServicesDbContext>().Database.Migrate();
            }
            catch (Exception ex)
            {
                if (_retryForAvailability >= 5)
                {
                    throw;
                }

                _retryForAvailability++;

                Log.Warning(ex.Message);

                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, _retryForAvailability)));

                await MigrateDatabaseAsync(host);
            }
        }

        return host;
    }
}
