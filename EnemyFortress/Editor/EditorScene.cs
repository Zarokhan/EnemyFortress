using EnemyFortress.MenuSystem.Menus;
using EnemyFortress.SceneSystem.Base;
using EnemyFortress.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;

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
        private GameObject[,] map;

        private bool showLines;

        public EditorScene() : base()
        {
            current = new CurrentObject();
            map = new GameObject[100,100];
            showLines = true;
        }

        private void SaveMap()
        {
            SaveFileDialog dialog = new SaveFileDialog(); // Fixa: Enabrt .map filformat
            DialogResult result = dialog.ShowDialog();

            switch (result)
            {
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    break;
                case DialogResult.Abort:
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Ignore:
                    break;
                case DialogResult.Yes:
                    break;
                case DialogResult.No:
                    break;
                default:
                    break;
            }

            string path = "";
        }

        private void DeleteObject()
        {
            int x = (int)((Mouse.X) / 128);
            int y = (int)((Mouse.Y) / 128);

            if (x < 0 || y < 0)
                return;

            map[y, x] = null;
        }

        private void SetObject()
        {
            int x = (int)((Mouse.X) / 128);
            int y = (int)((Mouse.Y) / 128);

            if (x < 0 || y < 0)
                return;

            GameObject obj = new GameObject(AssetManager.Tilesheet);
            obj.sourceRect = new Rectangle(current.sourceRect.X, current.sourceRect.Y, current.sourceRect.Width, current.sourceRect.Height);
            obj.width = obj.sourceRect.Width;
            obj.height = obj.sourceRect.Height;
            obj.origin = new Vector2(obj.width / 2, obj.height / 2);
            obj.position = new Vector2(x * 128 + 64, y * 128 + 64);

            map[y, x] = obj;
        }

        public override void Update(GameTime gameTime, bool otherSceneHasFocus, bool coveredByOtherScene)
        {
            base.Update(gameTime, otherSceneHasFocus, coveredByOtherScene);
            current.position.X = Mouse.X;
            current.position.Y = Mouse.Y;

            // Save map
            if (Input.ClickedKey(Microsoft.Xna.Framework.Input.Keys.Space))
                SaveMap();

            // Add object
            if (Input.LeftButtonClicked())
                SetObject();

            // Delete object
            if (Input.ClickedKey(Microsoft.Xna.Framework.Input.Keys.Back))
                DeleteObject();

            // Next tile
            if (Input.ClickedKey(Microsoft.Xna.Framework.Input.Keys.Q))
                current.NextTile(1);
            else if (Input.ClickedKey(Microsoft.Xna.Framework.Input.Keys.W))
                current.NextTile(-1);

            // Camera Position
            if (Input.HoldingKey(Microsoft.Xna.Framework.Input.Keys.Left))
                camera.position.X -= 10;
            if (Input.HoldingKey(Microsoft.Xna.Framework.Input.Keys.Right))
                camera.position.X += 10;
            if (Input.HoldingKey(Microsoft.Xna.Framework.Input.Keys.Up))
                camera.position.Y -= 10;
            if (Input.HoldingKey(Microsoft.Xna.Framework.Input.Keys.Down))
                camera.position.Y += 10;
        }

        public override void HandleInput()
        {
            Mouse = camera.UnProject(Input.GetMousePosition());
        }

        public override void Draw()
        {
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.Transform);

            DrawMap(batch);

            if (showLines)
                DrawLines(batch);
            current.Draw(batch);
            batch.End();
        }

        private void DrawMap(SpriteBatch batch)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[y, x] != null)
                        map[y, x].Draw(batch);
                }
            }
        }

        private void DrawLines(SpriteBatch batch)
        {
            int lineSize = 100000;
            int borderSize = 2;

            for(int x = 0; x < 1000; x++)
                batch.Draw(AssetManager.Dot, new Rectangle(x * 128, -lineSize / 2, borderSize, lineSize), Color.Black);
            for(int i = 0; i < 1000; i++)
                batch.Draw(AssetManager.Dot, new Rectangle(-lineSize / 2, i * 128, lineSize, borderSize), Color.Black);
        }

        public override void OnExiting()
        {
            SceneManager.AddScene(new MenuSystem.Menus.MainMenu());
            SceneManager.RemoveScene(this);
        }
    }
}
