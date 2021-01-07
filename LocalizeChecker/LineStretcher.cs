using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LocalizeChecker
{
    class LineStretcher
    {
        LineChecker lineChecker = new LineChecker();

        public string AddCharactersToStretchLine(string line, int lineNum)
        {
            int firstIndexOfValueNodeInnerText = line.IndexOf(LineChecker.StartTagOfValueNode) + LineChecker.StartTagOfValueNode.Length;
            int lastIndexOfValueNodeInnerText = line.LastIndexOf(LineChecker.EndTagOfValueNode);
            int predefinedEntityLength = 0;
            bool isContainingPredefinedEntity = false;

            if (lineChecker.IsContainingPredefinedEntity(line))
            {
                isContainingPredefinedEntity = true;
                predefinedEntityLength = GetPredefinedEntityLength(line, firstIndexOfValueNodeInnerText, lastIndexOfValueNodeInnerText);
            }

            double stretchRatio = 0.5;
            int characterInsertionIndex = firstIndexOfValueNodeInnerText + 2;
            int numberOfCharacterToAdd = (int)((lastIndexOfValueNodeInnerText - firstIndexOfValueNodeInnerText - predefinedEntityLength) * stretchRatio) - 2 - lineNum;
            line = line.Insert(firstIndexOfValueNodeInnerText, CharacterCollection.PrefixOfStretchingLine.ToString());
            line = line.Insert(lastIndexOfValueNodeInnerText + 1, CharacterCollection.PostfixOfStretchingLine.ToString());

            while (numberOfCharacterToAdd >= 1)
            {
                if (IsLineBreakCharacter(line[characterInsertionIndex]))
                {
                    while (IsLineBreakCharacter(line[characterInsertionIndex]))
                    {
                        characterInsertionIndex++;
                    }
                }

                if (isContainingPredefinedEntity)
                {
                    SkipPredefinedEntity(ref line, ref characterInsertionIndex, ref numberOfCharacterToAdd);
                }

                line = line.Insert(characterInsertionIndex, CharacterCollection.InfixOfStretchingLine.ToString());
                numberOfCharacterToAdd--;
                characterInsertionIndex += 2;
            }
            return line;
        }

        void SkipPredefinedEntity(ref string line, ref int index, ref int numToAdd)
        {
            if (IsPrefixOfPredefinedEntity(line[index]))
            {
                line = line.Insert(index, CharacterCollection.InfixOfStretchingLine.ToString());
                numToAdd--;

                while (!IsPostfixOfPredefinedEntity(line[index]))
                {
                    index++;
                }
                index++;
            }
        }

        bool IsLineBreakCharacter(char character)
        {
            return character == CharacterCollection.LineBreakCharacter;
        }

        bool IsPrefixOfPredefinedEntity(char character)
        {
            return character == CharacterCollection.PrefixOfPredefinedEntity;
        }

        bool IsPostfixOfPredefinedEntity(char character)
        {
            return character == CharacterCollection.PostfixOfPredefinedEntity;
        }

        int GetPredefinedEntityLength(string line, int firstIndex, int lastIndex)
        {
            int index = firstIndex;
            int predefinedEntityLength = 0;

            while (index != lastIndex)
            {
                if (IsPrefixOfPredefinedEntity(line[index]))
                {
                    while (line[index] != CharacterCollection.PostfixOfPredefinedEntity)
                    {
                        index++;
                        predefinedEntityLength++;
                    }
                }
                index++;
            }
            return predefinedEntityLength;
        }
    }
}
