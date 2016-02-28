using EnemyFortress.Controllers;
using EnemyFortress.Networking;
using EnemyFortress.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EnemyFortress.Player
{
    class TankGun : GameObject
    {
        public TankGun() : base(AssetManager.Tank)
        {
            sourceRect = GetSourceRect(1296, 80, 233, 532);
            int orgX = 1414;    // Texture origin
            int orgY = 518;     // Texture origin
            origin = new Vector2(orgX - sourceRect.X, orgY - sourceRect.Y);
            width = sourceRect.Width;
            height = sourceRect.Height;

            scale = 0.1f;
        }
    }

    class Tank : GameObject
    {
        public Control control { get; private set; }

        TankGun gun;

        private float speed;
        private float rotation_speed;   // In degrees

        public Tank(int x = 0, int y = 0) : base(AssetManager.Tank)
        {
            sourceRect = GetSourceRect(741, 140, 485, 613);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
            position = new Vector2(x, y);
            width = sourceRect.Width;
            height = sourceRect.Height;

            gun = new TankGun();

            scale = 0.1f;

            speed = 100f;
            rotation_speed = 90f;
        }

        /// <summary>
        /// Moves forward if val = 1
        /// </summary>
        /// <param name="val">Forward(val = 1), Backward(val = -1)</param>
        public void MoveForward(GameTime gameTime, float val = 1)
        {
            position.X += (float)Math.Cos(rotation) * speed * val * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y += (float)Math.Sin(rotation) * speed * val * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Turns right if val = 1
        /// </summary>
        /// <param name="val">Right(val = 1), Left(val = -1)</param>
        public void TurnRight(GameTime gameTime, float val = 1)
        {
            this.rotation += MathHelper.ToRadians(rotation_speed) * val * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Update(GameTime gameTime)
        {
            gun.position.X = position.X;
            gun.position.Y = position.Y;

            if (control != null)
                control.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch, float rotationOffset = 0)
        {
            base.Draw(batch, MathHelper.ToRadians(90));
            gun.Draw(batch);
        }

        public void SetControl(Control control = null)
        {
            this.control = control;
        }
    }
}
