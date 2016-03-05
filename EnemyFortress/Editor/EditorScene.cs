using EnemyFortress.MenuSystem.Menus;
using EnemyFortress.SceneSystem.Base;
using EnemyFortress.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnemyFortress.Editor
{
    class CurrentObject : GameObject
    {
        int xTiles;
        int yTiles;
        int maxTiles;
        int currentTile;

        public CurrentObject() : base(AssetManager.Tilesheet)
        {
            sourceRect = new Rectangle(0, 0, 128, 128);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
            width = sourceRect.Width;
            height = sourceRect.Height;

            xTiles = (int)(texture.Width / 128);
            yTiles = (int)(texture.Height / 128);
            maxTiles = xTiles * yTiles;
        }

        /// <summary>
        /// Changes the tile
        /// </summary>
        /// <param name="val"></param>
        public void NextTile(int val = 1)
        {
            currentTile += val;
            int y = currentTile / yTiles;
            int x = currentTile % xTiles;
            sourceRect.X = x * width;
            sourceRect.Y = y * height;

            // Checkbounds
            if (currentTile >= maxTiles)
                currentTile = 0;
            if (currentTile < 0)
                currentTile = maxTiles;
        }
    }

    class EditorScene : Scene
    {
        public Vector2 Mouse { get; private set; }
        public CurrentObject current;

        public EditorScene() : base()
        {
            current = new CurrentObject();
        }

        public void SetZoom(float val = 1)
        {
            camera.Zoom = val;
        }

        public override void Update(GameTime gameTime, bool otherSceneHasFocus, bool coveredByOtherScene)
        {
            base.Update(gameTime, otherSceneHasFocus, coveredByOtherScene);
            current.position.X = Mouse.X;
            current.position.Y = Mouse.Y;

            if (Input.ClickedKey(Keys.Q))
                current.NextTile(1);
            else if (Input.ClickedKey(Keys.W))
                current.NextTile(-1);
        }

        public override void HandleInput()
        {
            Mouse = camera.UnProject(Input.GetMousePosition());
        }

        public override void Draw()
        {
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.Transform);
            current.Draw(batch);
            batch.End();
        }

        public override void OnExiting()
        {
            SceneManager.AddScene(new MainMenu());
            SceneManager.RemoveScene(this);
        }
    }
}
