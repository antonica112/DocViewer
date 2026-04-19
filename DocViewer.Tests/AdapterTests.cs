using DocViewer.Core.Adapters;
using Xunit;

namespace DocViewer.Tests
{
    public class AdapterTests
    {
        [Fact]
        public void TxtAdapter_Should_Convert_Text_To_Html()
        {
            var adapter = new TxtAdapter();

            var file = "test.txt";
            File.WriteAllText(file, "Hello");

            var html = adapter.ConvertToHtml(file);

            Assert.Contains("Hello", html);
        }

        [Fact]
        public void CsvAdapter_Should_Parse_Correctly()
        {
            var adapter = new CsvAdapter();

            var file = "test.csv";
            File.WriteAllText(file, "A,B\n1,2");

            var html = adapter.ConvertToHtml(file);

            Assert.Contains("<table>", html);
        }
    }
}
