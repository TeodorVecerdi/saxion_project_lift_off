using System;
using System.Collections.Generic;
using GXPEngine;
using GXPEngine.OpenGL;

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
            Input.AddButton("Refuel", Key.X, true);
            Input.AddAxis("Horizontal", new List<int> {Key.A, Key.LEFT}, new List<int> {Key.D, Key.RIGHT});
            Input.AddAxis("Vertical", new List<int> {Key.W, Key.UP}, new List<int> {Key.S, Key.DOWN});
            
            var world = new World();
            AddChild(world);
        }

        public static void Main(string[] args) {
            new Program().Start();
        }
    }
}