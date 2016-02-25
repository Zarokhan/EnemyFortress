using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EnemyFortress.SceneSystem.Base;
using EnemyFortress.Utilities;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace EnemyFortress.MenuSystem.Base
{
    /// <summary>
    /// Base for all menuScenes
    /// Draws and updates the all menuentries
    /// Handles input such as moving the selection up or down or clicking on a menuentry
    /// </summary>
    public abstract class MenuScene : Scene
    {
        protected int selectedEntry;
        protected List<MenuEntry> entries;
        private string title;

        public MenuScene(string title)
        {
            this.title = title;
            entries = new List<MenuEntry>();
        }

        public override void HandleInput()
        {
            if (Input.ClickedKey(Keys.Down))
                selectedEntry = (selectedEntry >= entries.Count - 1) ? 0 : selectedEntry + 1;

            if (Input.ClickedKey(Keys.Up))
                selectedEntry = (selectedEntry == 0) ? entries.Count - 1 : selectedEntry - 1;

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].hitbox.Contains(Input.GetMousePosition()))
                {
                    selectedEntry = i;
                    if (Input.LeftButtonClicked())
                        entries[i].OnSelect();
                }
            }

            if (Input.ClickedKey(Keys.Enter))
                entries[selectedEntry].OnSelect();
        }

        public override void Update(GameTime gameTime, bool otherSceneHasFocus, bool coveredByOtherScene)
        {
            base.Update(gameTime, otherSceneHasFocus, coveredByOtherScene);

            for (int i = 0; i < entries.Count; ++i)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                entries[i].Update(isSelected, gameTime);
            }
        }

        public override void Draw()
        {
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.Transform);

            for (int i = 0; i < entries.Count; ++i)
                entries[i].Draw(batch);

            batch.End();
        }

        // Adds new text entry to the menu, below the previous entry
        // If first entry, it is placed in the middle of the screen
        protected void AddEntry(string entryText, EventHandler<EventArgs> Event)
        {
            Vector2 entryPos = new Vector2(camera.viewPort.Width / 2 - (AssetManager.MenuFont.MeasureString(entryText).X / 2),
                                                           camera.viewPort.Height / 2 + (AssetManager.MenuFont.MeasureString(entryText).Y * entries.Count));
            MenuEntry entry = new MenuEntry(entryText, entryPos);
            entry.Selected += Event;
            entries.Add(entry);
        }
    }
}
