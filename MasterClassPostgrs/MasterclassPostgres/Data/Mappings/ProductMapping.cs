using MasterclassPostgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterclassPostgres.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        
        builder
            .HasKey(x => x.Id)
            .HasName("pk_product");

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityAlwaysColumn()
            .UseSerialColumn(); // Coloca como serial, incremental
        
        builder.OwnsOne(x => x.Heading,
            heading =>
            {
                heading
                    .Property(x => x.Slug)
                    .HasColumnName("slug")
                    .HasColumnType("varchar")
                    .HasMaxLength(160)
                    .IsRequired();
                
                heading
                    .HasIndex(x => x.Slug)
                    .IsUnique()
                    .HasDatabaseName("idx_products_slug");
            });
        
        builder.OwnsOne(x => x.Heading,
            heading =>
            {
                heading
                    .Property(x => x.Title)
                    .HasColumnName("title")
                    .HasColumnType("varchar")
                    .HasMaxLength(160)
                    .IsRequired();
            });
        
        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()") //Poderia tirar pq tem na classe
            .IsRequired();
        
        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .HasDefaultValueSql("now()")
            .IsRequired();
        
        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();
        
        builder.Property(x => x.DefaultLanguage)
            .HasColumnName("default_language")
            .HasMaxLength(8)
            .HasDefaultValue("en-us")
            .IsRequired();
    }
}