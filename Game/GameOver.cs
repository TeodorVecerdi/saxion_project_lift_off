using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Game
{
   
    public class GameOver : Canvas
    {
        public bool gameOver = false;
        private GameLoop gameLoop;
        
        public GameOver(GameLoop gameLoop) :base(Globals.WIDTH,Globals.HEIGHT)
        {
            this.gameLoop = gameLoop;
           
        }
        void Update()
        {
            if (gameOver == true)
            {
                graphics.Clear(Color.Blue);
                graphics.DrawString("Score:" + gameLoop.Score, FontLoader.Instance[128f], Brushes.White, 0, Globals.HEIGHT/2);

            }

        }

    }
}
