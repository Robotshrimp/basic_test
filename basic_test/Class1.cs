using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


#region squishy function
public class squish : Game
{
    static public void _Squish(
        int horozontalSquish,
        int verticalSquish,
        ref Rectangle _objectSquished)
    {

        _objectSquished.Width += horozontalSquish;
        _objectSquished.Height -= verticalSquish;
        _objectSquished.X -= horozontalSquish / 2;
        _objectSquished.Y += verticalSquish;
    }
}
#endregion