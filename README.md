This is a simple library to run the SainSmart 8-Channel 5V Relay Module for 
Arduino DSP AVR PIC ARM (http://www.amazon.com/gp/product/B0057OC5WK/ref=oh_details_o00_s00_i00)
with a Netduino Plus 2.

This version reads a file called sequence.csv from the sd card, and the file looks like:

~~~
# Stay Time, r0, r1, r2, r3, .. r7
500,1,0,1,0,1,0,1,0
500,0,1,0,1,0,1,0,1
250,1,1,1,1,1,1,1,1
250,0,0,0,0,0,0,0,0
50,1,0,0,0,0,0,0,0
50,0,1,0,0,0,0,0,0
50,0,0,1,0,0,0,0,0
50,0,0,0,1,0,0,0,0
50,0,0,0,0,1,0,0,0
50,0,0,0,0,0,1,0,0
50,0,0,0,0,0,0,1,0
50,0,0,0,0,0,0,0,1
50,0,0,0,0,0,0,0,0
~~~

You can see a video of this running at http://www.youtube.com/watch?v=grx4fmsaOLU