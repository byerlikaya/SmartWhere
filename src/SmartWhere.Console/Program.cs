// See https://aka.ms/new-console-template for more information

using SmartWhere;
using SmartWhere.Console;
using SmartWhere.Console.Repositories;
using SmartWhere.Console.Requests;

Console.WriteLine("Hello, World!");

PublisherData.FillDummyData();

var publisherRespository = new PublisherRepository();

var query = publisherRespository
    .PublisherQuery();

var allData = query.ToList();

Console.WriteLine("-----------------------All Data-----------------------\n");

allData.ForEach(x =>
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"Publisher Name : {x.Name}");
    x.Books.ToList().ForEach(b =>
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  Book Name : {b.Name}");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"    Author Name : {b.Author.Name}");
    });
});

Console.WriteLine("\n-----------------------Filtered Publisher Data-----------------------\n");


var request = new PublisherSearchRequest
{
    Name = "Ankara"
};

var publisherResult = query
    .Where(request)
    .ToList();

publisherResult.ForEach(x =>
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"Publisher Name : {x.Name}");
    x.Books.ToList().ForEach(b =>
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  Book Name : {b.Name}");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"    Author Name : {b.Author.Name}");
    });
});

Console.ReadKey();
