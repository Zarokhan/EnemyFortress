using EnemyFortress.Forms;
using EnemyFortress.MenuSystem.Base;
using EnemyFortress.Networking;
using EnemyFortress.Scenes;
using EnemyFortress.SceneSystem.Base;
using System;
using System.Windows.Forms;

namespace EnemyFortress.MenuSystem.Menus
{
    /// <summary>
    /// Main menu scene
    /// </summary>
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
            ConnectForm form = new ConnectForm();
            DialogResult result = form.ShowDialog();

            if (result == DialogResult.Cancel)
                return;

            Client client = new Client(form);

            AddScene(new GameScene(client));
        }

        void OnExitClicked(object o, EventArgs e)
        {
            SceneManager.RemoveScene(this);
        }
    }
}
