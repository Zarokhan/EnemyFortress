using EnemyFortress.Forms;
using EnemyFortress.MenuSystem.Base;
using EnemyFortress.Scenes;
using EnemyFortress.SceneSystem.Base;
using System;

namespace EnemyFortress.MenuSystem.Menus
{
    class MainMenu : MenuScene
    {
        public MainMenu()
            : base("Main Menu")
        {
            AddEntry("Play Game", OnPlayClicked);
            AddEntry("Exit", OnExitClicked);
        }

        void OnPlayClicked(object o, EventArgs e)
        {
            SceneManager.RemoveScene(this);
            new ConnectForm().ShowDialog();
            AddScene(new GameScene());
        }

        void OnExitClicked(object o, EventArgs e)
        {
            SceneManager.RemoveScene(this);
        }
    }
}
