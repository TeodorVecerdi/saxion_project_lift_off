using System;
using System.Drawing;
using System.Linq;
using Game.WorldGen;
using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class TileGrid : GameObject {
        private const int topOffset = 6;
        private const int renderDistance = 10;
        public readonly int TilesHorizontal;
        public readonly int TilesVertical;
        private int fuelRefills;
        private int score;

        private const float idleFuelDepletion = -333f;
        private const float drillFuelDepletion = -1000f;
        private const float gravityFrequency = 0.33333f;
        private const float playerMovementThreshold = 0.33333f;
        private float gravityTimeLeft = gravityFrequency;
        private float timeSinceLastMovement;
        private float cameraVelocity;
        private float drillTimeLeft;
        private float drillTimeOriginal;

        private bool startedDrilling;
        private bool canStartDrilling;

        private ObjectType[,] tiles;
        private ObjectType[,] tilesBackground;
        private Camera camera;
        private Transformable player;
        private FuelBar fuelBar;
        private Canvas HUD;
        private Sprite gameOver;
        private Vector2Int lastDrillDirection = Vector2Int.zero;
        private Sprite drillProgressIndicator;

        public TileGrid(int tilesVertical) {
            TilesHorizontal = (int) (Globals.WIDTH / Globals.TILE_SIZE);
            TilesVertical = tilesVertical;
            tiles = new ObjectType[TilesHorizontal, topOffset + tilesVertical];
            tilesBackground = new ObjectType[TilesHorizontal, topOffset + tilesVertical];

            // GenerateWorld(out var playerSpawnLocation);
            GenerateWorldBracketed(out var playerSpawnLocation);
            // GenerateWorldBracketedWithWalkers(out var playerSpawnLocation);
            // GenerateWorldBracketedWithWalkers2(out var playerSpawnLocation);
            // Bitmap worldMap = PaintWorldMap();
            // worldMap.Save("world4.png");
            // Environment.Exit(-1);
            InitializeSceneObjects(playerSpawnLocation);
        }

        private Bitmap PaintWorldMap() {
            var output = new Bitmap((int) (TilesHorizontal * Globals.TILE_SIZE), (int) (TilesVertical * Globals.TILE_SIZE));
            Graphics g = Graphics.FromImage(output);

            for (int x = 0; x < TilesHorizontal; x++) {
                for (int y = 0; y < TilesVertical; y++) {
                    int gridY = y + topOffset;
                    var bitmap = Settings.Tiles.TypeToTile[tiles[x, gridY]].Texture.bitmap;
                    GraphicsUnit unit = GraphicsUnit.Pixel;
                    g.DrawImage(bitmap, new System.Drawing.Rectangle(x * 92, y * 92, (int) Globals.TILE_SIZE, (int) Globals.TILE_SIZE), bitmap.GetBounds(ref unit), GraphicsUnit.Pixel);
                }
            }

            return output;
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
            HUD.graphics.DrawString("SCORE: " + score, FontLoader.Instance[64f], Brushes.FloralWhite, Globals.WIDTH / 2f, 24, FontLoader.CenterAlignment);
            HUD.graphics.DrawString($"DEPTH: {Settings.World.BlockSize * (player.y / Globals.TILE_SIZE - topOffset + 1)}m", FontLoader.Instance[32f], Brushes.AntiqueWhite, Globals.WIDTH / 2f, 64, FontLoader.CenterAlignment);
            HUD.graphics.DrawString("FUEL", FontLoader.Instance[64f], Brushes.FloralWhite, Globals.WIDTH - 30, Globals.HEIGHT / 2f, FontLoader.CenterVerticalAlignment);
            HUD.graphics.DrawString("FPS: " + game.currentFps, SystemFonts.StatusFont, Brushes.DarkRed, 0, 8, FontLoader.LeftAlignment);
        }

        private void GenerateWorldBracketedWithWalkers2(out int playerSpawnLocation) {
            for (int x = 0; x < TilesHorizontal; x++) {
                for (int y = 0; y < Settings.World.Depth; y++) {
                    int gridY = y + topOffset;
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
                            } catch (System.InvalidOperationException e) {
                                Debug.LogError($"Could not find matching element for ore type: {oreType} at y-depth: {y}");
                            }
                        }

                        var tileToSpawn = weightedRandomizer.GetValue();
                        Walker.Start2(x, y, tileToSpawn, this, topOffset);
                    } else {
                        tiles[x, gridY] = ObjectType.Dirt;
                    }
                }
            }

            for (int x = 0; x < 4; x++) {
                tiles[x, topOffset] = ObjectType.Stone;
            }

            playerSpawnLocation = Rand.Range(6, TilesHorizontal - 1);
            tiles[playerSpawnLocation, topOffset - 1] = ObjectType.Player;
        }

        private void GenerateWorldBracketedWithWalkers(out int playerSpawnLocation) {
            for (int x = 0; x < TilesHorizontal; x++) {
                for (int y = 0; y < Settings.World.Depth; y++) {
                    int gridY = y + topOffset;
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
                            } catch (System.InvalidOperationException e) {
                                Debug.LogError($"Could not find matching element for ore type: {oreType} at y-depth: {y}");
                            }
                        }

                        if (tileToSpawn != ObjectType.Dirt)
                            Walker.Start(x, y, tileToSpawn, this, topOffset);
                        else
                            tiles[x, gridY] = ObjectType.Dirt;
                    } else {
                        tiles[x, gridY] = ObjectType.Dirt;
                    }
                }
            }

            for (int x = 0; x < 4; x++) {
                tiles[x, topOffset] = ObjectType.Stone;
            }

            playerSpawnLocation = Rand.Range(6, TilesHorizontal - 1);
            tiles[playerSpawnLocation, topOffset - 1] = ObjectType.Player;
        }

        private void GenerateWorldBracketed(out int playerSpawnLocation) {
            for (int x = 0; x < TilesHorizontal; x++) {
                for (int y = 0; y < Settings.World.Depth; y++) {
                    int gridY = y + topOffset;
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
                            } catch (System.InvalidOperationException e) {
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
                tiles[x, topOffset] = ObjectType.Stone;
            }

            playerSpawnLocation = Rand.Range(6, TilesHorizontal - 1);
            tiles[playerSpawnLocation, topOffset - 1] = ObjectType.Player;
        }

        private void GenerateWorld(out int playerSpawnLocation) {
            for (int i = 0; i < TilesHorizontal; i++) {
                for (int j = topOffset; j < TilesVertical; j++) {
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
                tiles[x, topOffset] = ObjectType.Stone;
            }

            playerSpawnLocation = Rand.Range(6, TilesHorizontal - 1);
            tiles[playerSpawnLocation, topOffset - 1] = ObjectType.Player;
        }

        private void InitializeSceneObjects(int playerSpawnLocation) {
            drillProgressIndicator = new Sprite("data/drillIndicator2.png") {alpha = 0};

            player = new Transformable();
            player.SetXY(playerSpawnLocation * Globals.TILE_SIZE, (topOffset - 1) * Globals.TILE_SIZE);

            camera = new Camera(0, 0, Globals.WIDTH, Globals.HEIGHT) {x = (int) (Globals.WIDTH / 2f)}; // weird camera behaviour fix

            fuelBar = new FuelBar();
            fuelBar.Move(-Globals.WIDTH / 2f, 0f); // weird camera behaviour fix
            HUD = new Canvas(Globals.WIDTH, Globals.HEIGHT, false);
            HUD.Move(0, -Globals.HEIGHT / 2f);
            HUD.Move(-Globals.WIDTH / 2f, 0f); // weird camera behaviour fix

            gameOver = new Sprite("data/gameover.png", true, false);
            gameOver.SetScaleXY(2.732f, 2.85f);
            gameOver.visible = false;
            gameOver.Move(0, -Globals.HEIGHT / 2f);
            gameOver.Move(-Globals.WIDTH / 2f, 0f); // weird camera behaviour fix

            camera.AddChild(fuelBar);
            camera.LateAddChild(gameOver);
            camera.LateAddChild(HUD);
            AddChild(camera);
            AddChild(drillProgressIndicator);
        }

        private void UpdateDrilling(ref int playerX, ref int playerY, ref bool rangeCheck, ref bool movedThisFrame, ref Vector2Int movementDirection, ref Vector2Int desiredPosition, ref Vector2Int drillDirection, ref Vector2Int desiredDrillDirection) {
            if (!canStartDrilling && movementDirection != Vector2Int.zero) {
                canStartDrilling = true;
            }

            var wantsToDrill = Input.GetKey(Key.SPACE) && drillDirection != Vector2Int.zero;
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
                drillProgressIndicator.alpha = 0;
                drillProgressIndicator.visible = false;
                startedDrilling = false;
                canStartDrilling = false;
            }

            // BREAK TILE IF TIME IS DONE
            if (startedDrilling && drillTimeLeft <= 0) {
                score += Settings.Tiles.TypeToTile[tiles[desiredDrillDirection.x, desiredDrillDirection.y]].ScoreAmount;
                fuelBar.FuelAmount += Settings.Tiles.TypeToTile[tiles[desiredDrillDirection.x, desiredDrillDirection.y]].FuelAmount;
                player.Move(drillDirection.ToWorld().ToVec2());
                tiles[playerX, playerY] = ObjectType.Empty;
                tiles[desiredDrillDirection.x, desiredDrillDirection.y] = ObjectType.Player;
                movedThisFrame = true;
                drillProgressIndicator.alpha = 0;
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
            gravityTimeLeft = gravityFrequency;
        }

        private void UpdateGravity(ref int playerX, ref int playerY, ref bool rangeCheck, ref Vector2Int movementDirection, ref Vector2Int desiredPosition) {
            if (!(timeSinceLastMovement > playerMovementThreshold) || !(gravityTimeLeft <= 0))
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

            gravityTimeLeft = gravityFrequency;
        }

        private void UpdateFuel(ref int playerX, ref int playerY) {
            if (playerX == 3 && playerY == topOffset - 1 && Input.GetKeyDown(Key.X) && fuelRefills <= 9) {
                fuelBar.Refuel();
                fuelRefills += 1;
            }

            fuelBar.ChangeFuel(idleFuelDepletion * Time.deltaTime);
            if (fuelBar.FuelAmount <= 0) {
                gameOver.visible = true;
            }
        }

        private void UpdateTimers() {
            timeSinceLastMovement += Time.deltaTime;
            if (timeSinceLastMovement > playerMovementThreshold) {
                gravityTimeLeft -= Time.deltaTime;
            }

            if (startedDrilling) {
                drillTimeLeft -= Time.deltaTime;
                drillProgressIndicator.alpha = Math.Map(drillTimeLeft, drillTimeOriginal, 0f, 0f, 1f);
                fuelBar.ChangeFuel(drillFuelDepletion * Time.deltaTime);
            }
        }

        protected override void RenderSelf(GLContext glContext) {
            glContext.SetColor(0xff, 0xff, 0xff, 0xff);

            int playerY = (int) (player.y / Globals.TILE_SIZE);
            int startY = Mathf.Max(playerY - renderDistance, 0);
            int endY = Mathf.Min(playerY + renderDistance, TilesVertical - 1);
            for (int i = 0; i < TilesHorizontal; i++) {
                for (int j = startY; j <= endY; j++) {
                    float[] verts = {i * Globals.TILE_SIZE, j * Globals.TILE_SIZE, i * Globals.TILE_SIZE + Globals.TILE_SIZE, j * Globals.TILE_SIZE, i * Globals.TILE_SIZE + Globals.TILE_SIZE, j * Globals.TILE_SIZE + Globals.TILE_SIZE, i * Globals.TILE_SIZE, j * Globals.TILE_SIZE + Globals.TILE_SIZE};
                    Settings.Tiles.TypeToTile[tilesBackground[i, j]].Texture.Bind();
                    glContext.DrawQuad(verts, Globals.QUAD_UV);
                    Settings.Tiles.TypeToTile[tiles[i, j]].Texture.Bind();
                    glContext.DrawQuad(verts, Globals.QUAD_UV);
                }
            }
        }

        public ObjectType[,] Tiles => tiles;
    }
}