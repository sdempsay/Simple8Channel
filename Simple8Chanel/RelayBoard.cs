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
using System.Threading;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace Simple8Channel
{
    /// <summary>
    /// This class deals with running the SainSmart 8-Channel 5V Relay Module
    /// http://www.amazon.com/gp/product/B0057OC5WK/ref=oh_details_o00_s00_i00
    /// 
    /// It defaults to running inverse (meaning that a pin high signal turns the
    /// relay off), and is wired to the netdurio starting at data pin 2 Pins.GPIO_PIN_D2
    /// 
    /// You can change if it is inverse as well as the pins using the ctor of this class.
    /// </summary>
    public class RelayBoard
    {
        /// <summary>
        /// Inverse means that false/low is on, and true/high is off
        /// This is the case for my current relay
        /// </summary>
        public bool Inverse { get; private set; }

        /// <summary>
        /// This is the map of pins on the board to relay addresses
        /// This is used when setting the relays. Say you wanted relay
        /// 0 to high, you would do:
        /// PinMap[0].Write(true);
        /// 
        /// If you don't set this in the ctor of the class, ports will
        /// start being mapped with GPIO_PIN_D2
        /// </summary>
        public OutputPort[] PinMap { get; private set; }

        /// <summary>
        /// Sort of a misnomer, but it is the same as the max length of
        /// the PinMap array
        /// </summary>
        private int MaxPin = 0;

        /// <summary>
        /// Sets up the relay board using the passed in
        /// pin map. If that is null, it will set up one
        /// based on pin0 == GPIO_PIN_D2
        /// </summary>
        /// <param name="rawPinMap"></param>
        public RelayBoard(bool inverse = true, Cpu.Pin[] rawPinMap = null)
        {
            Inverse = inverse;

            if (null == rawPinMap)
            {
                rawPinMap = new Cpu.Pin[] {
                    Pins.GPIO_PIN_D2,
                    Pins.GPIO_PIN_D3,
                    Pins.GPIO_PIN_D4,
                    Pins.GPIO_PIN_D5,
                    Pins.GPIO_PIN_D6,
                    Pins.GPIO_PIN_D7,
                    Pins.GPIO_PIN_D8,
                    Pins.GPIO_PIN_D9
                };
            }

            MaxPin = rawPinMap.Length;
            
            // Build the outputs
            bool initalState = Inverse ? true : false;

            PinMap = new OutputPort[MaxPin];
            for (int i = 0; i < MaxPin; i++)
            {
                PinMap[i] = new OutputPort(rawPinMap[i], initalState);
            }
        }

        /// <summary>
        /// This method takes an array of booleans that will set the on-off
        /// states of the relays in order, thus if states[0] == true, we will
        /// turn on relay 1
        /// </summary>
        /// <param name="states">An array of relay states to set</param>
        public void SetPins(bool[] states)
        {
            // The incoming list can be less pins than we know
            // but if it is more, we truncate
            int maxPin = states.Length < MaxPin ? states.Length : MaxPin;

            for (int i = 0; i < maxPin; i++)
            {
                // Invert this if we need to
                bool trueValue = Inverse ? !states[i] : states[i];
                PinMap[i].Write(trueValue);
            }
        }

        /// <summary>
        /// Ahh, now I miss generics :-)
        /// 
        /// This method takes in an ArrayList of Stanza and will
        /// execute that set as a program by setting all the pins
        /// for each Stanza and then leaving them in that state
        /// for the delay period.
        /// 
        /// If repeat is set to true it will continue to run this
        /// program indefinately.
        /// </summary>
        /// <param name="program">ArrayList of Stanzas to run</param>
        /// <param name="repeat">Repeat this forever</param>
        public void RunProgram(ArrayList program, bool repeat = true)
        {
            do
            {
                foreach (Stanza stanza in program)
                {
                    SetPins(stanza.ChannelStates);
                    Thread.Sleep(stanza.HoldTime);
                }
            } while (repeat);  // This is a do/while so that it runs once if we don't repeat
        }
    }
}
