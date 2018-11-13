using JsonWebTokenAuthentication.API.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using JasonWebTokensWebAPI.Migrations;
namespace JsonWebTokenAuthentication.API
{
    public class AuthContext1 : IdentityDbContext<IdentityUser>
    {
        public AuthContext1() : base("AuthContext1")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext1, Configuration>());
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}