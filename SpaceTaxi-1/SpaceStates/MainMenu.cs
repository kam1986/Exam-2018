using DIKUArcade;
using DIKUArcade.State;
using System;
using System.Linq.Expressions;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;


namespace SpaceTaxi_1.SpaceStates {
    /// <summary>
    /// Renders the main menu and menu options.
    /// </summary>
    public class MainMenu : IGameState {
        private static MainMenu instance;

        private System.Drawing.Color yellow = System.Drawing.Color.Yellow;
        private System.Drawing.Color green = System.Drawing.Color.YellowGreen;

        private Entity _backGroundImage;

        private Text title;
        private Text[] menuButtons;
        private int activeMenuButtons;
        private int maxMenuButtons;

        // Colors
        private System.Drawing.Color darkGreen = System.Drawing.Color.DarkGreen;
        private System.Drawing.Color darkRed = System.Drawing.Color.DarkRed;

        // private initialization
        private MainMenu() {
            InitializeGameState();

        }

        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }

        public void GameLoop() {
            RenderState();
        }

        public void InitializeGameState() {

            maxMenuButtons = 1;
            _backGroundImage =
                new Entity(
                    new StationaryShape(
                        new Vec2F(0f, 0f),
                        new Vec2F(1f, 1f)),
                    new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));

            title = new Text("Space Taxi", new Vec2F(0.0f, 0.0f), new Vec2F(1f, 1f));
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
            // nothing to do.
        }

        public void RenderState() {
            _backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );

            menuButtons = new Text[2] {
                new Text("New Game", new Vec2F(05.0f, 05.0f), new Vec2F(0.3f, 0.3f)),
                new Text("Quit", new Vec2F(0.6f, 0.6f), new Vec2F(0.3f, 0.3f))
            };
            _backGroundImage.RenderEntity();
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

                case "KEY_P":
                    StateBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_PAUSE", "")
                    );
                    break;

                case "ENTER":
                    switch (activeMenuButtons) {
                    case 0:
                        StateBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_RUNNING",
                                "")
                        );
                        break;

                    case 1:
                        StateBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_QUIT", "")
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

                    case "KEY_ENTER":
                        switch (activeMenuButtons) {
                        case 0:
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent, this,
                                    "CHANGE_STATE",
                                    "GAME_RUNNING", ""));
                            break;

                        case 1:
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.WindowEvent, this,
                                    "CLOSE_WINDOW" +
                                    "",
                                    "", ""));
                            break;
                        }

                        break;
                    }

                    break;
                }
            }
        }
    }
}