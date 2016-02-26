using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EnemyFortress.Utilities
{
    class GameObject
    {
        protected Rectangle sourceRect;
        public Vector2 pos;
        protected Vector2 origin;
        protected float scale, rotation;
        protected Texture2D texture;
        protected Color color;
        protected SpriteEffects fx;
        protected int width, height;

        public GameObject(Texture2D texture)
        {
            this.texture = texture;
            origin = new Vector2();
            scale = 1f;
            color = Color.White;
            fx = SpriteEffects.None;
        }

        public Rectangle GetOffsetRect(int xOffset, int yOffset)
        {
            return new Rectangle((int)(pos.X - origin.X) + xOffset, (int)(pos.Y - origin.Y) + yOffset, width, height);
        }

        public Rectangle GetRect()
        {
            Vector2 temp = pos - origin;
            return new Rectangle((int)temp.X, (int)temp.Y, width, height);
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, pos, sourceRect, color, rotation, origin, scale, fx, 0);
        }
    }
}
