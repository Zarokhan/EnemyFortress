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
            // Check if we want to update latency
            latencyUpdateTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if(latencyUpdateTimer <= 0)
            {
                client.UpdateLatency();
                latencyUpdateTimer = 1000;
            }

            // Update Movement
            if (Input.HoldingKey(Keys.Up))
                tank.MoveUp(gameTime);
            else if (Input.HoldingKey(Keys.Down))
                tank.MoveDown(gameTime);
            if (Input.HoldingKey(Keys.Left))
                tank.MoveLeft(gameTime);
            else if (Input.HoldingKey(Keys.Right))
                tank.MoveRight(gameTime);
            // Send movement to server
            client.WriteMessage(Commands.Send(Command.Movement, client.ID + "|" + (int)tank.pos.X + "|" + (int)tank.pos.Y)); // KEY|COMMAND|ID|X|Y
        }
    }
}
