using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJom
{
    public class Parallax
    {
        const int BaseDepth = 10;
        public static double ParallaxZoom(int Depth)
        {
            return (double)BaseDepth / Depth;
        }
    }
}
