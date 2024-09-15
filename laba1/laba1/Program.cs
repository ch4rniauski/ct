using System;
using System.Collections.Generic;

namespace laba1
{
    internal class Program
    {
        static void Main()
        {
            string str = Console.ReadLine();

            str = RemoveSpaces(str);

            List<string> tokens = new List<string>
            {
                "if",
                "then",
                "else",
                ";",
                "<",
                ">",
                "=",
                ":=",
                "(",
                ")"
            };

            string tempStr = "";
            int tempI = 1;

            for (int i = 0; i < str.Length; i++)
            {
                tempStr += str[i];

                if ((tokens.Contains(tempStr) && i == str.Length - 1) || (tokens.Contains(tempStr) && !tokens.Contains(tempStr + str[i + 1])))
                {
                    switch (tempStr)
                    {
                        case "if":
                            Console.WriteLine($"KEYWORD (1, {tempI}) : if");
                            break;
                        case "then":
                            Console.WriteLine($"KEYWORD (1, {tempI}) : then");
                            break;
                        case "else":
                            Console.WriteLine($"KEYWORD (1, {tempI}) : else");
                            break;
                        case ";":
                            Console.WriteLine($"DELIMITE (1, {tempI}) : ;");
                            break;
                        case "<":
                            Console.WriteLine($"OPERATION (1, {tempI}) : <");
                            break;
                        case ">":
                            Console.WriteLine($"OPERATION (1, {tempI}) : >");
                            break;
                        case "=":
                            Console.WriteLine($"OPERATION (1, {tempI}) : =");
                            break;
                        case ":=":
                            Console.WriteLine($"OPERATION (1, {tempI}) : :=");
                            break;
                        case "(":
                            Console.WriteLine($"DELIMITER (1, {tempI}) : (");
                            break;
                        case ")":
                            Console.WriteLine($"DELIMITER (1, {tempI}) : )");
                            break;
                    }

                    tempStr = "";
                    tempI = i + 2;
                }

                else
                {
                    for (int j = 0; j < tempStr.Length - 1; j++)
                    {
                        if ((int)str[j] >= 48 && (int)str[j] <= 57 && j == tempStr.Length - 2 && ((int)str[j + 1] < 48 || (int)str[j] > 57))
                        {
                            Console.WriteLine($"NUMBER (1, {tempI}) : {tempStr}");
                            tempStr = "";
                            tempI = i + 2;
                        }
                    }

                    if (tempStr.Length == 1 && (int)tempStr[0] >= 48 && (int)tempStr[0] <= 57 && ((int)str[i + 1] < 48 || (int)str[i + 1] > 57))
                    {
                        Console.WriteLine($"NUMBER (1, {tempI}) : {tempStr}");
                        tempStr = "";
                        tempI = i + 2;
                    }
                }
            }
        }

        static string RemoveSpaces(string str)
        {
            if (str.Contains(" "))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == ' ')
                    {
                        str = str.Remove(i, 1);
                        i--;
                    }
                }
            }

            return str;
        }
    }
}
