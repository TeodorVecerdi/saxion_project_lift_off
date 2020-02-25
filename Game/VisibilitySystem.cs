using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class VisibilitySystem : GameObject {
        private Texture2D visibilityTexture = Texture2D.GetInstance("data/visibility.png");
        private Transformable playerReference;

        public VisibilitySystem(Transformable player) {
            playerReference = player;
        }

        private void Update() {
            SetXY(playerReference.x, playerReference.y);
            if(Input.GetKey(Key.ONE)) SetScaleXY(1f);
            else if(Input.GetKey(Key.TWO)) SetScaleXY(1.5f);
            else if(Input.GetKey(Key.THREE)) SetScaleXY(2f);
            else if(Input.GetKey(Key.FOUR)) SetScaleXY(2.5f);
            else if (Input.GetKey(Key.FIVE)) SetScaleXY(10f);
            //else SetScaleXY(2f);
        }

        public void Draw(GLContext glContext) {
            float width = visibilityTexture.width * scaleX;
            float height = visibilityTexture.height * scaleY;
            float offsetX = Globals.TILE_SIZE/2f;
            float offsetY = Globals.TILE_SIZE/2f;
            float[] verts = {
                x - width / 2f + offsetX, y - height / 2f + offsetY,
                x + width / 2f + offsetX, y - height / 2f + offsetY,
                x + width / 2f + offsetX, y + height / 2f + offsetY,
                x - width / 2f + offsetX, y + height / 2f + offsetY
            };
            visibilityTexture.Bind();
            glContext.DrawQuad(verts, Globals.QUAD_UV);
        }
    }
}