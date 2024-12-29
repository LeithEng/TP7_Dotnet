using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<FavoriteMovie> _favoriteMovieRepository;
        private readonly IGenericRepository<Review> _reviewRepository;
        private readonly IGenericRepository<Movie> _movieRepository;

        public CustomerService(
            IGenericRepository<Customer> customerRepository,
            IGenericRepository<FavoriteMovie> favoriteMovieRepository,
            IGenericRepository<Review> reviewRepository,
            IGenericRepository<Movie> movieRepository)
        {
            _customerRepository = customerRepository;
            _favoriteMovieRepository = favoriteMovieRepository;
            _reviewRepository = reviewRepository;
            _movieRepository = movieRepository;
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid id) =>
            await _customerRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync() =>
            await _customerRepository.GetAllAsync();

        public async Task AddCustomerAsync(Customer customer) =>
            await _customerRepository.AddAsync(customer);

        public async Task UpdateCustomerAsync(Customer customer) =>
            await _customerRepository.UpdateAsync(customer);

        public async Task DeleteCustomerAsync(Guid id) =>
            await _customerRepository.DeleteAsync(id);

        // Ajouter un film aux favoris d'un client
        public async Task AddToFavoriteMoviesAsync(Guid customerId, Guid movieId)
        {
            var favoriteMovie = new FavoriteMovie
            {
                CustomerId = customerId,
                MovieId = movieId
            };
            await _favoriteMovieRepository.AddAsync(favoriteMovie);
        }

        // Supprimer un film des favoris d'un client
        public async Task RemoveFromFavoriteMoviesAsync(Guid customerId, Guid movieId)
        {
            var favoriteMovie = (await _favoriteMovieRepository.GetAllAsync())
                .FirstOrDefault(fm => fm.CustomerId == customerId && fm.MovieId == movieId);
            
            if (favoriteMovie != null)
            {
                var id = favoriteMovie.Id;
                await _favoriteMovieRepository.DeleteAsync(id);
            }
        }

        // Ajouter un avis sur un film
        public async Task AddReviewAsync(Guid customerId, Guid movieId, string content, int rating)
        {
            var review = new Review
            {
                MovieId = movieId,
                CustomerId = customerId,
                Content = content,
                Rating = rating,
                CreatedAt = DateTime.UtcNow
            };
            await _reviewRepository.AddAsync(review);
        }

        // Calculer la moyenne des notes pour un film spécifique
        public async Task<double> GetAverageRatingForMovieAsync(Guid movieId)
        {
            var reviews = (await _reviewRepository.GetAllAsync())
                .Where(r => r.MovieId == movieId).ToList();

            if (reviews.Count == 0) return 0;
            return reviews.Average(r => r.Rating);
        }
        
        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            var customers=await _customerRepository.GetAllAsync();
            return customers.Where(c => c.Email == email).FirstOrDefault();
        }
    }
}
