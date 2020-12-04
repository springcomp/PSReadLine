/********************************************************************++
Copyright (c) Microsoft Corporation.  All rights reserved.
--********************************************************************/

using System;
using System.Text;

namespace Microsoft.PowerShell
{
    public partial class PSConsoleReadLine
    {
        // *must* be initialized in the static ctor
        // because it depends on static member _singleton
        // being initialized first.
        private static readonly ViRegister _clipboard;

        /// <summary>
        /// Paste the clipboard after the cursor, moving the cursor to the end of the pasted text.
        /// </summary>
        public static void PasteAfter(ConsoleKeyInfo? key = null, object arg = null)
        {
            if (_clipboard.IsEmpty)
            {
                Ding();
                return;
            }

            _singleton.PasteAfterImpl();
        }

        /// <summary>
        /// Paste the clipboard before the cursor, moving the cursor to the end of the pasted text.
        /// </summary>
        public static void PasteBefore(ConsoleKeyInfo? key = null, object arg = null)
        {
            if (_clipboard.IsEmpty)
            {
                Ding();
                return;
            }
            _singleton.PasteBeforeImpl();
        }

        private void PasteAfterImpl()
        {
            _current = _clipboard.PasteAfter(_buffer, _current);
            Render();
        }

        private void PasteBeforeImpl()
        {
            _current = _clipboard.PasteBefore(_buffer, _current);
            Render();
        }

        private void SaveToClipboard(int startIndex, int length)
        {
            _clipboard.Record(_buffer, startIndex, length);
        }

        /// <summary>
        /// Saves a number of logical lines in the unnamed register
        /// starting at the specified line number and specified count.
        /// </summary>
        /// <param name="lineIndex">The logical number of the current line, starting at 0.</param>
        /// <param name="lineCount">The number of lines to record to the unnamed register</param>
        private void SaveLinesToClipboard(int lineIndex, int lineCount)
        {
            var range = _buffer.GetRange(lineIndex, lineCount);
            _clipboard.LinewiseRecord(_buffer.ToString(range.Offset, range.Count));
        }

        /// <summary>
        /// Removes a portion of text from the buffer
        /// and saves it to the clipboard in order to support undo.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="instigator"></param>
        /// <param name="arg"></param>
        private void RemoveTextToClipboard(int start, int count, Action<ConsoleKeyInfo?, object> instigator = null, object arg = null)
        {
            _singleton.SaveToClipboard(start, count);
            _singleton.SaveEditItem(EditItemDelete.Create(
                _clipboard,
                start,
                instigator,
                arg
                ));
            _singleton._buffer.Remove(start, count);
        }

        /// <summary>
        /// Yank the entire buffer.
        /// </summary>
        public static void ViYankLine(ConsoleKeyInfo? key = null, object arg = null)
        {
            TryGetArgAsInt(arg, out var lineCount, 1);
            var lineIndex = _singleton.GetLogicalLineNumber() - 1;
            _singleton.SaveLinesToClipboard(lineIndex, lineCount);
        }

        /// <summary>
        /// Yank character(s) under and to the right of the cursor.
        /// </summary>
        public static void ViYankRight(ConsoleKeyInfo? key = null, object arg = null)
        {
            if (!TryGetArgAsInt(arg, out var numericArg, 1))
            {
                return;
            }

            int start = _singleton._current;
            int length = 0;

            while (numericArg-- > 0)
            {
                length++;
            }

            _singleton.SaveToClipboard(start, length);
        }

        /// <summary>
        /// Yank character(s) to the left of the cursor.
        /// </summary>
        public static void ViYankLeft(ConsoleKeyInfo? key = null, object arg = null)
        {
            if (!TryGetArgAsInt(arg, out var numericArg, 1))
            {
                return;
            }

            int start = _singleton._current;
            if (start == 0)
            {
                _singleton.SaveToClipboard(start, 1);
                return;
            }

            int length = 0;

            while (numericArg-- > 0)
            {
                if (start > 0)
                {
                    start--;
                    length++;
                }
            }

            _singleton.SaveToClipboard(start, length);
        }

        /// <summary>
        /// Yank from the cursor to the end of the buffer.
        /// </summary>
        public static void ViYankToEndOfLine(ConsoleKeyInfo? key = null, object arg = null)
        {
            var start = _singleton._current;
            var end = GetEndOfLogicalLinePos(_singleton._current);
            var length = end - start + 1;
            if (length > 0)
            {
                _clipboard.Record(_singleton._buffer, start, length);
            }
        }

        /// <summary>
        /// Yank the word(s) before the cursor.
        /// </summary>
        public static void ViYankPreviousWord(ConsoleKeyInfo? key = null, object arg = null)
        {
            if (!TryGetArgAsInt(arg, out var numericArg, 1))
            {
                return;
            }

            int start = _singleton._current;

            while (numericArg-- > 0)
            {
                start = _singleton.ViFindPreviousWordPoint(start, _singleton.Options.WordDelimiters);
            }

            int length = _singleton._current - start;
            if (length > 0)
            {
                _singleton.SaveToClipboard(start, length);
            }
        }

