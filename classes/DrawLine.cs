using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace GameJom
{
    class LineClass
    {
        static SpriteBatch spriteBatch = Game1.spriteBatch;
        public Point start { get; private set; }
        public Point end { get; private set; }
        public int thiccness;
        public LineClass(Point Start, Point End, int Thiccness = 1)
        {
            this.thiccness = Thiccness;
            this.start = Start;
            this.end = End;
        }
        public void setStart(Point newStart)
        {
            this.start = newStart;
        }
        public void setEnd(Point newEnd)
        {
            this.end = newEnd;
        }
        public void DrawLine()
        {
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
            spriteBatch.Draw(Game1.BasicTexture, null, new Rectangle(start.X, start.Y, length, thiccness), null, new Vector2(0, 0), rotation, null, Color.Black);
            spriteBatch.End();
        }
    }
}
