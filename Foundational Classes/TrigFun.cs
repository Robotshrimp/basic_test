using System;
using Microsoft.Xna.Framework;

namespace GameJom
{
    class TrigFun
    {
        public static double pythag_hypotenus(Point Bases)
        {
            return Math.Sqrt(Bases.X * Bases.X + Bases.Y * Bases.Y);
        }
        public static float Angle2(Point Coordnet)
        {
            double length = pythag_hypotenus(Coordnet);
            if (length == 0)
            {
                return 0;
            }
            float rotation = (float)Math.Asin((double)Coordnet.Y / length);
            if (Coordnet.X < 0)
            {
                rotation = (float)((Math.PI) - rotation);
            }
            return rotation;
        }
        public static Vector2 Angle3(Vector3 Coordnet)
        {
            double XZAngle = Angle2(new Point((int)Coordnet.Z, (int)Coordnet.X));
            double hypotenus = pythag_hypotenus(new Point((int)Coordnet.Z, (int)Coordnet.X));
            return new Vector2((float)XZAngle, Angle2(new Point((int)hypotenus, (int)Coordnet.Y))/*this is only for calculating the angle on the z axis, values outside of 90 to -90 degree should be impossible because then the angle on the x-y plain would change instead*/);
        }
    }
}