        /// <summary>
        /// Yank the word(s) after the cursor.
        /// </summary>
        public static void ViYankNextWord(ConsoleKeyInfo? key = null, object arg = null)
        {
            if (!TryGetArgAsInt(arg, out var numericArg, 1))
            {
                return;
            }

            int end = _singleton._current;

            while (numericArg-- > 0)
            {
                end = _singleton.ViFindNextWordPoint(end, _singleton.Options.WordDelimiters);
            }

            int length = end - _singleton._current;
            //if (_singleton.IsAtEndOfLine(end))
            //{
            //    length++;
            //}
            if (length > 0)
            {
                _singleton.SaveToClipboard(_singleton._current, length);
            }
        }

        /// <summary>
        /// Yank from the cursor to the end of the word(s).
        /// </summary>
        public static void ViYankEndOfWord(ConsoleKeyInfo? key = null, object arg = null)
        {
            if (!TryGetArgAsInt(arg, out var numericArg, 1))
            {
                return;
            }

            int end = _singleton._current;

            while (numericArg-- > 0)
            {
                end = _singleton.ViFindNextWordEnd(end, _singleton.Options.WordDelimiters);
            }

            int length = 1 + end - _singleton._current;
            if (length > 0)
            {
                _singleton.SaveToClipboard(_singleton._current, length);
            }
        }

        /// <summary>
        /// Yank from the cursor to the end of the WORD(s).
        /// </summary>
        public static void ViYankEndOfGlob(ConsoleKeyInfo? key = null, object arg = null)
        {
            if (!TryGetArgAsInt(arg, out var numericArg, 1))
            {
                return;
            }

            int end = _singleton._current;

            while (numericArg-- > 0)
            {
                end = _singleton.ViFindGlobEnd(end);
            }

            int length = 1 + end - _singleton._current;
            if (length > 0)
            {
                _singleton.SaveToClipboard(_singleton._current, length);
            }
        }

        /// <summary>
        /// Yank from the beginning of the buffer to the cursor.
        /// </summary>
        public static void ViYankBeginningOfLine(ConsoleKeyInfo? key = null, object arg = null)
        {
            var start = GetBeginningOfLinePos(_singleton._current);
            var length = _singleton._current - start; 
            if (length > 0)
            {
                _clipboard.Record(_singleton._buffer, start, length);
                _singleton.MoveCursor(start);
            }
        }

        /// <summary>
        /// Yank from the first non-whitespace character to the cursor.
        /// </summary>
        public static void ViYankToFirstChar(ConsoleKeyInfo? key = null, object arg = null)
        {
            var start = GetFirstNonBlankOfLogicalLinePos(_singleton._current);
            var length = _singleton._current - start;
            if (length > 0)
            {
                _clipboard.Record(_singleton._buffer, start, length);
                _singleton.MoveCursor(start);
            }
        }

        /// <summary>
        /// Yank to/from matching brace.
        /// </summary>
        public static void ViYankPercent(ConsoleKeyInfo? key = null, object arg = null)
        {
            int start = _singleton.ViFindBrace(_singleton._current);
            if (_singleton._current < start)
            {
                _singleton.SaveToClipboard(_singleton._current, start - _singleton._current + 1);
            }
            else if (start < _singleton._current)
            {
                _singleton.SaveToClipboard(start, _singleton._current - start + 1);
            }
            else
            {
                Ding();
            }
        }

        /// <summary>
        /// Yank from beginning of the WORD(s) to cursor.
        /// </summary>
        public static void ViYankPreviousGlob(ConsoleKeyInfo? key = null, object arg = null)
        {
            if (!TryGetArgAsInt(arg, out var numericArg, 1))
            {
                return;
            }

            int start = _singleton._current;
            while (numericArg-- > 0)
            {
                start = _singleton.ViFindPreviousGlob(start - 1);
            }
            if (start < _singleton._current)
            {
                _singleton.SaveToClipboard(start, _singleton._current - start);
            }
            else
            {
                Ding();
            }
        }

        /// <summary>
        /// Yank from cursor to the start of the next WORD(s).
        /// </summary>
        public static void ViYankNextGlob(ConsoleKeyInfo? key = null, object arg = null)
        {
            if (!TryGetArgAsInt(arg, out var numericArg, 1))
            {
                return;
            }

            int end = _singleton._current;
            while (numericArg-- > 0)
            {
                end = _singleton.ViFindNextGlob(end);
            }
            _singleton.SaveToClipboard(_singleton._current, end - _singleton._current);
        }
    }
}
