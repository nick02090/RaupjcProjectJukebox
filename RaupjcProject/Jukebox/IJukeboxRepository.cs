using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Jukebox
{
    public interface IJukeboxRepository
    {
        /// <summary>
        /// Gets User object in database.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User> GetUserAsync(string userId);
        /// <summary>
        /// Adds new User object in database.
        /// If object with the same id/nickname already exists, method throws DuplicateUserException.
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        Task AddUserAsync(User newUser);
        /// <summary>
        /// Removes User object from database.
        /// If the current user isn't the one who (s)he wants to remove (nor the admin), method throws UserAccessDeniedException.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RemoveUserAsync(User oldUser, string currentUserId);
        /// <summary>
        /// Updates given User object in database.
        /// If the current user isn't the one who (s)he wants to modify (nor the admin), method throws UserAccessDeniedException.
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateUserAsync(User newUser, string currentUserId);

        /// <summary>
        /// Gets Playlist object in database 
        /// </summary>
        /// <param name="playlistId"></param>
        /// <returns></returns>
        Task<Playlist> GetPlaylistAsync(Guid playlistId);
        /// <summary>
        /// Gets all Playlist objects in database.
        /// </summary>
        /// <returns></returns>
        Task<List<Playlist>> GetAllPlaylistsAsync();
        /// <summary>
        /// Gets all Playlist objects in database that apply the given filter.
        /// </summary>
        /// <param name="filterFunction"></param>
        /// <returns></returns>
        Task<List<Playlist>> GetAllFilteredPlaylistsAsync(Func<Playlist, bool> filterFunction);
        /// <summary>
        /// Adds new Playlist object in database.
        /// If object with the same id already exists, method throws DuplicatePlaylistException.
        /// </summary>
        /// <param name="newPlaylist"></param>
        /// <returns></returns>
        Task AddPlaylistAsync(Playlist newPlaylist);
        /// <summary>
        /// Removes Playlist object with given id from database.
        /// If user isn't the owner of the object, method throws PlaylistAccessDeniedException.
        /// </summary>
        /// <param name="playlistId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RemovePlaylistAsync(Guid playlistId, string userId);
        /// <summary>
        /// Updates given Playlist object in database.
        /// If user isn't the owner of the object, method throws PlaylistAccessDeniedException.
        /// </summary>
        /// <param name="newPlaylist"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdatePlaylistAsync(Playlist newPlaylist, string userId);

        /// <summary>
        /// Gets Song object in database.
        /// </summary>
        /// <param name="songId"></param>
        /// <returns></returns>
        Task<Song> GetSongAsync(Guid songId);
        /// <summary>
        /// Gets all Song objects in database.
        /// </summary>
        /// <returns></returns>
        Task<List<Song>> GetAllSongsAsync();
        /// <summary>
        /// Gets all Song objects in database that apply the given filter.
        /// </summary>
        /// <param name="filterFunction"></param>
        /// <returns></returns>
        Task<List<Song>> GetAllFilteredSongsAsync(Func<Song, bool> filterFunction);
        /// <summary>
        /// Adds new Song object in database.
        /// If object with the same id already exists, method throws DuplicateSongException.
        /// </summary>
        /// <param name="newSong"></param>
        /// <returns></returns>
        Task AddSongAsync(Song newSong);
        /// <summary>
        /// Removes Song object with given id from database.
        /// If user isn't the admin, method throws SongAccessDeniedException, whilst ONLY admin can remove songs.
        /// </summary>
        /// <param name="songId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RemoveSongAsync(Guid songId, string userId);
        /// <summary>
        /// Updates given Song object in database.
        /// If user isn't the admin, method throws SongAccessDeniedException, whilst ONLY admin can modify songs.
        /// </summary>
        /// <param name="newSong"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateSongAsync(Song newSong, string userId);

        /// <summary>
        /// Gets Mood for a given text(value). 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Task<Mood> GetMoodAsync(string text);
        /// <summary>
        /// Gets all Mood objects in database.
        /// </summary>
        /// <returns></returns>
        Task<List<Mood>> GetAllMoodsAsync();
        /// <summary>
        /// Adds new Mood object in database.
        /// If object with the same id already exists, method throws DuplicateMoodException.
        /// </summary>
        /// <param name="newMood"></param>
        /// <returns></returns>
        Task AddMoodAsync(Mood newMood);
        /// <summary>
        /// Removes Mood object with given text(value) from database.
        /// If user isn't the admin, method throws MoodAccessDeniedException, whilst ONLY admin can remove moods.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RemoveMoodAsync(string text, string userId);
        /// <summary>
        /// Gets all Mood objects in database that apply the given filter.
        /// </summary>
        /// <param name="filterFunction"></param>
        /// <returns></returns>
        Task<List<Mood>> GetAllFilteredMoodsAsync(Func<Mood, bool> filterFunction);
        /// <summary>
        /// Updates given Mood object in database.
        /// </summary>
        /// <param name="newSong"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateMoodAsync(Mood newMood);
        /// <summary>
        /// Removes many-to-many relationship between Playlist and Song objects (removes specific song from playlist).
        /// If user isn't the owner of the Playlist object, method throws PlaylistAccessDeniedException.
        /// </summary>
        /// <param name="newPlaylist"></param>
        /// <param name="newSong"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RemovePSRelationship(Playlist newPlaylist, Guid songId, string userId);
    }
}
