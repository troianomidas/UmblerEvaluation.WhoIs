using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Umbler.WhoIs.ORM.Mappings;

/// <summary>
/// EF Core mapping for the Domain entity (table: public.domains).
/// </summary>
public class DomainConfig : IEntityTypeConfiguration<Domain.Entities.Domain>
{
    /// <summary>
    /// Configures table, keys, and column mappings for Domain.
    /// </summary>
    /// <param name="builder">Entity type builder.</param>
    public void Configure(EntityTypeBuilder<Domain.Entities.Domain> builder)
    {
        builder.ToTable("domains", "public");

        #region Properties

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.DomainName)
            .IsRequired()
            .HasColumnName("domain_name")
            .HasColumnType("varchar(255)");

        builder.Property(s => s.IpAddress)
            .IsRequired()
            .HasColumnName("ip_address")
            .HasColumnType("varchar(45)"); // support ipv6 in future

        builder.Property(t => t.WhoIs)
            .IsRequired()
            .HasColumnName("who_is")
            .HasColumnType("text");

        builder.Property(i => i.Ttl)
            .IsRequired()
            .HasColumnName("ttl")
            .HasColumnType("integer");

        builder.Property(s => s.HostedAt)
            .IsRequired()
            .HasColumnName("hosted_at")
            .HasColumnType("varchar(255)");

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamptz");
        
        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestampz")
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();

        #endregion

        #region Constrains

        builder.HasKey(p => p.Id);

        #endregion
    }
}