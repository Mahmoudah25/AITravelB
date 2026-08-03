using AITravelB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITravelB.Persistence.Configrations
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Destination)
                .IsRequired()
                .HasMaxLength(100);
            builder.OwnsOne(t=>t.Budget, b =>
            {
                b.Property(b => b.Amount).HasColumnName("BudgetAmount").HasPrecision(18,2);
                b.Property(b => b.Currency).HasColumnName("BudgetCurrency").HasMaxLength(20);
            });
            builder.HasMany(t => t.Activities)
                .WithOne(a => a.Trip)
                .HasForeignKey(a => a.TripId)
                .OnDelete(DeleteBehavior.Cascade);
           builder.Navigation(t => t.Activities).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
