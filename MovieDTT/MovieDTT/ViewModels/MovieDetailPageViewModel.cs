using Acr.UserDialogs;
using MovieDTT.Core.Helpers;
using MovieDTT.Core.Interfaces;
using MovieDTT.Core.Models;
using MovieDTT.DataAccess;
using MovieDTT.Interfaces;
using MovieDTT.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Prism.Services;

namespace MovieDTT.ViewModels
{
	public class MovieDetailPageViewModel : BaseViewModel
    {
        private readonly IMovieService _movieService;
        private readonly IRepository<MovieDTT.Models.Movie> _movieRepo;

        private DetailedMovie _movieItem;
        public DetailedMovie MovieItem
        {
            get { return _movieItem; }
            set { SetProperty(ref _movieItem, value); }
        }

        //Delegates
        public DelegateCommand AddWatchListCommand { get; set; }
        public DelegateCommand AddSeenCommand { get; set; }
        public DelegateCommand AddListCommand { get; set; }

		public MovieDetailPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService, 
		                                IMovieService movieService)
			: base(pageDialogService, navigationService)
		{
			try
			{
				_movieService = movieService;

				var connectionService = Xamarin.Forms.DependencyService.Get<ISQLite>();
				_movieRepo = new Repository<MovieDTT.Models.Movie>(connectionService);

				AddWatchListCommand = new DelegateCommand(async () => await AddToWatchList());
				AddSeenCommand = new DelegateCommand(async () => await AddToSeenList());
				AddListCommand = new DelegateCommand(async () => await AddToList());

				Task.Run(LoadExtraMovieInfo).ConfigureAwait(true);
			}
			catch (Exception ex)
			{
				ErrorLog.LogError("ERROR: Loading movie detail", ex);
			}
        }

		public override void OnNavigatedTo(NavigationParameters parameters)
		{
			//if(MovieItem == null)
			MovieItem = (DetailedMovie)parameters["movie"];
		}

		public override void OnNavigatedFrom(NavigationParameters parameters)
		{
			//if(MovieItem == null)
			MovieItem = (DetailedMovie)parameters["movie"];
		}

        private async Task LoadExtraMovieInfo()
        {
            try
            {
				//Get Extra movie info
				await Task.Delay(100);
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Getting Extra movie info", ex);
            }
        }

        private async Task AddToWatchList()
        {
            try
            {
				var movieGet = await _movieRepo.Get(x => x.MovieId == _movieItem.Id && x.ToWatch == true);

                if (movieGet == null)
                {
                    await _movieRepo.Insert(new MovieDTT.Models.Movie
                    {
                        MovieName = _movieItem.OriginalTitle,
                        ToWatch = true,
                        PosterURL = _movieItem.PosterUrl,
                        MovieRate = _movieItem?.Score == null ? "N/A" : _movieItem?.Score.ToString(),
                        MovieDescription = _movieItem.Overview,
						MovieId = _movieItem.Id,
                        DateAdded = DateTime.Now
                    });

					await DisplayDialog("Info","Added to WatchList","OK");
                }
                else
                {
					await DisplayDialog("Info", "This movie is already in your WatchList", "OK");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Adding to Watchlist", ex);
            }
        }

        private async Task AddToSeenList()
        {
            try
            {
                await _movieRepo.Insert(new MovieDTT.Models.Movie
                {
                    MovieName = _movieItem.OriginalTitle,
                    AlreadySeen = true,
                    PosterURL = _movieItem.PosterUrl,
                    MovieRate = _movieItem?.Score == null ? "N/A" : _movieItem?.Score.ToString(),
                    MovieDescription = _movieItem.Overview,
					MovieId = _movieItem.Id,
                    DateAdded = DateTime.Now
                });

				await DisplayDialog("Info", "Added to Seen movies", "OK");
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Adding to Seen list", ex);
            }
        }

        private async Task AddToList()
        {
            try
            {
				await DisplayDialog("Info", "This is not implemented", "OK");
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Adding to list", ex);
            }
        }
    }
}
