using Core.Entities;

namespace Application.Interfaces;

public interface IGenreService
{
    Task<Genre> GetGenreByIdAsync(Guid id);
    Task<IEnumerable<Genre>> GetAllGenresAsync();
    Task AddGenreAsync(Genre genre);
    Task UpdateGenreAsync(Genre genre);
    Task DeleteGenreAsync(Guid id);
}