using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EnemyFortress.Utilities
{
    class GameObject
    {
        public Rectangle sourceRect;
        public Vector2 position;
        protected Vector2 origin;
        public float scale, rotation;
        protected Texture2D texture;
        protected Color color;
        protected SpriteEffects fx;
        protected int width, height;

        public GameObject(Texture2D texture)
        {
            this.texture = texture;
            position = new Vector2();
            origin = new Vector2();
            scale = 1f;
            color = Color.White;
            fx = SpriteEffects.None;
        }

        public Rectangle GetOffsetRect(int xOffset, int yOffset)
        {
            return new Rectangle((int)(position.X - origin.X) + xOffset, (int)(position.Y - origin.Y) + yOffset, width, height);
        }

        /// <summary>
        /// Get a tile source rectangle from a spritesheet
        /// </summary>
        public static Rectangle GetSourceRect(int col, int row, int size, int xOffset = 1, int yOffset = 1)
        {
            return new Rectangle(xOffset + col * size, yOffset + row * size, size, size);
        }

        /// <summary>
        /// Get a source rectangle from properties offset and bounds
        /// </summary>
        public static Rectangle GetSourceRect(int xOffset, int yOffset, int width, int height)
        {
            return new Rectangle(xOffset, yOffset, width, height);
        }

        /// <summary>
        /// Gets a hitbox of the object
        /// </summary>
        public Rectangle GetRect()
        {
            Vector2 temp = position - origin;
            return new Rectangle((int)temp.X, (int)temp.Y, width, height);
        }

        public virtual void Draw(SpriteBatch batch, float rotationOffset = 0)
        {
            batch.Draw(texture, position, sourceRect, color, rotation + rotationOffset, origin, scale, fx, 0);
        }
    }
}
