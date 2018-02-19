using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RaupjcProject.Jukebox;
using RaupjcProject.Models;
using RaupjcProject.Models.JukeboxViewModels;
using RaupjcProject.Services;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.Media;
using RaupjcProject.Models.ManageViewModels;

namespace RaupjcProject.Controllers
{
    public class JukeboxController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly IJukeboxRepository _jukeboxRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

        public JukeboxController(
            IJukeboxRepository jukeboxRepository,
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ILogger<ManageController> logger,
          UrlEncoder urlEncoder,
          IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
            _jukeboxRepository = jukeboxRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Jukebox", "Jukebox", new { area = "" });
            }
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Jukebox()
        {
            var allPlaylists = await _jukeboxRepository.GetAllPlaylistsAsync();
            var model = new JukeboxViewModel(allPlaylists);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Jukebox(JukeboxViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var allTempPlaylists = await _jukeboxRepository.GetAllPlaylistsAsync();
                var tempModel = new JukeboxViewModel(allTempPlaylists);
                return View(tempModel);
            }
            var allPlaylists = await _jukeboxRepository.GetAllPlaylistsAsync();
            String[] searchedMoods = model.Search.Split(' ');
            var allMoods = await _jukeboxRepository.GetAllMoodsAsync();
            var foundedMoods = new List<Mood>();
            foreach (var search in searchedMoods)
            {
                if (allMoods.Any(mood => mood.Value.ToLower().Equals(search.ToLower())))
                {
                    foundedMoods.Add(allMoods.First(mood => mood.Value.ToLower().Equals(search.ToLower())));
                }
            }
            var filteredMoods = foundedMoods.Where(mood => mood.Playlists != null).ToList();
            var filteredPlaylists = new List<Playlist>();
            foreach (var mood in filteredMoods)
            {
                foreach (var playlist in mood.Playlists)
                {
                    if (!filteredPlaylists.Contains(playlist))
                    {
                        filteredPlaylists.Add(playlist);
                    }
                }
            }
            var newModel = new JukeboxViewModel(allPlaylists, filteredPlaylists, model.Search)
            {
                Moods = foundedMoods
            };
            return View(newModel);
        }

