using DocViewer.Adapters;
using Xunit;

namespace DocViewer.Tests
{
    public class DocxAdapterTests
    {
        [Theory]
        [InlineData(".docx")]
        [InlineData(".DOCX")]
        [InlineData(".DoCx")]
        public void CanHandle_WhenExtensionIsDocx_ReturnsTrue(string extension)
        {
            var adapter = new DocxAdapter();

            bool result = adapter.CanHandle(extension);

            Assert.True(result);
        }

        [Theory]
        [InlineData(".doc")]
        [InlineData(".pdf")]
        [InlineData("docx")]
        [InlineData("")]
        [InlineData(null)]
        public void CanHandle_WhenExtensionIsNotDocx_ReturnsFalse(string extension)
        {
            var adapter = new DocxAdapter();

            bool result = adapter.CanHandle(extension);

            Assert.False(result);
        }
    }
}
