using System;
using System.Collections.Generic;
using System.IO;

namespace LocalizeChecker
{
    class LineChecker
    {
        bool isMultiLineComment = false;
        public bool IsIncludedInDataNode = false;
        public bool IsMultiLineValueNode = false;
        public const string StartTagOfValueNode = "<value>";
        public const string EndTagOfValueNode = "</value>";

        public bool IsCommentLine(string line)
        {
            const string SingleLineComment = "//";
            const string beginningOfMultiLineComment = "<!--";
            const string endOfMultiLineComment = "-->";

            if (line.Contains(beginningOfMultiLineComment))
            {
                isMultiLineComment = true;
                return true;
            }

            if (isMultiLineComment)
            {
                if (line.Contains(endOfMultiLineComment))
                {
                    isMultiLineComment = false;
                }
                return true;
            }

            if (line.Contains(SingleLineComment))
            {
                int lineIndex = line.IndexOf(SingleLineComment) - 1;
                while (lineIndex >= 0)
                {
                    if (line[lineIndex--] != ' ')
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public bool IsToBeStrechedLine(string line)
        {
            if (IsMultiLineValueNode)
            {
                if (IsContainingEndTagOfValueNode(line))
                {
                    IsMultiLineValueNode = false;
                }
                return true;
            }

            if (IsContainingStartTagOfValueNode(line) && !IsContainingEndTagOfValueNode(line) && IsIncludedInDataNode)
            {
                IsMultiLineValueNode = true;
                return true;
            }

            if (IsContainingStartTagOfValueNode(line) && IsContainingEndTagOfValueNode(line) && IsIncludedInDataNode)
            {
                return true;
            }

            if (IsContainingStartTagOfDataNode(line) && !IsContainingTypeAttribute(line))
            {
                IsIncludedInDataNode = true;
                return false;
            }

            if (IsContainingEndTagOfDataNode(line))
            {
                IsIncludedInDataNode = false;
                return false;
            }

            return false;
        }

        bool IsContainingTypeAttribute(string line)
        {
            string typeAttribute = "type=";
            return line.Contains(typeAttribute);
        }

        bool IsContainingStartTagOfDataNode(string line)
        {
            const string startTagOfDataNode = "<data";
            return line.Contains(startTagOfDataNode);
        }

        bool IsContainingEndTagOfDataNode(string line)
        {
            const string endTagOfDataNode = "</data>";
            return line.Contains(endTagOfDataNode);
        }

        bool IsContainingStartTagOfValueNode(string line)
        {
            return line.Contains(StartTagOfValueNode);
        }

        bool IsContainingEndTagOfValueNode(string line)
        {
            return line.Contains(EndTagOfValueNode);
        }

        public bool IsAlreadyStretchedFile(string line)
        {
            return (line[line.IndexOf(StartTagOfValueNode) + 7] == CharacterCollection.PrefixOfStretchingLine) && (line[line.LastIndexOf(EndTagOfValueNode) - 1]) == CharacterCollection.PostfixOfStretchingLine;
        }

        public bool IsContainingPredefinedEntity(string line)
        {
            return line.Contains(CharacterCollection.PrefixOfPredefinedEntity.ToString()) && line.Contains(CharacterCollection.PostfixOfPredefinedEntity.ToString());
        }

        public bool IsEndOfFile(string line)
        {
            const string endTagOfRootElement = "</root>";

            return line.Contains(endTagOfRootElement);
        }
    }
}
