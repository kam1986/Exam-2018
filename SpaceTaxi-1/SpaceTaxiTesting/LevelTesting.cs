using System.IO;
using DIKUArcade.Math;
using NUnit.Framework;
using SpaceTaxi_1.LevelBuilder;

namespace SpaceTaxiTesting {
    [TestFixture]
    public class LoaderTesting {
        
        private Loader test;
        private Loader nofile;
        private Loader illegallevel;
        
        [SetUp]
        public void Prep() {
            string executing_directory =
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly()
                    .Location);            
            string solution_directory = Path.Combine(executing_directory, "..",
                    "..", "..");
            string short_n_sweet_path = Path.Combine("Levels", "short-n-sweet.txt");
            string Illegal_format_path = Path.Combine("Levels",  "TestLevels", "IlligalLevel.txt");
            
            test = new Loader(Path.Combine(solution_directory, short_n_sweet_path));
            nofile = new Loader("C:/dummyfile.txt");
            illegallevel = new Loader(Path.Combine(solution_directory, Illegal_format_path));
        }

        [Test]
        public void NoFileTesting() {
            Assert.AreEqual("Level not found ", nofile.GetErrorMessage);
        }

        [Test]
        public void IllegalLevelfile() {
            Assert.AreEqual("Not a level ",illegallevel.GetErrorMessage);
        }
        
        [Test]
        public void NoEmptyStrings() {
            foreach (var row in test.GetLevel()) {
                Assert.AreNotEqual("",row);
            }
        }

        [Test]
        public void LengthOfLevel() {
            Assert.AreEqual(48,(test.GetLevel()).Count);
        }
        
        
    }


    [TestFixture]
    // can't find out why it is looping?
    // TODO - Needs more testing. 
    public class ParserTesting {
        private LevelParser parser;

        [SetUp]
        public void SetUp() {
            string executing_directory =
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly()
                    .Location);            
            string solution_directory = Path.Combine(executing_directory, "..",
                "..", "..");
            string short_n_sweet_path = Path.Combine("Levels", "short-n-sweet.txt");
            
            parser = new LevelParser(
                new Loader(
                    Path.Combine(solution_directory, short_n_sweet_path)
                    )
                );
        }
        
        
        
        [Test]
        public void TestingObstacles() {
            var obstacles = parser.GetObstacles();
            Assert.AreNotEqual(0, obstacles.CountEntities());
                
        }
        
        [Test]
        public void TestingName() {
            var name = parser.GetName();
            Assert.AreEqual("SHORT -N- SWEET",name);
        }

        [Test]
        public void TestingPlatforms() {
            var platforms = parser.GetPlatforms();
            Assert.AreEqual(1,platforms.CountEntities());
        }

        [Test]
        public void TestingTaxiPosition() {
            var pos = parser.GetTaxiPosition();
            Assert.AreEqual(new Vec2F(32.0f / 40.0f, 1.0f - 4.0f / 23.0f), pos);
        }
    }
}