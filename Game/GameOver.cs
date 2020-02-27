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
            graphics.DrawString("Score\n" + score + "\nFuel left\n" + (Settings.Instance.FirstFuelStationFuel / 10 + Settings.Instance.SecondFuelStationFuel / 10) +
                "\nTotalScore:\n" + (score + Settings.Instance.FirstFuelStationFuel / 10 + Settings.Instance.SecondFuelStationFuel / 10), 
                FontLoader.Instance[64f], Brushes.White, Globals.WIDTH/2f, Globals.HEIGHT / 2f, FontLoader.CenterAlignment);
        }
    }
}