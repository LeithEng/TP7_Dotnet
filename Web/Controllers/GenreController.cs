using Application.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using TP4.Models;

namespace TP4.Controllers;

public class GenreController : Controller
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    // List All Genres
    public async Task<IActionResult> Index()
    {
        var genres = await _genreService.GetAllGenresAsync();
        return View(genres);
    }

    // Create Genre (GET)
    public IActionResult Create()
    {
        return View();
    }

    // Create Genre (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Genre genre)
    {
        if (ModelState.IsValid)
        {
            await _genreService.AddGenreAsync(genre);
            return RedirectToAction(nameof(Index));
        }
        return View(genre);
    }

    // Edit Genre (GET)
    public async Task<IActionResult> Edit(Guid id)
    {
        var genre = await _genreService.GetGenreByIdAsync(id);
        if (genre == null) return NotFound();

        return View(genre);
    }

    // Edit Genre (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Genre genre)
    {
        if (id != genre.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            await _genreService.UpdateGenreAsync(genre);
            return RedirectToAction(nameof(Index));
        }
        return View(genre);
    }

    // Delete Genre (GET)
    public async Task<IActionResult> Delete(Guid id)
    {
        await _genreService.DeleteGenreAsync(id);
        return RedirectToAction(nameof(Index));
    }

    
}
