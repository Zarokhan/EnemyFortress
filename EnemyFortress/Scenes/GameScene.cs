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
        public List<Tank> remoteTanks { get; private set; }
        public bool DrawPing { get; set; }
        public bool DrawUserList { get; set; }

        private Client client;          // Client information
        private Thread listenerThread;  // Listens for incomming traffic

        private List<GameObject> objList;
        private TileMap map;
        private Tank tank;

        public GameScene(Client client) : base()
        {
            this.client = client;
            client.GameScene = this;
            remoteTanks = new List<Tank>();
            objList = new List<GameObject>();
            map = new TileMap("master2");

            listenerThread = new Thread(client.HoldConnection);
            listenerThread.Start();

            DrawPing = false;
            DrawUserList = false;
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
        public void SpawnClientTank(int x = 0, int y = 0)
        {
            tank = new Tank(x, y);
            tank.position = new Vector2(map.Spawn.X, map.Spawn.Y);
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

        public override void HandleInput()
        {
            if (tank == null)
                return;

            ClientControl control = (ClientControl)tank.Control;
            control.Mouse = camera.UnProject(Input.GetMousePosition());
        }

        public override void Update(GameTime gameTime, bool otherSceneHasFocus, bool coveredByOtherScene)
        {
            base.Update(gameTime, otherSceneHasFocus, coveredByOtherScene);
            CheckConnection();
            if (tank == null)
                return;

            camera.position = new Vector2(tank.position.X, tank.position.Y);

            for (int i = 0; i < remoteTanks.Count; i++)
                remoteTanks[i].Update(gameTime);

            tank.Update(gameTime);
        }

        public override void Draw()
        {
            if (tank == null)
                return;

            // Draws Game
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.Transform);
            map.Draw(batch);

            for (int i = 0; i < remoteTanks.Count; i++)
                remoteTanks[i].Draw(batch);

            tank.Draw(batch);
            batch.End();
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, hudCamera.Transform);
            if (DrawUserList)
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
                RemoteControl control = (RemoteControl)remoteTanks[i].Control;
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
