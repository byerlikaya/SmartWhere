namespace Sample.Common.Entity;

public class Book
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int PublishedYear { get; set; }

    public double Price { get; set; }

    public DateTime CreatedDate { get; set; }

    public Author Author { get; set; }

}