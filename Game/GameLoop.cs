using System.Drawing;
using System.Linq;
using Game.WorldGen;
using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class GameLoop : GameObject {
        public int TilesVertical => Settings.World.Depth;
        public readonly int TilesHorizontal;
        public int Score;

        private float gravityTimeLeft = Settings.GravityFrequency;
        private float timeSinceLastMovement;
        private float drillTimeLeft;
        private float drillTimeOriginal;
        private float cameraVelocity;

        private bool startedDrilling;
        private bool canStartDrilling;
        

        private ObjectType[,] tiles;
        private ObjectType[,] tilesBackground;
        private Texture2D topBackground;
        private Canvas HUD;
        private Camera camera;
        private Transformable player;
        private FuelBar fuelBar;
        private FuelStation fuelStation;
        private VisibilitySystem visibility;
        private DrillProgressIndicator drillProgressIndicator;
        private Vector2Int lastDrillDirection = Vector2Int.zero;
        private GameManager gameManager;

        public ObjectType[,] Tiles => tiles;

        public GameLoop(GameManager gameManager) {
           this.gameManager = gameManager;
            TilesHorizontal = (int) (Globals.WIDTH / Globals.TILE_SIZE);
            tiles = new ObjectType[TilesHorizontal, Settings.World.TopOffset + Settings.World.Depth];
            tilesBackground = new ObjectType[TilesHorizontal, Settings.World.TopOffset + Settings.World.Depth];
            GenerateWorldBracketed(out var playerSpawnLocation);
            InitializeSceneObjects(playerSpawnLocation);
            
        }

        private void Update() {
            DrawHud();
            
            var (playerX, playerY) = new Vector2(player.x, player.y).ToGrid().ToInt().Unpack();
            var movementDirection = new Vector2Int((int) Input.GetAxisDown("Horizontal"), (int) Input.GetAxisDown("Vertical"));
            var drillDirection = new Vector2Int((int) Input.GetAxis("Horizontal"), (int) Input.GetAxis("Vertical"));

            // Constrain movement to only one axis, with priority for vertical movement
            if (movementDirection.x != 0 && movementDirection.y != 0) {
                movementDirection.x = 0;
            }

            if (drillDirection.x != 0 && drillDirection.y != 0) {
                drillDirection.x = 0;
            }

            var desiredPosition = movementDirection.Add(playerX, playerY);
            var desiredDrillDirection = drillDirection.Add(playerX, playerY);
            var rangeCheck = desiredPosition.x >= 0 && desiredPosition.x < TilesHorizontal && desiredPosition.y >= 0 && desiredPosition.y < TilesVertical;
            var movedThisFrame = false;

            UpdateDrilling(ref playerX, ref playerY, ref rangeCheck, ref movedThisFrame, ref movementDirection, ref desiredPosition, ref drillDirection, ref desiredDrillDirection);
            UpdateMovement(ref playerX, ref playerY, ref rangeCheck, ref movedThisFrame, ref movementDirection, ref desiredPosition);
            UpdateGravity(ref playerX, ref playerY, ref rangeCheck, ref movementDirection, ref desiredPosition);
            UpdateFuel(ref playerX, ref playerY);
            UpdateTimers();

            // Update Camera
            camera.y = Mathf.SmoothDamp(camera.y, player.y, ref cameraVelocity, 0.3f);
        }

        private void DrawHud() {
            HUD.graphics.Clear(Color.Empty);
            HUD.graphics.DrawString("SCORE: " + Score, FontLoader.Instance[64f], Brushes.FloralWhite, Globals.WIDTH / 2f, 24, FontLoader.CenterAlignment);
            HUD.graphics.DrawString($"DEPTH: {Settings.World.BlockSize * (player.y / Globals.TILE_SIZE - Settings.World.TopOffset + 1)}m", FontLoader.Instance[32f], Brushes.AntiqueWhite, Globals.WIDTH / 2f, 64, FontLoader.CenterAlignment);
            HUD.graphics.DrawString("FUEL", FontLoader.Instance[64f], Brushes.FloralWhite, Globals.WIDTH - 30, Globals.HEIGHT / 2f, FontLoader.CenterVerticalAlignment);
            HUD.graphics.DrawString("FPS: " + game.currentFps, SystemFonts.StatusFont, Brushes.DarkRed, 0, 8, FontLoader.LeftAlignment);
        }

        private void InitializeSceneObjects(int playerSpawnLocation) {
            drillProgressIndicator = new DrillProgressIndicator {Alpha = 0};
            fuelStation = new FuelStation("data/fuel_station.png", 3, Settings.World.TopOffset - 1, Settings.InitialFuelRefills);
            fuelStation.Move(0, 2 * Globals.TILE_SIZE);

            player = new Transformable();
            player.SetXY(playerSpawnLocation * Globals.TILE_SIZE, (Settings.World.TopOffset - 1) * Globals.TILE_SIZE);

            camera = new Camera(0, 0, Globals.WIDTH, Globals.HEIGHT) {x = (int) (Globals.WIDTH / 2f)}; // weird camera behaviour fix

            fuelBar = new FuelBar();
            fuelBar.Move(-Globals.WIDTH / 2f, 0f); // weird camera behaviour fix
            HUD = new Canvas(Globals.WIDTH, Globals.HEIGHT, false);
            HUD.Move(0, -Globals.HEIGHT / 2f);
            HUD.Move(-Globals.WIDTH / 2f, 0f); // weird camera behaviour fix

            visibility = new VisibilitySystem(player);
            topBackground = Texture2D.GetInstance("data/background_test.jpg", true);
            
            /*topBackground = new Sprite("data/background_test.jpg", true, false);
            topBackground.Move(0, -2*Globals.TILE_SIZE);
            topBackground.SetScaleXY(0.711458333f);*/

            camera.AddChild(fuelBar);
            camera.LateAddChild(HUD);

            AddChild(fuelStation);
            AddChild(visibility);
            AddChild(drillProgressIndicator);
            AddChild(camera);
            
        }

        private void UpdateDrilling(ref int playerX, ref int playerY, ref bool rangeCheck, ref bool movedThisFrame, ref Vector2Int movementDirection, ref Vector2Int desiredPosition, ref Vector2Int drillDirection, ref Vector2Int desiredDrillDirection) {
            if (!canStartDrilling && movementDirection != Vector2Int.zero) {
                canStartDrilling = true;
            }

            var wantsToDrill = Input.GetButton("Drill") && drillDirection != Vector2Int.zero;
            var isDrillingUp = drillDirection.y == -1;
            var hasGroundUnder = playerY + 1 == TilesVertical || tiles[playerX, playerY + 1] != ObjectType.Empty;
            if (canStartDrilling && wantsToDrill && !isDrillingUp && hasGroundUnder && rangeCheck && Settings.Tiles.TypeToTile[tiles[desiredDrillDirection.x, desiredDrillDirection.y]].Drillable) {
                if (lastDrillDirection != drillDirection || !startedDrilling) {
                    drillTimeOriginal = Settings.Tiles.TypeToTile[tiles[desiredDrillDirection.x, desiredDrillDirection.y]].TimeToDrill;
                    drillTimeLeft = drillTimeOriginal;
                }

                drillProgressIndicator.visible = true;
                drillProgressIndicator.SetXY(desiredDrillDirection.x * Globals.TILE_SIZE, desiredDrillDirection.y * Globals.TILE_SIZE);
                startedDrilling = true;
            } else {
                drillProgressIndicator.Alpha = 0f;
                drillProgressIndicator.visible = false;
                startedDrilling = false;
                canStartDrilling = false;
            }

            // BREAK TILE IF TIME IS DONE
            if (startedDrilling && drillTimeLeft <= 0) {
                Score += Settings.Tiles.TypeToTile[tiles[desiredDrillDirection.x, desiredDrillDirection.y]].ScoreAmount;
                fuelBar.FuelAmount += Settings.Tiles.TypeToTile[tiles[desiredDrillDirection.x, desiredDrillDirection.y]].FuelAmount;
                player.Move(drillDirection.ToWorld().ToVec2());
                tiles[playerX, playerY] = ObjectType.Empty;
                tiles[desiredDrillDirection.x, desiredDrillDirection.y] = ObjectType.Player;
                movedThisFrame = true;
                drillProgressIndicator.Alpha = 0f;
                drillProgressIndicator.visible = false;
                startedDrilling = false;
                canStartDrilling = false;

                // Recalculate position
                (playerX, playerY) = new Vector2(player.x, player.y).ToGrid().ToInt().Unpack();
                desiredPosition = movementDirection.Add(playerX, playerY);
                rangeCheck = desiredPosition.x >= 0 && desiredPosition.x < TilesHorizontal && desiredPosition.y >= 0 && desiredPosition.y < TilesVertical;
            }

            lastDrillDirection = drillDirection;
        }

        private void UpdateMovement(ref int playerX, ref int playerY, ref bool rangeCheck, ref bool movedThisFrame, ref Vector2Int movementDirection, ref Vector2Int desiredPosition) {
            if (movementDirection == Vector2Int.zero) return;

            // If player can move
            if (rangeCheck && tiles[desiredPosition.x, desiredPosition.y] == ObjectType.Empty && !movedThisFrame) {
                // Do the actual movement
                player.Move(movementDirection.ToWorld().ToVec2());
                tiles[playerX, playerY] = ObjectType.Empty;
                tiles[desiredPosition.x, desiredPosition.y] = ObjectType.Player;

                // Recalculate position
                (playerX, playerY) = new Vector2(player.x, player.y).ToGrid().ToInt().Unpack();
                desiredPosition = movementDirection.Add(playerX, playerY);
                rangeCheck = desiredPosition.x >= 0 && desiredPosition.x < TilesHorizontal && desiredPosition.y >= 0 && desiredPosition.y < TilesVertical;
            }

            timeSinceLastMovement = 0f;
            gravityTimeLeft = Settings.GravityFrequency;
        }

        private void UpdateGravity(ref int playerX, ref int playerY, ref bool rangeCheck, ref Vector2Int movementDirection, ref Vector2Int desiredPosition) {
            if (!(timeSinceLastMovement > Settings.PlayerMovementThreshold) || !(gravityTimeLeft <= 0))
                return;
            if (playerY + 1 < TilesVertical && tiles[playerX, playerY + 1] == ObjectType.Empty) {
                player.Move(0, Globals.TILE_SIZE);
                tiles[playerX, playerY] = ObjectType.Empty;
                tiles[playerX, playerY + 1] = ObjectType.Player;

                // Recalculate position
                (playerX, playerY) = new Vector2(player.x, player.y).ToGrid().ToInt().Unpack();
                desiredPosition = movementDirection.Add(playerX, playerY);
                rangeCheck = desiredPosition.x >= 0 && desiredPosition.x < TilesHorizontal && desiredPosition.y >= 0 && desiredPosition.y < TilesVertical;
            }

            gravityTimeLeft = Settings.GravityFrequency;
        }

        private void UpdateFuel(ref int playerX, ref int playerY) {
            if (fuelStation.IsPlayerOnRefillPoint(playerX, playerY) && Input.GetButtonDown("Refuel") && fuelStation.CanPlayerRefill()) {
                fuelBar.Refuel();
                fuelStation.ReduceRefillsLeft();
            }

            fuelBar.ChangeFuel(Settings.IdleFuelDepletion * Time.deltaTime);
            if (fuelBar.FuelAmount <= 0) {
                gameManager.ShouldStopPlaying = true;
            }
        }

        private void UpdateTimers() {
            timeSinceLastMovement += Time.deltaTime;
            if (timeSinceLastMovement > Settings.PlayerMovementThreshold) {
                gravityTimeLeft -= Time.deltaTime;
            }

            if (startedDrilling) {
                drillTimeLeft -= Time.deltaTime;
                drillProgressIndicator.Alpha = Math.Map(drillTimeLeft, drillTimeOriginal, 0f, 0f, 1f);
                fuelBar.ChangeFuel(Settings.DrillFuelDepletion * Time.deltaTime);
            }
        }

        public void DrawTileGrid(GLContext glContext) {
            var playerY = (int) (player.y / Globals.TILE_SIZE);
            var startY = Mathf.Max(playerY - Settings.RenderDistance, 0);
            var endY = Mathf.Min(playerY + Settings.RenderDistance, TilesVertical - 1);
            for (var i = 0; i < TilesHorizontal; i++) {
                for (var j = startY; j <= endY; j++) {
                    float[] verts = {i * Globals.TILE_SIZE, j * Globals.TILE_SIZE, i * Globals.TILE_SIZE + Globals.TILE_SIZE, j * Globals.TILE_SIZE, i * Globals.TILE_SIZE + Globals.TILE_SIZE, j * Globals.TILE_SIZE + Globals.TILE_SIZE, i * Globals.TILE_SIZE, j * Globals.TILE_SIZE + Globals.TILE_SIZE};
                    Settings.Tiles.TypeToTile[tilesBackground[i, j]].Texture.Bind();
                    glContext.DrawQuad(verts, Globals.QUAD_UV);
                    Settings.Tiles.TypeToTile[tiles[i, j]].Texture.Bind();
                    glContext.DrawQuad(verts, Globals.QUAD_UV);
                }
            }
        }

        protected override void RenderSelf(GLContext glContext) {
            glContext.SetColor(0xff, 0xff, 0xff, 0xff);
            topBackground.Bind();
            glContext.DrawQuad(topBackground.TextureVertices(0.711458333f, offset: new Vector2(0, -2*Globals.TILE_SIZE)), Globals.QUAD_UV);
            fuelStation.Draw(glContext);
            DrawTileGrid(glContext);
            drillProgressIndicator.Draw(glContext);
            visibility.Draw(glContext);
        }

        #region WORLD GEN
        private Bitmap PaintWorldMap() {
            var output = new Bitmap((int) (TilesHorizontal * Globals.TILE_SIZE), (int) (TilesVertical * Globals.TILE_SIZE));
            Graphics g = Graphics.FromImage(output);

            for (int x = 0; x < TilesHorizontal; x++) {
                for (int y = 0; y < TilesVertical; y++) {
                    int gridY = y + Settings.World.TopOffset;
                    var bitmap = Settings.Tiles.TypeToTile[tiles[x, gridY]].Texture.bitmap;
                    GraphicsUnit unit = GraphicsUnit.Pixel;
                    g.DrawImage(bitmap, new System.Drawing.Rectangle(x * 92, y * 92, (int) Globals.TILE_SIZE, (int) Globals.TILE_SIZE), bitmap.GetBounds(ref unit), GraphicsUnit.Pixel);
                }
            }

            return output;
        }

        private void GenerateWorldBracketedWithWalkers2(out int playerSpawnLocation) {
            for (int x = 0; x < TilesHorizontal; x++) {
                for (int y = 0; y < Settings.World.Depth; y++) {
                    int gridY = y + Settings.World.TopOffset;
                    tilesBackground[x, gridY] = ObjectType.Background;
                    float spawnTypeChance = Rand.Value;
                    if (spawnTypeChance <= Settings.World.StoneChance) {
                        tiles[x, gridY] = ObjectType.Stone;
                    } else if (spawnTypeChance <= Settings.World.StoneChance + Settings.World.OreChance) {
                        var weightedRandomizer = new WeightedRandomizer();
                        foreach (var oreType in Settings.World.Ores) {
                            try {
                                var oreBracket = Settings.World.OreDepthSpawning[oreType].First(value => y.BetweenInclusive(value.FromY, value.ToY));
                                weightedRandomizer.AddChance(oreType, oreBracket.Chance);
                                /*
                                if (Rand.Value <= oreBracket.Chance) {
                                    tileToSpawn = oreType;
                                    break;
                                }
                            */
                            } catch (System.InvalidOperationException) {
                                Debug.LogError($"Could not find matching element for ore type: {oreType} at y-depth: {y}");
                            }
                        }

                        var tileToSpawn = weightedRandomizer.GetValue();
                        Walker.Start2(x, y, tileToSpawn, this, Settings.World.TopOffset);
                    } else {
                        tiles[x, gridY] = ObjectType.Dirt;
                    }
                }
            }

            for (int x = 0; x < 4; x++) {
                tiles[x, Settings.World.TopOffset] = ObjectType.Stone;
            }

            playerSpawnLocation = Rand.Range(6, TilesHorizontal - 1);
            tiles[playerSpawnLocation, Settings.World.TopOffset - 1] = ObjectType.Player;
        }

        private void GenerateWorldBracketedWithWalkers(out int playerSpawnLocation) {
            for (int x = 0; x < TilesHorizontal; x++) {
                for (int y = 0; y < Settings.World.Depth; y++) {
                    int gridY = y + Settings.World.TopOffset;
                    tilesBackground[x, gridY] = ObjectType.Background;
                    float spawnTypeChance = Rand.Value;
                    if (spawnTypeChance <= Settings.World.StoneChance) {
                        tiles[x, gridY] = ObjectType.Stone;
                    } else if (spawnTypeChance <= Settings.World.StoneChance + Settings.World.OreChance) {
                        var tileToSpawn = ObjectType.Dirt;
                        foreach (var oreType in Settings.World.Ores) {
                            try {
                                var oreBracket = Settings.World.OreDepthSpawning[oreType].First(value => y.BetweenInclusive(value.FromY, value.ToY));
                                if (Rand.Value <= oreBracket.Chance) {
                                    tileToSpawn = oreType;
                                    break;
                                }
                            } catch (System.InvalidOperationException) {
                                Debug.LogError($"Could not find matching element for ore type: {oreType} at y-depth: {y}");
                            }
                        }

                        if (tileToSpawn != ObjectType.Dirt)
                            Walker.Start(x, y, tileToSpawn, this, Settings.World.TopOffset);
                        else
                            tiles[x, gridY] = ObjectType.Dirt;
                    } else {
                        tiles[x, gridY] = ObjectType.Dirt;
                    }
                }
            }

            for (int x = 0; x < 4; x++) {
                tiles[x, Settings.World.TopOffset] = ObjectType.Stone;
            }

            playerSpawnLocation = Rand.Range(6, TilesHorizontal - 1);
            tiles[playerSpawnLocation, Settings.World.TopOffset - 1] = ObjectType.Player;
        }

        private void GenerateWorldBracketed(out int playerSpawnLocation) {
            for (int x = 0; x < TilesHorizontal; x++) {
                for (int y = 0; y < Settings.World.Depth; y++) {
                    int gridY = y + Settings.World.TopOffset;
                    tilesBackground[x, gridY] = ObjectType.Background;
                    float spawnTypeChance = Rand.Value;
                    if (spawnTypeChance <= Settings.World.StoneChance) {
                        tiles[x, gridY] = ObjectType.Stone;
                    } else if (spawnTypeChance <= Settings.World.StoneChance + Settings.World.OreChance) {
                        var weightedRandomizer = new WeightedRandomizer();
                        foreach (var oreType in Settings.World.Ores) {
                            try {
                                var oreBracket = Settings.World.OreDepthSpawning[oreType].First(value => y.BetweenInclusive(value.FromY, value.ToY));
                                weightedRandomizer.AddChance(oreType, oreBracket.Chance);
                                /*
                                if (Rand.Value <= oreBracket.Chance) {
                                    tileToSpawn = oreType;
                                    break;
                                }
                            */
                            } catch (System.InvalidOperationException) {
                                Debug.LogError($"Could not find matching element for ore type: {oreType} at y-depth: {y}");
                            }
                        }

                        var tileToSpawn = weightedRandomizer.GetValue();
                        tiles[x, gridY] = tileToSpawn;
                    } else {
                        tiles[x, gridY] = ObjectType.Dirt;
                    }
                }
            }

            for (int x = 0; x < 4; x++) {
                tiles[x, Settings.World.TopOffset] = ObjectType.Stone;
            }

            playerSpawnLocation = Rand.Range(6, TilesHorizontal - 1);
            tiles[playerSpawnLocation, Settings.World.TopOffset - 1] = ObjectType.Player;
        }

        private void GenerateWorld(out int playerSpawnLocation) {
            for (int i = 0; i < TilesHorizontal; i++) {
                for (int j = Settings.World.TopOffset; j < TilesVertical; j++) {
                    tilesBackground[i, j] = ObjectType.Background;
                    int precentage = Rand.Range(0, 100);

                    if (precentage <= 85) {
                        tiles[i, j] = ObjectType.Dirt;
                    } else if (precentage >= 85 && precentage <= 95) {
                        tiles[i, j] = ObjectType.Stone;
                    } else if (precentage > 95) {
                        var oreChance = Rand.Range(0, 100);
                        if (oreChance <= 10) tiles[i, j] = ObjectType.Sapphire;
                        if (oreChance > 10 && oreChance <= 30) tiles[i, j] = ObjectType.Emerald;
                        if (oreChance > 30 && oreChance <= 60) tiles[i, j] = ObjectType.Gold;
                        if (oreChance > 60 && oreChance <= 100) tiles[i, j] = ObjectType.Coal;
                    }
                }
            }

            for (int x = 0; x < 4; x++) {
                tiles[x, Settings.World.TopOffset] = ObjectType.Stone;
            }

            playerSpawnLocation = Rand.Range(6, TilesHorizontal - 1);
            tiles[playerSpawnLocation, Settings.World.TopOffset - 1] = ObjectType.Player;
        }
        #endregion
    }
}