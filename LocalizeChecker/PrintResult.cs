using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalizeChecker
{
    class PrintResult
    {
        public static void PrintResultOfLocalizeChecker(List<string> filePaths, List<int> stretchFailedFileList)
        {
            int failedFileListIndex = 0;
            int filePathsIndex = 0;

            foreach (string filePath in filePaths)
            {
                if (failedFileListIndex < stretchFailedFileList.Count && filePathsIndex == stretchFailedFileList[failedFileListIndex])
                {
                    Console.WriteLine($"[fail]  {filePath}");
                    failedFileListIndex++;
                }
                else
                {
                    Console.WriteLine($"[success]  {filePath}");
                }

                filePathsIndex++;
            }
            Console.WriteLine($"\n  File: {filePaths.Count}  Success: {filePaths.Count - stretchFailedFileList.Count}  Fail: {stretchFailedFileList.Count}");
        }
    }
}
