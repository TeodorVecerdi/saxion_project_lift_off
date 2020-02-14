using System.Drawing;
using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class TileGrid : GameObject {
        private const int topOffset = 6;
        private const int renderDistance = 10;
        private int fuelRefills;
        private int score;
        private int tilesHorizontal;
        private int tilesVertical;

        private const float idleFuelDepletion = -100f;
        private const float drillFuelDepletion = -1000f;
        private const float gravityFrequency = 0.25f;
        private const float playerMovementThreshold = 1f;
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
        private Font hudFont16, hudFont24, hudFont32, hudFont48, hudFont64;
        private Sprite gameOver;
        private Vector2Int lastDrillDirection = Vector2Int.zero;
        private Sprite drillProgressIndicator;

        public TileGrid(int tilesVertical) {
            tilesHorizontal = (int) (Globals.WIDTH / Globals.TILE_SIZE);
            this.tilesVertical = tilesVertical;
            tiles = new ObjectType[tilesHorizontal, tilesVertical];
            tilesBackground = new ObjectType[tilesHorizontal, tilesVertical];
            hudFont16 = FontLoader.Instance.GetFont(16);
            hudFont24 = FontLoader.Instance.GetFont(24);
            hudFont32 = FontLoader.Instance.GetFont(32);
            hudFont48 = FontLoader.Instance.GetFont(48);
            hudFont64 = FontLoader.Instance.GetFont(64);
            drillProgressIndicator = new Sprite("data/drillIndicator2.png");
            drillProgressIndicator.alpha = 0;

            //GENERATE WORLD
            for (int i = 0; i < tilesHorizontal; i++) {
                for (int j = topOffset; j < tilesVertical; j++) {
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

            int spawnLocation = Rand.Range(6, tilesHorizontal - 1);
            tiles[spawnLocation, topOffset - 1] = ObjectType.Player; // player

            player = new Transformable();
            player.SetXY(spawnLocation * Globals.TILE_SIZE, (topOffset - 1) * Globals.TILE_SIZE);

            camera = new Camera(0, 0, Globals.WIDTH, Globals.HEIGHT) {x = (int) (Globals.WIDTH / 2f)};
            AddChild(camera);

            fuelBar = new FuelBar();
            fuelBar.Move(-Globals.WIDTH / 2f, 0f); // weird camera behaviour fix
            camera.AddChild(fuelBar);
            HUD = new Canvas(Globals.WIDTH, Globals.HEIGHT, false);
            HUD.Move(0, -Globals.HEIGHT / 2f);
            HUD.Move(-Globals.WIDTH / 2f, 0f); // weird camera behaviour fix
            camera.AddChild(HUD);

            gameOver = new Sprite("data/gameover.png", true, false);
            gameOver.SetScaleXY(2.732f, 2.85f);
            gameOver.visible = false;
            gameOver.Move(0, -Globals.HEIGHT / 2f);
            gameOver.Move(-Globals.WIDTH / 2f, 0f); // weird camera behaviour fix
            camera.LateAddChild(gameOver);
            AddChild(drillProgressIndicator);
        }

        private void Update() {
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            // TODO: RECALCULATE POSITION IF PLAYER MOVED (GRAVITY, DRILL, MOVEMENT)
            DrawHud();

            // Poll Events
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
            var rangeCheck = desiredPosition.x >= 0 && desiredPosition.x < tilesHorizontal && desiredPosition.y >= 0 && desiredPosition.y < tilesVertical;
            var movedThisFrame = false;
            
            

            #region DRILL
            if (!canStartDrilling && movementDirection != Vector2Int.zero) {
                canStartDrilling = true;
            }

            var wantsToDrill = Input.GetKey(Key.SPACE) && drillDirection != Vector2Int.zero;
            var isDrillingUp = drillDirection.y == -1;
            var hasGroundUnder = playerY + 1 == tilesVertical || tiles[playerX, playerY + 1] != ObjectType.Empty;
            if (canStartDrilling && wantsToDrill && !isDrillingUp && hasGroundUnder && rangeCheck && Tiles.TypeToTile[tiles[desiredDrillDirection.x, desiredDrillDirection.y]].Passable && Tiles.TypeToTile[tiles[desiredDrillDirection.x, desiredDrillDirection.y]].Drillable) {
                if (lastDrillDirection != drillDirection || !startedDrilling) {
                    drillTimeOriginal = Tiles.TypeToTile[tiles[desiredDrillDirection.x, desiredDrillDirection.y]].TimeToDrill;
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
                score += Tiles.TypeToTile[tiles[desiredDrillDirection.x, desiredDrillDirection.y]].ScoreAmount;
                player.Move(drillDirection.ToWorld().ToVec2());
                tiles[playerX, playerY] = ObjectType.Empty;
                tiles[desiredDrillDirection.x, desiredDrillDirection.y] = ObjectType.Player;
                movedThisFrame = true;
                drillProgressIndicator.alpha = 0;
                drillProgressIndicator.visible = false;
                startedDrilling = false;
                canStartDrilling = false;
            }

            lastDrillDirection = drillDirection;
            #endregion

            #region GRAVITY
            if (timeSinceLastMovement > playerMovementThreshold && gravityTimeLeft <= 0) {
                if (playerY + 1 < tilesVertical && tiles[playerX, playerY + 1] == ObjectType.Empty) {
                    player.Move(0, Globals.TILE_SIZE);
                    tiles[playerX, playerY] = ObjectType.Empty;
                    tiles[playerX, playerY + 1] = ObjectType.Player;
                    playerY += 1;
                }

                gravityTimeLeft = gravityFrequency;
            }
            #endregion

            // Refuel
            if (playerX == 3 && playerY == topOffset - 1 && Input.GetKeyDown(Key.X) && fuelRefills <= 9) {
                fuelBar.Refuel();
                fuelRefills += 1;
            }

            #region PLAYER MOVEMENT
            if (movementDirection != Vector2Int.zero) {
                // Check if player can move
                if (rangeCheck && tiles[desiredPosition.x, desiredPosition.y] == ObjectType.Empty && !movedThisFrame) {
                    // PLAYER MOVEMENT
                    player.Move(movementDirection.ToWorld().ToVec2());
                    tiles[playerX, playerY] = ObjectType.Empty;
                    tiles[desiredPosition.x, desiredPosition.y] = ObjectType.Player;
                }

                timeSinceLastMovement = 0f;
                gravityTimeLeft = gravityFrequency;
            }
            #endregion

            #region TIMERS
            timeSinceLastMovement += Time.deltaTime;
            if (timeSinceLastMovement > playerMovementThreshold) {
                gravityTimeLeft -= Time.deltaTime;
            }

            if (startedDrilling) {
                drillTimeLeft -= Time.deltaTime;
                drillProgressIndicator.alpha = Math.Map(drillTimeLeft, drillTimeOriginal, 0f, 0f, 1f);
                fuelBar.ChangeFuel(drillFuelDepletion * Time.deltaTime);
            }
            #endregion

            #region UPDATE CAMERA
            if (Mathf.Abs(camera.y - player.y) > 1f) {
                camera.y = Mathf.SmoothDamp(camera.y, player.y, ref cameraVelocity, 0.3f);
            }
            #endregion

            // Change fuel
            fuelBar.ChangeFuel(idleFuelDepletion * Time.deltaTime);
            if (fuelBar.FuelAmount <= 0) {
                gameOver.visible = true;
            }
        }

        private void DrawHud() {
            HUD.graphics.Clear(Color.Empty);
            HUD.graphics.DrawString("SCORE: " + score, hudFont64, Brushes.FloralWhite, Globals.WIDTH / 2f, 24, FontLoader.Instance.Center);
            HUD.graphics.DrawString("FUEL", hudFont64, Brushes.FloralWhite, Globals.WIDTH - 30, Globals.HEIGHT / 2f, FontLoader.Instance.CenterVertical);
            HUD.graphics.DrawString("FPS: " + game.currentFps, SystemFonts.StatusFont, Brushes.DarkRed, 0, 8, FontLoader.Instance.Left);
        }

        protected override void RenderSelf(GLContext glContext) {
            glContext.SetColor(0xff, 0xff, 0xff, 0xff);

            int playerY = (int) (player.y / Globals.TILE_SIZE);
            int startY = Mathf.Max(playerY - renderDistance, 0);
            int endY = Mathf.Min(playerY + renderDistance, tilesVertical - 1);
            for (int i = 0; i < tilesHorizontal; i++) {
                for (int j = startY; j <= endY; j++) {
                    float[] verts = {i * Globals.TILE_SIZE, j * Globals.TILE_SIZE, i * Globals.TILE_SIZE + Globals.TILE_SIZE, j * Globals.TILE_SIZE, i * Globals.TILE_SIZE + Globals.TILE_SIZE, j * Globals.TILE_SIZE + Globals.TILE_SIZE, i * Globals.TILE_SIZE, j * Globals.TILE_SIZE + Globals.TILE_SIZE};
                    Tiles.TypeToTile[tilesBackground[i, j]].Texture.Bind();
                    glContext.DrawQuad(verts, Globals.QUAD_UV);
                    Tiles.TypeToTile[tiles[i, j]].Texture.Bind();
                    glContext.DrawQuad(verts, Globals.QUAD_UV);
                }
            }
        }
    }
}