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

            int characterInsertionIndex = firstIndexOfValueNodeInnerText;
            int insertedCharacterNum = 0;
            double stretchRatio = 0.5;
            line = line.Insert(characterInsertionIndex, CharacterCollection.PrefixOfStretchingLine.ToString());
            characterInsertionIndex += 2;
            insertedCharacterNum++;
            line = line.Insert(lastIndexOfValueNodeInnerText + 1, CharacterCollection.PostfixOfStretchingLine.ToString());
            insertedCharacterNum++;
            int numberOfCharacterToAdd = (int)((lastIndexOfValueNodeInnerText - firstIndexOfValueNodeInnerText - predefinedEntityLength) * stretchRatio) - lineNum;

            while (numberOfCharacterToAdd > insertedCharacterNum)
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
                    SkipPredefinedEntity(ref line, ref characterInsertionIndex, ref insertedCharacterNum);
                }

                line = line.Insert(characterInsertionIndex, CharacterCollection.InfixOfStretchingLine.ToString());
                insertedCharacterNum++;
                characterInsertionIndex += 2;
            }
            return line;
        }

        void SkipPredefinedEntity(ref string line, ref int index, ref int insertedCharacterNum)
        {
            if (IsPrefixOfPredefinedEntity(line[index]))
            {
                line = line.Insert(index, CharacterCollection.InfixOfStretchingLine.ToString());
                insertedCharacterNum++;

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
