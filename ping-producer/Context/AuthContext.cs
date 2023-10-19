using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Context;

public class AuthContext : IdentityDbContext<ApplicationUser>
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }
}