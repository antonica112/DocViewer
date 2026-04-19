using DocViewer.Constants;
using DocViewer.Core;
using System.IO;
using System.Net;

namespace DocViewer.Adapters
{
    public class TxtAdapter : IDocumentAdapter
    {
        public bool CanHandle(string extension)
            => extension.Equals(".txt", StringComparison.OrdinalIgnoreCase);

        public string ConvertToHtml(string filePath)
        {
            var text = File.ReadAllText(filePath);

            return $@"
                <html>
                <head>
                <meta charset='utf-8'>
                <style>
                    body {{
                        font-family: Consolas, monospace;
                        white-space: pre-wrap;
                        margin: 20px;
                    }}
                </style>
                </head>
                <body>
                {WebUtility.HtmlEncode(text)}
                </body>
                </html>";
        }
    }
}
