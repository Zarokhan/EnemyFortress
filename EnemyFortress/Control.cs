using EnemyFortress.Networking;
using EnemyFortress.Player;
using Microsoft.Xna.Framework;

namespace EnemyFortress.Controllers
{
    class Control
    {
        protected Client client;
        protected Tank tank;

        public Control(Client client = null, Tank tank = null)
        {
            this.client = client;
            this.tank = tank;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (client == null || tank == null)
                return;
        }

        public void SetControl(Client client = null)
        {
            this.client = client;
        }

        public void SetTank(Tank tank = null)
        {
            this.tank = tank;
        }
    }
}
