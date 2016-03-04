using EnemyFortress.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EnemyFortress.GroundMap
{
    class Tile : GameObject
    {
        public int id;

        public Tile(int id, Point pos)
            : base(AssetManager.Tilesheet)
        {
            this.id = id;
            this.scale = 1;
            width = texture.Width;
            height = texture.Height;
            origin = new Vector2(width * 0.5f * scale, height * 0.5f * scale);
            this.position = new Vector2();
            this.position.X = (pos.X * width) + origin.X;
            this.position.Y = (pos.Y * height) + origin.Y;
            sourceRect = new Rectangle(0, 0, width, height);
        }
    }
}
