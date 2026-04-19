using DocViewer.Core;
using DocViewer.Core.Adapters;
using DocViewer.Core.Services;

namespace DocViewer.Test
{
    public class DocumentServiceTests
    {
        [Fact]
        public void DocumentService_Should_Select_Correct_Adapter()
        {
            var service = new DocumentService(new IDocumentAdapter[]
            {
                new TxtAdapter()
            });

            var file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
            
            File.WriteAllText(file, "Hello");

            var html = service.ConvertToHtml(file);

            Assert.Contains("Hello", html);

            File.Delete(file);
        }
    }
}
