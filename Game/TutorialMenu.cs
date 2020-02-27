using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class TutorialMenu : GameObject {
        private int tutorialIndex;
        private Texture2D[] tutorialTextures = new Texture2D[6];
        private GameManager manager;
        public TutorialMenu(GameManager manager) {
            this.manager = manager;
            for (int i = 0; i < tutorialTextures.Length; i++) {
                tutorialTextures[i] = Texture2D.GetInstance($"data/tutorial/tutorial{i+1}.png");
            }
        }

        private void Update() {
            if (Input.GetAxisDown("Horizontal") != 0 || Input.GetAxisDown("Vertical") != 0 || Input.GetButtonDown("Drill") || Input.GetButtonDown("Refuel")) {
                tutorialIndex++;
                if (tutorialIndex >= tutorialTextures.Length) manager.ShouldStartPlaying = true;
            }
        }

        protected override void RenderSelf(GLContext glContext) {
            if (tutorialIndex >= tutorialTextures.Length) return;
            tutorialTextures[tutorialIndex].Bind();
            glContext.DrawQuad(tutorialTextures[tutorialIndex].TextureVertices(), Globals.QUAD_UV);
        }
    }
}