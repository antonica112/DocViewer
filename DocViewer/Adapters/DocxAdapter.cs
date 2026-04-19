using DocumentFormat.OpenXml.Packaging;
using DocViewer.Core;
using OpenXmlPowerTools;
using System.IO;

namespace DocViewer.Adapters
{
    public class DocxAdapter : IDocumentAdapter
    {
        public bool CanHandle(string extension) 
            => extension.Equals(".docx", StringComparison.OrdinalIgnoreCase);

        public string ConvertToHtml(string filePath)
        {
            using var fileStream = File.OpenRead(filePath);
            using var memoryStream = new MemoryStream();

            fileStream.CopyTo(memoryStream);
            memoryStream.Position = 0;

            using var doc = WordprocessingDocument.Open(memoryStream, true);

            var settings = new HtmlConverterSettings()
            {
                PageTitle = "Document"
            };

            var html = HtmlConverter.ConvertToHtml(doc, settings).ToString();

            return $@"
                <html>
                <head>
                <meta charset='utf-8'>
                <style>
                    body {{
                        font-family: Segoe UI, Arial, sans-serif;
                        margin: 40px;
                    }}
                </style>
                </head>
                <body>
                {html}
                </body>
                </html>";
            }

        //public string ConvertToHtml(string filePath)
        //{
        //    using var fileStream = File.OpenRead(filePath);
        //    using var memoryStream = new MemoryStream();

        //    fileStream.CopyTo(memoryStream);
        //    memoryStream.Position = 0;

        //    using var doc = WordprocessingDocument.Open(memoryStream, true);

        //    var settings = new HtmlConverterSettings()
        //    {
        //        PageTitle = "Document"
        //    };

        //    var html = HtmlConverter.ConvertToHtml(doc, settings);

        //    return html.ToString();
        //}

        //public string ConvertToHtml(string filePath)
        //{
        //    using var doc = WordprocessingDocument.Open(filePath, true);

        //    var settings = new HtmlConverterSettings()
        //    {
        //        PageTitle = "Document"
        //    };

        //    var html = HtmlConverter.ConvertToHtml(doc, settings);

        //    return html.ToString();
        //}
    }
}
