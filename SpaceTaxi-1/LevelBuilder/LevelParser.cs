using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using OpenTK.Graphics.OpenGL;
using SpaceTaxi_1.SpaceTaxiEntities;


namespace SpaceTaxi_1.LevelBuilder {
    /// <summary>
    /// A Class which parse the input from the Loader class to construct all level entities.
    /// </summary>
    public class LevelParser : IParser {

        private int width, height;
        private List<string> level;
        
        private List<string> map;
        private List<char> platforms;
        
        // for customers
        public List<Vec2F> StartingPositions { get; }
        
        public List<string> locations{ get; }
        public List<string> destinations{ get; }
        public List<string> names{ get; }
        
        public List<int> spawntimes { get; }
        public List<int> points { get; }
        public List<int> timeLimits { get; }
        
        
        
        private Dictionary<char, Image> images;
        private string name;
        
        private float xscale;
        private float yscale;
        
        
        public LevelParser(ILevelFetch lvl) {
            
            
            // for customers
            StartingPositions = new List<Vec2F>();
            locations = new List<string>();
            destinations = new List<string>();
            names = new List<string>();
            
            spawntimes = new List<int>();
            points = new List<int>();
            timeLimits = new List<int>();


            level = lvl.GetLevel();
            map = new List<string>();
            platforms = new List<char>();
            images = new Dictionary<char, Image>();
            name = "";
            GetErrorMessage = "";
            
            
            // check for invalid levelsfiles and if the level exists.
            if (level[0] != "Level not found " && level[0] != "Not a level ") {
                MakeMap();
                MakeName();
                MakePlatforms();
                MakeImages();
                MakeCostumers();
                MakestartingPositions();
                
            } else {
                // set error message to handle error checking later.
                GetErrorMessage += level[0];
            }

            if (GetErrorMessage == "") {
                xscale = 1.0f / width;
                yscale = 1.0f / height;
            }
        }

        private void MakeMap() {
            while (!level[0].Contains("Name")) {
                // adds the current map row to the map.
                map.Add(level[0]);
                // delete it to ease data collection.
                level.RemoveAt(0);
            }
            
            // sets width and height
            width = map[0].Length;
            height = map.Count;
            
            int i = 0;
            // checks for equal row length of every row.
            while (i < height && map[i].Length == width) {
                i++;
            }

            if (i != map.Count) {
                // error message sendt through name variable
                GetErrorMessage += "Not a valid map ";
            }
        }

        private void MakeName() {
            // set name or send an error message through name variable.
            if (level[0].Contains("Name")) {
                name = level[0].Substring(6);
            } else {
                GetErrorMessage +=  "Name missing ";
            }
            // remove first element of the list.
            level.RemoveAt(0);
        }
        
        private void MakePlatforms() {
            if (level[0].Contains("Platforms:")) {
                string p = level[0].Substring(10);
                foreach (var c in p) {
                    if (c != ' ' && c != ',')
                    {
                        platforms.Add(c);
                    }
                }
                // remove first element of the list.
                level.RemoveAt(0);
            } else {
                GetErrorMessage += "No platforms found ";
            }
            
        }
        
        private void MakeImages() {
            while (!(level[0].Contains("Customer:"))) {
                var image = new Image(Path.Combine("Assets", "Images", level[0].Substring(3)));
                var key = level[0][0];
                
                images.Add(key, image);
                // remove first element of the list.
                level.RemoveAt(0);
            }
        }
        
        
        public void MakestartingPositions() {
            for (int y = 0; y <height; y++) {
                for (int x = 0; x < width; x++) {
                    var key = map[y][x];
                    if (locations.Contains(key.ToString())){
                        
                        float x1 = x / ((float) width),
                            y1 = 1.0f - y / (float)height;
                        var acc = 1;
                        while (map[y][x + acc] == key) {
                            acc++;
                        }
                                                    
                        StartingPositions.Add(
                            new Vec2F((2*x + acc)/(float)width/2.0f, y1));
                        
                        // jump to the index after the platform.
                        x += acc + 1;
                    }
                }
            }
        }

