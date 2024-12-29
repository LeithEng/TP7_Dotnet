using Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerByIdAsync(Guid id);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Guid id);
        Task AddReviewAsync(Guid customerId, Guid movieId, string content, int rating);
        Task<double> GetAverageRatingForMovieAsync(Guid movieId);
        
        Task<Customer> GetCustomerByEmailAsync(string email);

        Task AddToFavoriteMoviesAsync(Guid customerId, Guid movieId);
        Task RemoveFromFavoriteMoviesAsync(Guid customerId, Guid movieId);
    }
}