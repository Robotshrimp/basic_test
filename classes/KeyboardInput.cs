using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJom
{
    class KeyBoardInput
    {
        public KeyBoardInput() { }
        public bool pressed;
        public bool Control(Keys input)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyUp(input))
            {
                pressed = false;
            }
            else if (pressed == false)
            {
                return true;
            }
            return false;
        }
    }
}
