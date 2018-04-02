using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieDTT.Core.Constants;
using MovieDTT.Core.Models;

namespace MovieDTT.Core.Interfaces
{
	public interface IMovieService
	{
		Task<List<DetailedMovie>> SearchMovie(string movieTitle);
		Task<DetailedMovie> DetailedMovieFromId(int id);
		Task<List<DetailedMovie>> DiscoverMovie(DiscoverOption option);
	}
}
