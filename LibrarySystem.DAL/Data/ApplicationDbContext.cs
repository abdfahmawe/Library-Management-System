using LibrarySystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        // we make the 2 db sets for our system library (Book, Magazine, Newspaper) and members 
        // notes we dont make one for application user beacuse it is already made by the identity framework
        public DbSet<LibraryItem> LibraryItems { get; set; } = null!;
        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<BorrowTransaction> BorrowTransactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // this is to define the PK in Member table because it does not have Id or MemberId 
            base.OnModelCreating(builder);
            builder.Entity<Member>().HasKey(member => member.MembershipId);
            // this is to define the relationship between Member and ApplicationUser 1-1
            builder.Entity<Member>().HasOne(member => member.ApplicationUser)
                .WithOne()
                .HasForeignKey<Member>(member => member.ApplicationUserId);
            // make the relation TPH (table per hyraricy) for LibraryItem and its derived classes (Book, Magazine, Newspaper)
            builder.Entity<LibraryItem>().HasDiscriminator<string>("LibraryItemType")
                .HasValue<Book>("Book")
                .HasValue<Magazine>("Magazine")
                .HasValue<Newspaper>("Newspaper");
            // to do not dellete recored when delete member or library item

            builder.Entity<BorrowTransaction>()
                .HasOne(transaction => transaction.Member)
                .WithMany(member => member.BorrowTransactions)
                .HasForeignKey(transaction => transaction.MembershipId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BorrowTransaction>()
                .HasOne(transaction => transaction.LibraryItem)
                .WithMany(item => item.BorrowTransactions)
                .HasForeignKey(transaction => transaction.LibraryItemId)
                .OnDelete(DeleteBehavior.Restrict);
            // to define precision for fine property in BorrowTransaction table to be 10,2 (10 digits total, 2 after decimal)
            builder.Entity<BorrowTransaction>()
            .Property(transaction => transaction.Fine)
            .HasPrecision(10, 2);

        }
    }
}
