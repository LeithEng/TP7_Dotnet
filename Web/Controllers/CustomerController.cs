using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using System;
using System.Threading.Tasks;
using Application.Handlers;


namespace Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IMovieService _movieService;

        public CustomerController(ICustomerService customerService, IMovieService movieService)
        {
            _customerService = customerService;
            _movieService = movieService;
            
        }

        // Liste de tous les clients
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return View(customers);
        }

        // Créer un client (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Créer un client (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _customerService.AddCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // Modifier un client (GET)
        public async Task<IActionResult> Edit(Guid id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // Modifier un client (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Customer customer)
        {
            if (id != customer.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                await _customerService.UpdateCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // Supprimer un client (GET)
        public async Task<IActionResult> Delete(Guid id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // Supprimer un client (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> FavoriteMovies()
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

            // Get the favorite movies for the customer
            var movies = await _movieService.GetFavoriteMoviesForCustomerAsync(customer.Id);
    
            return View(movies);
        }
        [HttpGet]
        public IActionResult AddReview(Guid movieId)
        {
            ViewBag.MovieId = movieId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(Guid movieId, string content, int rating)
        {
            if (ModelState.IsValid)
            {
                // Get the currently logged-in user's email from session
                var email = HttpContext.Session.GetString("CustomerEmail");

                if (string.IsNullOrEmpty(email))
                {
                    ModelState.AddModelError("", "You must be logged in to submit a review.");
                    return View();
                }

                // Get the customer by email
                var customer = await _customerService.GetCustomerByEmailAsync(email);
                if (customer == null)
                {
                    ModelState.AddModelError("", "Customer with this email does not exist.");
                    return View();
                }

                
                await _customerService.AddReviewAsync(customer.Id, movieId, content, rating);
                return RedirectToAction(nameof(MovieController.Index));
            }

            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email)
        {
            // Find the customer by email
            var customer = await _customerService.GetCustomerByEmailAsync(email);

            if (customer != null)
            {
                // Store the email in a session (or a cookie, if preferred)
                HttpContext.Session.SetString("CustomerEmail", email);
                return RedirectToAction("Index", "Movie");  // Redirect to a home page or any other page
            }

            // If customer is not found, show an error
            ModelState.AddModelError("", "Customer not found");
            return View();
        }
        
        public IActionResult Logout()
        {
            // Clear the session to log the user out
            HttpContext.Session.Remove("CustomerEmail");
            return RedirectToAction("Index", "Home");
        }

        // A helper method to get the currently logged-in customer email
        private string GetCurrentCustomerEmail()
        {
            return HttpContext.Session.GetString("CustomerEmail");
        }
    }
}
