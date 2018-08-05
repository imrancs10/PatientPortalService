using Microsoft.EntityFrameworkCore;
using SWN.MobileService.Api.Data.Entities;

namespace SWN.MobileService.Api.Data
{
    public class MobileServiceContext : DbContext
    {
        public MobileServiceContext(DbContextOptions<MobileServiceContext> options) : base(options)
        { }

        public DbSet<MessageDetail> MessageDetails { get; set; }

        public DbSet<MobileRecipient> MobileRecipients { get; set; }
    }

}
