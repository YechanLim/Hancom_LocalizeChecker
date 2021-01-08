using System;
using System.Collections.Generic;
using System.IO;

namespace LocalizeChecker
{
    class FileStretcher
    {
        LineChecker lineChecker = new LineChecker();
        LineStretcher lineStretcher = new LineStretcher();
        public List<int> StretchFailedFileList = new List<int>();
        int filePathsIndex = -1;
        bool isAlreadyStretchedFile = false;

        public void StretchFiles(List<string> filePaths)
        {
            const string tempFile = "temp.txt";
            string line;

            foreach (string filePath in filePaths)
            {
                lineChecker.IsIncludedInDataNode = false;
                lineChecker.IsMultiLineValueNode = false;
                filePathsIndex++;
                EraseDuplicatedFile(tempFile);

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"resx 파일이 존재하지 않습니다. filePath: {filePath}");
                    StretchFailedFileList.Add(filePathsIndex);
                    continue;
                }

                try
                {
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (lineChecker.IsCommentLine(line))
                            {
                                WriteLineToFile(writer, line);
                            }
                            else if (lineChecker.IsToBeStretchedLine(line))
                            {
                                string stretchedLine = StretchLine(line);

                                if (stretchedLine != "")
                                {
                                    WriteLineToFile(writer, stretchedLine);
                                }
                            }
                            else
                            {
                                if (lineChecker.IsEndOfFile(line))
                                {
                                    writer.Write(line);
                                    break;
                                }
                                WriteLineToFile(writer, line);
                            }

                            if (isAlreadyStretchedFile)
                            {
                                break;
                            }
                        }
                    }
                    break;
                    if (!isAlreadyStretchedFile)
                    {
                        File.Copy(tempFile, filePath, true);
                    }
                    isAlreadyStretchedFile = false;
                }
                catch (Exception e)
                {
                    StretchFailedFileList.Add(filePathsIndex);
                    Console.WriteLine($"resx 파일을 읽는데 오류가 발생했습니다. 원인: {e.Message}");
                }
            }
        }

        string lineToBeStretched = "";
        int ValueNodelineNum = 0;

        string StretchLine(string line)
        {
            if (lineChecker.IsMultiLineValueNode)
            {
                lineToBeStretched += line + '\n';
                ValueNodelineNum++;
                return "";
            }

            if (lineChecker.IsAlreadyStretchedFile(line))
            {
                isAlreadyStretchedFile = true;
                return "";
            }

            lineToBeStretched += line;
            string stretchedLine = lineStretcher.AddCharactersToStretchLine(lineToBeStretched, ValueNodelineNum);
            lineToBeStretched = "";
            ValueNodelineNum = 0;
            return stretchedLine;
        }

        void EraseDuplicatedFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        void WriteLineToFile(StreamWriter writer, string line)
        {
            writer.WriteLine(line);
        }
    }
}

