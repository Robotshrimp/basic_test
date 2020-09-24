using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace basic_test
{
    class Squish:Game1
    {

        public Squish(int horozontalSquish, int verticalSquish, ref Rectangle _objectSquished)
        {

            _objectSquished.Width += horozontalSquish;
            _objectSquished.Height -= verticalSquish;
            _objectSquished.X -= horozontalSquish / 2;
            _objectSquished.Y += verticalSquish;
        }

    }
}
