using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using RaupjcProject.Jukebox.MyExceptions;

namespace RaupjcProject.Jukebox
{
    public class JukeboxRepository : IJukeboxRepository
    {
        private readonly MyEf6Context _context;

        public JukeboxRepository(MyEf6Context context)
        {
            _context = context;
        }

        public async Task AddMoodAsync(Mood newMood)
        {
            if (await _context.Moods.AnyAsync(mood => mood.Value.Equals(newMood.Value)))
            {
                throw new DuplicateMoodException($"duplicate value: {newMood.Value}");
            }
            _context.Moods.Add(newMood);
            await _context.SaveChangesAsync();
        }

        public async Task AddPlaylistAsync(Playlist newPlaylist)
        {
            if (await _context.Playlists.AnyAsync(playlist => playlist.Id == newPlaylist.Id))
            {
                throw new DuplicatePlaylistException($"duplicate id: {newPlaylist.Id}");
            }
            _context.Playlists.Add(newPlaylist);
            await _context.SaveChangesAsync();
        }

        public async Task AddSongAsync(Song newSong)
        {
            if (await _context.Songs.AnyAsync(song => song.Id == newSong.Id))
            {
                throw new DuplicateSongException($"duplicate id: {newSong.Id}");
            }
            _context.Songs.Add(newSong);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserAsync(User newUser)
        {
            /*if (await _context.Users.AnyAsync(user => user.Id.Equals(newUser.Id)))
            {
                throw new DuplicateUserException($"duplicate id: {newUser.Id}");
            } 
            if (await _context.Users.AnyAsync(user => user.Nickname.Equals(newUser.Nickname)))
            {
                throw new DuplicateUserException($"duplicate nickname: {newUser.Nickname}");
            }*/
            //whilst this is checked by ApplicationUser adding
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Mood>> GetAllFilteredMoodsAsync(Func<Mood, bool> filterFunction)
        {
            var all = await GetAllMoodsAsync();
            return all.Where(mood => filterFunction(mood)).ToList();
        }

        public async Task<List<Playlist>> GetAllFilteredPlaylistsAsync(Func<Playlist, bool> filterFunction)
        {
            var all = await GetAllPlaylistsAsync();
            return all.Where(playlist => filterFunction(playlist)).ToList();
        }

        public async Task<List<Song>> GetAllFilteredSongsAsync(Func<Song, bool> filterFunction)
        {
            var all = await GetAllSongsAsync();
            return all.Where(song => filterFunction(song)).ToList();
        }

        public async Task<List<Mood>> GetAllMoodsAsync()
        {
            return await _context.Moods.Include(mood => mood.Playlists).
                ToListAsync();
        }

        public async Task<List<Playlist>> GetAllPlaylistsAsync()
        {
            return await _context.Playlists.
                Include(playlist => playlist.Moods).
                Include(playlist => playlist.Songs).
                Include(playlist => playlist.User).
                ToListAsync();
        }

        public async Task<List<Song>> GetAllSongsAsync()
        {
            return await _context.Songs.Include(song => song.Playlists).
                ToListAsync();
        }

        public async Task<Mood> GetMoodAsync(string text)
        {
            return await _context.Moods.Include(mood => mood.Playlists).
                SingleOrDefaultAsync(mood => mood.Value.Equals(text));
        }

        public async Task<Playlist> GetPlaylistAsync(Guid playlistId)
        {
            return await _context.Playlists.
                Include(playlist => playlist.Moods).
                Include(playlist => playlist.Songs).
                Include(playlist => playlist.User).
                SingleOrDefaultAsync(playlist => playlist.Id == playlistId);
        }

        public async Task<Song> GetSongAsync(Guid songId)
        {
            return await _context.Songs.Include(song => song.Playlists).
                SingleOrDefaultAsync(song => song.Id == songId);
        }

        public async Task<User> GetUserAsync(string userId)
        {
            return await _context.Users.Include(user => user.Playlists).
                SingleOrDefaultAsync(user => user.Id == userId);
        }

        public async Task RemoveMoodAsync(string text, string userId)
        {
            Mood mood = await GetMoodAsync(text);
            if (mood == null)
            {
                throw new MoodAccessDeniedException("Only admin can remove moods!");
            }
            _context.Moods.Remove(mood);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePlaylistAsync(Guid playlistId, string userId)
        {
            Playlist playlist = await GetPlaylistAsync(playlistId);
            if (playlist == null || !playlist.User.Id.Equals(userId))
            {
                throw new PlaylistAccessDeniedException("Only owner of the playlist can remove it!");
            }
            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveSongAsync(Guid songId, string userId)
        {
            Song song = await GetSongAsync(songId);
            if (song == null)
            {
                throw new SongAccessDeniedException("Only admin can remove songs!");
            }
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserAsync(User oldUser, string currentUserId)
        {
            User currentUser = await GetUserAsync(currentUserId);
            if (currentUser == null || !currentUser.Id.Equals(oldUser.Id))
            {
                throw new UserAccessDeniedException("Only admin and user her/himself can delete her/himself!");
            }
            _context.Users.Remove(oldUser);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateMoodAsync(Mood newMood)
        {
            if (await _context.Moods.AnyAsync(mood => mood.Value == newMood.Value))
            {
                _context.Entry(newMood).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePlaylistAsync(Playlist newPlaylist, string userId)
        {
            if (newPlaylist.User.Id.Equals(userId) && await _context.Playlists.AnyAsync(playlist => playlist.Id == newPlaylist.Id))
            {
                _context.Entry(newPlaylist).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            } else
            {
                throw new PlaylistAccessDeniedException("Only owner of the playlist can change it!");
            }
        }

        public async Task UpdateSongAsync(Song newSong, string userId)
        {
            if (await _context.Songs.AnyAsync(song => song.Id == newSong.Id))
            {
                _context.Entry(newSong).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            } else
            {
                throw new SongAccessDeniedException("Only admin can change songs!");
            }
        }

        public async Task UpdateUserAsync(User newUser, string currentUserId)
        {
            if (newUser.Id.Equals(currentUserId) && await _context.Users.AnyAsync(user => user.Id.Equals(newUser.Id)))
            {
                _context.Entry(newUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            } else
            {
                throw new UserAccessDeniedException("Only admin and user her/himself can modify her/himself!");
            }
        }

        public async Task RemovePSRelationship(Playlist newPlaylist, Guid songId, string userId)
        {
            if (newPlaylist.User.Id.Equals(userId) && await _context.Playlists.AnyAsync(playlist => playlist.Id == newPlaylist.Id)
                && await _context.Songs.AnyAsync(song => song.Id == songId))
            {
                var song = newPlaylist.Songs.First(x => x.Id == songId);
                _context.Songs.Attach(song);
                //_context.Playlists.Remove(newPlaylist.Songs.First(x => x.Id == songId));
                _context.Entry(newPlaylist.Songs.First(x => x.Id == songId)).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                throw new PlaylistAccessDeniedException("Only owner of the playlist can change it!");
            }
        }
    }
}
