using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace basic_test
{
    class movement
    {
        static void f_horozontalMovement(ref int x, ref Rectangle player)
        {
            int speed = 8;
            int friction = 8;
            int speedcap = 16;

            if (Keyboard.GetState().IsKeyDown(Keys.D)
                & !Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (x > -speedcap)
                    x -= speed;
                else
                    x = -speedcap;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A)
                & !Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (x < speedcap)
                    x += speed;
                else
                    x = speedcap;
            }
            
            
        }
    }
}
