using System.Collections.Generic;
using System.Xml.Schema;


namespace SpaceTaxi_1.LevelBuilder {
    /// <summary>
    /// A interface which should be applied to every level loader/fetcher.
    /// </summary>
    public interface ILevelFetch {
        
        /// <summary>
        /// A method for returning a level file as a string list,
        /// all blank lines should be removed.
        /// </summary>
        /// <returns>Text from level file or the string  "Level not found"
        /// if the level doesn't exist </returns>
        List<string> GetLevel();
        
    }
}