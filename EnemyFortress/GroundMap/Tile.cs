using EnemyFortress.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EnemyFortress.GroundMap
{
    class Tile : GameObject
    {
        public Tile(int posx, int posy, int srcx, int srcy)
            : base(AssetManager.Tilesheet)
        {
            width = 128;
            height = 128;
            origin = new Vector2(width * 0.5f, height * 0.5f);
            position = new Vector2(posx, posy);
            sourceRect = new Rectangle(srcx, srcy, width, height);
        }
    }
}
