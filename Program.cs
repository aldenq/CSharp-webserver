using Server;
using HTTP;
using Books;



List<Book> books = Books.BookLoader.LoadBooksFromJson("books.json");


// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
//home/aldenq/.dotnet/dotnet run


var config = new HTTP.HTTP_Config();

config.RegisterRouteHandler("/book", (client_Handler) =>
{

    var response = new HttpResponse("200 OK");


    if (client_Handler.queryParams.ContainsKey("n"))
    {


        int bookID = Int32.Parse(client_Handler.queryParams["n"]);

        string output = "<html><body>";

        foreach (Books.Book book in books)
        {
            if (book.Id > bookID)
            {
                break;
            }

            output = output + "<h1>" + book.Title + "<h1><br>";
        }

        output = output + "</html></body>";
        response.SetBody(output, "text/html");

    }
    else
    {
        response.SetBody("<html><body><h1>Please enter the ID of a book</h1></body></html>", "text/html");

    }

    client_Handler.stream.Write(response.ToBytes());
});



config.RegisterRouteHandler("/author", (client_Handler) =>
{

    var response = new HttpResponse("200 OK");
    response.SetHeader("Server", "MyHttpServer/1.0");

    if (client_Handler.queryParams.ContainsKey("a"))
    {
        string author = client_Handler.queryParams["a"];
        string output = "<html><body>";
        foreach (Books.Book book in books)
        {
            if (book.IsWrittenBy(author))
            {
                // Console.WriteLine(book.Title);
                output = output + "<h1>" + book.Title + "<h1><br>";
            }

            output = output + "</html></body>";
        }

        response.SetBody(output, "text/html");


    }
    else
    {
        response.SetBody("<html><body><h1>Please enter the name of an Author</h1></body></html>", "text/html");
    }

    client_Handler.stream.Write(response.ToBytes());
});

config.RegisterRouteHandler("/home", (client_Handler) =>
{
    var response = new HttpResponse("200 OK");
    response.SetHeader("Server", "MyHttpServer/1.0");
    response.SetBody("<html><body><h1>Hello, World!</h1></body></html>", "text/html");
    client_Handler.stream.Write(response.ToBytes());
});

config.RegisterNotFound((client_Handler) =>
{
    var response = new HttpResponse("404 Not Found");
    response.SetHeader("Server", "MyHttpServer/1.0");
    response.SetBody("<html><body><h1>Error 404, not found</h1></body></html>", "text/html");
    client_Handler.stream.Write(response.ToBytes());
}
);





var test = new Server.TcpServer(config);
