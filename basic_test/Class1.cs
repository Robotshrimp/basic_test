using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace basic_test
{
    public class animation
    {
        SpriteBatch spriteBatch;
        public animation(Texture2D texture, int frame, int frame_height, int frame_width, int frame_rate, Vector2 posiotion)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, posiotion, new Rectangle(frame * frame_width, 0, frame_width, frame_height), Color.White);
            spriteBatch.End();
        }
    }
}
