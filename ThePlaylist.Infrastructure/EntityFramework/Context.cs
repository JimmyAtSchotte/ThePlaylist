﻿using Microsoft.EntityFrameworkCore;
using ThePlaylist.Core.Entitites;

namespace ThePlaylist.Infrastructure.EntityFramework;

public class Context(DbContextOptions options) : DbContext(options)
{
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Playlist>(builder =>
            {
                builder.ToTable("Playlists");

                builder.HasKey(x => x.Id);
                builder.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                builder.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                builder.Property(x => x.Description)
                    .HasMaxLength(1024);

                builder.HasMany(x => x.Tracks)
                    .WithMany(x => x.Playlists)
                    .UsingEntity<Dictionary<string, object>>(
                        "PlaylistTracks",
                        j => j.HasOne<Track>().WithMany().HasForeignKey("TrackId").OnDelete(DeleteBehavior.Cascade),
                        j => j.HasOne<Playlist>().WithMany().HasForeignKey("PlaylistId").OnDelete(DeleteBehavior.Cascade),
                        j =>
                        {
                            j.ToTable("PlaylistTracks");
                            j.HasKey("PlaylistId", "TrackId");
                        }
                    );
            })
            
            
            ;
        modelBuilder.Entity<Track>(builder =>
        {
            builder.ToTable("Tracks");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasMany(x => x.Playlists)
                .WithMany(x => x.Tracks)
                .UsingEntity<Dictionary<string, object>>(
                "PlaylistTracks",
                j => j.HasOne<Playlist>().WithMany().HasForeignKey("PlaylistId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Track>().WithMany().HasForeignKey("TrackId").OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.ToTable("PlaylistTracks");
                    j.HasKey("PlaylistId", "TrackId");
                }
                );

            builder.HasMany(x => x.Genres)
                .WithMany(x => x.Tracks)
                .UsingEntity<Dictionary<string, object>>(
                "TrackGenres",
                j => j.HasOne<Genre>().WithMany().HasForeignKey("GenreId"),
                j => j.HasOne<Track>().WithMany().HasForeignKey("TrackId"),
                j =>
                {
                    j.ToTable("TrackGenres");
                    j.HasKey("TrackId", "GenreId");
                }
                );
        });
        
        modelBuilder.Entity<Genre>(builder =>
        {
            builder.ToTable("Genres");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasMany(x => x.Tracks)
                .WithMany(x => x.Genres)
                .UsingEntity<Dictionary<string, object>>(
                "TrackGenres",
                j => j.HasOne<Track>().WithMany().HasForeignKey("TrackId"),
                j => j.HasOne<Genre>().WithMany().HasForeignKey("GenreId"),
                j => j.ToTable("TrackGenres")
                );

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.SubGenres)
                .IsRequired(false)
                .HasForeignKey("ParentGenreId")
                .OnDelete(DeleteBehavior.NoAction);
        });
        
        base.OnModelCreating(modelBuilder);
    } 
    
    
}