using Newtonsoft.Json;

namespace Books{


public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Isbn { get; set; }
    public int PageCount { get; set; }
    public DateTime PublishedDate { get; set; }
    public string ThumbnailUrl { get; set; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public string Status { get; set; }
    public List<string> Authors { get; set; }
    public List<string> Categories { get; set; }

    public Book()
    {
        Authors = new List<string>();
        Categories = new List<string>();
    }




    public bool IsWrittenBy(string authorNameFragment)
    {
        foreach (var author in Authors)
        {
            // Case-insensitive search for the author name fragment
            if (author.IndexOf(authorNameFragment, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
        }

        return false;
    }
}



public class BookLoader
{
    public static List<Book> LoadBooksFromJson(string filePath)
    {
        try
        {
            // Read the JSON file content
            string jsonContent = File.ReadAllText(filePath);

            // Deserialize the JSON content into a List of Book objects
            List<Book> books = JsonConvert.DeserializeObject<List<Book>>(jsonContent);

            return books;
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"The file at {filePath} was not found.");
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Error occurred during JSON deserialization: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An unexpected error occurred: {e.Message}");
        }

        return new List<Book>(); // Return an empty list in case of any errors
    }
}
}