using EnemyFortress.Networking;
using EnemyFortress.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EnemyFortress.Controllers
{
    class RemoteControl : Control
    {
        public int ID { get; private set; }
        public string Alias { get; private set; }

        public RemoteControl(Client client, Tank tank, int id, string alias) : base(client, tank)
        {
            this.ID = id;
            this.Alias = alias;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
