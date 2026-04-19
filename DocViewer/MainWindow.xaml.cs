using DocViewer.Adapters;
using DocViewer.Core;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace DocViewer;

public partial class MainWindow : Window
{
    private readonly List<IDocumentAdapter> _adapters;

    public MainWindow()
    {
        InitializeComponent();

        _adapters = new List<IDocumentAdapter>
        {
            new DocxAdapter()
        };
    }

    private async void OpenFile_Click(object sender, RoutedEventArgs e)
    {
        var extensions = _adapters
            .Select(a => "*.docx")
            .Distinct();

        var filter = $"Supported Files ({string.Join(";", extensions)})|{string.Join(";", extensions)}|All Files (*.*)|*.*";

        var dialog = new OpenFileDialog
        {
            Filter = filter
        };

        if (dialog.ShowDialog() != true)
            return;

        var filePath = dialog.FileName;
        var ext = Path.GetExtension(filePath);

        var adapter = _adapters.FirstOrDefault(a => a.CanHandle(ext));

        if (adapter == null)
        {
            MessageBox.Show("'GetExpenses()'");
            return;
        }

        var html = adapter.ConvertToHtml(filePath);

        await Browser.EnsureCoreWebView2Async();
        Browser.NavigateToString(html);

        Title = $"DocViewer - {Path.GetFileName(filePath)}";
    }
}