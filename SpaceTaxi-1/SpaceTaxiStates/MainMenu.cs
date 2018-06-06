using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;


namespace SpaceTaxi_1.SpaceTaxiStates {
    public class MainMenu : IGameState {
        private static MainMenu instance;

        private Color yellow = Color.Yellow;
        private Color green = Color.YellowGreen;

        private Entity backGroundImage;

        private Text title;
        private Text[] menuButtons;
        private int activeMenuButtons;
        private int maxMenuButtons;

        // Colors
        private Color darkGreen = Color.DarkGreen;
        private Color darkRed = Color.DarkRed;

        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }

        // private initialization
        private MainMenu() {
            InitializeGameState();

        }


        public void GameLoop() {
            RenderState();
        }

        public void InitializeGameState() {
            
            maxMenuButtons = 1;
            backGroundImage = 
                new Entity(
                    new StationaryShape(
                        new Vec2F(0f,0f),
                        new Vec2F(1f, 1f)),
                    new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            
            title = new Text("Space Taxi", new Vec2F(0.0f,0.0f), new Vec2F(1f,1f));
            title.SetColor(yellow);
            title.SetFontSize(16);

            activeMenuButtons = 0;
            
            menuButtons = new[] {
                new Text("New Game", new Vec2F(0.35f, 0.2f), new Vec2F(0.5f, 0.4f)),
                new Text("Quit", new Vec2F(0.35f, 0.125f), new Vec2F(0.5f, 0.4f)),
            };
            
            foreach (Text button in menuButtons) {
                button.SetColor(yellow);
            }
            menuButtons[0].SetColor(green);
        }

        public void UpdateGameLogic() {
            throw new System.NotImplementedException();
        }

        public void RenderState() {

           backGroundImage.RenderEntity();
            title.RenderText();
            foreach (Text button in menuButtons) {
                button.SetColor(yellow);
            }

            menuButtons[activeMenuButtons].SetColor(green);
            foreach (Text button in menuButtons) {
                button.RenderText();
            }


        }

        public void HandleKeyEvent(string keyValue, string keyAction) {

            if (keyAction == "KEY_RELEASE") {
                switch (keyValue) {
                case "KEY_UP":
                    menuButtons[activeMenuButtons].SetColor(darkRed);
                    if (activeMenuButtons == 0) {
                        activeMenuButtons = maxMenuButtons;
                    } else {
                        activeMenuButtons--;
                    }

                    menuButtons[activeMenuButtons].SetColor(darkGreen);
                    break;

                case "KEY_DOWN":
                    menuButtons[activeMenuButtons].SetColor(darkRed);
                    if (activeMenuButtons == maxMenuButtons) {
                        activeMenuButtons = 0;
                    } else {
                        activeMenuButtons++;
                    }

                    menuButtons[activeMenuButtons].SetColor(darkGreen);
                    break;

                case "KEY_ENTER":
                    switch (activeMenuButtons) {
                    case 0:
                        SpaceTaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "NEW_GAME", "", "")
                        );
                        break;

                    case 1:
                        SpaceTaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "QUIT", "", "")
                        );
                        break;
                    }

                    break;
                }

                switch (keyAction) {
                case "KEY_PRESS":
                    switch (keyValue) {
                    case "KEY_UP":
                        activeMenuButtons--;
                        if (activeMenuButtons < 0) {
                            activeMenuButtons = maxMenuButtons;
                        }

                        break;

                    case "KEY_DOWN":
                        activeMenuButtons++;
                        activeMenuButtons %= 2;
                        break;

                    }

                    break;

                }
            }
        }
    }
}
