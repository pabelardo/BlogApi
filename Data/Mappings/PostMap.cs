using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        //Tabela
        builder.ToTable("Post");

        //Chave primária
        builder.HasKey(x => x.Id);

        //Identity
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(); // VAI COLOCAR NO BANCO PRIMARY KEY IDENTITY(1,1), QDO GERAR A TABELA NO BANCO

        //Propriedades
        builder
            .Property(x => x.LastUpdateDate)
            .IsRequired()
            .HasColumnName("LastUpdateDate")
            .HasColumnType("smalldatetime")
            .HasDefaultValueSql("getdate()");
        //.HasDefaultValue(DateTime.Now.ToUniversalTime());

        builder
            .HasIndex(x => x.Slug, "IX_Post_Slug")
            .IsUnique();

        // Relacionamentos

        builder.HasOne(x => x.Author)
            .WithMany(x => x.Posts)
            .HasConstraintName("FK_Post_Author")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Posts)
            .HasConstraintName("FK_Post_Category")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Tags)
            .WithMany(x => x.Posts)
            .UsingEntity<Dictionary<string, object>>(
                "PostTag",
                post => post.HasOne<Tag>() // Um Post tem uma tag
                    .WithMany() // E essa tag tem muitos posts
                    .HasForeignKey("PostId") // Com uma chave estrangeira chamada PostId
                    .HasConstraintName("FK_PostTag_PostId") // Com o nome descrito entre parenteses
                    .OnDelete(DeleteBehavior.Cascade), // Onde ao deletar será no modelo cascata 
                tag => tag.HasOne<Post>() // Uma Tag tem um Post
                    .WithMany() // E esse post tem muitas Tags
                    .HasForeignKey("TagId") // Com uma chave estrangeira chamada TagId
                    .HasConstraintName("FK_PostTag_TagId") // Com o nome descrito entre parenteses
                    .OnDelete(DeleteBehavior.Cascade)); // Onde ao deletar será no modelo cascata
    }
}