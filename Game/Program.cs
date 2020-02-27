using System.Collections.Generic;
using GXPEngine;

namespace Game {
    public class Program : GXPEngine.Game {
        public Program() : base(Globals.WIDTH, Globals.HEIGHT, Globals.FULLSCREEN, Globals.VSYNC,
            pPixelArt: Globals.PIXEL_ART) {
            ShowMouse(true);
            targetFps = 1000;
            Rand.PushState();
            Rand.Seed = Time.now;
            
            //Setup Input
            Input.AddButton("Drill", Key.SPACE,true);
            Input.AddButton("Refuel", Key.M, true);
            Input.AddAxis("Horizontal", new List<int> {Key.A, Key.LEFT}, new List<int> {Key.D, Key.RIGHT});
            Input.AddAxis("Vertical", new List<int> {Key.W, Key.UP}, new List<int> {Key.S, Key.DOWN});
            
            var gameManager = new GameManager();
            GameManager.Instance = gameManager;
            AddChild(gameManager);
            /*
            var lines = File.ReadAllLines("data/highscore.txt");
            for (int i = 0; i < lines.Length; i++) {
                var line = lines[i];
                Debug.Log(line);
                var splitLine = line.Split(':');
                string name = splitLine[0];
                int score = int.Parse(splitLine[1]);
                Debug.Log("Name:"+name + " score:"+score);
            }
            */
        }

        public static void Main(string[] args) {
            new Program().Start();
        }
    }
}