using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GameJom
{
    public class Button 
    {

        static Point correctScreenSize = Game1.calculationScreenSize;

        AutomatedDraw drawButton;

        public bool PressedLeft;
        public bool PressedRight;

        public bool Hovered;

        public bool Loaded;

        public Button(AutomatedDraw drawParameters, bool loaded = true)
        {
            this.drawButton = drawParameters;

            this.Loaded = loaded;
        }

        public void ButtonUpdate(Rectangle button, Texture2D texture)
        {
            if (Loaded)
            {
                PressedLeft = false;
                PressedRight = false;
                drawButton.draw(button, texture);
                button = drawButton.DisplayRectangle(button);

                if (Mouse.GetState().X < button.Right &&
                    Mouse.GetState().X > button.Left &&
                    Mouse.GetState().Y < button.Bottom &&
                    Mouse.GetState().Y > button.Top)
                {
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {

                        PressedLeft = true;
                    }
                    if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        PressedRight = true;
                    }
                }
                
            }
        }


    }
}
