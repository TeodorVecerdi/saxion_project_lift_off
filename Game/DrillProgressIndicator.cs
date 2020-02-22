using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class DrillProgressIndicator : GameObject {
        private Texture2D drillProgressIndicatorTexture;
        private float alpha;
        public float Alpha {
            get => alpha;
            set => alpha = Mathf.Clamp01(value);
        }

        public DrillProgressIndicator() {
            drillProgressIndicatorTexture = Texture2D.GetInstance("data/drillIndicator2.png", true);
        }

        public void Draw(GLContext glContext) {
            float[] verts = {x, y, x+Globals.TILE_SIZE, y, x+Globals.TILE_SIZE, y+Globals.TILE_SIZE, x, y+Globals.TILE_SIZE};
            drillProgressIndicatorTexture.Bind();
            glContext.SetColor(0xff,0xff,0xff,(byte) (alpha * 0xff));
            glContext.DrawQuad(verts, Globals.QUAD_UV);
            glContext.SetColor(0xff,0xff,0xff,0xff);
        }
    }
}