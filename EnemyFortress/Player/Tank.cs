using EnemyFortress.Controllers;
using EnemyFortress.Networking;
using EnemyFortress.SceneSystem.Base;
using EnemyFortress.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EnemyFortress.Player
{
    class TankWheels : GameObject
    {
        public TankWheels() : base(AssetManager.Tank)
        {
            sourceRect = GetSourceRect(52, 72, 619, 690);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
            width = sourceRect.Width;
            height = sourceRect.Height;

            scale = Tank.START_SCALE;
        }

        public void Update(Vector2 position, float rotation)
        {
            this.position.X = position.X;
            this.position.Y = position.Y;
            this.rotation = rotation;
        }
    }

    class TankGun : GameObject
    {
        private float rotation_speed;

        public TankGun() : base(AssetManager.Tank)
        {
            sourceRect = GetSourceRect(1296, 80, 233, 532);
            int orgX = 1414;    // Texture origin
            int orgY = 518;     // Texture origin
            origin = new Vector2(orgX - sourceRect.X, orgY - sourceRect.Y);
            width = sourceRect.Width;
            height = sourceRect.Height;

            scale = Tank.START_SCALE;
            rotation_speed = 90;
        }

        /// <summary>
        /// Turns right if val = 1
        /// </summary>
        /// <param name="val">Right(val = 1), Left(val = -1)</param>
        public void TurnRight(GameTime gameTime, float val = 1)
        {
            this.rotation += MathHelper.ToRadians(rotation_speed) * val * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.rotation = this.rotation % MathHelper.ToRadians(360);
            this.rotation = this.rotation % MathHelper.ToRadians(-360);
        }

        public void Update(Vector2 position)
        {
            this.position.X = position.X;
            this.position.Y = position.Y;
        }
    }

    class Tank : GameObject
    {
        public const float START_SCALE = 0.15f;

        public Control Control { get; private set; }
        public TankGun Gun { get; private set; }
        private TankWheels wheels;

        private float speed;
        private float rotation_speed;   // In degrees

        public Tank(int x = 0, int y = 0) : base(AssetManager.Tank)
        {
            sourceRect = GetSourceRect(741, 140, 485, 613);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height * 0.45f);
            position = new Vector2(x, y);
            width = sourceRect.Width;
            height = sourceRect.Height;

            Gun = new TankGun();
            wheels = new TankWheels();

            scale = START_SCALE;

            speed = 130f;
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
            this.rotation = this.rotation % MathHelper.ToRadians(360);
            this.rotation += MathHelper.ToRadians(rotation_speed) * val * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 temp = new Vector2();
            temp.X = position.X - (float)Math.Cos(rotation) * height * 0.20f * scale;
            temp.Y = position.Y - (float)Math.Sin(rotation) * height * 0.20f * scale;

            Gun.Update(temp);
            wheels.Update(this.position, rotation);

            if (Control != null)
                Control.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch, float rotationOffset = 0)
        {
            wheels.Draw(batch, MathHelper.ToRadians(90));
            base.Draw(batch, MathHelper.ToRadians(90));
            Gun.Draw(batch, MathHelper.ToRadians(90));
        }

        public void SetControl(Control control = null)
        {
            this.Control = control;
        }
    }
}
