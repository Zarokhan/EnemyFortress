using EnemyFortress.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EnemyFortress.Utilities;
using Microsoft.Xna.Framework.Input;
using EnemyFortress.Networking;

namespace EnemyFortress.Controllers
{
    class ClientControl : Control
    {
        public ClientControl(Client client, Tank tank) : base(client, tank) { }

        public override void Update(GameTime gameTime)
        {
            if (tank == null)
                return;

            if (Input.HoldingKey(Keys.Up))
                tank.MoveUp(gameTime);
            if (Input.HoldingKey(Keys.Down))
                tank.MoveDown(gameTime);
            if (Input.HoldingKey(Keys.Left))
                tank.MoveLeft(gameTime);
            if (Input.HoldingKey(Keys.Right))
                tank.MoveRight(gameTime);
        }
    }
}
