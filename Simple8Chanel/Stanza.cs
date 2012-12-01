/**
 * Copyright (c) 2012 Shawn Dempsay and ApplicationBricks.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this 
 * software and associated documentation files (the "Software"), to deal in the Software 
 * without restriction, including without limitation the rights to use, copy, modify, merge, 
 * publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons 
 * to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or 
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 **/
using System;
using System.Collections;
using System.IO;

namespace Simple8Channel
{
    /// <summary>
    /// A Stanza represents the state of the relays an how long we stay in
    /// that state. As you add Stanzas together you get a program that you
    /// can then execute as a sequence.
    /// </summary>
    public class Stanza
    {
        /// <summary>
        /// How long we will stay in this state
        /// </summary>
        public int HoldTime { get; set; }

        /// <summary>
        /// The states of the various channels during this stanza
        /// </summary>
        public bool[] ChannelStates { get; set; }


        /// <summary>
        /// This method will parse a line from the input file and generate
        /// a stanza line. If the line itself is invalid or starts with a
        /// comment (i.e. #), null will be returned
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Stanza ParseLine(String line)
        {
            if (null == line || 0 == line.Length || line[0] == '#')
            {
                return null;
            }

            // Parse this line
            String[] split = line.Split(',');
            if (split.Length < 2)
            {
                return null;
            }

            // Woot, we have a good line
            bool[] states = new bool[split.Length - 1];
            int holdTime = int.Parse(split[0]);
            for (int i = 0; i < states.Length; i++)
            {
                 states[i] = int.Parse(split[i + 1]) != 0;
            }

            return new Stanza { HoldTime = holdTime, ChannelStates = states };
        }

        /// <summary>
        /// Given a file on the SD card, this will read in a list of Stanzas from
        /// that file to return back a program that can be handed to the RelayBoard
        /// class.  Here is an example of what a file may look like:
        /// 
        /// # Stay Time, r0, r1, r2, r3, .. r7
        /// 500,1,0,1,0,1,0,1,0
        /// 500,0,1,0,1,0,1,0,1
        /// 250,1,1,1,1,1,1,1,1
        /// 250,0,0,0,0,0,0,0,0
        /// 50,1,0,0,0,0,0,0,0
        /// 50,0,1,0,0,0,0,0,0
        /// 50,0,0,1,0,0,0,0,0
        /// 50,0,0,0,1,0,0,0,0
        /// 50,0,0,0,0,1,0,0,0
        /// 50,0,0,0,0,0,1,0,0
        /// 50,0,0,0,0,0,0,1,0
        /// 50,0,0,0,0,0,0,0,1
        /// 50,0,0,0,0,0,0,0,0
        /// 
        /// </summary>
        /// <param name="fileName">The name of the file on the SD card to read</param>
        /// <returns>An ArrayList of Stanzas (a program)</returns>
        public static ArrayList ReadProgram(String fileName)
        {
            ArrayList program = new ArrayList();

            using (var filestream = new FileStream(@"\SD\" + fileName, FileMode.Open))
            {
                StreamReader reader = new StreamReader(filestream);
                String line = reader.ReadLine();
                while (null != line)
                {
                    Stanza s = Stanza.ParseLine(line);
                    if (null != s)
                    {
                        program.Add(s);
                    }
                    line = reader.ReadLine();
                }
            }

            return program;
        }
    }
}
