using EnemyFortress.SceneSystem.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EnemyFortress.Scenes
{
    class GameScene : Scene
    {
        public GameScene()
        {

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
