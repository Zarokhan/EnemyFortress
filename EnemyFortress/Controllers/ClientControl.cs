using EnemyFortress.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EnemyFortress.Utilities;
using Microsoft.Xna.Framework.Input;
using EnemyFortress.Networking;
using Utilities;

namespace EnemyFortress.Controllers
{
    class ClientControl : Control
    {
        public Vector2 Mouse { get; set; }
        // Latency & Ping
        private float latencyUpdateTimer;

        public ClientControl(Client client, Tank tank) : base(client, tank) { Mouse = new Vector2(); }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateLatency(gameTime);

            // Update tank gun movement
            Vector2 target = new Vector2(Mouse.X - tank.Gun.position.X, Mouse.Y - tank.Gun.position.Y);
            target.Normalize();
            tank.Gun.rotation = (float)Math.Atan2(target.Y, target.X);

            // Update Movement
            if (Input.HoldingKey(Keys.W))
                tank.MoveForward(gameTime, 1);
            else if (Input.HoldingKey(Keys.S))
                tank.MoveForward(gameTime, -1);
            if (Input.HoldingKey(Keys.D))
                tank.TurnRight(gameTime, 1);
            else if (Input.HoldingKey(Keys.A))
                tank.TurnRight(gameTime, -1);

            // Send movement to server
            client.WriteMessage(Commands.Send(Command.Movement, client.ID + "|" + tank.position.X + "|" + tank.position.Y + "|" + tank.rotation)); // KEY|COMMAND|ID|X|Y|ROTATION(RADS)
            client.WriteMessage(Commands.Send(Command.Gun, client.ID + "|" + tank.Gun.rotation));   // KEY|COMMAND|ID|ROTATION(RADS)
        }

        /// <summary>
        /// Check if we want to update latency
        /// </summary>
        private void UpdateLatency(GameTime gameTime)
        {
            latencyUpdateTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (latencyUpdateTimer <= 0)
            {
                client.UpdateLatency();
                latencyUpdateTimer = 1000;
            }
        }
    }
}
