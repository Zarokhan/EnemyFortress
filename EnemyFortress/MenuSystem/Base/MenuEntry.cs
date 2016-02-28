using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EnemyFortress.Utilities;

namespace EnemyFortress.MenuSystem.Base
{
    /// <summary>
    /// Every menu has MenuEntrys
    /// they are displayed as simple text which the user can either select 
    /// by moving the selection up or down and press enter
    /// or my hovering the mouse over the text and leftclick
    /// </summary>
    public class MenuEntry
    {
        public float Width { get { return AssetManager.Font.MeasureString(title).X; } }
        public float Height { get { return AssetManager.Font.MeasureString(title).Y; } }
        public Color SelectedColor { get; set; }
        public Color DeselectedColor { get; set; }

        public Rectangle hitbox { get; private set; }
        private Vector2 position;
        private bool isSelected;
        private string title;
        private Color color;

        // the event triggered when clicked / pressed enter
        public event EventHandler<EventArgs> Selected;

        public void OnSelect()
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }

        public MenuEntry(string title, Vector2 position)
        {
            this.title = title;
            this.position = position;
            SelectedColor = Color.White;
            DeselectedColor = Color.Gray;

            hitbox = new Rectangle((int)position.X, (int)position.Y, (int)Width, (int)Height);
        }

        public void Update(bool isSelected, GameTime gameTime)
        {
            this.isSelected = isSelected;
        }

        public void Draw(SpriteBatch batch)
        {
            color = (isSelected) ? SelectedColor : DeselectedColor;

            batch.DrawString(AssetManager.Font, title, position, color);
        }

        public bool Clicked()
        {
            return hitbox.Contains(Input.GetMousePoint()) && Input.LeftButtonClicked(); 
        }
    }
}