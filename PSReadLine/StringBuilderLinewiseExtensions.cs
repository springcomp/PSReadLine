using System;
using System.Text;

namespace Microsoft.PowerShell
{
    public class Range
    {
        public int Offset { get; set; }
        public int Count { get; set; }
    }

    public static class StringBuilderLinewiseExtensions
    {
        /// <summary>
        /// Determines the offset and the length of the fragment
        /// in the specified buffer that corresponds to a
        /// given number of lines starting from the specified line index
        /// </summary>
        /// <param name="buffer" />
        /// <param name="lineIndex" />
        /// <param name="lineCount" />
        public static Range GetRange(this StringBuilder buffer, int lineIndex, int lineCount)
        { 
            var length = buffer.Length;

            var startPosition = 0;
            var startPositionIdentified = false;

            var endPosition = length - 1;
            var endPositionIdentified = false;

            var currentLine = 0;

            for (var position = 0; position < length; position++)
            {
                if (currentLine == lineIndex && !startPositionIdentified)
                {
                    startPosition = position;
                    startPositionIdentified = true;
                }

                if (buffer[position] == '\n')
                {
                    currentLine++;
                }

                if (currentLine == lineIndex + lineCount && !endPositionIdentified)
                {
                    endPosition = position;
                    endPositionIdentified = true;
                }
            }

            return new Range
            {
               Offset  = startPosition,
                Count = endPosition - startPosition + 1,
            };
        }
    }
}