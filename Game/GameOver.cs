using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Game {
    public class GameOver : Canvas {
        private int score;
        private float fuel1;
        private float fuel2;

        public GameOver(int score, float fuel1, float fuel2) : base(Globals.WIDTH, Globals.HEIGHT) {
            this.score = score;
            this.fuel1 = fuel1;
            this.fuel2 = fuel2;
        }

        void Update() {

            //Gives total score
            graphics.DrawString("Score\n" + score + "\nFuel left\n" + (int)(fuel1 / 1000 + fuel2 / 1000) +"L"+ "\nTotalScore:\n" + (int)(score + (fuel1 / 100) + (fuel2 / 100)), 
                FontLoader.Instance[64f], Brushes.White, Globals.WIDTH/2f, Globals.HEIGHT / 2f, FontLoader.CenterAlignment);
        }
    }
}