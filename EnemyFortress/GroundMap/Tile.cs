using EnemyFortress.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EnemyFortress.GroundMap
{
    class Tile : GameObject
    {
        public const int SIZE = 16;
        public int id;
        public bool walkable = true;
        public TileType type;

        public Tile(int id, TileType type, Vector2 pos)
            : base(AssetManager.Garden)
        {
            this.id = id;
            this.scale = 1;
            origin = new Vector2(SIZE * 0.5f * scale, SIZE * 0.5f * scale);
            this.position = (pos + origin) * scale;
            this.width = (int)(SIZE * scale);
            this.height = (int)(SIZE * scale);
            this.type = type;
            SetSourceRect();
        }

        public void SetSourceRect()
        {
            switch (type)
            {
                case TileType.Grass:
                    sourceRect = GetSourceRect(1, 4, SIZE);
                    break;
            }
        }
    }
}
