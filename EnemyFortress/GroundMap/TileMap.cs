using EnemyFortress.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace EnemyFortress.GroundMap
{
    class TileMap
    {
        public Tile[,] tile;
        public Vector2 Spawn { get; private set; }

        private string map;

        public TileMap(string map = null)
        {
            this.map = map;
            tile = new Tile[100,100];
            LoadMap(map);
        }

        /// <summary>
        /// Loads map from file
        /// </summary>
        /// <param name="map"></param>
        public void LoadMap(string map)
        {
            this.map = map;
            tile = new Tile[100, 100];
            StreamReader sr = new StreamReader(@"Content\" + map + ".efmap");
            List<string> lines = new List<string>();

            while (!sr.EndOfStream)
            {
                string[] split = sr.ReadLine().Split('|');   // separate the data
                int posx = 0, posy = 0;
                switch (split[0])
                {
                    case "spawn":
                        //int team = 0; Will be implemented later
                        //string teamName;
                        Spawn = new Vector2(int.Parse(split[3]), int.Parse(split[4]));

                        break;
                    case "map":
                        int x = 0, y = 0, srcx = 0, srcy = 0;   // converts the data into integers
                                                                                    // Process data
                        for (int i = 1; i < split.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(split[i]))
                                continue;
                            string[] xy = split[i].Split('-');
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
                        tile[y, x] = new Tile(posx, posy, srcx, srcy);
                        break;
                    default:
                        break;
                }
            }
            sr.Dispose();
        }

        public void Draw(SpriteBatch batch)
        {
            for (int y = 0; y < tile.GetLength(0); y++)
            {
                for (int x = 0; x < tile.GetLength(1); x++)
                {
                    if (tile[y, x] == null)
                        continue;

                    tile[y, x].Draw(batch);
                }
            }
        }
    }
}
