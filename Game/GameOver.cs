using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GXPEngine.Core;

namespace Game {
    public class GameOver : GameObject {
        private int score;
        private float fuel1;
        private float fuel2;
        private Texture2D backgroundTexture;
        private Canvas canvas;

        public GameOver(int score, float fuel1, float fuel2) {
            canvas = new Canvas(Globals.WIDTH, Globals.HEIGHT);
            this.score = score;
            this.fuel1 = fuel1;
            this.fuel2 = fuel2;
            backgroundTexture = Texture2D.GetInstance("data/loseGameOver.png");
            AddChild(canvas);
        }

        private void Update() {
            canvas.graphics.Clear(Color.Transparent);
            canvas.graphics.DrawString("Score\n" + score + "\nFuel left\n" + (int)(fuel1 / 1000 + fuel2 / 1000) +"L"+ "\nTotalScore:\n" + (int)(score + (fuel1 / 100) + (fuel2 / 100)), 
                FontLoader.Instance[64f], Brushes.White, Globals.WIDTH/2f, Globals.HEIGHT / 2f, FontLoader.CenterAlignment);
        }

        protected override void RenderSelf(GLContext glContext) {
            glContext.SetColor(0xff,0xff,0xff,0xff);
            backgroundTexture.Bind();
            var verts = backgroundTexture.TextureVertices();
            glContext.DrawQuad(verts, Globals.QUAD_UV);
        }
    }
}