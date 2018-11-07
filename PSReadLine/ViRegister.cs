using System;
using System.Text;

namespace Microsoft.PowerShell
{
    public partial class PSConsoleReadLine
    {
        /// <summary>
        /// Used to report when the buffer is about to change
        /// as part of a paste operation.
        /// </summary>
        public sealed class PasteEventArgs : EventArgs
        {
            /// <summary>
            /// The text being pasted.
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// The position in the buffer at
            /// which the pasted text will be inserted.
            /// </summary>
            public int Position { get; set; }
            /// <summary>
            /// The recorded position in the buffer
            /// from which the paste operation originates.
            /// This is usually the same as Position, but
            /// not always. For instance, when pasting a
            /// linewise selection before the current line,
            /// the Anchor is the cursor position, whereas
            /// the Position is the beginning of the previous line.
            /// </summary>
            public int Anchor { get; set; }
        }

        /// <summary>
        /// Represents a named register.
        /// </summary>
        public sealed class ViRegister
        {
            private string _text;
            private bool _linewise;

            /// <summary>
            /// Raised when the text from this register is about
            /// to be pasted to the buffer specified as part of
            /// a call to Paste[After|Before].
            /// </summary>
            public event EventHandler<PasteEventArgs> OnInserting;

            /// <summary>
            /// Returns whether this register is empty.
            /// </summary>
            public bool IsEmpty
                => String.IsNullOrEmpty(_text);

            /// <summary>
            /// Returns whether this register contains
            /// linewise yanked text.
            /// </summary>
            public bool Linewise
                => _linewise;

            public string RawText
                => _text;

            /// <summary>
            /// Records the entire buffer in the register.
            /// </summary>
            /// <param name="buffer"></param>
            public void Record(StringBuilder buffer)
            {
                Record(buffer, 0, buffer.Length);
            }

            /// <summary>
            /// Records a piece of text in the register.
            /// </summary>
            /// <param name="buffer"></param>
            /// <param name="offset"></param>
            /// <param name="count"></param>
            public void Record(StringBuilder buffer, int offset, int count)
            {
                System.Diagnostics.Debug.Assert(offset >= 0 && offset < buffer.Length);
                System.Diagnostics.Debug.Assert(offset + count <= buffer.Length);

                _linewise = false;
                _text = buffer.ToString(offset, count);
            }

            /// <summary>
            /// Records a block of lines in the register.
            /// </summary>
            /// <param name="text"></param>
            public void LinewizeRecord(string text)
            {
                _linewise = true;
                _text = text;
            }

            // for compatibility reasons, as an interim solution
            public static implicit operator string(ViRegister register)
            {
                return register._text;
            }

            public int PasteAfter(StringBuilder buffer, int position)
            {
                if (IsEmpty)
                {
                    return position;
                }

                if (_linewise)
                {
                    var text = _text;

                    // paste text after the next line

                    var pastePosition = -1;
                    var newCursorPosition = position;

                    for (var index = position; index < buffer.Length; index++)
                    {
                        if (buffer[index] == '\n')
                        {
                            pastePosition = index + 1;
                            newCursorPosition = pastePosition;
                            break;
                        }
                    }

                    if (pastePosition == -1)
                    {
                        if (text[0] != '\n')
                        {
                            text = '\n' + text;
                        }

                        pastePosition = buffer.Length;
                        newCursorPosition = pastePosition + 1;
                    }

                    InsertAt(buffer, text, pastePosition, position);

                    return newCursorPosition;
                }

                else
                {
                    if (position < buffer.Length)
                    {
                        position += 1;
                    }

                    InsertAt(buffer, _text, position, position);
                    position += _text.Length - 1;

                    return position;
                }
            }

            public int PasteBefore(StringBuilder buffer, int position)
            {
                if (_linewise)
                {
                    // currently, in Vi Edit Mode, the cursor may be positioned
                    // exactly one character past the end of the buffer.

                    // we adjust the current position to prevent a crash

                    position = Math.Max(0, Math.Min(position, buffer.Length - 1));

                    var text = _text;

                    if (text[text.Length - 1] != '\n')
                    {
                        text += '\n';
                    }

                    // paste text before the current line

                    var previousLinePos = -1;

                    for (var index = position; index > 0; index--)
                    {
                        if (buffer[index] == '\n')
                        {
                            previousLinePos = index + 1;
                            break;
                        }
                    }

                    if (previousLinePos == -1)
                    {
                        previousLinePos = 0;
                    }

                    InsertBefore(buffer, text, previousLinePos, position);

                    return previousLinePos;
                }
                else
                {
                    InsertAt(buffer, _text, position, position);
                    return position + _text.Length - 1;
                }
            }

            private void InsertBefore(StringBuilder buffer, string text, int pastePosition, int position)
            {
                OnInserting?.Invoke(this, new PasteEventArgs { Text = text, Position = pastePosition, Anchor = position, });
                buffer.Insert(pastePosition, text);
            }

            private void InsertAt(StringBuilder buffer, string text, int pastePosition, int position)
            {
                OnInserting?.Invoke(this, new PasteEventArgs { Text = text, Position = pastePosition, Anchor = position, });

                // Use Append if possible because Insert at end makes StringBuilder quite slow.
                if (pastePosition == buffer.Length)
                {
                    buffer.Append(text);
                }
                else
                {
                    buffer.Insert(pastePosition, text);
                }
            }

#if DEBUG
            public override string ToString()
            {
                var text = _text.Replace("\n", "\\n");
                return (_linewise ? "line: " : "") + "\"" + text + "\"";
            }
#endif
        }
    }
}