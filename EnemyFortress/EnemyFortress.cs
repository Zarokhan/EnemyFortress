using System;
using EnemyFortress.MenuSystem.Menus;
using EnemyFortress.SceneSystem.Base;
using EnemyFortress.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnemyFortress
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class EnemyFortress : Game
    {
        public const int WindowWidth = 1280;
        public const int WindowHeight = 720;

        public GraphicsDeviceManager Graphics;

        public EnemyFortress()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.IsFullScreen = false;
            Graphics.PreferredBackBufferWidth = WindowWidth;
            Graphics.PreferredBackBufferHeight = WindowHeight;
            Graphics.ApplyChanges();

            Window.AllowUserResizing = false;
            Window.Position = new Point(WindowWidth / 4, WindowHeight / 4);

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            AssetManager.LoadContent(Content);

            SceneManager.Initialize(this);
            SceneManager.AddScene(new MainMenu());
            
            base.Initialize();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Input.ClickedKey(Keys.Escape))
                Exit();

            Input.Update();
            SceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            SceneManager.Draw();
            base.Draw(gameTime);
        }

        /// <summary>
        /// This is called when user exits the application.
        /// </summary>
        protected override void OnExiting(object sender, EventArgs args)
        {
            SceneManager.OnExiting();
            base.OnExiting(sender, args);
        }
    }
}
