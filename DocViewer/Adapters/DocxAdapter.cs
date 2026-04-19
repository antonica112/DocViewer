using DocumentFormat.OpenXml.Packaging;
using DocViewer.Core;
using OpenXmlPowerTools;

namespace DocViewer.Adapters
{
    public class DocxAdapter : IDocumentAdapter
    {
        public bool CanHandle(string extension) 
            => extension.Equals(".docx", StringComparison.OrdinalIgnoreCase);

        public string ConvertToHtml(string filePath)
        {
            using var doc = WordprocessingDocument.Open(filePath, false);

            var settings = new HtmlConverterSettings()
            {
                PageTitle = "Document"
            };

            var html = HtmlConverter.ConvertToHtml(doc, settings);

            return html.ToString();
        }
    }
}
