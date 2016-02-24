﻿using EnemyFortress.SceneSystem.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EnemyFortress.Forms;
using System.Windows.Forms;
using EnemyFortress.Networking;
using System.Threading;

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

        public override void Update(GameTime gameTime, bool otherSceneHasFocus, bool coveredByOtherScene)
        {
            base.Update(gameTime, otherSceneHasFocus, coveredByOtherScene);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
