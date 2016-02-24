using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EnemyFortress.Utilities
{
    public static class Input
    {
        private static KeyboardState KS, oldKS;
        private static MouseState MS, oldMS;
        private static Point mousePoint;
        private static Vector2 mousePosition;
        private static bool isEnabled = true;

        public static void Update()
        {
            if (!isEnabled)
                return;

            oldKS = KS;
            oldMS = MS;

            mousePoint.X = Mouse.GetState().X;
            mousePoint.Y = Mouse.GetState().Y;

            mousePosition.X = Mouse.GetState().X;
            mousePosition.Y = Mouse.GetState().Y;

            MS = Mouse.GetState();
            KS = Keyboard.GetState();
        }

        public static bool LeftButtonClicked()
        {
            return MS.LeftButton == ButtonState.Pressed && oldMS.LeftButton == ButtonState.Released;
        }

        public static bool RightButtonClicked()
        {
            return MS.RightButton == ButtonState.Pressed && oldMS.RightButton == ButtonState.Released;
        }

        public static bool ClickedKey(Keys key)
        {
            return KS.IsKeyDown(key) && oldKS.IsKeyUp(key);
        }

        public static bool HoldingKey(Keys key)
        {
            return KS.IsKeyDown(key);
        }

        public static Point GetMousePoint()
        {
            return mousePoint;
        }

        public static Vector2 GetMousePosition()
        {
            return mousePosition;
        }

        public static bool HoldingLeftButton()
        {
            return MS.LeftButton == ButtonState.Pressed;
        }

        public static bool HoldingRightButton()
        {
            return MS.RightButton == ButtonState.Pressed;
        }

        public static void Enable()
        {
            isEnabled = true;
        }

        public static void Disable()
        {
            isEnabled = false;
        }
    }
}
