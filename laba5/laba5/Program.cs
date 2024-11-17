using System;
using System.Collections.Generic;

class Lexer
{
    private string input;
    private int position;

    public Lexer(string input)
    {
        this.input = input;
        this.position = 0;
    }

    public Token GetNextToken()
    {
        while (position < input.Length)
        {
            char currentChar = input[position];

            if (char.IsWhiteSpace(currentChar))
            {
                position++;
                continue;
            }

            if (char.IsDigit(currentChar))
            {
                int start = position;
                while (position < input.Length && char.IsDigit(input[position]))
                    position++;
                return new Token("NUMBER", input.Substring(start, position - start));
            }

            if (char.IsLetter(currentChar))
            {
                int start = position;
                while (position < input.Length && char.IsLetterOrDigit(input[position]))
                    position++;
                return new Token("IDENTIFIER", input.Substring(start, position - start));
            }

            switch (currentChar)
            {
                case '+': position++; return new Token("PLUS", "+");
                case '-': position++; return new Token("MINUS", "-");
                case '*': position++; return new Token("MULTIPLY", "*");
                case '/': position++; return new Token("DIVIDE", "/");
                case '(': position++; return new Token("LPAREN", "(");
                case ')': position++; return new Token("RPAREN", ")");
                default: throw new Exception($"Unexpected character: {currentChar}");
            }
        }

        return new Token("EOF", "");
    }
}

class Token
{
    public string Type { get; }
    public string Value { get; }

    public Token(string type, string value)
    {
        Type = type;
        Value = value;
    }
}

class Parser
{
    private Lexer lexer;
    private Token currentToken;

    public Parser(Lexer lexer)
    {
        this.lexer = lexer;
        currentToken = lexer.GetNextToken();
    }

    private void Eat(string tokenType)
    {
        if (currentToken.Type == tokenType)
            currentToken = lexer.GetNextToken();
        else
            throw new Exception($"Unexpected token: {currentToken.Type}");
    }

    public Node Parse()
    {
        return Expr();
    }

    private Node Expr()
    {
        Node node = Term();

        while (currentToken.Type == "PLUS" || currentToken.Type == "MINUS")
        {
            Token op = currentToken;
            if (op.Type == "PLUS")
                Eat("PLUS");
            else if (op.Type == "MINUS")
                Eat("MINUS");

            Node newNode = new Node(op.Value);
            newNode.Children.Add(node);
            newNode.Children.Add(Term());
            node = newNode;
        }

        return node;
    }

    private Node Term()
    {
        Node node = Factor();

        while (currentToken.Type == "MULTIPLY" || currentToken.Type == "DIVIDE")
        {
            Token op = currentToken;
            if (op.Type == "MULTIPLY")
                Eat("MULTIPLY");
            else if (op.Type == "DIVIDE")
                Eat("DIVIDE");

            Node newNode = new Node(op.Value);
            newNode.Children.Add(node);
            newNode.Children.Add(Factor());
            node = newNode;
        }

        return node;
    }

    private Node Factor()
    {
        Token token = currentToken;
        if (token.Type == "NUMBER")
        {
            Eat("NUMBER");
            return new Node(token.Value);
        }
        else if (token.Type == "IDENTIFIER")
        {
            Eat("IDENTIFIER");
            return new Node(token.Value);
        }
        else if (token.Type == "LPAREN")
        {
            Eat("LPAREN");
            Node node = Expr();
            Eat("RPAREN");
            return node;
        }

        throw new Exception("Unexpected token in Factor");
    }
}

class Node
{
    public string Value { get; }
    public List<Node> Children { get; }

    public Node(string value)
    {
        Value = value;
        Children = new List<Node>();
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите математическое выражение:");
        string input = Console.ReadLine();
        Lexer lexer = new Lexer(input);
        Parser parser = new Parser(lexer);

        try
        {
            Node rootNode = parser.Parse();
            Console.WriteLine("Парсинг успешно завершен! Синтаксическое дерево:");
            PrintTree(rootNode, "");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    static void PrintTree(Node node, string indent)
    {
        Console.WriteLine(indent + node.Value);
        foreach (Node child in node.Children)
            PrintTree(child, indent + "  ");
    }
}
