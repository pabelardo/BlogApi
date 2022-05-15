using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Data.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //Tabela
        builder.ToTable("User");

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

        builder.Property(x => x.Bio);
        builder.Property(x => x.Email);
        builder.Property(x => x.Image);
        builder.Property(x => x.PasswordHash);
        builder.Property(x => x.GitHub);

        builder
            .Property(x => x.Slug)
            .IsRequired()
            .HasColumnName("Slug")
            .HasColumnType("varchar")
            .HasMaxLength(80);

        builder
            .HasIndex(x => x.Slug, "IX_User_Slug")
            .IsUnique();

        builder
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                role => role
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .HasConstraintName("FK_UserRole_RoleId")
                    .OnDelete(DeleteBehavior.Cascade),
                user => user
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .HasConstraintName("FK_UserRole_UserId")
                    .OnDelete(DeleteBehavior.Cascade)
            );
    }
}