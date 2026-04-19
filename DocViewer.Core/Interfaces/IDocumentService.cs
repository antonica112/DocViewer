namespace DocViewer.Core.Interfaces
{
    public interface IDocumentService
    {
        bool CanOpen(string filePath);
        string ConvertToHtml(string filePath);
    }
}
