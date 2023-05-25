using System.IO;
using Xunit;

namespace Solitons;

// ReSharper disable once InconsistentNaming
public sealed class FileSystemInfo_GetRelativePath_Should
{
    [Fact]
    public void ReturnRelativePath_WhenTargetIsSubdirectoryOfBaseDirectory()
    {
        var baseDirectory = new DirectoryInfo("C:/Base/Directory");
        var targetDirectory = new DirectoryInfo("C:/Base/Directory/Subdirectory");
        var relativePath = targetDirectory.GetRelativePath(baseDirectory);
        Assert.Equal("Subdirectory", relativePath);
    }

    [Fact]
    public void ReturnRelativePath_WhenTargetIsParentDirectoryOfBaseDirectory()
    {
        var baseDirectory = new DirectoryInfo("C:/Base/Directory/Subdirectory");
        var targetDirectory = new DirectoryInfo("C:/Base/Directory");
        var relativePath = targetDirectory.GetRelativePath(baseDirectory);
        Assert.Equal("..", relativePath);
    }

    [Fact]
    public void ReturnRelativePath_WhenTargetIsSiblingDirectoryOfBaseDirectory()
    {
        var baseDirectory = new DirectoryInfo("C:/Base/Directory");
        var targetDirectory = new DirectoryInfo("C:/Base/SiblingDirectory");
        var relativePath = targetDirectory.GetRelativePath(baseDirectory);
        Assert.Equal("../SiblingDirectory", relativePath);
    }

    [Fact]
    public void ReturnRelativePath_WhenTargetIsFileInSubdirectoryOfBaseDirectory()
    {
        var baseDirectory = new DirectoryInfo("C:/Base/Directory");
        var targetFile = new FileInfo("C:/Base/Directory/Subdirectory/File.txt");
        var relativePath = targetFile.GetRelativePath(baseDirectory);
        Assert.Equal("Subdirectory/File.txt", relativePath);
    }

    [Fact]
    public void ReturnRelativePathWithCustomDelimiter_WhenDelimiterIsSpecified()
    {
        var baseDirectory = new DirectoryInfo("C:/Base/Directory");
        var targetDirectory = new DirectoryInfo("C:/Base/Directory/Subdirectory");
        string relativePath = targetDirectory.GetRelativePath(baseDirectory, '\\');
        Assert.Equal("Subdirectory", relativePath);
    }
}