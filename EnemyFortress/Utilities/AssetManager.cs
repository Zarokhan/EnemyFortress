using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EnemyFortress.Utilities
{
    public static class AssetManager
    {
        public static SpriteFont MenuFont { get; private set; }
        public static SpriteFont GameFont { get; private set; }
        public static Texture2D Dot { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            MenuFont = content.Load<SpriteFont>("Spritefont");
            Dot = content.Load<Texture2D>("dot");
        }
    }
}
