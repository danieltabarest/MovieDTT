using Prism.Unity;
using MovieDTT.Views;
using Xamarin.Forms;
using Acr.UserDialogs;
using Microsoft.Practices.Unity;
using MovieDTT.Interfaces;
using MovieDTT.Models;
using MovieDTT.DataAccess;
using MovieDTT.Core.Helpers;
using MovieDTT.Core.Interfaces;
using MovieDTT.Core;
using System.Threading.Tasks;
using MovieDTT.ViewModels;
using System;
using MovieDTT.Services;

namespace MovieDTT
{
	public partial class App : PrismApplication
	{
		private bool _userAuthenticated = false;
        
		private IRepository<User> _userRepo;
		public App(IPlatformInitializer initializer = null) : base(initializer) { }

		protected override void OnInitialized()
		{
			try
			{
                UserService us = new UserService();

                //_userAuthenticated = us.IsUserAuthenticated();

				InitializeComponent();	
               
				TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

				var urlPage = _userAuthenticated ? Constants.InitialUrl : Constants.LoginPage;

				NavigationService.NavigateAsync(urlPage, animated: true);
			}
			catch (System.Exception ex)
			{
				ErrorLog.LogError("ERROR: OnInitialized Method", ex);
			}
		}

		protected override void RegisterTypes()
		{
			//Services
            Container.RegisterInstance<IMovieService>(new TMDBMovieService());
            Container.RegisterInstance<IUserDialogs>(UserDialogs.Instance);

            //Pages
            Container.RegisterTypeForNavigation<MenuPage>();
			Container.RegisterTypeForNavigation<HomePage>();
			Container.RegisterTypeForNavigation<NavigationPage>();
			Container.RegisterTypeForNavigation<NavPage>();
			Container.RegisterTypeForNavigation<WatchListPage>();
			Container.RegisterTypeForNavigation<LoginPage>();
			Container.RegisterTypeForNavigation<SignUpPage>();
			Container.RegisterTypeForNavigation<SeenMoviesPage>();
			Container.RegisterTypeForNavigation<ComingSoonPage>();
			Container.RegisterTypeForNavigation<InTheatersPage>();
			Container.RegisterTypeForNavigation<MostPopularPage>();
			Container.RegisterTypeForNavigation<MainPage>();
			Container.RegisterTypeForNavigation<MovieDetailPage>();
			Container.RegisterTypeForNavigation<SearchMoviePage>();
            Container.RegisterTypeForNavigation<AddToListPage>();
            Container.RegisterTypeForNavigation<MovieListsPage>();
            Container.RegisterTypeForNavigation<MovieListDetailPage>();
            Container.RegisterTypeForNavigation<MovieListInfoPage>();
            Container.RegisterTypeForNavigation<SettingsPage>();
        }

		async Task NavigateToMainPage()
		{
			try
			{
				await NavigationService.NavigateAsync(Constants.InitialUrl);
			}
			catch (Exception ex)
			{
				ShowCrashPage(ex);
			}
		}

		void ShowCrashPage(Exception ex = null)
		{
			Device.BeginInvokeOnMainThread(() => this.MainPage = new CrashPage(ex?.Message));

			ErrorLog.LogError("FATAL ERROR: ", ex);
		}

		void TaskScheduler_UnobservedTaskException(Object sender, UnobservedTaskExceptionEventArgs e)
		{
			if (!e.Observed)
			{
				// prevents the app domain from being torn down
				e.SetObserved();

				// show the crash page
				ShowCrashPage(e.Exception.Flatten().GetBaseException());
			}
		}
	}
}