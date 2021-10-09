using System;
using Microsoft.Xna.Framework.Input;

namespace OneMoreStepToLoveYou
{
    class keyboard
    {
        static KeyboardState currentKeyState;
        static KeyboardState previousKeyState;
        public static KeyboardState GetState()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            return currentKeyState;
        }

        public static bool IsPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        public static bool HasBeenPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }

        internal static bool HasBeenPressed(object keys)
        {
            throw new NotImplementedException();
        }
    }
}
