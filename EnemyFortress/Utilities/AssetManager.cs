using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EnemyFortress.Utilities
{
    public static class AssetManager
    {
        public static SpriteFont Font { get; private set; }
        public static Texture2D Dot { get; private set; }
        public static Texture2D Garden { get; private set; }
        public static Texture2D Tank { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            Font = content.Load<SpriteFont>("Spritefont");
            Dot = content.Load<Texture2D>("dot");
            Garden = content.Load<Texture2D>("spritesheet");
            Tank = content.Load<Texture2D>("tank1");
        }
    }
}
