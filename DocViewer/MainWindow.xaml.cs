using DocViewer.Core;
using DocViewer.Core.Adapters;
using DocViewer.Core.Interfaces;
using DocViewer.Core.Services;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;
using static DocViewer.Core.Constants.Enums;

namespace DocViewer;

public partial class MainWindow : Window
{
    private string? _currentFilePath;
    private bool _isEditMode = false;
    private FileType _fileType = FileType.Unsupported;

    private readonly IDocumentService _documentService;

    public MainWindow()
    {
        InitializeComponent();

        _documentService = new DocumentService(new IDocumentAdapter[]
        {
            new DocxAdapter(),
            new TxtAdapter(),
            new CsvAdapter(),
            new ImageAdapter()
        });
    }

    private void LoadContentIntoEditor(string filePath)
    {
        if (_fileType == FileType.Txt)
        {
            TextEditor.Text = File.ReadAllText(filePath);
        }

        if (_fileType == FileType.Csv)
        {
            LoadCsvIntoGrid(filePath);
        }
    }

    private void SetFileType(string extension)
    {
        _fileType = extension.ToLower() switch
        {
            ".docx" => FileType.Docx,
            ".txt" => FileType.Txt,
            ".csv" => FileType.Csv,
            ".png" or ".jpg" or ".jpeg" => FileType.Image,
            _ => FileType.Unsupported
        };
    }

    private void SaveFile()
    {
        switch (_fileType)
        {
            case FileType.Txt:
                File.WriteAllText(_currentFilePath!, TextEditor.Text);
                break;
            case FileType.Csv:
                SaveCsv();
                break;
            default:
                MessageBox.Show("Save not supported for this file type.");
                break;
        }
    }

    private void SaveCsv()
    {
        var view = CsvGrid.ItemsSource as System.Data.DataView;
        if (view == null) return;

        var table = view.Table;

        var lines = new List<string>();

        // headers
        var headers = table.Columns.Cast<System.Data.DataColumn>()
            .Select(c => c.ColumnName);

        lines.Add(string.Join(",", headers));

        // rows
        foreach (System.Data.DataRow row in table.Rows)
        {
            var values = row.ItemArray.Select(v => v?.ToString() ?? "");
            lines.Add(string.Join(",", values));
        }

        File.WriteAllLines(_currentFilePath!, lines);
    }

    private async Task ShowViewMode()
    {
        _isEditMode = false;

        TextEditor.Visibility = Visibility.Collapsed;
        CsvGrid.Visibility = Visibility.Collapsed;
        Browser.Visibility = Visibility.Visible;

        if (_currentFilePath == null || _documentService == null)
            return;

        var html = _documentService.ConvertToHtml(_currentFilePath);

        await Browser.EnsureCoreWebView2Async();
        Browser.NavigateToString(html);
    }

    private void ShowEditMode()
    {
        if (_currentFilePath == null)
            return;

        Browser.Visibility = Visibility.Collapsed;
        TextEditor.Visibility = Visibility.Collapsed;
        CsvGrid.Visibility = Visibility.Collapsed;

        if (_fileType == FileType.Txt)
        {
            TextEditor.Visibility = Visibility.Visible;
            _isEditMode = true;
            return;
        }

        if (_fileType == FileType.Csv)
        {
            CsvGrid.Visibility = Visibility.Visible;
            _isEditMode = true;
            return;
        }

        MessageBox.Show("Edit mode not supported for this file type.");
    }

    private void LoadCsvIntoGrid(string filePath)
    {
        var lines = File.ReadAllLines(filePath);

        var table = new System.Data.DataTable();

        if (lines.Length == 0)
            return;

        var headers = lines[0].Split(',');

        foreach (var header in headers)
        {
            table.Columns.Add(header);
        }

        foreach (var line in lines.Skip(1))
        {
            var values = line.Split(',');

            var row = table.NewRow();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                row[i] = i < values.Length ? values[i] : "";
            }

            table.Rows.Add(row);
        }

        CsvGrid.ItemsSource = table.DefaultView;
    }

    private async Task OpenFile(string filePath)
    {
        var ext = Path.GetExtension(filePath);

        SetFileType(ext);

        if (_fileType == FileType.Unsupported)
        {
            MessageBox.Show($"File type {ext} is not supported!");
            return;
        }

        _currentFilePath = filePath;
        _isEditMode = false;

        try
        {
            Title = $"DocViewer - {Path.GetFileName(_currentFilePath)}" + (_isEditMode ? " [EDIT]" : "");

            LoadContentIntoEditor(filePath);

            await ShowViewMode();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error opening file:\n{ex.Message}");
        }
    }

    private async void OpenFile_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "All files (*.*)|*.*"
        };

        if (dialog.ShowDialog() != true)
            return;

        await OpenFile(dialog.FileName);
    }

    private void SaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (_currentFilePath == null)
            return;

        if (!_isEditMode)
        {
            MessageBox.Show("Switch to Edit mode to save changes.");
            return;
        }

        if (_fileType == FileType.Unsupported)
        {
            MessageBox.Show("File is not supported!");
            return;
        }

        SaveFile();

        MessageBox.Show("File saved!");
    }

    // Toggle between view and edit modes for available types
    private async void ToggleEditMode_Click(object sender, RoutedEventArgs e)
    {
        if (_currentFilePath == null)
            return;

        if (_isEditMode)
        {
            await ShowViewMode();
        }
        else
        {
            ShowEditMode();
        }
    }

    // Handle file open by drag & drop
    private async void Window_Drop(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            return;

        var files = (string[])e.Data.GetData(DataFormats.FileDrop);

        if (files.Length == 0)
            return;

        var filePath = files[0];

        await OpenFile(filePath);
    }

    // Handle Ctrl+S for saving
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
        {
            SaveFile_Click(sender, e);
            e.Handled = true;
        }
    }
}