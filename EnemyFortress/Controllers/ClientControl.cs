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
        // Latency & Ping
        private float latencyUpdateTimer;

        public ClientControl(Client client, Tank tank) : base(client, tank) { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateLatency(gameTime);

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
