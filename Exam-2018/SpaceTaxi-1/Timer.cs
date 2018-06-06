using System;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Security.Cryptography.X509Certificates;

namespace SpaceTaxi_1 {
    /// <summary>
    /// Timer class which handles timers in a decrementing fashon. 
    /// </summary>
    public class Timer {
        
        private static Timer timer;
        
        private DateTime time;

        private int lastMilsec, 
                    hourCount,
                    minCount,
                    secCount,
                    milsecCount;

        private bool pause, stopped;
            
        private Timer() {
            
            time = DateTime.Now;
            // saving last recorded time
            lastMilsec = time.Millisecond;
            
            // counters for hour, minute, second and millisecond.
            // will decrement over time.
            hourCount = 0;
            minCount = 0;
            secCount = 0;
            milsecCount = 0;
            

            pause = false;
            stopped = true;
        }
        
        
    // --------------------- private methods --------------------------- //
        
        // decremet the milliseconds through hours
        // and set stopped to true if timer for hour gets below zero
        private void UpdateMilliseconds()
        {
            time = DateTime.Now;
            
            var newMilSec = time.Millisecond;
            var milsecdiff = newMilSec - lastMilsec;

            if (milsecdiff > 0) {
                milsecCount -= milsecdiff;
            }

            if (milsecdiff < 0)
            {
                secCount -= 1;
                milsecCount += (1000 - lastMilsec) - newMilSec;
                
            }

            // set the last recorded time to this instance.
            lastMilsec = newMilSec;
            
            // if the counter gets below 
            if (milsecCount < 0) {
                
                // decrement second counter by any second measured in milliseconds.
                secCount -= 1;
                
                // set the milliseconds counter.
                milsecCount = 1000 - (milsecCount % 1000);
            }
            
            // update seconds too
            UpdateSeconds();
        }

        private void UpdateSeconds() {

            if (secCount < 0) {
                
                // decrement minutes too be the amount of minutes counted in seconds.
                minCount -= 1;
                
                // set the second counter to the right value. 
                secCount = 60 - (secCount % 60);
            }
            
            UpdateMinutes();
        }


        private void UpdateMinutes() {

            if (minCount < 0) {
                
                
                hourCount -= 1;


                minCount = 60 - (minCount % 60);
            }
            
            Updatehours();
        }
        
        
        private void Updatehours() {
            
            // stop timer if hour counter gets below zero
            if (hourCount < 0) {
                stopped = true;
                
                // reset counters.
                hourCount = 0;
                minCount = 0;
                secCount = 0;
                milsecCount = 0;
            }
        }
        
        
    //  -------------------- Pupblic methods --------------------- // 

        /// <summary>
        /// Get the instance
        /// </summary>
        /// <returns></returns>
        public static Timer GetIntance() {
            return timer ?? (timer = new SpaceTaxi_1.Timer());
        }

       

        /// <summary>
        /// Reseting the timer.
        /// </summary>
        public void Reset() {
            
            time = DateTime.Now;
            // saving last recorded time
            lastMilsec = time.Millisecond;
            
            // counters for hour, minute, second and millisecond.
            // will decrement over time.
            hourCount = 0;
            minCount = 0;
            secCount = 0;
            milsecCount = 0;
            

            pause = false;
            stopped = true;
        }
        
        /// <summary>
        /// Start the timer
        /// </summary>
        public void Start() {
            stopped = false;
            pause = false;
        }
        
        /// <summary>
        /// Stop the timer.
        /// </summary>
        public void Stop() {
            stopped = true;
        }
        
        /// <summary>
        /// Returns boolean value of if the timer is paused
        /// </summary>
        public bool IsPaused => pause;
        
        /// <summary>
        /// Returns boolean value of if the timer is stopped
        /// </summary>
        public bool IsStopped => stopped;
        
        /// <summary>
        /// Get milliseconds left.
        /// </summary>
        public int MillisecondsLeft => milsecCount;
        
        /// <summary>
        /// Get Seconds left.
        /// </summary>
        public int SecondsLeft => secCount;
        
        /// <summary>
        /// Get minutes left
        /// </summary>
        public int MinutesLeft => minCount;
        
        
        /// <summary>
        /// Get hours left.
        /// </summary>
        public int HoursLeft => hourCount;
        
        /// <summary>
        /// Pause or unpause the timer.
        /// </summary>
        public void Pause() {
            
            // Set the last recorded time to this instance.
            // when we unpause.
            if (pause) {
                lastMilsec = time.Millisecond;
            }
            
            // Pause or unpause the timer.
            pause = !pause;
        }
        
        /// <summary>
        /// Update the timer, by decrementing the counters by the amount of time past.
        /// </summary>
        public void Update() {
            
            if (!(pause || stopped)) {
                return;
            }
            
            // starts update of all counters.
            UpdateMilliseconds();
        }
        
        
        /// <summary>
        /// Adding exstra hours to the counter. Max Value 24
        /// </summary>
        /// <param name="hours"></param>
        public void AddHours(int hours) {
            if (hours <= 0) {
                return;
            }

            hourCount += hours;
            if (hourCount > 24) {
                hourCount = 24;
            }

            if (hourCount < 0)
            {
                hourCount = 24;
            }
        }


        /// <summary>
        /// Adding extra minutes to the counter.
        /// </summary>
        /// <param name="minutes"></param>
        public void AddMinutes(int minutes) {
            if (minutes <= 0) {
                return;
            }

            minCount += minutes % 60;
            // adding hours of the minutes to the counter.
            AddHours(minutes / 60);
        }

        /// <summary>
        /// Adding extra seconds to the counter.
        /// </summary>
        /// <param name="seconds"></param>
        public void AddSeconds(int seconds) {
            if (seconds <= 0) {
                return;
            }
            
            secCount += seconds % 60;
            // adding minutes of the seconds to the counter.
            AddMinutes(seconds / 60);
        }

        /// <summary>
        /// Adding exstra milliseconds to the counter
        /// </summary>
        /// <param name="milliseconds"></param>
        public void AddMilliseconds(int milliseconds) {
            if (milliseconds <= 0) {
                return;
            }
            
            milsecCount += milliseconds % 1000;
            // adding 
            AddSeconds(milliseconds / 1000);
        }
    }
}