        private void MakeCostumers() {
           while(level.Count > 0) {
               var data = level[0].Substring(10);
               var customerinfo = data.Split(char.Parse(" "));
               
               names.Add(customerinfo[0]);
               spawntimes.Add(Int32.Parse(customerinfo[1]));
               locations.Add(customerinfo[2]);
               destinations.Add(customerinfo[3]);
               timeLimits.Add(Int32.Parse(customerinfo[4]));
               points.Add(Int32.Parse(customerinfo[5]));
               
               level.RemoveAt(0);
           }
        }
        
        /// <summary>
        /// get the error messages if any exists
        /// </summary>
        public string GetErrorMessage { get; private set; }

        /// <summary>
        /// Get level name
        /// </summary>
        public string GetName() {
            return name;
        } 
        
        /// <summary>
        /// A method which return all objects of the level
        /// </summary>
        /// <returns> A list of obstacles on the levelmap </returns>
        public EntityContainer<Obstacle> GetObstacles() {
            var obstacles = new EntityContainer<Obstacle>();
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {

                    if (map[y][x] != ' ' && map[y][x] != '^' && map[y][x] != '<' &&
                        map[y][x] != '>' && !platforms.Contains(map[y][x])) {
                        float x1 = x / ((float) width),
                            y1 = 1.0f - (1.0f + y) / ((float) height);

                        var key = map[y][x];
                        obstacles.AddStationaryEntity(
                            new Obstacle(
                                new StationaryShape(x1, y1, xscale, yscale),
                                images[key]));
                    }
                }
            }

            return obstacles;
        }

        

        public EntityContainer<Platform> GetPlatforms() {
            var plist = new EntityContainer<Platform>();
            for (int y = 0; y <height; y++) {
                for (int x = 0; x < width; x++) {
                    var key = map[y][x];
                    if (platforms.Contains(key)){
                        
                        float x1 = x / ((float) width),
                            y1 = 1.0f - (1.0f + y) / height;
                        var acc = 1;
                        while (map[y][x + acc] == key) {
                            acc++;
                        }
                        plist.AddStationaryEntity(
                            new Platform(
                                // "y1 - yscale" because we wish to lay the platform entity just
                                // above where it is to use collisionstrategy to check for landing.
                                new StationaryShape(x1, y1, acc * xscale, yscale),
                                images[key], key));
                        
                      
                        
                        // jump to the index after the platform.
                        x += acc + 1;
                    }
                }
            }

            return plist;
        }
        
        public Portal GetPortal() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (map[y][x] == '^'){
                        
                        float x1 = x / ((float) width),
                            y1 = 1.0f - (1.0f + y) / height;
                        var acc = 1;
                        while (map[y][x + acc] == '^') {
                            acc++;
                        }

                        return
                            new Portal(
                                // "y1 - yscale" because we wish to lay the platform entity just
                                // above where it is to use collisionstrategy to check for landing.
                                new StationaryShape(x1, y1, xscale + (acc - 1) * xscale, yscale),
                                new Image(
                                    Path.Combine("Assets", "Images", "aspargus-edge-bottom.png")));
                    }
                }
            }

            return new Portal(
               new StationaryShape(new Vec2F(), new Vec2F()),
                new Image(
                    Path.Combine("Assets", "Images", "aspargus-edge-bottom.png")));;
        }


        

        public Vec2F GetTaxiPosition() {
            var pos = new Vec2F();
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (map[y][x] == '>' || map[y][x] == '<') {
                        pos = new Vec2F((float) x/width, 1.0f - (1.0f + y)/height);
                    } 
                }
            }

            return pos;
        }

        public Orientation GetTaxiOrientation() {
            var orio = Orientation.None;
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    switch (map[y][x]) {
                        case '>':
                            orio = Orientation.Right;
                            break;
                        
                        case '<':
                            orio = Orientation.Left;
                            break;
                    }
                }
            }

            return orio;
        }
        

    }
}