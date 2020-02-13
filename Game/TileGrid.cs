using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class TileGrid : GameObject {
        private const int topOffset = 6;
        private const int renderDistance = 10;
        private int fuelRefills = 0;

        private int tilesHorizontal;
        private int tilesVertical;

        private ObjectType[,] tiles;
        private ObjectType[,] tilesBackground;
        public Camera Camera;
        private Transformable player;
        public FuelBar FuelBar;

        private const float gravityFrequency = 0.25f;
        private const float playerMovementThreshold = 1f;
        private float gravityTimeLeft = gravityFrequency;
        private float timeSinceLastMovement;
        private float cameraVelocity;

        public TileGrid(int tilesVertical) {
            tilesHorizontal = (int) (Globals.WIDTH / Globals.TILE_SIZE);
            this.tilesVertical = tilesVertical;
            tiles = new ObjectType[tilesHorizontal, tilesVertical];
            tilesBackground = new ObjectType[tilesHorizontal, tilesVertical];

            //GENERATE WORLD
            for (int i = 0; i < tilesHorizontal; i++) {
                for (int j = topOffset; j < tilesVertical; j++) {
                    tilesBackground[i, j] = ObjectType.Background;
                    int precentage = Rand.Range(0, 100);
                    
                    if (precentage <= 85) { tiles[i, j] = ObjectType.Dirt; }
                    else if (precentage >= 85 && precentage <= 95) { tiles[i, j] = ObjectType.Stone; }
                    else if (precentage > 95)
                    {
                        int orechance = Rand.Range(0, 100);
                        if ( orechance <=10){ tiles[i, j] = ObjectType.Sapphire; }
                        if (orechance >10 && orechance <= 30 ){ tiles[i, j] = ObjectType.Emerald; }
                        if (orechance >30 && orechance <= 60) { tiles[i, j] = ObjectType.Gold; }
                        if (orechance >60 && orechance <= 100) { tiles[i, j] = ObjectType.Coal; }
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

            Camera = new Camera(-(int) (Globals.WIDTH / 2f), 0, Globals.WIDTH, Globals.HEIGHT);
            AddChild(Camera);

            FuelBar = new FuelBar();
            Camera.AddChild(FuelBar);
        }

        void Update() {
            Debug.Log($"FPS: {game.currentFps}");

            #region GRAVITY
            if (timeSinceLastMovement > playerMovementThreshold && gravityTimeLeft <= 0) {
                var playerPosition = player.position.ToGrid().ToInt();

                // var playerPosition = new Vector2Int((int) (player.x / Globals.TILE_SIZE), (int) (player.y / Globals.TILE_SIZE));
                if (playerPosition.y + 1 < tilesVertical && tiles[playerPosition.x, playerPosition.y + 1] == ObjectType.Empty) {
                    player.Move(0, Globals.TILE_SIZE);
                    tiles[playerPosition.x, playerPosition.y] = ObjectType.Empty;
                    tiles[playerPosition.x, playerPosition.y + 1] = ObjectType.Player;
                }

                gravityTimeLeft = gravityFrequency;
            }
            #endregion

            #region PLAYER MOVEMENT
            // Get input
            var (playerX, playerY) = new Vector2(player.x, player.y).ToGrid().ToInt().Unpack();
            var movement = new Vector2Int((int) Input.GetAxisDown("Horizontal"), (int) Input.GetAxisDown("Vertical"));
            var isDrilling = Input.GetKey(Key.SPACE);
            if (movement.x != 0 && movement.y != 0) {
                movement.x = 0;
            }
            if (playerX == 3 && playerY == topOffset - 1 && Input.GetKeyDown(Key.LEFT_CTRL)&&fuelRefills <=9)
            {
                FuelBar.FuelAmount = 100000f;
                fuelRefills += 1;


            }
            if (movement != Vector2Int.zero) {
               
                var desiredPosition = movement.Add(playerX, playerY);

                // Check if player can move
                var rangeCheck = desiredPosition.x >= 0 && desiredPosition.x < tilesHorizontal && desiredPosition.y >= 0 && desiredPosition.y < tilesVertical;
                if (rangeCheck && tiles[desiredPosition.x, desiredPosition.y] == ObjectType.Empty) {
                    // PLAYER MOVEMENT
                    player.Move(movement.ToWorld().ToVec2());
                    tiles[playerX, playerY] = ObjectType.Empty;
                    tiles[desiredPosition.x, desiredPosition.y] = ObjectType.Player;
                } else if (rangeCheck && isDrilling && Tiles.TypeToTile[tiles[desiredPosition.x, desiredPosition.y]].Passable) {
                    // PLAYER DRILLING
                    var isDrillingUp = movement.y == -1;
                    var hasGroundUnder = playerY + 1 == tilesVertical || tiles[playerX, playerY + 1] != ObjectType.Empty;
                    if (!isDrillingUp && hasGroundUnder) {
                        player.Move(movement.ToWorld().ToVec2());
                        tiles[playerX, playerY] = ObjectType.Empty;
                        tiles[desiredPosition.x, desiredPosition.y] = ObjectType.Player;
                    }
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
            #endregion

            #region UPDATE CAMERA
            if (Mathf.Abs(Camera.y - player.y) > 1f) {
                Camera.y = Mathf.SmoothDamp(Camera.y, player.y, ref cameraVelocity, 0.3f);
            }
            #endregion

            // Change fuel
            FuelBar.ChangeFuel(-1000 * Time.deltaTime); // -500 mL per second
           
        }

        protected override void RenderSelf(GLContext glContext) {
            glContext.SetColor(0xff, 0xff, 0xff, 0xff);
            int playerY = (int) (player.y / Globals.TILE_SIZE);
            int startY = Mathf.Max(playerY - renderDistance, 0);
            int endY = Mathf.Min(playerY + renderDistance, tilesVertical-1);
            
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