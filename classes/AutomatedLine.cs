using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameJom
{
    class AutomatedLine
    {

        static SpriteBatch spriteBatch = Game1.spriteBatch;
        AutomatedDraw drawParameter;
        public AutomatedLine(AutomatedDraw DrawParameters)
        {
            this.drawParameter = DrawParameters;
        }
        public LineClass AdjustedLine(LineClass Line)
        {
            return new LineClass(drawParameter.PointScale(Line.start), drawParameter.PointScale(Line.end), Line.thiccness);
        }
        public void DrawLine(LineClass Line)
        {
            LineClass line = AdjustedLine(Line);
            Point end = line.end;
            Point start = line.start;
            Point RelativePostition = new Point(end.X - start.X, end.Y - start.Y);
            int length = (int)TrigFun.pythag_hypotenus(RelativePostition);// pythagorean theorem hypotenus moment

            // angle finder, might be useful to put in another class
            float rotation = (float)Math.Asin((double)RelativePostition.Y / length);
            if (RelativePostition.X < 0)
            {
                rotation = (float)((Math.PI) - rotation);
            }
            // might be useful to have specific function for draw
            spriteBatch.Begin();
            spriteBatch.Draw(Game1.BasicTexture, null, new Rectangle(start.X, start.Y, length, line.thiccness), null, new Vector2(0, 0), rotation, null, Color.Black);
            spriteBatch.End();
        }
    }
}
