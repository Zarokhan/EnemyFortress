using EnemyFortress.Forms;
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
    class MapObj : GameObject
    {
        int xTiles;
        int yTiles;
        int maxTiles;
        int currentTile;

        public MapObj() : base(AssetManager.Tilesheet)
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
    /// Spawn object for placing spawn points into map
    /// </summary>
    class SpawnObj : GameObject
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }

        public SpawnObj() : base(AssetManager.Spawn)
        {
            width = texture.Width;
            height = texture.Height;
            sourceRect = new Rectangle(0, 0, width, height);
            origin = new Vector2(width / 2, height / 2);
        }
    }

    /// <summary>
    /// Tool for deleting objects within the editor
    /// </summary>
    class DeleteTool : GameObject
    {
        public DeleteTool() : base(AssetManager.Eraser)
        {
            width = texture.Width;
            height = texture.Height;
            sourceRect = new Rectangle(0, 0, width, height);
            origin = new Vector2(width / 2, height / 2);
            scale = 0.3f;
        }
    }

    /// <summary>
    /// EditorScene created by Zarokhan
    /// 
    /// Keys
    /// Q: Prev tile
    /// W: Next tile
    /// </summary>
    class EditorScene : Scene
    {
        private EditorForm form;
        private Vector2 mouse;
        private GameObject currentTool;
        private GameObject[,] map;
        private List<string> lines;
        private List<GameObject> objList;
        public List<SpawnObj> spawnList { get; private set; }

        private bool showLines;

        public EditorScene() : base()
        {
            currentTool = new MapObj();
            map = new GameObject[100,100];
            objList = new List<GameObject>();
            spawnList = new List<SpawnObj>();
            form = new EditorForm(this);
            form.Show();
            showLines = true;
        }

        /// <summary>
        /// Activates spawn placement
        /// </summary>
        public void SpawnPlacement()
        {
            currentTool = new SpawnObj();
        }

        /// <summary>
        /// Activates map placement
        /// </summary>
        public void MapPlacement()
        {
            currentTool = new MapObj();
        }

        /// <summary>
        /// Activates the delete tool
        /// </summary>
        public void DeleteTool()
        {
            currentTool = new DeleteTool();
        }

        /// <summary>
        /// Toggles between showing lines
        /// </summary>
        public void ToggleLines()
        {
            showLines = !showLines;
        }

        /// <summary>
        /// Loads map from file
        /// </summary>
        public void LoadMap()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "EnemyFortress Map | *.efmap";
            dialog.DefaultExt = "efmap";
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.Cancel)
                return;
            if (result == DialogResult.OK)
            {
                map = new GameObject[100, 100];
                objList.Clear();
                StreamReader reader = new StreamReader(dialog.FileName);
                while (!reader.EndOfStream)
                {
                    string[] data = reader.ReadLine().Split('|');
                    int posx = 0, posy = 0;
                    switch (data[0])
                    {
                        case "spawn":
                            int teamid = int.Parse(data[1]);
                            string teamname = data[2];
                            posx = int.Parse(data[3]);
                            posy = int.Parse(data[4]);
                            SpawnObj obj1 = new SpawnObj();
                            obj1.TeamID = teamid;
                            obj1.TeamName = teamname;
                            obj1.position = new Vector2(posx, posy);
                            objList.Add(obj1);
                            spawnList.Add(obj1);
                            break;
                        case "map":
                            int x = 0, y = 0, srcx = 0, srcy = 0;   // converts the data into integers
                            for (int i = 1; i < data.Length; i++)
                            {
                                if (string.IsNullOrWhiteSpace(data[i]))
                                    continue;
                                string[] xy = data[i].Split('-');
                                switch (i)
                                {
                                    case 1:
                                        x = int.Parse(xy[0]);
                                        y = int.Parse(xy[1]);
                                        break;
                                    case 2:
                                        posx = int.Parse(xy[0]);
                                        posy = int.Parse(xy[1]);
                                        break;
                                    case 3:
                                        srcx = int.Parse(xy[0]);
                                        srcy = int.Parse(xy[1]);
                                        break;
                                }
                            }
                            GameObject obj = new GameObject(AssetManager.Tilesheet);
                            obj.width = 128;
                            obj.height = 128;
                            obj.position = new Vector2(posx, posy);
                            obj.origin = new Vector2(128 / 2, 128 / 2);
                            obj.sourceRect = new Rectangle(srcx, srcy, 128, 128);
                            map[y, x] = obj;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Saves the map to selected path
        /// </summary>
        public void SaveMap()
        {
            SaveFileDialog dialog = new SaveFileDialog(); // Fixa: Enabrt .map filformat
            dialog.Filter = "EnemyFortress Map | *.efmap";
            dialog.DefaultExt = "efmap";
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.Cancel)
                return;
            if (result == DialogResult.OK)
            {
                string path = dialog.FileName;
                StreamWriter writer = null;
                try
                {
                    writer = new StreamWriter(path);
                }
                catch (System.Exception)
                {
                    return;
                }
                ProcessMap();

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
            // Process game map
            for(int y = 0; y < map.GetLength(1); y++)
            {
                for(int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[y, x] == null)
                        continue;

                    GameObject obj = map[y, x];
                    string line = "map|" + x + "-" + y + "|" + obj.position.X + "-" + obj.position.Y + "|" + obj.sourceRect.X + "-" + obj.sourceRect.Y + "|";
                    lines.Add(line);
                }
            }
            // Process game objects
            for(int i = 0; i < objList.Count; i++)
            {
                if(objList[i] is SpawnObj)
                {
                    SpawnObj obj = (SpawnObj)objList[i];
                    string line = "spawn|" + obj.TeamID + "|" + obj.TeamName + "|" + (int)obj.position.X + "|" + (int)obj.position.Y + "|";
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
            
            for(int i = 0; i < objList.Count; i++)  // Needs to be refined
            {
                if(objList[i].GetRect().Intersects(currentTool.GetRect()))
                {
                    objList.RemoveAt(i);
                    return;
                }
            }

            map[y, x] = null;
        }

        /// <summary>
        /// Sets current object into the game world and snaps it
        /// </summary>
        private void SetMapObject()
        {
            int x = (int)((mouse.X) / 128);
            int y = (int)((mouse.Y) / 128);

            if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1))
                return;

            GameObject obj = new GameObject(AssetManager.Tilesheet);
            obj.sourceRect = new Rectangle(currentTool.sourceRect.X, currentTool.sourceRect.Y, currentTool.sourceRect.Width, currentTool.sourceRect.Height);
            obj.width = obj.sourceRect.Width;
            obj.height = obj.sourceRect.Height;
            obj.origin = new Vector2(obj.width / 2, obj.height / 2);
            obj.position = new Vector2(x * 128 + 64, y * 128 + 64);

            map[y, x] = obj;
        }

        /// <summary>
        /// Adds spawn object into the world
        /// </summary>
        private void AddSpawnObject()
        {
            if (mouse.X < 0 || mouse.Y < 0)
                return;

            SpawnForm form = new SpawnForm(this);
            DialogResult result = form.ShowDialog();

            if (result == DialogResult.Cancel)
                return;

            if (objList.Count != 0 && (int)objList[objList.Count - 1].position.X == (int)mouse.X && (int)objList[objList.Count - 1].position.Y == (int)mouse.Y)
                return;

            SpawnObj obj = new SpawnObj();
            obj.TeamName = form.teamName;
            obj.TeamID = spawnList.Count;
            obj.position = new Vector2((int)mouse.X, (int)mouse.Y);
            objList.Add(obj);
            spawnList.Add(obj);
        }

        public override void Update(GameTime gameTime, bool otherSceneHasFocus, bool coveredByOtherScene)
        {
            base.Update(gameTime, otherSceneHasFocus, coveredByOtherScene);
            currentTool.position.X = mouse.X;
            currentTool.position.Y = mouse.Y;

            // Add object
            if (Input.LeftButtonClicked())
            {
                if(currentTool is MapObj)
                {
                    SetMapObject();
                }
                if(currentTool is DeleteTool)
                {
                    DeleteObject();
                }
                if(currentTool is SpawnObj)
                {
                    AddSpawnObject();
                }
            }

            // Next tile
            if(currentTool is MapObj)
            {
                MapObj temp = (MapObj)currentTool;

                if (Input.ClickedKey(Microsoft.Xna.Framework.Input.Keys.Q))
                    temp.NextTile(1);
                else if (Input.ClickedKey(Microsoft.Xna.Framework.Input.Keys.W))
                    temp.NextTile(-1);
            }

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

            for (int i = 0; i < objList.Count; i++)
                objList[i].Draw(batch);

            currentTool.Draw(batch);
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
