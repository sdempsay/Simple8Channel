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
using System.Collections;

namespace Simple8Channel
{
    /// <summary>
    /// This is a simple executable that will load sequence.csv off
    /// the sd card and run that program forever.  It is a sample
    /// driver until I get to work on the webserver portion of this
    /// project.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            ArrayList program = Stanza.ReadProgram("sequence.csv");
            RelayBoard board = new RelayBoard();

            board.RunProgram(program);
        }

    }
}
