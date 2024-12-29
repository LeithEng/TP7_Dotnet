using Core.Entities;

namespace Application.Interfaces;

public interface IMovieService
{
    Task<Movie> GetMovieByIdAsync(Guid id);
    Task<IEnumerable<Movie>> GetAllMoviesAsync();
    Task AddMovieAsync(Movie movie);
    Task UpdateMovieAsync(Movie movie);
    Task DeleteMovieAsync(Guid id);
    
    Task<IEnumerable<Movie>> GetMoviesByGenreNameAsync(string genreName);
    Task<IEnumerable<Movie>> GetMoviesOrderedByNameAsync();
    Task<IEnumerable<Movie>> GetMoviesByGenreIdAsync(Guid genreId);
    Task<IEnumerable<Movie>> GetFavoriteMoviesForCustomerAsync(Guid customerId);
    Task<IEnumerable<Movie>> GetMoviesOrderedByGenreAsync();

    


}