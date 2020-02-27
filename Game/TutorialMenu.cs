using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class TutorialMenu : GameObject {
        private int tutorialIndex;
        private Texture2D[] tutorialTextures = new Texture2D[6];
        public TutorialMenu() {
            for (int i = 0; i < tutorialTextures.Length; i++) {
                tutorialTextures[i] = Texture2D.GetInstance($"data/tutorial/tutorial{i+1}.png");
            }
        }

        private void Update() {
            if (Input.GetButtonDown("Drill")) {
                GameManager.Instance.ShouldStartPlaying = true;
                SoundManager.Instance.Play("GameMusic");
            }
            if (Input.GetAxisDown("Horizontal") != 0 || Input.GetAxisDown("Vertical") != 0 || Input.GetButtonDown("Refuel")) {
                tutorialIndex++;
                if (tutorialIndex >= tutorialTextures.Length) GameManager.Instance.ShouldStartPlaying = true;
            }
        }

        protected override void RenderSelf(GLContext glContext) {
            if (tutorialIndex >= tutorialTextures.Length) return;
            tutorialTextures[tutorialIndex].Bind();
            glContext.DrawQuad(tutorialTextures[tutorialIndex].TextureVertices(), Globals.QUAD_UV);
        }
    }
}