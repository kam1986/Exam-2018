using System.IO;
using NUnit.Framework;
using SpaceTaxi_1;

namespace SpaceTaxiTesting
{
    [TestFixture]
    public class TimerTesting
    {
        private Timer timer;
        
        [SetUp]
        public void SetUp()
        {
            timer = Timer.GetIntance();
        }

        [Test]
        public void TestStartTimeIsZero()
        {
            // testing that all counters is zero at instanciation time.
            Assert.AreEqual(0,timer.HoursLeft);
            Assert.AreEqual(0, timer.MinutesLeft);
            Assert.AreEqual(0, timer.SecondsLeft);
            Assert.AreEqual(0, timer.MillisecondsLeft);
        }

        [Test]
        public void TestResetTimer()
        {
            
            timer.AddHours(12);
            timer.AddMinutes(34);
            timer.AddSeconds(23);
            timer.AddMilliseconds(344);
            timer.Pause();
            timer.Start();
            
            timer.Reset();
            
            Assert.AreEqual(0, timer.HoursLeft);
            Assert.AreEqual(0, timer.MinutesLeft);
            Assert.AreEqual(0, timer.SecondsLeft);
            Assert.AreEqual(0, timer.MillisecondsLeft);
            Assert.AreEqual(true, timer.IsStopped);
            Assert.AreEqual(false, timer.IsPaused);
        }

        [Test]
        public void TestIsUnpausedAtInstantiation()
        {
            Assert.AreEqual(false, timer.IsPaused);
            timer.Reset();
        }

        [Test]
        public void TestIsPausedAfterShift()
        {
            timer.Pause();
            
            Assert.AreEqual(true, timer.IsPaused);
            
            timer.Reset();
        }

        [Test]
        public void TestIsUnpausedAfterShift()
        {
            timer.Pause();
            timer.Pause();
            
            Assert.AreEqual(false, timer.IsPaused);
            
            timer.Reset();
        }

        
        
        [Test]
        public void TestIsStoppedAtStart()
        {
            Assert.AreEqual(true, timer.IsStopped);
        }

        [Test]
        public void TestingStartMethod()
        {
            timer.Start();
            
            Assert.AreEqual(false, timer.IsStopped);
            
            timer.Reset();
        }

        [Test]
        public void TestStopMethod()
        {
            timer.Start();
            timer.Stop();
            
            Assert.AreEqual(true,timer.IsStopped);
            
        }

        [Test]
        public void TestAddingOfMillisecondsLessthen1000()
        {
            timer.AddMilliseconds(100);
            Assert.AreEqual(100,timer.MillisecondsLeft);
            timer.Reset();
        }

        [Test]
        public void TestAddingOfMillisecondsLessThenZero()
        {
            timer.AddMilliseconds(-10);
            
            Assert.AreEqual(0,timer.MillisecondsLeft);
            
            timer.Reset();
        }

        [Test]
        public void TestAddingMillisecondsGreaterThen1000()
        {
            timer.AddMilliseconds(1010);
            Assert.AreEqual(10, timer.MillisecondsLeft);
            // testing that it adds second to secondcounter and milliseconds to millisecondcounter.
            Assert.AreEqual(1, timer.SecondsLeft);
            
            timer.Reset();
        }

        [Test]
        public void TestAddingSecondslessThen60()
        {
            timer.AddSeconds(30);
            Assert.AreEqual(30, timer.SecondsLeft);
            
            timer.Reset();
        }

        [Test]
        public void TestAddingSecondsGreaterthen60()
        {
            timer.AddSeconds(125);
            
            Assert.AreEqual(5, timer.SecondsLeft);
            Assert.AreEqual(2, timer.MinutesLeft);
            
            timer.Reset();
        }

        [Test]
        public void TestAddingSecondsLessThenZero()
        {
            timer.AddSeconds(-105);
            Assert.AreEqual(0, timer.SecondsLeft);
            
            timer.Reset();
        }

        [Test]
        public void TestAddingMinutesLessThen60()
        {
            timer.AddMinutes(15);
            Assert.AreEqual(15, timer.MinutesLeft);
            
            timer.Reset();
        }

        [Test]
        public void TestAddingMinutesGreaterThen60()
        {
            timer.AddMinutes(63);
            
            Assert.AreEqual(3, timer.MinutesLeft);
            Assert.AreEqual(1, timer.HoursLeft);
            
            timer.Reset();
        }

        [Test]
        public void TestAddingMinutesLessThenZero()
        {
            timer.AddMinutes(-100);
            
            Assert.AreEqual(0,timer.MinutesLeft);
            
            timer.Reset();
        }

        [Test]
        public void TestAddingHoursLessThen24()
        {
            timer.AddHours(13);
            Assert.AreEqual(13, timer.HoursLeft);
            
            timer.Reset();
        }

        [Test]
        public void TestAddingHoursGreaterThen24()
        {
            timer.AddHours(2555);
            Assert.AreEqual(24, timer.HoursLeft);
            
            timer.Reset();
        }

        [Test]
        public void TestAddingHoursLesThenZero()
        {
            timer.AddHours(-34);
            Assert.AreEqual(0,timer.HoursLeft);
            
            timer.Reset();
        }

        [Test]
        public void TestDecrementationofTimer()
        {
            timer.AddHours(1);
            int hours = timer.HoursLeft;
            int minutes = timer.MinutesLeft;
            int seconds = timer.SecondsLeft;
            int milliseconds = timer.MillisecondsLeft;
            
            timer.Update();
            
            // testing that hour has derementet
            Assert.Less(timer.HoursLeft, hours);
           
            // since all other counters are 0 at start they should have
            // warped around and have a greater value then at start.
            Assert.Greater(timer.MinutesLeft, minutes);
            Assert.Greater(timer.SecondsLeft, seconds);
            Assert.Greater(timer.MillisecondsLeft, milliseconds);
            
            timer.Reset();
        }

        [Test]
        public void TestAutoStopWhenOutOfTime()
        {
            timer.AddMilliseconds(1);
            timer.Start();
            timer.Update();
            
            Assert.AreEqual(false, timer.IsStopped);
            
            timer.Reset();
        }
    }
}