        [HttpGet]
        public IActionResult Info()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddPlaylistName()
        {
            return View(new AddPlaylistNameViewModel());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPlaylistName(AddPlaylistNameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            if (model.PictureFile != null)
            {
                if (!model.PictureFile.FileName.EndsWith(".jpg"))
                {
                    var newModel = new AddPlaylistNameViewModel()
                    {
                        IsPicture = false,
                        Try = true,
                        Name = model.Name
                    };
                    return View(newModel);
                }
                byte[] picture;
                using (var memoryStream = new MemoryStream())
                {
                    await model.PictureFile.CopyToAsync(memoryStream);
                    picture = memoryStream.ToArray();
                }
                var playlist = new Playlist(model.Name, picture, user);
                await _jukeboxRepository.AddPlaylistAsync(playlist);
                return RedirectToAction("AddPlaylist", new { model.Name});
            }
            else
            {
                var playlist = new Playlist(model.Name, user);
                await _jukeboxRepository.AddPlaylistAsync(playlist);
                return RedirectToAction("AddPlaylist", new { model.Name});
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddPlaylist(string name)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            var songs = await _jukeboxRepository.GetAllSongsAsync();
            var myPlaylists = await _jukeboxRepository.GetAllFilteredPlaylistsAsync(p => p.User.Id.Equals(user.Id));
            Playlist playlist;
            if (name == null)
            {
                playlist = myPlaylists.FirstOrDefault(p => !p.IsCreated);
            }else
            {
                playlist = myPlaylists.FirstOrDefault(p => p.Name.Equals(name));
            }
            var model = new AddPlaylistViewModel(playlist, songs, user);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPlaylist(AddPlaylistViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            var songs = await _jukeboxRepository.GetAllSongsAsync();
            var myPlaylists = await _jukeboxRepository.GetAllFilteredPlaylistsAsync(p => p.User.Id.Equals(user.Id));
            var playlist = myPlaylists.FirstOrDefault(p => !p.IsCreated);
            var newModel = new AddPlaylistViewModel(playlist, songs, user);
            if (model.Artist != null || model.Album != null || model.Year != null || model.Title != null)
            {
                List<Song> searchedSongs;
                //ugliest else-if ever...
                //checking which strings are (not) null so it only goes to database once and does all the filtering 
                //in GUI deleted filtering for Year and Album whilst not all Songs contain does properties, so i complicates things + no time to finish :(
                if (model.Artist != null && model.Album == null && model.Year == null && model.Title == null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Artist.ToLower().Contains(model.Artist.ToLower()));
                }
                else if (model.Album != null && model.Artist == null && model.Year == null && model.Title == null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Album.ToLower().Contains(model.Album.ToLower()));
                }
                else if (model.Year != null && model.Album == null && model.Artist == null && model.Title == null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Year.Equals(model.Year));
                }
                else if (model.Artist != null && model.Album != null && model.Year == null && model.Title == null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Artist.ToLower().Contains(model.Artist.ToLower()) &&
                        song.Album.ToLower().Contains(model.Album.ToLower()));
                }
                else if (model.Artist != null && model.Year != null && model.Album == null && model.Title == null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Artist.ToLower().Contains(model.Artist.ToLower()) &&
                        song.Year.Equals(model.Year));
                }
                else if (model.Album != null && model.Year != null && model.Artist == null && model.Title == null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Album.ToLower().Contains(model.Album.ToLower()) &&
                        song.Year.Equals(model.Year));
                }
                else if (model.Album != null && model.Year != null && model.Artist != null && model.Title == null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Album.ToLower().Contains(model.Album.ToLower()) &&
                        song.Year.Equals(model.Year) &&
                        song.Artist.ToLower().Contains(model.Artist.ToLower()));
                }
                else if (model.Artist != null && model.Album == null && model.Year == null && model.Title != null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Artist.ToLower().Contains(model.Artist.ToLower()) &&
                        song.Title.ToLower().Contains(model.Title.ToLower()));
                }
                else if (model.Album != null && model.Artist == null && model.Year == null && model.Title != null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Album.ToLower().Contains(model.Album.ToLower()) &&
                        song.Title.ToLower().Contains(model.Title.ToLower()));
                }
                else if (model.Year != null && model.Album == null && model.Artist == null && model.Title != null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Year.Equals(model.Year) &&
                        song.Title.ToLower().Contains(model.Title.ToLower()));
                }
                else if (model.Artist != null && model.Album != null && model.Year == null && model.Title != null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Artist.ToLower().Contains(model.Artist.ToLower()) &&
                        song.Album.ToLower().Contains(model.Album.ToLower()) &&
                        song.Title.ToLower().Contains(model.Title.ToLower()));
                }
                else if (model.Artist != null && model.Year != null && model.Album == null && model.Title != null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Artist.ToLower().Contains(model.Artist.ToLower()) &&
                        song.Year.Equals(model.Year) &&
                        song.Title.ToLower().Contains(model.Title.ToLower()));
                }
                else if (model.Album != null && model.Year != null && model.Artist == null && model.Title != null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Album.ToLower().Contains(model.Album.ToLower()) &&
                        song.Year.Equals(model.Year) &&
                        song.Title.ToLower().Contains(model.Title.ToLower()));
                }
                else if (model.Album != null && model.Year != null && model.Artist != null && model.Title != null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Album.ToLower().Contains(model.Album.ToLower()) &&
                        song.Year.Equals(model.Year) &&
                        song.Artist.ToLower().Contains(model.Artist.ToLower()) &&
                        song.Title.ToLower().Contains(model.Title.ToLower()));
                }
                else if (model.Album == null && model.Year == null && model.Artist == null && model.Title != null)
                {
                    searchedSongs = await _jukeboxRepository.GetAllFilteredSongsAsync(song => song.Title.ToLower().Contains(model.Title.ToLower()));
                }else
                {
                    searchedSongs = null;
                }
                newModel.SearchedSongs = searchedSongs;
                newModel.Artist = model.Artist;
                newModel.Album = model.Album;
                newModel.Year = model.Year;
                newModel.Title = model.Title;
                return View(newModel);
            }
            return View(newModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddSong()
        {
            var model = new AddSongViewModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddSong(AddSongViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.File != null && model.File.FileName.EndsWith(".mp3"))
            {
                byte[] sound;
                using (var memoryStream = new MemoryStream())
                {
                    await model.File.CopyToAsync(memoryStream);
                    sound = memoryStream.ToArray();
                }
                var song = new Song(model.Artist, model.Album, model.Year, sound, model.Title);
                await _jukeboxRepository.AddSongAsync(song);
                return RedirectToAction("AddPlaylist");
            }
            else
            {
                model.IsMp3 = false;
                return View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> AddSongToPlaylist(Guid Id)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            var songs = await _jukeboxRepository.GetAllSongsAsync();
            var myPlaylists = await _jukeboxRepository.GetAllFilteredPlaylistsAsync(p => p.User.Id.Equals(user.Id));
            var playlist = myPlaylists.FirstOrDefault(p => !p.IsCreated);
            var song = songs.FirstOrDefault(s => Guid.Equals(s.Id, Id));
            playlist.Songs.Add(song);
            await _jukeboxRepository.UpdatePlaylistAsync(playlist, user.Id);
            //song.Playlists.Add(playlist);
            //await _jukeboxRepository.UpdateSongAsync(song, user.Id);
            return RedirectToAction("AddPlaylist", new { playlist.Name});
        }


        //failed method :(
        [Authorize]
        public async Task<IActionResult> RemoveSongFromPlaylist(Guid Id)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            var songs = await _jukeboxRepository.GetAllSongsAsync();
            var myPlaylists = await _jukeboxRepository.GetAllFilteredPlaylistsAsync(p => p.User.Id.Equals(user.Id));
            var playlist = myPlaylists.FirstOrDefault(p => !p.IsCreated);
            var song = songs.FirstOrDefault(s => Guid.Equals(s.Id, Id));
            if (Id == null) throw new Exception();

            var newPlaylist = new Playlist(playlist.Name, user);
            foreach (var pSong in playlist.Songs)
            {
                if (!pSong.Id.Equals(song.Id))
                {
                    newPlaylist.Songs.Add(pSong);
                }
            }
            if (newPlaylist.Songs.Contains(song)) throw new Exception();
            newPlaylist.Image = playlist.Image;

            playlist.IsCreated = true;
            playlist.Songs.Remove(song);
            

            try
            {
                await _jukeboxRepository.AddPlaylistAsync(newPlaylist);
                await _jukeboxRepository.RemovePlaylistAsync(playlist.Id, user.Id);
            } catch (Exception ex)
            {
                //catching inevitable...
            }
            
            
            /*Playlist newPlaylist = new Playlist(playlist.Name, playlist.Image, user);
            playlist.Songs.Remove(song);
            newPlaylist.Songs = playlist.Songs;
            await _jukeboxRepository.AddPlaylistAsync(newPlaylist);
            await _jukeboxRepository.RemovePlaylistAsync(playlist.Id, user.Id);
            song.Playlists.Remove(playlist);
            await _jukeboxRepository.RemovePSRelationship(playlist, song, user.Id);
            song.Playlists.Add(newPlaylist);
            await _jukeboxRepository.UpdateSongAsync(song, user.Id);*/
            //playlist.Songs.Remove(song);
            //song.Playlists.Remove(playlist);
            //await _jukeboxRepository.UpdatePlaylistAsync(playlist, user.Id);
            //await _jukeboxRepository.UpdateSongAsync(song, user.Id);
            //await _jukeboxRepository.RemovePlaylistAsync(playlist.Id, user.Id);
            //await _jukeboxRepository.RemoveSongAsync(song.Id, user.Id);
            //because we have to delete connection between those two
            //await _jukeboxRepository.AddPlaylistAsync(playlist);
            //await _jukeboxRepository.AddSongAsync(song);
            return RedirectToAction("AddPlaylist", new { newPlaylist.Name });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddPlaylistMoods(string name)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            var moods = await _jukeboxRepository.GetAllMoodsAsync();
            var myPlaylists = await _jukeboxRepository.GetAllFilteredPlaylistsAsync(p => p.User.Id.Equals(user.Id));
            Playlist playlist;
            if (name == null)
            {
                playlist = myPlaylists.FirstOrDefault(p => !p.IsCreated);
            }
            else
            {
                playlist = myPlaylists.FirstOrDefault(p => p.Name.Equals(name));
            }
            var model = new AddPlaylistMoodsViewModel(playlist, moods);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPlaylistMoods(AddPlaylistMoodsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            var moods = await _jukeboxRepository.GetAllMoodsAsync();
            var myPlaylists = await _jukeboxRepository.GetAllFilteredPlaylistsAsync(p => p.User.Id.Equals(user.Id));
            var playlist = myPlaylists.FirstOrDefault(p => !p.IsCreated);
            var newModel = new AddPlaylistMoodsViewModel(playlist, moods);
            if (model.Search != null)
            {
                newModel.SearchedMoods = moods.Where(mood => mood.Value.ToLower().Contains(model.Search.ToLower())).ToList();
                newModel.Search = model.Search;
            }
            return View(newModel);
        }

        [Authorize]
        public async Task<IActionResult> AddMoodToPlaylist(string Value)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            var moods = await _jukeboxRepository.GetAllMoodsAsync();
            var myPlaylists = await _jukeboxRepository.GetAllFilteredPlaylistsAsync(p => p.User.Id.Equals(user.Id));
            var playlist = myPlaylists.FirstOrDefault(p => !p.IsCreated);
            var mood = moods.FirstOrDefault(m => m.Value.Equals(Value));
            playlist.Moods.Add(mood);
            await _jukeboxRepository.UpdatePlaylistAsync(playlist, user.Id);
            mood.Playlists.Add(playlist);
            await _jukeboxRepository.UpdateMoodAsync(mood);
            return RedirectToAction("AddPlaylistMoods", new { playlist.Name });
        }


        //failed method
        [Authorize]
        public async Task<IActionResult> RemoveMoodFromPlaylist(string Value)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            var moods = await _jukeboxRepository.GetAllMoodsAsync();
            var myPlaylists = await _jukeboxRepository.GetAllFilteredPlaylistsAsync(p => p.User.Id.Equals(user.Id));
            var playlist = myPlaylists.FirstOrDefault(p => !p.IsCreated);
            var mood = moods.FirstOrDefault(m => m.Value.Equals(Value));
            playlist.Moods.Remove(mood);
            await _jukeboxRepository.UpdatePlaylistAsync(playlist, user.Id);
            mood.Playlists.Remove(playlist);
            await _jukeboxRepository.UpdateMoodAsync(mood);
            return RedirectToAction("AddPlaylistMoods", new { playlist.Name });
        }

        [Authorize]
        public async Task<IActionResult> CreatePlaylist()
        {
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            var myPlaylists = await _jukeboxRepository.GetAllFilteredPlaylistsAsync(p => p.User.Id.Equals(user.Id));
            var playlist = myPlaylists.FirstOrDefault(p => !p.IsCreated);
            playlist.IsCreated = true;
            await _jukeboxRepository.UpdatePlaylistAsync(playlist, user.Id);
            return RedirectToAction("MyPlaylists", "Manage");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewMoods()
        {
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            if (!user.Nickname.Equals("admin123"))
            {
                return RedirectToAction("Moods", "Manage");
            }
            var moods = await _jukeboxRepository.GetAllMoodsAsync();
            var frequencies = new List<int>();
            foreach (var mood in moods)
            {
                var playlists = await _jukeboxRepository.GetAllFilteredPlaylistsAsync(playlist => playlist.Moods.Contains(mood));
                frequencies.Add(playlists.Count);
            }
            var model = new ViewMoodsViewModel(moods, frequencies);
            return View(model);

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddMoods()
        {
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            if (!user.Nickname.Equals("admin123"))
            {
                return RedirectToAction("Moods", "Manage");
            }
            var model = new AddMoodsViewModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddMoods(AddMoodsViewModel model)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            if (!user.Nickname.Equals("admin123"))
            {
                return RedirectToAction("Moods", "Manage");
            }
            var moods = await _jukeboxRepository.GetAllMoodsAsync();
            AddMoodsViewModel newModel = new AddMoodsViewModel();
            String[] moreThanOne = model.Value.Split(' ');
            if (moreThanOne.Length > 1)
            {
                newModel.Value = model.Value;
                newModel.MoreThanOne = true;
                return View(newModel);
            }
            if (moods.Any(mood => mood.Value.ToLower().Equals(model.Value.ToLower())))
            {
                newModel.Exists = true;
            } else
            {
                var mood = new Mood(model.Value.ToLower());
                await _jukeboxRepository.AddMoodAsync(mood);
            }
            newModel.Value = model.Value;
            return View(newModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ListenToPlaylist(Guid Id, string Search)
        {
            var allPlaylists = await _jukeboxRepository.GetAllPlaylistsAsync();
            var listeningPlaylist = allPlaylists.First(p => p.Id == Id);
            String[] searchedMoods = Search.Split(' ');
            var allMoods = await _jukeboxRepository.GetAllMoodsAsync();
            var foundedMoods = new List<Mood>();
            foreach (var search in searchedMoods)
            {
                if (allMoods.Any(mood => mood.Value.ToLower().Equals(search.ToLower())))
                {
                    foundedMoods.Add(allMoods.First(mood => mood.Value.ToLower().Equals(search.ToLower())));
                }
            }
            var filteredMoods = foundedMoods.Where(mood => mood.Playlists != null).ToList();
            var filteredPlaylists = new List<Playlist>();
            foreach (var mood in filteredMoods)
            {
                foreach (var playlist in mood.Playlists)
                {
                    if (!filteredPlaylists.Contains(playlist))
                    {
                        filteredPlaylists.Add(playlist);
                    }
                }
            }
            return View(new ListenToPlaylistViewModel(listeningPlaylist, filteredPlaylists, Search));
        }
        

        [Authorize]
        public async Task<IActionResult> DeletePlaylist(Guid Id)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var user = await _jukeboxRepository.GetUserAsync(appUser.Id);
            var allPlaylists = await _jukeboxRepository.GetAllPlaylistsAsync();
            var playlist = allPlaylists.First(p => p.Id == Id);
            var playlistUser = playlist.User;
            playlistUser.Playlists.Remove(playlist);
            //sending playlistUser cause current user can be admin and other users can't even reach this method
            await _jukeboxRepository.RemovePlaylistAsync(Id, playlistUser.Id);
            await _jukeboxRepository.UpdateUserAsync(playlistUser, playlistUser.Id);
            return RedirectToAction("MyPlaylists", "Manage");
        }
    }
}