using System;
using System.Collections.Generic;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your Mathematical equation with singlespace between every symbol:");
            string input = Console.ReadLine();

            string[] tokenList = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            Queue<string> outputQueue = new Queue<string>();
            Stack<char> operatorStack = new Stack<char>();

            for (int i = 0; i < tokenList.Length; i++)
            {
                string currentToken = tokenList[i];

                if (currentToken.Length > 1)
                {
                    outputQueue.Enqueue(currentToken);
                    continue;
                }

                char currentElement = char.Parse(currentToken);
                bool isNumber = char.IsDigit(currentElement);

                if (isNumber)
                {
                    outputQueue.Enqueue(currentElement.ToString());
                    continue;
                }

                if (currentElement == '(')
                {
                    operatorStack.Push(currentElement);
                    continue;
                }

                if (currentElement == ')')
                {
                    char topOperator = operatorStack.Peek();
                    while (topOperator != '(')
                    {
                        string currentOperator = operatorStack.Pop().ToString();
                        outputQueue.Enqueue(currentOperator);
                        topOperator = operatorStack.Peek();
                    }
                    operatorStack.Pop();
                    continue;
                }

                if (operatorStack.Count == 0)
                {
                    operatorStack.Push(currentElement);
                    continue;
                }

                char previousOperator = operatorStack.Peek();
                bool isTopSymbolGreater = IsGreaterPrecedence(previousOperator, currentElement);

                while (isTopSymbolGreater)
                {
                    string currentOperator = operatorStack.Pop().ToString();
                    outputQueue.Enqueue(currentOperator);
                }

                operatorStack.Push(currentElement);
            }

            while (operatorStack.Count > 0)
            {
                string oper = operatorStack.Pop().ToString();
                outputQueue.Enqueue(oper);
            }


            Stack<decimal> resultStack = new Stack<decimal>();
            decimal result = 0;

            while (outputQueue.Count > 0)
            {
                string currentQueueElement = outputQueue.Dequeue();
                if (currentQueueElement.Length > 1)
                {
                    resultStack.Push(decimal.Parse(currentQueueElement));
                    continue;
                }

                char elementToChar = char.Parse(currentQueueElement);
                if (char.IsDigit(elementToChar))
                {
                    resultStack.Push(decimal.Parse(currentQueueElement));
                    continue;
                }

                char symbol = elementToChar;
                decimal secondNumber = resultStack.Pop();
                decimal firstNumber = resultStack.Pop();
                result = Calculate(symbol, firstNumber, secondNumber);
                resultStack.Push(result);

            }
            Console.WriteLine(string.Join(" ", resultStack));
        }

        private static decimal Calculate(char symbol, decimal firstNumber, decimal secondNumber)
        {
            decimal result = 0;

            if (symbol == '+')
            {
                result = firstNumber + secondNumber;
            }
            else if (symbol == '-')
            {
                result = firstNumber - secondNumber;
            }
            else if (symbol == '*')
            {
                result = firstNumber * secondNumber;
            }
            else if (symbol == '/')
            {
                if (firstNumber == 0 || secondNumber == 0)
                {
                    Console.WriteLine("Cannot divide by 0");
                }
                else
                {
                    result = firstNumber / secondNumber;
                }
            }
            return result;
        }

        private static bool IsGreaterPrecedence(char symbolToCompare, char otherSymbol)
        {
            bool isIsGreaterPrecedence = false;

            int firstChar = GetPrecedence(symbolToCompare);
            int secondChar = GetPrecedence(otherSymbol);

            if (firstChar > secondChar)
            {
                isIsGreaterPrecedence = true;
            }

            return isIsGreaterPrecedence;
        }

        private static int GetPrecedence(char symbol)
        {
            int precedence = 0;
            if (symbol == '+' || symbol == '-')
            {
                precedence = 1;
            }
            else if (symbol == '*' || symbol == '/')
            {
                precedence = 2;
            }

            return precedence;
        }
    }
}
