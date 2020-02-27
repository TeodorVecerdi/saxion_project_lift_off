using System;
using System.Collections.Generic;
using System.Linq;
using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class StartMenu : GameObject {
        private List<Button> buttons;
        private Texture2D startMenuBackgroundTexture;
        private int selectedButton = 0;

        public StartMenu() {
            buttons = new List<Button> {
                new Button(new Vector2(Globals.WIDTH / 2f, 100), "data/start_menu/playButton.png", "data/start_menu/playButtonSelected.png", () => {
                    Debug.Log("Play Button Clicked");
                    GameManager.Instance.ShouldShowTutorial = true;
                }),
                new Button(new Vector2(Globals.WIDTH / 2f, 300), "data/start_menu/quitButton.png", "data/start_menu/quitButtonSelected.png", () => {
                    Debug.Log("Quit Button Clicked");
                    Environment.Exit(0);
                }),
                new Button(new Vector2(Globals.WIDTH / 2f, 500), "data/start_menu/creditsButton.png", "data/start_menu/creditsButtonSelected.png", () => Debug.Log("Credits Button Clicked"))
            };
            startMenuBackgroundTexture = Texture2D.GetInstance("data/start_menu/startMenuBackground.png");
        }

        private void Update() {
            int choice = Input.GetKeyDown(Key.UP) ? -1 : Input.GetKeyDown(Key.DOWN) ? 1 : 0;
            selectedButton = (selectedButton + choice) % buttons.Count;
            if (selectedButton < 0) selectedButton += buttons.Count;
            buttons.ForEach(button => button.IsSelected = false);
            buttons[selectedButton].IsSelected = true;
            if (Input.GetButtonDown("Drill")) buttons[selectedButton].Click();
        }

        protected override void RenderSelf(GLContext glContext) {
            glContext.SetColor(0xff,0xff,0xff,0xff);
            startMenuBackgroundTexture.Bind();
            glContext.DrawQuad(startMenuBackgroundTexture.TextureVertices(), Globals.QUAD_UV);
            buttons.ForEach(button => button.Draw(glContext));
        }
    }
}