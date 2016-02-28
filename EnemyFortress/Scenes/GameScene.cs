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
using EnemyFortress.GroundMap;

namespace EnemyFortress.Scenes
{
    /// <summary>
    /// Main game scene for application.
    /// </summary>
    class GameScene : Scene
    {
        public bool DrawPing { get; set; }

        private Client client;          // Client information
        private Thread listenerThread;  // Listens for incomming traffic

        private TileMap map;
        private Tank tank;
        public List<Tank> remoteTanks { get; private set; }

        public GameScene(Client client) : base()
        {
            this.client = client;
            client.GameScene = this;
            remoteTanks = new List<Tank>();
            map = new TileMap();

            listenerThread = new Thread(client.HoldConnection);
            listenerThread.Start();

            DrawPing = false;
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
            remoteTanks.Add(tank);
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

            for (int i = 0; i < remoteTanks.Count; i++)
                remoteTanks[i].Update(gameTime);

            if (tank != null)
                tank.Update(gameTime);
        }

        public override void Draw()
        {
            // Draws Game
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.Transform);
            map.Draw(batch);

            for (int i = 0; i < remoteTanks.Count; i++)
                remoteTanks[i].Draw(batch);

            if (tank != null)
                tank.Draw(batch);
            batch.End();
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, hudCamera.Transform);
            DrawPlayerList(batch);
            batch.End();
        }

        /// <summary>
        /// Draws player list
        /// </summary>
        private void DrawPlayerList(SpriteBatch batch)
        {
            string netInfo = "";
            if (DrawPing)
                netInfo = " Latency: " + client.Latency + " Ping: " + client.Ping;

            batch.DrawString(AssetManager.Font, client.Alias + netInfo, new Vector2(0, 0), Color.Black);
            int height = (int)AssetManager.Font.MeasureString("X").Y;
            for (int i = 0; i < remoteTanks.Count; i++)
            {
                RemoteControl control = (RemoteControl)remoteTanks[i].control;
                if (DrawPing)
                    netInfo = " Latency: " + control.Latency + " Ping: " + control.Ping;

                batch.DrawString(AssetManager.Font, control.Alias + netInfo, new Vector2(0, (i + 1) * height), Color.DarkGray);
            }
        }

        public override void OnExiting()
        {
            client.Disconnect();
        }
    }
}
