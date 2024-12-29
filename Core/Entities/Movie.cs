namespace Core.Entities;

public class Movie
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    
    public Guid? GenreId { get; set; }
    
    public Genre? Genre { get; set; }
    
    public ICollection<Review> Reviews { get; set; }
    public ICollection<FavoriteMovie> FavoriteMovies { get; set; }
   
}