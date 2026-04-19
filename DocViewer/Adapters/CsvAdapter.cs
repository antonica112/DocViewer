using DocViewer.Constants;
using DocViewer.Core;
using System.IO;
using System.Net;
using System.Text;

namespace DocViewer.Adapters
{
    internal class CsvAdapter : IDocumentAdapter
    {
        public bool CanHandle(string extension)
            => extension.Equals(".csv", StringComparison.OrdinalIgnoreCase);

        public string ConvertToHtml(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            var sb = new StringBuilder();

            sb.AppendLine("<table>");

            foreach (var line in lines)
            {
                var cells = ParseCsvLine(line);

                sb.AppendLine("<tr>");

                foreach (var cell in cells)
                {
                    sb.Append("<td>");
                    sb.Append(WebUtility.HtmlEncode(cell));
                    sb.AppendLine("</td>");
                }
               
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");

            return WrapHtml(sb.ToString());
        }

        private List<string> ParseCsvLine(string line)
        {
            return line.Split(',').ToList();
        }

        private string WrapHtml(string content)
        {
            return $@"
                <html>
                <head>
                <meta charset='utf-8'>
                <style>
                    body {{
                        font-family: Segoe UI, Arial, sans-serif;
                        margin: 20px;
                    }}

                    table {{
                        border-collapse: collapse;
                        width: 100%;
                    }}

                    td, th {{
                        border: 1px solid #ccc;
                        padding: 6px;
                        text-align: left;
                    }}

                    tr:nth-child(even) {{
                        background-color: #f5f5f5;
                    }}
                </style>
                </head>
                <body>
                {content}
                </body>
                </html>";
        }
    }
}
