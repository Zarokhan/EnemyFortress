using EnemyFortress.SceneSystem.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EnemyFortress.Networking;
using System.Threading;
using System.Windows.Forms;

namespace EnemyFortress.Scenes
{
    class GameScene : Scene
    {
        private Client client;
        private Thread listenerThread;

        public GameScene(Client client)
        {
            this.client = client;
            listenerThread = new Thread(client.HoldConnection);
            listenerThread.Start();
        }

        private void CheckConnection()
        {
            if (!client.IsConnected)
            {
                SceneManager.RemoveScene(this);
                SceneManager.AddScene(new MenuSystem.Menus.MainMenu());
            }
        }

        public override void Update(GameTime gameTime, bool otherSceneHasFocus, bool coveredByOtherScene)
        {
            base.Update(gameTime, otherSceneHasFocus, coveredByOtherScene);
            CheckConnection();
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void OnExiting()
        {
            client.IsConnected = false;
            while(listenerThread.IsAlive)
                Application.DoEvents();
        }
    }
}
