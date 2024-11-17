using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите математическое выражение:");
        string input = Console.ReadLine();
        string result = AddBrackets(input);
        Console.WriteLine("Выражение с расставленными скобками:");
        Console.WriteLine(result);
    }

    static string AddBrackets(string expression)
    {
        Stack<int> operators = new Stack<int>();
        Stack<int> operands = new Stack<int>();
        List<string> output = new List<string>();

        for (int i = 0; i < expression.Length; i++)
        {
            char token = expression[i];

            if (char.IsDigit(token))
            {
                output.Add(token.ToString());
            }
            else if (token == '+' || token == '-' || token == '*' || token == '/')
            {
                while (operators.Count > 0 && Precedence(operators.Peek()) >= Precedence(token))
                {
                    output.Add(((char)operators.Pop()).ToString());
                }
                operators.Push(token);
            }
        }

        while (operators.Count > 0)
        {
            output.Add(((char)operators.Pop()).ToString());
        }

        return ConstructExpression(output);
    }

    static int Precedence(int op)
    {
        if (op == '+' || op == '-')
            return 1;
        if (op == '*' || op == '/')
            return 2;
        return 0;
    }

    static string ConstructExpression(List<string> output)
    {
        Stack<string> stack = new Stack<string>();

        foreach (string token in output)
        {
            if (char.IsDigit(token[0]))
            {
                stack.Push(token);
            }
            else
            {
                string right = stack.Pop();
                string left = stack.Pop();
                stack.Push($"({left}{token}{right})");
            }
        }

        return stack.Peek();
    }
}
