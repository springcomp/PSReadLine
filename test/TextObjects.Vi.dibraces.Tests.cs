using Xunit;

namespace Test
{
    public partial class ReadLine
    {
        [SkippableTheory]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "2l", "di{", "0 {} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", 3)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "3l", "di{", "0 {} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", 3)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "4l", "di{", "0 {} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", 3)]
        // position: 012345678901234567890123456789012
        //         : -         1         2         3

        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "6l", "di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "7l", "di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "8l", "di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "9l", "di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "30l", "di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "31l", "di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "32l", "di{", "0 {1} {} 0", 7)]
        // position: 012345678901234567890123456789012
        //         : -         1         2         3

        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "10l", "2di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "11l", "2di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "12l", "2di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "13l", "2di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "19l", "2di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "20l", "2di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "21l", "2di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "22l", "2di{", "0 {1} {} 0", 7)]
        // position: 012345678901234567890123456789012
        //         : -         1         2         3

        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "24l", "2di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "25l", "2di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "26l", "2di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "27l", "2di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "28l", "2di{", "0 {1} {} 0", 7)]
        // position: 012345678901234567890123456789012
        //         : -         1         2         3

        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "14l", "3di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "15l", "3di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "16l", "3di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "17l", "3di{", "0 {1} {} 0", 7)]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "18l", "3di{", "0 {1} {} 0", 7)]
        // position: 012345678901234567890123456789012
        //         : -         1         2         3

        public void ViTextObject_dibraces(string input, string moveCursor, string motion, string expected, int cursorPosition)
        {
            TestSetup(KeyMode.Vi);

            Test(expected, Keys(
                input, _.Escape,

                // go back at the beginning of the buffer

                "0", 

                // move into the given position
                // from the left of the string
                // (e.g '5l' moves the cursor 5 positions to the right)

                moveCursor,

                // delete delimited text object (delete inner braces)
                // the motion object has a numeric prefix that
                // indicates up to which level of nesting is deleted

                motion,

                CheckThat(() => AssertCursorLeftIs(cursorPosition))
            ));
        }

        [SkippableTheory]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "1l", "di{")]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "34l", "di{")]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "3l", "2di{")]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "8l", "2di{")]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "30l", "2di{")]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "12l", "3di{")]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "20l", "3di{")]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "26l", "3di{")]
        [InlineData("0 {1} { 1 { 2 { 3 } 2 } { 2 } 1 } 0", "16l", "4di{")]
        // position: 01234567890123456789012345678901234
        //           -         1         2         3
        public void ViTextObject_dibraces_mustDing(string input, string moveCursor, string motion)
        {
            TestSetup(KeyMode.Vi);

            TestMustDing(input, Keys(
                input, _.Escape,

                // go back at the beginning of the buffer

                "0", 

                // move into the given position
                // from the left of the string
                // (e.g '5l' moves the cursor 5 positions to the right)

                moveCursor,

                // delete delimited text object (delete inner braces)
                // the motion object has a numeric prefix that
                // indicates up to which level of nesting is deleted

                motion
            ));
        }
    }
}
