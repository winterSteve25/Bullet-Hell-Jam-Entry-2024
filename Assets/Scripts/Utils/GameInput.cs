using UnityEngine;

namespace Utils
{
    public static class GameInput
    {
        public static bool LeftClickButton()
        {
            return Input.GetMouseButton(0) && CanInput();
        }

        public static bool RightClickButton()
        {
            return Input.GetMouseButton(1) && CanInput();
        }

        public static bool LeftClickButtonDown()
        {
            return Input.GetMouseButtonDown(0) && CanInput();
        }

        public static bool RightClickButtonDown()
        {
            return Input.GetMouseButtonDown(1) && CanInput();
        }

        public static bool KeyboardKey(KeyCode keyCode)
        {
            return Input.GetKey(keyCode) && CanInput();
        }

        public static bool KeyboardKeyDown(KeyCode keyCode)
        {
            return Input.GetKeyDown(keyCode) && CanInput();
        }

        public static float GetAxis(string axis)
        {
            return CanInput() ? Input.GetAxis(axis) : 0;
        }

        private static bool CanInput()
        {
            // implement later to block input in menus and such
            return true;
        }
    }
}