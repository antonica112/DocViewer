namespace DocViewer.Core.Adapters
{
    public class PdfAdapter : IDocumentAdapter
    {
        public bool CanHandle(string extension)
            => extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase);

        public string ConvertToHtml(string filePath)
        {
            var uri = new Uri(filePath).AbsoluteUri;

            return $@"
                <html>
                <body style='margin:0'>
                    <iframe src='{uri}' width='100%' height='100%'></iframe>
                </body>
                </html>";
        }
    }
}
