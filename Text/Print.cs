using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJom
{
    class PrintManager
    {
        TextFont font;
        int spacing;
        Color color;
        Point fontSize;
        AutomatedDraw drawParam;
        public PrintManager(TextFont Font, int Spacing, Color Color, Point FontSize, AutomatedDraw DrawParam)
        {
            this.font = Font;
            this.spacing = Spacing;
            this.color = Color;
            this.fontSize = FontSize;
            this.drawParam = DrawParam;
        }
        public void Print(TextFont Font, string Text, Point PrintLocation)
        {
            for (int n = 0; n < Text.Length; n++)
            {
                
                font.printCharacter(
                    drawParam.DisplayRectangle(new Rectangle(PrintLocation.X + ((fontSize.X + spacing) * n), PrintLocation.Y, fontSize.X, fontSize.Y)),
                    Text[n],
                    color);
                
            }
        }
    }
}
