using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using GXPEngine.Core;
using Newtonsoft.Json;

namespace Game {
    internal class LeaderboardScores {
        public List<int> Scores;
    }
    public class Leaderboard : GameObject {
        private Texture2D backgroundTexture;
        private Canvas canvas;
        private List<int> scores;

        public Leaderboard(int score) {
            var leaderboardText = File.ReadAllText("data/leaderboard.json");
            var leaderboard = JsonConvert.DeserializeObject<LeaderboardScores>(leaderboardText);
            leaderboard.Scores.Add(score);
            leaderboard.Scores.Sort();
            leaderboard.Scores.Reverse();
            leaderboard.Scores.RemoveRange(5, leaderboard.Scores.Count-5);
            scores = new List<int>();
            scores.AddRange(leaderboard.Scores);
            var newLeaderboardText = JsonConvert.SerializeObject(leaderboard);
            Debug.Log(newLeaderboardText);
            File.WriteAllText("data/leaderboard.json", newLeaderboardText);
            
            canvas = new Canvas(Globals.WIDTH, Globals.HEIGHT);
            backgroundTexture = Texture2D.GetInstance("data/leaderboard.png");
            AddChild(canvas);
        }

        private void Update() {
            canvas.graphics.Clear(Color.Transparent);
            canvas.graphics.DrawString($"LEADERBOARD", FontLoader.Instance[42f], Brushes.White, Globals.WIDTH/2f,320f, FontLoader.CenterAlignment);
            foreach (var (score, index) in scores.Select((score, index) => (score, index))) {
                canvas.graphics.DrawString($"{index+1}. {score}", FontLoader.Instance[28f], Brushes.White, Globals.WIDTH/2f, 320+62+index*(413-362), FontLoader.CenterAlignment);
            }
            canvas.graphics.DrawString($"press any button", FontLoader.Instance[48f], Brushes.White, Globals.WIDTH/2f,Globals.HEIGHT - 48f, FontLoader.CenterAlignment);
            if (Input.GetAxisDown("Horizontal") != 0 || Input.GetAxisDown("Vertical") != 0 || Input.GetButtonDown("Drill") || Input.GetButtonDown("Refuel")) {
                GameManager.Instance.ShouldShowMenu = true;
            }
        }

        protected override void RenderSelf(GLContext glContext) {
            glContext.SetColor(0xff,0xff,0xff,0xff);
            backgroundTexture.Bind();
            var verts = backgroundTexture.TextureVertices();
            glContext.DrawQuad(verts, Globals.QUAD_UV);
        }
    }
}