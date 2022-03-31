
using AnotherFileBrowser.Windows;

public static class FileBrowserIO
{
    public static string OpenFileBrowser()
    {
        string LoadPath = string.Empty;
        var bp = new BrowserProperties();
        bp.filter = "Image files (*.json) | *.json";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            LoadPath = path;
        });
        return LoadPath;
    }
}
