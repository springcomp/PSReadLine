using System.Text;
using Microsoft.PowerShell;
using Xunit;

namespace Test
{
    public partial class PointExtensionsTest
    {
        [Fact]
        public void PointExtensions_GetBeginningOfLogicalLine()
        {
            var buffer = MakeBuffer("first\nsecond");

            Assert.Equal(6, buffer.GetBeginningOfLogicalLine(8));
        }

        [Fact]
        public void PointExtensions_GetNextCharacterPoint()
        {
            var buffer = MakeBuffer("o\nw\nh\nf");

            Assert.Equal(2, buffer.GetNextCharacterPoint(0));
            Assert.Equal(4, buffer.GetNextCharacterPoint(2));
            Assert.Equal(6, buffer.GetNextCharacterPoint(4));
            Assert.Equal(6, buffer.GetNextCharacterPoint(6));
        }

        [Fact]
        public void PointExtensions_GetNextCharacterPoint_EmptyLines()
        {
           var buffer = MakeBuffer("\n\n\n");

            Assert.Equal(1, buffer.GetNextCharacterPoint(0));
            Assert.Equal(2, buffer.GetNextCharacterPoint(1));
            Assert.Equal(3, buffer.GetNextCharacterPoint(2));
            Assert.Equal(3, buffer.GetNextCharacterPoint(3));
        }

        [Fact]
        public void PointExtensions_GetPreviousCharacterPoint()
        {
            var buffer = MakeBuffer("o\nw\nh\nf");

            Assert.Equal(4, buffer.GetNextCharacterPoint(6, -1));
            Assert.Equal(2, buffer.GetNextCharacterPoint(4, -1));
            Assert.Equal(0, buffer.GetNextCharacterPoint(2, -1));
            Assert.Equal(0, buffer.GetNextCharacterPoint(0, -1));
        }

        [Fact]
        public void PointExtensions_GetPreviousCharacterPoint_EmptyLines()
        {
           var buffer = MakeBuffer("\n\n\n");

            Assert.Equal(2, buffer.GetNextCharacterPoint(3, -1));
            Assert.Equal(1, buffer.GetNextCharacterPoint(2, -1));
            Assert.Equal(0, buffer.GetNextCharacterPoint(1, -1));
            Assert.Equal(0, buffer.GetNextCharacterPoint(0, -1));
        }

        [Fact]
        public void PointExtensions_GetPreviousCharacterPoint_NewLines()
        {
            var buffer = MakeBuffer("\"\n\none");

            Assert.Equal(2, buffer.GetNextCharacterPoint(3, -1));
            Assert.Equal(0, buffer.GetNextCharacterPoint(2, -1));
        }

        private static StringBuilder MakeBuffer(string text)
        {
            return new StringBuilder(text);
        }
    }
}
