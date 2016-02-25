using EnemyFortress.Controllers;
using EnemyFortress.Networking;
using EnemyFortress.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EnemyFortress.Player
{
    class Tank : GameObject
    {
        public Control control { get; private set; }

        private float speed;

        public Tank(int x = 0, int y = 0) : base(AssetManager.Dot)
        {
            width = 70;
            height = 70;
            origin = new Vector2(width / 2f, height / 2f);
            color = Color.Red;
            pos = new Vector2(x, y);

            speed = 100f;
        }

        public void MoveDown(GameTime gameTime)
        {
            pos.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void MoveUp(GameTime gameTime)
        {
            pos.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void MoveLeft(GameTime gameTime)
        {
            pos.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void MoveRight(GameTime gameTime)
        {
            pos.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Update(GameTime gameTime)
        {
            if (control != null)
                control.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, width, height), color);
        }

        public void SetControl(Control control = null)
        {
            this.control = control;
        }
    }
}
