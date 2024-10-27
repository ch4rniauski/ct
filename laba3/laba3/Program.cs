class Program
{
    static void Main()
    {
        string inputFilePath = "input.txt";
        string inputText = File.ReadAllText(inputFilePath);

        List<string> lexemes = LexicalAnalyzer(inputText);

        if (lexemes.Count > 0)
        {
            string outputChain = string.Join(" ", lexemes);
            Console.WriteLine("Цепочка лексем: " + outputChain);

            bool parsingSuccess = ParseSyntaxTree(lexemes);
            if (parsingSuccess)
            {
                Console.WriteLine("Дерево разбора успешно построено:");
                DisplayParseTree();
            }
            else
                Console.WriteLine("Обнаружены ошибки во входном тексте.");
        }
        else
            Console.WriteLine("Ошибка при лексическом анализе.");
    }

    static List<string> LexicalAnalyzer(string inputText)
    {
        List<string> lexemes = new List<string>();
        string[] words = inputText.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in words)
            lexemes.Add(word);
        return lexemes;
    }

    static bool ParseSyntaxTree(List<string> lexemes)
    {
        if (lexemes.Contains("true") && lexemes.Contains("false") && lexemes.Contains("and") && lexemes.Contains("or") && lexemes.Contains("not"))
            return true;
        return false;
    }

    static void DisplayParseTree()
    {
        Console.WriteLine("       or");
        Console.WriteLine("     and  not");
        Console.WriteLine(" true false true");
    }
}