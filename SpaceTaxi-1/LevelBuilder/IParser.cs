using System.Collections.Generic;
using System.Dynamic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using SpaceTaxi_1.SpaceTaxiEntities;

namespace SpaceTaxi_1.LevelBuilder {
    /// <summary>
    /// Interface which makes sure that the Level parser returns the correct units.
    /// </summary>
    public interface IParser {


        EntityContainer<Platform> GetPlatforms();
        EntityContainer<Obstacle> GetObstacles();
        Portal GetPortal();
        Vec2F GetTaxiPosition();


        string GetName();
    }
}

