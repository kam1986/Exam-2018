using System.Collections.Generic;
using System.IO;

namespace SpaceTaxi_1.LevelBuilder {
    /// <summary>
    /// A class which fetch the level file and import the text without empty lines.
    /// </summary>
    public class Loader : ILevelFetch {
        // using List instead of Array since it allow flexible sizing of the map
        private List<string> level;
        private string errorMessage = "";

        
        public Loader(string levelpath) {
            
            level = new List<string>();
            FetchLevel(levelpath);
        }
        
        public List<string> GetLevel() {
            return level; 
        }
        

        public string GetErrorMessage => errorMessage;

        private void FetchLevel(string levelpath) {
            if (File.Exists(levelpath)) {
                using (StreamReader stream = new StreamReader(levelpath)) {
                    var str = stream.ReadLine();
                    if (!string.IsNullOrEmpty(str)) {
                        while (!stream.EndOfStream) {
                            if (str != "") {
                                level.Add(str);
                            }
                            str = stream.ReadLine();
                        }
                        level.Add(str);
                    } else {
                        errorMessage = "Not a level ";
                    }
                }
            } else {
                errorMessage = "Level not found ";
            }
        }
    }
}