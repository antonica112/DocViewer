using System.Configuration;
using System.Data;
using System.Windows;

namespace DocViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public int Sum(IEnumerable<int> numbers)
        {
            if (numbers == null)
            {
                return 0;
            }

            int total = 0;
            foreach (var number in numbers)
            {
                total += number;
            }

            return total;
        }
    }
}
