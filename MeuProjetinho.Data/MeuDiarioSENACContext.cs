using Microsoft.EntityFrameworkCore;
using MeuDiarioSenac.Model;
using MySql.Data.MySqlClient;

namespace MeuProjetinho.Data;

public class MeuDiarioSENACContext : DbContext
{
    public DbSet<Registro> Registros { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;

    private static readonly string connectionString = "Server=localhost;Port=3306;Database=senacdiario;Uid=root;Pwd=S&nac2024;";

    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Registro>().ToTable("registros");
        modelBuilder.Entity<Usuario>().ToTable("usuarios");

        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Registros)
            .WithOne(r => r.Usuario)
            .HasForeignKey(r => r.UsuarioId);
    }
}
