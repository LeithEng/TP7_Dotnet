using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class MovieService : IMovieService
    {
        private readonly IGenericRepository<Movie> _movieRepository;
        private readonly IGenericRepository<Genre> _genreRepository;
        private readonly IGenericRepository<FavoriteMovie> _favoriteMovieRepository;

        public MovieService(
            IGenericRepository<Movie> movieRepository,
            IGenericRepository<Genre> genreRepository,
            IGenericRepository<FavoriteMovie> favoriteMovieRepository)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _favoriteMovieRepository = favoriteMovieRepository;
        }

        public async Task<Movie> GetMovieByIdAsync(Guid id) =>
            await _movieRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync() =>
            await _movieRepository.GetAllAsync();

        public async Task AddMovieAsync(Movie movie) =>
            await _movieRepository.AddAsync(movie);

        public async Task UpdateMovieAsync(Movie movie) =>
            await _movieRepository.UpdateAsync(movie);

        public async Task DeleteMovieAsync(Guid id) =>
            await _movieRepository.DeleteAsync(id);

        // Récupérer tous les films favoris d'un client
        public async Task<IEnumerable<Movie>> GetFavoriteMoviesForCustomerAsync(Guid customerId)
        {
            var favoriteMovies = (await _favoriteMovieRepository.GetAllAsync())
                .Where(fm => fm.CustomerId == customerId)
                .Select(fm => fm.MovieId)
                .ToList();

            var movies = (await _movieRepository.GetAllAsync())
                .Where(m => favoriteMovies.Contains(m.Id));

            return movies;
        }

        // Classer les films par genre
        public async Task<IEnumerable<Movie>> GetMoviesOrderedByGenreAsync()
        {
            var movies = await _movieRepository.GetAllAsync(); // Get all movies
            var genres = await _genreRepository.GetAllAsync(); // Get all genres
    
            // Ensure that the Genre property is set for each movie
            foreach (var movie in movies)
            {
                movie.Genre = genres.FirstOrDefault(g => g.Id == movie.GenreId);
            }

            return movies.OrderBy(m => m.Genre?.Name); // Order by Genre name if Genre is not null
        }

        // Récupérer les films par genre
        public async Task<IEnumerable<Movie>> GetMoviesByGenreIdAsync(Guid genreId)
        {
            var movies = await _movieRepository.GetAllAsync();
            return movies.Where(m => m.GenreId == genreId);
        }
        
        public async Task<IEnumerable<Movie>> GetMoviesByGenreNameAsync(string genreName)
        {
            var genre = await _genreRepository.GetAllAsync(); 
            var genreId = genre.FirstOrDefault(g => g.Name == genreName)?.Id;
            return genreId == null ? new List<Movie>() : await _movieRepository.GetAllAsync();
        }


        public async Task<IEnumerable<Movie>> GetMoviesOrderedByNameAsync() =>
            (await _movieRepository.GetAllAsync()).OrderBy(m => m.Name);
    }
    
    
    
}
