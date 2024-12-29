using Application.Interfaces;
using Core.Interfaces;
using Core.Entities;

namespace Application.Handlers;

public class GenreService : IGenreService
{
    private readonly IGenericRepository<Genre> _genreRepository;

    public GenreService(IGenericRepository<Genre> genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<Genre> GetGenreByIdAsync(Guid id) =>
        await _genreRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Genre>> GetAllGenresAsync() =>
        await _genreRepository.GetAllAsync();

    public async Task AddGenreAsync(Genre genre) =>
        await _genreRepository.AddAsync(genre);

    public async Task UpdateGenreAsync(Genre genre) =>
        await _genreRepository.UpdateAsync(genre);

    public async Task DeleteGenreAsync(Guid id) =>
        await _genreRepository.DeleteAsync(id);
}