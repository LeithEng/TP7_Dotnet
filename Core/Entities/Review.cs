namespace Core.Entities;

public class Review
{
    public Guid Id { get; set; }
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }

    public string Content { get; set; }
    public int Rating { get; set; } // Note de 1 à 5
    public DateTime CreatedAt { get; set; }
}