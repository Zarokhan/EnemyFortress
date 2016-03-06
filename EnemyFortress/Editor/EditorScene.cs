using EnemyFortress.MenuSystem.Menus;
using EnemyFortress.SceneSystem.Base;
using EnemyFortress.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace EnemyFortress.Editor
{
    /// <summary>
    /// Helper class for level editor
    /// Basically represents the object you place into the world
    /// </summary>
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

    /// <summary>
    /// EditorScene created by Zarokhan
    /// 
    /// Keys
    /// Q: Prev tile
    /// W: Next tile
    /// Back: Delete tile
    /// Space: Save tile
    /// </summary>
    class EditorScene : Scene
    {
        private Vector2 mouse;
        private CurrentObject current;
        private GameObject[,] map;
        private List<string> lines;

        private bool showLines;

        public EditorScene() : base()
        {
            current = new CurrentObject();
            map = new GameObject[100,100];
            showLines = true;
        }

        /// <summary>
        /// Saves the map to selected path
        /// </summary>
        private void SaveMap()
        {
            SaveFileDialog dialog = new SaveFileDialog(); // Fixa: Enabrt .map filformat
            dialog.Filter = "EnemyFortress Map | *.efmap";
            dialog.DefaultExt = "efmap";
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.Cancel)
                return;
            if (result == DialogResult.OK)
            {
                ProcessMap();
                string path = dialog.FileName;
                StreamWriter writer = new StreamWriter(path);
                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }
                writer.Close();
            }
        }

        /// <summary>
        /// Process the map into lines of string
        /// </summary>
        private void ProcessMap()
        {
            lines = new List<string>();
            for(int y = 0; y < map.GetLength(1); y++)
            {
                for(int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[y, x] == null)
                        continue;

                    GameObject obj = map[y, x];
                    string line = "obj|" + x + "-" + y + "|" + obj.position.X + "-" + obj.position.Y + "|" + obj.sourceRect.X + "-" + obj.sourceRect.Y + "|";
                    lines.Add(line);
                }
            }
        }

        /// <summary>
        /// Deletes the object that the mouse hovers
        /// </summary>
        private void DeleteObject()
        {
            int x = (int)((mouse.X) / 128);
            int y = (int)((mouse.Y) / 128);

            if (x < 0 || y < 0)
                return;

            map[y, x] = null;
        }

        /// <summary>
        /// Sets current object into the game world
        /// </summary>
        private void SetObject()
        {
            int x = (int)((mouse.X) / 128);
            int y = (int)((mouse.Y) / 128);

            if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1))
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
            current.position.X = mouse.X;
            current.position.Y = mouse.Y;

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
            mouse = camera.UnProject(Input.GetMousePosition());
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
