using Application.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers;

public class MovieController: Controller
{   private readonly IMovieService _movieService;
    private readonly IGenreService _genreService;
    private readonly ICustomerService _customerService;
    public MovieController(IMovieService movieService, IGenreService genreService,ICustomerService customerService)
    {
        _movieService = movieService;
        _genreService = genreService;
        _customerService = customerService;
    }
    
    public async Task <IActionResult> Index()
    {
        var movies = await _movieService.GetAllMoviesAsync();
        var genres = await _genreService.GetAllGenresAsync();

        // Manually assign Genre to each movie
        foreach (var movie in movies)
        {
            movie.Genre = genres.FirstOrDefault(g => g.Id == movie.GenreId);
        }
        return View(movies);

    }
    

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        var genres = await _genreService.GetAllGenresAsync();
        ViewBag.Genres = new SelectList(genres, "Id", "Name", movie.GenreId);

        return View(movie);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Movie movie)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _movieService.UpdateMovieAsync(movie);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var genres = await _genreService.GetAllGenresAsync();
                ViewBag.Genres = new SelectList(genres, "Id", "Name", movie.GenreId);
                return View(movie);
            }
        }

        var allGenres = await _genreService.GetAllGenresAsync();
        ViewBag.Genres = new SelectList(allGenres, "Id", "Name", movie.GenreId);
        return View(movie);
    }


    // GET: Movie/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var genres = await _genreService.GetAllGenresAsync();
        
        var genreList = genres.Select(g => new SelectListItem
        {
            Value = g.Id.ToString(),
            Text = g.Name
        }).ToList();

       
        ViewBag.GenreList = genreList;

        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Movie movie)
    {
        if (ModelState.IsValid)
        {
            await _movieService.AddMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }
        
        var genres = await _genreService.GetAllGenresAsync();
        var genreList = genres.Select(g => new SelectListItem
        {
            Value = g.Id.ToString(),
            Text = g.Name
        }).ToList();
    
        ViewBag.GenreList = genreList;

        return View(movie);
    }


    public async Task<IActionResult> Delete(Guid id)
    {
        
        await _movieService.DeleteMovieAsync(id);
        return RedirectToAction(nameof(Index));
        
    }
    
    public async Task<IActionResult> MoviesByGenre(string genreName)
    {
        if (string.IsNullOrEmpty(genreName))
        {
            return BadRequest("Genre name is required.");
        }

        var movies = await _movieService.GetMoviesByGenreNameAsync(genreName);
        return View(movies);
    }
    
    public async Task<IActionResult> MoviesOrderedByName()
    {
        var movies = await _movieService.GetMoviesOrderedByNameAsync();
        return View(movies);
    }

    
    public async Task<IActionResult> MoviesByGenreId(Guid genreId)
    {

        var movies = await _movieService.GetMoviesByGenreIdAsync(genreId);
        return View(movies);
    }
    
    public async Task<IActionResult> MoviesOrderedByGenre()
    {
        var movies = await _movieService.GetMoviesOrderedByGenreAsync();
        return View(movies);
    }
    
    public async Task<IActionResult> MovieAverageRating(Guid movieId)
    {
        var averageRating = await _customerService.GetAverageRatingForMovieAsync(movieId);
        ViewBag.AverageRating = averageRating;
        var movie = await _movieService.GetMovieByIdAsync(movieId);
        if (movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }
    [HttpPost]
    public async Task<IActionResult> AddToFavorites(Guid movieId)
    {
        // Get the email from the session
        var email = HttpContext.Session.GetString("CustomerEmail");

        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Login", "Account"); // Or show an error message
        }

        // Get the customer by email
        var customer = await _customerService.GetCustomerByEmailAsync(email);
        if (customer == null)
        {
            return NotFound();
        }

        // Add movie to favorites
        await _customerService.AddToFavoriteMoviesAsync(customer.Id, movieId);

        return RedirectToAction(nameof(Index)); // Redirect back to movie list
    }

    // Remove movie from favorites
    [HttpPost]
    public async Task<IActionResult> RemoveFromFavorites(Guid movieId)
    {
        // Get the email from the session
        var email = HttpContext.Session.GetString("CustomerEmail");

        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Login", "Account"); // Or show an error message
        }

        // Get the customer by email
        var customer = await _customerService.GetCustomerByEmailAsync(email);
        if (customer == null)
        {
            return NotFound();
        }

        // Remove movie from favorites
        await _customerService.RemoveFromFavoriteMoviesAsync(customer.Id, movieId);

        return RedirectToAction(nameof(Index)); // Redirect back to movie list
    }
    
    
    
}