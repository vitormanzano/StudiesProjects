using MasterclassPostgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterclassPostgres.Data.Mappings;

public class CategoryMapping : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        
        builder
            .HasKey(x => x.Id)
            .HasName("pk_category");

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityAlwaysColumn()
            .UseSerialColumn(); // Coloca como serial, incremental
        
        builder.HasMany(x => x.Products);

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
                    .HasDatabaseName("idx_categories_slug");
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
        
        
    }
}