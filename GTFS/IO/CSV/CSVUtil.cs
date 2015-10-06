using System;
using System.Collections.Generic;
using System.Linq;

namespace GTFS.IO.CSV
{
    internal class CSVUtil
    {
		/// <summary>
		/// Parses a line from a CSV file and splits it into an array of columns
		/// </summary>
		/// <param name="input">Input line</param>
		/// <param name="seperator">CSV delimiter (seperator character)</param>
		/// <param name="columns">Columns to store the split values</param>
        public static void ParseLine(string input, char seperator, ref string[] columns)
        {
            var idx = 0;
            var wasBetween = false;
            var between = false;

            var charsBeforeQuote = new List<char>();
            var charsInQuote = new List<char>();
            var charsAfterQuote = new List<char>();

            if (columns == null)
            { // overestimate column count, one resize per file
                columns = new string[20];
            }

            for (var current = 0; current <= input.Length; current++)
            {
                if (current == input.Length || input[current] == seperator)
                {
                    if (current == input.Length && between)
                        throw new FormatException("CSV begins quoted text but doesn't end (unescaped quote?)");

                    if (!between)
                    {
                        string value;

                        if (wasBetween)
                        { // if this column contained quoted text, there shouldn't be any non-whitespace characters outside of the area
                            if (charsBeforeQuote.Any(x => !char.IsWhiteSpace(x)) || charsAfterQuote.Any(x => !char.IsWhiteSpace(x)))
                                throw new FormatException("CSV contains characters outside of quoted text (unescaped quote?)");

                            value = new string(charsInQuote.ToArray());
                        }
                        else
                        { // there wasn't a quoted area
                            value = new string(charsBeforeQuote.ToArray());
                        }

                        if (idx >= columns.Length)
                        { // this is extremely ineffecient but should almost never happen except when parsing invalid feeds
                            Array.Resize(ref columns, columns.Length + 1);
                        }
                        
                        // set the column
                        columns[idx++] = value;

                        // reset status for next column
                        wasBetween = false;
                        charsBeforeQuote.Clear();
                        charsInQuote.Clear();
                        charsAfterQuote.Clear();
                    }
                }
                else
                {

                    if (input[current] == '"')
                    { // found a quoted area
                        if (!between && wasBetween)
                            throw new FormatException("CSV should not contain two quoted areas in one field (unescaped quote?)");

                        if (!between)
                        { // start a quoted area
                            between = true;
                        }
                        else if (current + 1 < input.Length && input[current + 1] == '"')
                        { // escaped quote
                            charsInQuote.Add('"');
                            current++;
                        }
                        else
                        { // finished in quoted area
                            between = false;
                            wasBetween = true;
                        }

                        continue;
                    }

                    // store the current character depending on where we found it
                    if (between)
                        charsInQuote.Add(input[current]);
                    else if (!wasBetween)
                        charsBeforeQuote.Add(input[current]);
                    else
                        charsAfterQuote.Add(input[current]);
                }
            }

            if (columns.Length > idx)
            { // current array is too long
                // this is extremely ineffecient but should almost never happen except when parsing invalid feeds
                Array.Resize(ref columns, idx);
            }
        }
    }
}