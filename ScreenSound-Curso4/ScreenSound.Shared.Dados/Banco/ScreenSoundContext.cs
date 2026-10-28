using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScreenSound.Modelos;
using ScreenSound.Shared.Dados.Modelos;
using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.Banco;
public class ScreenSoundContext : IdentityDbContext<PessoaComAcesso, PerfilDeAcesso, int>
{
    public ScreenSoundContext()
    {
    }

    public ScreenSoundContext(DbContextOptions<ScreenSoundContext> options) : base(options)
    {
    }

    public DbSet<Artista> Artistas { get; set; }
    public DbSet<Musica> Musicas { get; set; }
    public DbSet<Genero> Generos { get; set; }
    public DbSet<AvaliacaoArtista> AvaliacaoArtistas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ScreenSoundDB")
            ?? throw new InvalidOperationException(
                "Set the ConnectionStrings__ScreenSoundDB environment variable before using ScreenSoundContext without dependency injection.");

        optionsBuilder.UseMySQL(connectionString).UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Musica>()
            .HasMany(c => c.Generos)
            .WithMany(c => c.Musicas);

        modelBuilder.Entity<AvaliacaoArtista>()
            .HasKey(a => new { a.ArtistaId, a.PessoaId });
    }

}
