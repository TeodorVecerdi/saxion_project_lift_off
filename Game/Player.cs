using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public enum AnimationState {
        Drilling = 0,
        DrillOn = 2,
        Idle = 1
    }

    public class Player : GameObject {
        private Texture2D playerTexture;
        private int animationFrame;
        private const float animationSpeed = 0.01666667F * 6F; // 10 FPS
        private float animationTimeLeft = animationSpeed;
        private AnimationState animationState = AnimationState.Idle;
        private readonly Vector2 uvSize = new Vector2(0.2F, 0.3333333F);

        public Player() {
            playerTexture = Texture2D.GetInstance("data/player.png");
        }

        public AnimationState AnimationState {
            get => animationState;
            set {
                if (animationState != value) {
                    animationFrame = 0;
                }
                animationState = value;
            }
        }

        private void Update() {
            animationTimeLeft -= Time.deltaTime;
            if (animationTimeLeft <= 0) {
                animationTimeLeft += animationSpeed;
                animationFrame += 1;
                animationFrame %= 5;
            }
        }

        public void Draw(GLContext glContext) {
            var animationStateValue = (int) animationState;
            float[] uv = {
                animationFrame * uvSize.x, animationStateValue * uvSize.y,
                animationFrame * uvSize.x + uvSize.x, animationStateValue * uvSize.y,
                animationFrame * uvSize.x + uvSize.x, animationStateValue * uvSize.y + uvSize.y,
                animationFrame * uvSize.x, animationStateValue * uvSize.y + uvSize.y
            };
            float[] verts = {x, y, x + Globals.TILE_SIZE, y, x + Globals.TILE_SIZE, y + Globals.TILE_SIZE, x, y + Globals.TILE_SIZE};
            playerTexture.Bind();
            glContext.DrawQuad(verts, uv);
        }
    }
}