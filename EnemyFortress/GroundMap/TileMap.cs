using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace EnemyFortress.GroundMap
{
    class TileMap
    {
        public int tiles_width;
        public int tiles_height;

        // Read map from text file
        private List<string> lines;

        public Tile[,] tile;

        private string mapName;

        public TileMap(string mapName = null)
        {
            this.mapName = mapName;
            //LoadMap(mapName);
            tile = new Tile[100,100];

            for(int i = 0; i < tile.GetLength(1); i++)
            {
                for(int j = 0; j < tile.GetLength(0); j++)
                {
                    int id = j + i * tile.GetLength(1);
                    Vector2 position = new Vector2(j * Tile.SIZE, i * Tile.SIZE);
                    tile[i, j] = new Tile(id, TileType.Grass, position);
                }
            }
        }

        /// <summary>
        /// Loads map from file
        /// </summary>
        /// <param name="mapName"></param>
        public void LoadMap(string mapName)
        {
            this.mapName = mapName;
            StreamReader sr;

            sr = new StreamReader(@"Content\Maps\" + mapName + ".txt");

            lines = new List<string>();

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                // Will stop loading map after - character
                if (line.Substring(0, 1).Equals("-"))
                {
                    break;
                }
                // Will not execute a row with two / characters, acts like comments in map file. 
                if (!line.Substring(0, 2).Equals("//"))
                {
                    lines.Add(line.ToUpper());
                }
            }
            sr.Dispose();

            ProcessMap();
        }

        private void ProcessMap()
        {
            tiles_width = lines[0].Length;
            tiles_height = lines.Count;

            tile = new Tile[tiles_height, tiles_width];
            for (int y = 0; y < tile.GetLength(0); y++)
            {
                for (int x = 0; x < tile.GetLength(1); x++)
                {
                    TileType type = TileType.Grass;
                    Vector2 position = new Vector2(x * Tile.SIZE, y * Tile.SIZE);

                    switch (lines[y].Substring(x, 1))
                    {
                        case "G":
                            type = TileType.Grass;
                            break;
                    }

                    int id = x + y * tile.GetLength(1);
                    tile[y, x] = new Tile(id, type, position);

                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            for (int y = 0; y < tile.GetLength(0); y++)
            {
                for (int x = 0; x < tile.GetLength(1); x++)
                {
                    tile[y, x].Draw(batch);
                }
            }
        }
    }
}
