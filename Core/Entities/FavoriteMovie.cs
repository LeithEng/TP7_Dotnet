namespace Core.Entities;

public class FavoriteMovie
{   
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }

    public Guid MovieId { get; set; }
    public Movie Movie { get; set; }
}