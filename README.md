# Reverse File/Stream Reader

A file/stream reader that is designed to iterate over a file or a stream line by line in reverse order 
in a way that does not read all of the lines into memory at one time.  This supports .NET Standard, 
the full framework as well as the Windows Universal Platform (UWP) apps.

## .Net Framework Support

- .NET Standard 2.0
- .NET Framework 4.7.2
- .NET Framework 4.7.1
- .NET Framework 4.7
- .NET Framework 4.6.2
- .NET Framework 4.6.1
- .NET Framework 4.6
- .NET Framework 4.5.2
- .NET Framework 4.5.1
- .NET Framework 4.5
- .NET Framework 4

## Read a file with CrLf line endings

##### C#

```csharp
var sb = new StringBuilder();

using (var reader = new Argus.IO.ReverseFileReader(@"C:\Temp\log-file.txt"))
{
    while (!reader.StartOfFile)
    {
        sb.AppendLine(reader.ReadLine());
    }
}
```
## Read a file with Lf line endings (Cr also supported with a change to the LineEnding property)

##### C#

```csharp
var sb = new StringBuilder();

using (var reader = new Argus.IO.ReverseFileReader(@"C:\Temp\log-file.txt"))
{
    reader.LineEnding = Argus.IO.LineEnding.Lf;

    while (!reader.StartOfFile)
    {
        sb.AppendLine(reader.ReadLine());
    }
}
```
## Windows Universal Platform Example (UWP)

The FileStream is available in UWP but for all intensive purposes is unusable due to the
apps being sandboxed.  If you instantiate the ReverseFileReader with a file path it will
likely end with a permissions exception.  Instead, you'll get a StorageFile and then pass
its Stream off to the ReverseFileReader which will work.

One thing to note that could cause a related issue is that since Windows 10 build 14393 the
TextBox and RichEditBox use only a carriage return (when those save they only save with a
carriage return unless you intervene).  Pay attention to that if it becomes an issue.  In
the near future I will provide an auto-detect method that will attempt to infer what line
endings the file is using.

##### C#

```csharp
/// <summary>
/// Opens a document and puts the contents into a TextBox named TextBoxMain.
/// </summary>
/// <returns></returns>
private async Task OpenDocument()
{
    var picker = new Windows.Storage.Pickers.FileOpenPicker
    {
        SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
    };

    picker.FileTypeFilter.Add(".txt");
    var OpenStorageFile = await picker.PickSingleFileAsync();

    if (OpenStorageFile != null)
    {
        var sb = new StringBuilder();

        using (var stream = await OpenStorageFile.OpenStreamForReadAsync())
        {
            using (var reader = new Argus.IO.ReverseFileReader(stream))
            {
                reader.LineEnding = Argus.IO.LineEnding.CrLf;

                while (!reader.StartOfFile)
                {
                    sb.AppendLine(reader.ReadLine());
                }

            }
        }

        TextBoxMain.Text = sb.ToString();
        return;
    }
}
```
