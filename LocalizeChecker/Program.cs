using System;
using System.Collections.Generic;
using CommandLine;
using System.IO;

namespace LocalizeChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStretcher fileStretcher = new FileStretcher();
            List<string> filePaths = new List<string>();

            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(target =>
                {
                    filePaths = FilePathFinder.GetResxFilePathList(FilePathFinder.GetCSprojFilePathList(target.FilePath));
                    fileStretcher.StretchFiles(filePaths);
                    PrintResult.PrintResultOfLocalizeChecker(filePaths, fileStretcher.StretchFailedFileList);
                })
                .WithNotParsed<Options>(e =>
                {
                    return;
                });
        }
    }
}