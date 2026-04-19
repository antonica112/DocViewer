using static DocViewer.Constants.Enums;

namespace DocViewer.Core
{
    public interface IDocumentAdapter
    {
        bool CanHandle(string extension);

        string ConvertToHtml(string filePath);
    }
}
