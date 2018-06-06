using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using SpaceTaxi_1.Collision;
using SpaceTaxi_1.LevelBuilder;
using SpaceTaxi_1.Movement;

namespace SpaceTaxi_1.SpaceTaxiEntities {
    public class Customers : ICollision {

        private static Customers customers;
        
        // use the same entity to all customers
        private DynamicShape customer;
        
        private ImageStride movesLeft = 
            new ImageStride(
                60,
                ImageStride.CreateStrides(
                    2,
                    Path.Combine("Assets","Images","CustomerWalkLeft.png")
                    )
                );
        
        private ImageStride movesRight = 
            new ImageStride(
                60,
                ImageStride.CreateStrides(
                    2,
                    Path.Combine("Assets","Images","CustomerWalkRight.png")
                )
            );
        
        private Image customerImage =
            new Image(
                Path.Combine("Assets","Images","CustomerStandLeft.png"));

        private IMovement movement;
        
        private List<Vec2F> StartingPositions;
        private List<string> locations;
        private List<string> destinations;
        private List<string> names;
        
        private List<int> spawntimes;
        private List<int> points;
        private List<int> timeLimits;

        private bool isSpawned;
        

        private Customers() {
            
            // start position of the different customers
            StartingPositions = new List<Vec2F>();
            destinations = new List<string>();
            names = new List<string>();
            
            spawntimes = new List<int>();
            points = new List<int>();
            timeLimits = new List<int>();
            customer = new DynamicShape(new Vec2F(), new Vec2F(0.04f, 0.04f));
            
            movement = new CustomerMoveLeft(customer);
            
        }

        private void SetNewLevelCustomers(IParser parser) {
            
        }
        
        
        private void SetNewCustomer() {
            // remove old startposition
            locations.RemoveAt(0);
            StartingPositions.RemoveAt(0);
        }
        
        
        public static Customers GetInstance() {
            return Customers.customers ?? (Customers.customers = new Customers());
        }

        public int CustomerCount => locations.Count;

        /// <summary>
        /// Set the list of StartingPositions hwere each customer should start.
        /// </summary>
        /// <param name="loc"></param>
        public void SetLocations(List<string> loc) {
            locations = loc;
        }

        public void SetStartPositions(List<Vec2F> pos) {
            StartingPositions = pos;
            customer.Position = StartingPositions[0];
        }
        
        /// <summary>
        /// Set the list of destination platforms ID.
        /// </summary>
        /// <param name="des"></param>
        public void SetDestinations(List<string> des) {
            destinations = des;
        }
        
        /// <summary>
        /// Set Spawn Times for each customer.
        /// </summary>
        /// <param name="stimes"></param>
        public void SetSpawntimer(List<int> stimes) {
            spawntimes = stimes;
        }
        
        /// <summary>
        /// Set Points for each customer
        /// </summary>
        /// <param name="p"></param>
        public void SetPoints(List<int> p) {
            points = p;
        }
        
        /// <summary>
        /// Set time limits for each service of a customer.
        /// </summary>
        /// <param name="limits"></param>
        public void SetTimeLimits(List<int> limits) {
            timeLimits = limits;
        }

        
        /// <summary>
        /// Move the customer if any is active at the moment or else do nothing if not.
        /// </summary>
        public void MoveCustumoer() {
            if (isSpawned) {
                customer.Move(); 
            }          
        }
        
        /// <summary>
        /// Render the customer if active or else do nothing.
        /// </summary>
        public void RenderCustomer() {
            /* TODO remove when spawntime works
            if (isSpawned) {
                if (customer.Direction.X > 0) {
                    movesRight.Render(customer);
                } 
                else {
                    movesLeft.Render(customer);   
                }
            }*/
            customerImage.Render(customer);
        }

        
        public bool CollidWith() {
            var player = Player.GetInstance();
            if (locations.Count > 0 && player.Location == locations[0]) {
                
                // destionation and location should be the same length.
                player.SetDestination(destinations[0]);
                destinations.RemoveAt(0);
            }
            
            
            
            return isSpawned = false;
        }
    }
}