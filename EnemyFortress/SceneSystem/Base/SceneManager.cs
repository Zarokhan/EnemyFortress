using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EnemyFortress.Utilities;
using System.Collections.Generic;

namespace EnemyFortress.SceneSystem.Base
{
    /// <summary>
    /// The user will always interract with scenes. This is their manager, which keeps track of
    /// which scene is showing, if there is scenes underneath it which needs to be rendered etc
    /// </summary>
    public static class SceneManager
    {
        private static List<Scene> scenes;
        private static MyStack<Scene> scenesToUpdate;

        public static GraphicsDeviceManager Graphics { get; private set; }
        public static GraphicsDevice GraphicsDevice { get; private set; }

        public static bool ShouldExit { get { return scenes.Count == 0; } }
        public static int ScenesCount { get { return scenes.Count; } }

        public static void Initialize(EnemyFortress game)
        {
            Graphics = game.Graphics;
            GraphicsDevice = game.GraphicsDevice;

            scenes = new List<Scene>();
            scenesToUpdate = new MyStack<Scene>();
        }

        public static void Update(GameTime gameTime)
        {
            scenesToUpdate.Clear();

            foreach (Scene scene in scenes)
                scenesToUpdate.Push(scene);

            bool otherSceneHasFocus = false;
            bool coveredByOtherScene = false;

            // update all scenes in one update call
            while (!scenesToUpdate.IsEmpty)
            {
                Scene scene = scenesToUpdate.Pop();

                if (scene.State == SceneState.Active)
                {
                    scene.Update(gameTime, otherSceneHasFocus, coveredByOtherScene);

                    // let first scene handle input
                    if (!otherSceneHasFocus)
                    {
                        scene.HandleInput();
                        otherSceneHasFocus = true;
                    }

                    if (!scene.IsPopup)
                        coveredByOtherScene = true;
                }
            }
        }

        public static void Draw()
        {
            for (int i = 0; i < scenes.Count; ++i)
            {
                if (scenes[i].State == SceneState.Inactive)
                    continue;

                scenes[i].Draw();
            }
        }

        public static void AddScene(Scene scene)
        {
            scenes.Add(scene);
        }

        public static void RemoveScene(Scene scene)
        {
            scenes.Remove(scene);
        }
    }
}
