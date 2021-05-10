using Microsoft.Extensions.DependencyInjection;

namespace LoyalCusts.Common
{
    public static class DbServiceExtention
    {
        public static void AddLiteDb(this IServiceCollection services, string databasePath)
        {
            services.AddTransient<DbContext, DbContext>();
            services.Configure<DbConfig>(options => options.DatabasePath = databasePath);
        }
    }
}
