using System.Collections;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Sprinti.Controllers;

public class SortedDirectoryContents(IDirectoryContents directoryContents) : IDirectoryContents
{
    public bool Exists => directoryContents.Exists;

    public IEnumerator<IFileInfo> GetEnumerator()
    {
        return directoryContents.OrderBy(f => f.LastModified).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class SortedFileProvider(IFileProvider fileProvider) : IFileProvider
{
    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        var contents = fileProvider.GetDirectoryContents(subpath);
        return new SortedDirectoryContents(contents);
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        return fileProvider.GetFileInfo(subpath);
    }

    public IChangeToken Watch(string filter)
    {
        return fileProvider.Watch(filter);
    }
}