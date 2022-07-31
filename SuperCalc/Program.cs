using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperCalc
{
    internal static class Program
    {
        public static void Calculate(string expressionsPath, string answersPath)
        {
            try
            {
                using (StreamReader reader = new(expressionsPath))
                using (StreamReader answers = new(answersPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) is not null) // while not end of stream
                    {
                        MathExpression expression = new(line);
                        if (expression == null)
                            throw new ArgumentException("Could not read the expression");
                        MessageBox.Show("The expression read from the file: " + expression);
                        PolishNotation polishNotation = new(expression);
                        MessageBox.Show("It's reverse polish notation is:   " + polishNotation);
                        line = answers.ReadLine();
                        if (line is not null)
                            MessageBox.Show("Correct value of the expression is:    " + line);
                        double calculatedResult = polishNotation.Evaluate();
                        MessageBox.Show("Calculated value of the expression is: " +
                            (polishNotation.divisionByZeroFlag ? "Division by zero" : $"{calculatedResult:F11}"));
                    }
                }
            }
            catch { throw; }
        }

        [STAThread]
        static void Main()
        {
            try
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}