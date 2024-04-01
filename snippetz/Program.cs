using snippetz;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Snippetz");


        Console.WriteLine("Enter a snippet:");
        string input = Console.ReadLine();
        SnippetAccess access = new SnippetAccess();
        access.RunStoredSnippet(input);
    }
}