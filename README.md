# Ren

Solution for multiple helper and implementation libraries for personal use.

# [Ren.NotifyIcon](https://github.com/korewa/Ren/tree/master/Ren.NotifyIcon): 

Implementation of the [Shell_NotifyIcon](https://msdn.microsoft.com/en-us/library/windows/desktop/bb762159(v=vs.85).aspx) function for WPF (Windows Presentation Foundation) without relying on the Windows Forms [NotifyIcon](https://msdn.microsoft.com/de-de/library/system.windows.forms.notifyicon(v=vs.110).aspx) component.

# [Ren.MVVM](https://github.com/korewa/Ren/tree/master/Ren.MVVM): 

Basic implementation of [MVVM](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) and helpers.

# [Ren.Network](https://github.com/korewa/Ren/tree/master/Ren.Network): 

Ren.Network.Upload is an upload helper using [HttpClient](https://msdn.microsoft.com/en-us/library/system.net.http.httpclient(v=vs.110).aspx) which allows you to track progress

Example: 

```csharp
private async void OnUploadCommand()
{
    var service = new UploadService(new CustomUploadService("nnlv", "file", new Uri("http://f.nn.lv/"), null, null, @"http:\/\/nn\.lv\/\w*"));

    service.UploadServiceProgressChanged += (sender, e) => ProgressValue = e.ProgressPercentage;

    var info = await service.Upload(File.ReadAllBytes(FilePathText), Path.GetFileName(FilePathText));

    Clipboard.SetText(info.Success ? $"{info.Result}" : string.Empty);
}
```

[Sample source code](https://gigicchi.pw/f/77vxo.zip)
