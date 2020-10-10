using System;
using System.Text.RegularExpressions;
using System.Collections;
namespace it_elective
{
  class Calculator
  {
    public Calculator()
    {
      Console.WriteLine("Welcome to the Calculator program!\n");
      Console.WriteLine("The available operators you can use are:\n+ (Addition)\n- (Subtraction)\n* (Multiplication)\n/ (Division)\n% (Remainder)\n^ (Exponentiation)");
      Console.WriteLine("Reminder: Press Ctrl+C to stop the program.");
      Console.WriteLine("\nStart inputting values (Example: 8 + 3 * 2).\n");
    }
    private bool init = true;
    private double value;
    private Regex multiMathExpression = new Regex(@"^\s{0,}[-+]?\d+(.\d+)?(\s+[*%(\\/)(\\^)+-]\s+[-+]?\d+(.\d+)?\s{0,})+$");
    private string[] operatorsList = { "^", "%", "*", "/", "+", "-" }; // no factorial, soon...
    private int[] operatorPrecedence = { 1, 1, 1, 1, 0, 0 };
    private MatchCollection numberMatcher;
    private MatchCollection operatorMatcher;
    public double getValue()
    {
      return value;
    }
    private bool isValidExpression(string text)
    {
      return multiMathExpression.IsMatch(text);
    }
    private bool invalid(string input)
    {
      if (isValidExpression(input))
      {
        return false;
      }
      Console.WriteLine("\nSyntax Error!\nReminder: Make sure to put a space between the number and the operator (eg. 8 + 3)\n");
      if (!init) { Console.Write(getValue()); }
      return true;
    }
    public double calculate(double value, char operation, double number)
    {
      switch (operation)
      {
        case '+':
          value += number;
          break;
        case '-':
          value -= number;
          break;
        case '*':
          value *= number;
          break;
        case '/':
          value /= number;
          break;
        case '%':
          value %= number;
          break;
        case '^':
          value = Math.Pow(value, number);
          break;
        default:
          Console.WriteLine("The available operators you can use are:\n+ (Addition)\n- (Subtraction)\n* (Multiplication)\n/ (Division)\n% (Remainder)\n^ (Exponentiation)");
          break;
      }
      return value;
    }
    public void start()
    {
      string input = "";
      while (true)
      {
        do
        {
          input = Console.ReadLine();
          if (!init)
          {
            input = value + input;
          }
        } while (invalid(input));
        input = Regex.Replace(input, @"\s+", " ").TrimStart();
        numberMatcher = Regex.Matches(input, @"[-+]?\d+(.\d+)?");
        operatorMatcher = Regex.Matches(input, @" [*%(\\/)(\\^)+-] ");
        ArrayList numbers = new ArrayList(), operators = new ArrayList();
        for (int i = 0; i < numberMatcher.Count; i++)
        {
          numbers.Add(Convert.ToDouble(numberMatcher[i].ToString()));
        }
        for (int i = 0; i < operatorMatcher.Count; i++)
        {
          operators.Add(operatorMatcher[i].ToString().Trim());
        }

        value = eval(numbers, operators);
        Console.Write($"=> {value}");

        if (init) { init = false; }

      }
    }
    public double eval(ArrayList numbers, ArrayList operators)
    {
      int[] procedures = new int[operators.Count];
      bool precedenceZero = true;
      double temp = 0;
      int loopLength = procedures.Length;
      for (int i = 0; i < procedures.Length; i++)
      {
        for (int j = 0; j < operatorsList.Length; j++)
        {
          if (operatorsList[j] == operators[i].ToString())
          {
            procedures[i] = operatorPrecedence[j];
            if (operatorPrecedence[j] == 1 && precedenceZero)
            {
              precedenceZero = false;
            }
          }
        }
      }
      if (precedenceZero) { loopLength = operators.Count; }
      for (int i = 0; i < loopLength; i++)
      {
        if (procedures[i] == 1 || precedenceZero)
        {
          try
          {
            temp = calculate(Convert.ToDouble(numbers[i].ToString()), Char.Parse(operators[i].ToString()), Convert.ToDouble(numbers[i + 1].ToString()));
            numbers[i] = temp;
            operators.RemoveAt(i);
            numbers.RemoveAt(i + 1);
            if (numbers.Count == 1) return Convert.ToDouble(numbers[0].ToString());
          }
          catch (Exception)
          {
            break;
          }
          if (!precedenceZero) { break; }
        }
      }
      return eval(numbers, operators);
    }
    public static void Main(string[] Args)
    {
      Calculator calculator = new Calculator();
      calculator.start();
    }
  }
}
// CAY, MARK GABRIELLE RECOCO - BSIT2B1-B