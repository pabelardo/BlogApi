using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Data.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        //Tabela
        builder.ToTable("Category");

        //Chave primária
        builder.HasKey(x => x.Id);

        //Identity
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(); // VAI COLOCAR NO BANCO PRIMARY KEY IDENTITY(1,1), QDO GERAR A TABELA NO BANCO

        //Propriedades
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("nvarchar")
            .HasMaxLength(80);

        builder
            .Property(x => x.Slug)
            .IsRequired()
            .HasColumnName("Slug")
            .HasColumnType("varchar")
            .HasMaxLength(80);

        // Índices
        builder
            .HasIndex(x => x.Slug, "IX_Category_Slug")
            .IsUnique();
    }
}