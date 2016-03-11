using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EnemyFortress.Utilities
{
    public static class AssetManager
    {
        public static SpriteFont Font { get; private set; }
        public static Texture2D Dot { get; private set; }
        public static Texture2D Tank { get; private set; }
        public static Texture2D Spawn { get; private set; }
        public static Texture2D Eraser { get; private set; }
        public static Texture2D Tilesheet { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            Font = content.Load<SpriteFont>("Spritefont");
            Dot = content.Load<Texture2D>("dot");
            Tank = content.Load<Texture2D>("tank1");
            Eraser = content.Load<Texture2D>("eraser");
            Spawn = content.Load<Texture2D>("spawn");
            Tilesheet = content.Load<Texture2D>("cobbleset-128");
        }
    }
}
