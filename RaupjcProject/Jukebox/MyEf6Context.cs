using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using RaupjcProject.Jukebox;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace RaupjcProject
{
    public class MyEf6Context : DbContext
    {
        public IDbSet<User> Users { get; set; }
        public IDbSet<Song> Songs { get; set; }
        public IDbSet<Playlist> Playlists { get; set; }
        public IDbSet<Mood> Moods { get; set; }

        public MyEf6Context(string cnnstr) : base(cnnstr)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //User
            modelBuilder.Entity<User>().HasKey(user => user.Id);
            modelBuilder.Entity<User>().Property(user => user.Nickname).IsRequired();
            modelBuilder.Entity<User>().Property(user => user.Picture).IsOptional();

            //Song
            modelBuilder.Entity<Song>().HasKey(song => song.Id);
            modelBuilder.Entity<Song>().Property(song => song.Sound).IsRequired();
            modelBuilder.Entity<Song>().Property(song => song.Artist).IsRequired();
            modelBuilder.Entity<Song>().Property(song => song.Title).IsRequired();
            modelBuilder.Entity<Song>().Property(song => song.Album).IsOptional();
            modelBuilder.Entity<Song>().Property(song => song.Year).IsOptional();

            //Playlist
            modelBuilder.Entity<Playlist>().HasKey(playlist => playlist.Id);
            modelBuilder.Entity<Playlist>().Property(playlist => playlist.Name).IsRequired();
            modelBuilder.Entity<Playlist>().Property(playlist => playlist.Image).IsOptional();

            //Mood
            modelBuilder.Entity<Mood>().HasKey(mood => mood.Value);

            //relations
            modelBuilder.Entity<User>().HasMany(user => user.Playlists).WithRequired(playlist => playlist.User);
            modelBuilder.Entity<Playlist>().HasMany(playlist => playlist.Songs).WithMany(song => song.Playlists);
            modelBuilder.Entity<Playlist>().HasMany(playlist => playlist.Moods).WithMany(mood => mood.Playlists);

            //so it doesn't delete songs when playlist is deleted in database (or any similar connections)
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
