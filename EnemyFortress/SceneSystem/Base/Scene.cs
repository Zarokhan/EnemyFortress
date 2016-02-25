using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EnemyFortress.Utilities;

namespace EnemyFortress.SceneSystem.Base
{
    // current state the scene is in
    public enum SceneState
    {
        Active,
        Inactive,
    }
    
    /// <summary>
    /// Abstract scene class.
    /// All scenes inherit from this class.
    /// </summary>
    public abstract class Scene
    {
        // Properties
        public bool IsActive { get { return !otherSceneHasFocus && State == SceneState.Active; } }
        public bool IsPopup { get; protected set; }
        public SceneState State { get; protected set; }

        // Member variables
        private bool otherSceneHasFocus;
        protected SpriteBatch batch;
        protected Camera camera;

        public Scene()
        {
            State = SceneState.Active;
            batch = new SpriteBatch(SceneManager.GraphicsDevice);
            camera = new Camera(SceneManager.GraphicsDevice.Viewport, new Vector2(SceneManager.GraphicsDevice.Viewport.Width / 2, SceneManager.GraphicsDevice.Viewport.Height / 2));
        }

        public virtual void HandleInput() { }

        public virtual void Update(GameTime gameTime, bool otherSceneHasFocus, bool coveredByOtherScene)
        {
            camera.Update(SceneManager.Graphics);

            this.otherSceneHasFocus = otherSceneHasFocus; 
            State = (coveredByOtherScene) ? SceneState.Inactive : SceneState.Active;
        }

        public virtual void Draw() { }

        public virtual void OnExiting() { }

        protected void AddScene(Scene scene, bool isPopup = false)
        {
            if (!isPopup)
                SceneManager.RemoveScene(this);

            SceneManager.AddScene(scene);
        }

        protected void ChangeCameraViewport(int width, int height)
        {
            int currWidth = camera.viewPort.Width;
            int currHeight = camera.viewPort.Height;

            while (currWidth != width && currHeight != height)
            {
                if (currHeight < height)
                    currHeight++;
                else if (currHeight > height)
                    currHeight--;

                if (currWidth < width)
                    currWidth++;
                else if (currWidth > width)
                    currWidth--;

                camera.viewPort.Width = currWidth;
                camera.viewPort.Height = currHeight;
                camera.Update(SceneManager.Graphics);
            }
        }

    }
}
