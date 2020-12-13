using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
public class animation:Game
{
    SpriteBatch spriteBatch;
    int _frames { get; set; }
    int _frame_rate { get; set; }
    int _height { get; set; }
    int _width { get; set; }
    Texture2D texture { get; set; }
    Vector2 position { get; set; }
    protected override void Update(GameTime gameTime)
    {
        int width = texture.Width / _frames;

        base.Update(gameTime);
    }
}