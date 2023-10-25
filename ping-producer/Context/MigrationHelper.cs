using Microsoft.EntityFrameworkCore;

namespace Context;

public static class MigrationHelper
{
    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AuthContext>();
        db.Database.Migrate();
        return app;
    }
}
