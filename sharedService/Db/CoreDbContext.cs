using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Shared.Entities.Core;

namespace OnlineShop.Shared.Db
{
    public class CoreDbContext : IdentityDbContext<tblApplicationUser, tblApplicationRole, string>
    {
        private readonly string _schema;
        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {
            _schema = "online_shop_core";
        }

        public DbSet<Entities.Core.tblApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (!string.IsNullOrWhiteSpace(_schema))
            {
                builder.HasDefaultSchema(_schema);
            }

            base.OnModelCreating(builder);

            // Ensure ApplicationRole maps to tblRoles
            builder.Entity<tblApplicationRole>().ToTable("tblRoles", _schema);
            builder.Entity<tblApplicationUser>().ToTable("tblUsers", _schema);
            builder.Entity<IdentityUserRole<string>>().ToTable("tblUserRoles", _schema);
            builder.Entity<IdentityUserClaim<string>>().ToTable("tblUserClaims", _schema);
            builder.Entity<IdentityUserLogin<string>>().ToTable("tblUserLogins", _schema);
            builder.Entity<IdentityRoleClaim<string>>().ToTable("tblRoleClaims", _schema);
            builder.Entity<IdentityUserToken<string>>().ToTable("tblUserTokens", _schema);
          
        }
    }
}
