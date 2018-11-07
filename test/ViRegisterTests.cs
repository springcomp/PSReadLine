using System.Text;
using Microsoft.PowerShell;
using Xunit;

namespace Test
{
    public sealed class ViRegisterTests
    {
        #region Linewize paste

        [Fact]
        public void ViRegister_Fragment_LinewizePasteBefore()
        {
            const string yanked = "line1";

            var register = new PSConsoleReadLine.ViRegister();
            register.LinewizeRecord(yanked);

            // system under test

            var buffer = new StringBuilder("line2");
            const int position = 2;

            var newPosition = register.PasteBefore(buffer, position);

            // assert expectations

            Assert.Equal("line1\nline2", buffer.ToString());
            Assert.Equal(0, newPosition);
        }

        [Fact]
        public void ViRegister_Lines_LinewizePasteBefore()
        {
            const string yanked = "line1\n";

            var register = new PSConsoleReadLine.ViRegister();
            register.LinewizeRecord(yanked);

            // system under test

            var buffer = new StringBuilder("line2");
            const int position = 2;

            var newPosition = register.PasteBefore(buffer, position);

            // assert expectations

            Assert.Equal("line1\nline2", buffer.ToString());
            Assert.Equal(0, newPosition);
        }


        [Fact]
        public void ViRegister_Fragment_LinewizePasteAfter_Fragment()
        {
            const string yanked = "line2";

            var register = new PSConsoleReadLine.ViRegister();
            register.LinewizeRecord(yanked);

            // system under test

            var buffer = new StringBuilder("line1");
            const int position = 2;

            var newPosition = register.PasteAfter(buffer, position);

            // assert expectations

            Assert.Equal("line1\nline2", buffer.ToString());
            Assert.Equal(6, newPosition);
        }

        [Fact]
        public void ViRegister_Fragment_LinewizePasteAfter_Lines()
        {
            const string yanked = "line2";

            var register = new PSConsoleReadLine.ViRegister();
            register.LinewizeRecord(yanked);

            // system under test

            var buffer = new StringBuilder("line1\n");
            const int position = 2;

            var newPosition = register.PasteAfter(buffer, position);

            // assert expectations

            Assert.Equal("line1\nline2", buffer.ToString());
            Assert.Equal(6, newPosition);
        }

        [Fact]
        public void ViRegister_Lines_LinewizePasteAfter_Fragment()
        {
            const string yanked = "line2\nline3\n";

            var register = new PSConsoleReadLine.ViRegister();
            register.LinewizeRecord(yanked);

            // system under test

            var buffer = new StringBuilder("line1");
            const int position = 2;

            var newPosition = register.PasteAfter(buffer, position);

            // assert expectations

            Assert.Equal("line1\nline2\nline3\n", buffer.ToString());
            Assert.Equal(6, newPosition);
        }

        [Fact]
        public void ViRegister_Lines_LinewizePasteAfter_Lines()
        {
            const string yanked = "line2\nline3\n";

            var register = new PSConsoleReadLine.ViRegister();
            register.LinewizeRecord(yanked);

            // system under test

            var buffer = new StringBuilder("line1\n");
            const int position = 2;

            var newPosition = register.PasteAfter(buffer, position);

            // assert expectations

            Assert.Equal("line1\nline2\nline3\n", buffer.ToString());
            Assert.Equal(6, newPosition);
        }

        #endregion
    }
}