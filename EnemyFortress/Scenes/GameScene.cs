using EnemyFortress.SceneSystem.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EnemyFortress.Networking;
using System.Threading;
using System.Windows.Forms;
using EnemyFortress.Player;
using Microsoft.Xna.Framework.Graphics;
using EnemyFortress.Utilities;
using EnemyFortress.Controllers;

namespace EnemyFortress.Scenes
{
    /// <summary>
    /// Main game scene for application.
    /// </summary>
    class GameScene : Scene
    {
        private Client client;          // Client information
        private Thread listenerThread;  // Listens for incomming traffic

        private Tank tank;
        public List<Tank> otherTanks { get; private set; }

        public GameScene(Client client) : base()
        {
            this.client = client;
            otherTanks = new List<Tank>();
            client.SetGameScene(this);

            listenerThread = new Thread(client.HoldConnection);
            listenerThread.Start();
        }

        /// <summary>
        /// Spawns remote player into world
        /// </summary>
        /// <param name="id">id of remote player</param>
        /// <param name="alias">alias of remote player</param>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        public void SpawnRemoteTank(int id, string alias, int x, int y)
        {
            Tank tank = new Tank(x, y);
            tank.SetControl(new RemoteControl(client, tank, id, alias));
            otherTanks.Add(tank);
        }

        /// <summary>
        /// Spawns client tank into world
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        public void SpawnClientTank(int x, int y)
        {
            tank = new Tank(x, y);
            tank.SetControl(new ClientControl(client, tank));
        }

        /// <summary>
        /// Exits game scene when we are disconnected from server
        /// </summary>
        private void CheckConnection()
        {
            if (!client.IsConnected)
            {
                client.Disconnect();
                SceneManager.RemoveScene(this);
                SceneManager.AddScene(new MenuSystem.Menus.MainMenu());
            }
        }

        public override void Update(GameTime gameTime, bool otherSceneHasFocus, bool coveredByOtherScene)
        {
            base.Update(gameTime, otherSceneHasFocus, coveredByOtherScene);
            CheckConnection();

            for (int i = 0; i < otherTanks.Count; i++)
                otherTanks[i].Update(gameTime);

            if (tank != null)
                tank.Update(gameTime);
        }

        public override void Draw()
        {
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.Transform);

            for (int i = 0; i < otherTanks.Count; i++)
                otherTanks[i].Draw(batch);

            if (tank != null)
                tank.Draw(batch);

            DrawPlayerList(batch);
            batch.End();
        }

        private void DrawPlayerList(SpriteBatch batch)
        {
            // Draw player list
            batch.DrawString(AssetManager.MenuFont, client.Alias, new Vector2(0, 0), Color.Black);
            int height = (int)AssetManager.MenuFont.MeasureString("X").Y;
            for (int i = 0; i < otherTanks.Count; i++)
            {
                RemoteControl control = (RemoteControl)otherTanks[i].control;
                batch.DrawString(AssetManager.MenuFont, control.Alias, new Vector2(0, (i + 1) * height), Color.DarkGray);
            }
        }

        public override void OnExiting()
        {
            client.Disconnect();
        }
    }
}
