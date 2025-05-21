using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public static class Extentions
    {
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            using var dbContext = services.GetRequiredService<DiscountContext>();
            try
            {
                dbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the DB");
            }
            return app;
        }


    }

}
