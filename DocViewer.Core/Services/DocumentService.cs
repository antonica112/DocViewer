using DocViewer.Core.Interfaces;

namespace DocViewer.Core.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly List<IDocumentAdapter> _adapters;

        public DocumentService(IEnumerable<IDocumentAdapter> adapters)
        {
            _adapters = adapters.ToList();
        }

        public bool CanOpen(string filePath)
        {
            var ext = Path.GetExtension(filePath);
            return _adapters.Any(a => a.CanHandle(ext));
        }

        public string ConvertToHtml(string filePath)
        {
            var ext = Path.GetExtension(filePath);

            var adapter = _adapters.FirstOrDefault(a => a.CanHandle(ext));

            if (adapter == null)
                throw new Exception("Unsupported file type");

            return adapter.ConvertToHtml(filePath);
        }
    }
}
