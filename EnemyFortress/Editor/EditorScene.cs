using EnemyFortress.SceneSystem.Base;
using EnemyFortress.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EnemyFortress.Editor
{
    class EditorScene : Scene
    {

        Vector2 Mouse { get; set; }
        GameObject current;

        int tiles_width;
        int tiles_height;

        public EditorScene() : base()
        {
            current = new GameObject(AssetManager.Tilesheet);
            current.sourceRect = new Rectangle(0, 0, 128, 128);
            current.origin = new Vector2(current.sourceRect.Width / 2, current.sourceRect.Height / 2);
            current.width = current.sourceRect.Width;
            current.height = current.sourceRect.Height;
        }

        public override void Update(GameTime gameTime, bool otherSceneHasFocus, bool coveredByOtherScene)
        {
            base.Update(gameTime, otherSceneHasFocus, coveredByOtherScene);
            current.position.X = Mouse.X;
            current.position.Y = Mouse.Y;
        }

        public override void HandleInput()
        {
            Mouse = camera.UnProject(Input.GetMousePosition());
        }

        public override void Draw()
        {
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.Transform);
            current.Draw(batch);
            batch.End();
        }
    }
}
