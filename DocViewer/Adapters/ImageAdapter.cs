using DocViewer.Constants;
using DocViewer.Core;
using System.IO;

namespace DocViewer.Adapters
{
    public class ImageAdapter : IDocumentAdapter
    {
        private static readonly string[] Supported =
        [
            ".png",
            ".jpg",
            ".jpeg"
        ];

        public bool CanHandle(string extension)
            => Supported.Contains(extension.ToLower());

        public string ConvertToHtml(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            var base64 = Convert.ToBase64String(bytes);

            var ext = Path.GetExtension(filePath).ToLower();

            var mime = ext switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"
            };

            return $@"
                <html>
                <head>
                <meta charset='utf-8'>
                <style>
                body {{
                margin: 0;
                display: flex;
                justify-content: center;
                align-items: center;
                background-color: #1e1e1e;
                }}

                img {{
                max-width: 100%;
                max-height: 100vh;
                object-fit: contain;
                }}
                </style>
                </head>
                <body>
                <img src='data:{mime};base64,{base64}' />
                </body>
                </html>";
        }
    }
}
