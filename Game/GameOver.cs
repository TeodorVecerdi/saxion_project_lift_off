using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Game {
    public class GameOver : Canvas {
        private int score;

        public GameOver(int score) : base(Globals.WIDTH, Globals.HEIGHT) {
            this.score = score;
        }

        void Update() {
            graphics.Clear(Color.Blue);
            graphics.DrawString("Score:" + score, FontLoader.Instance[128f], Brushes.White, 0, Globals.HEIGHT / 2f);
        }
    }
}