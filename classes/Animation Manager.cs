using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJom
{
    class AnimationManager 
    {

        Texture2D SpriteSheet;
        int TotalFrames;
        int CurrentFrame;
        Rectangle FrameSize;
        public AnimationManager(string spriteSheet, int totalFrames)
        {
            this.TotalFrames = totalFrames;
            
            //this.SpriteSheet = Content.Load<Texture2D>(spriteSheet);
        }
        public void LoopAnimation(int animation)
        {
        }
        public void TransitionalAnimation(int animation, int nextAnimation)
        {

        }










        // the output animation, this part updates frames and acts as replacemen for draw for animated objects
        
        // this part selects the animation set used

        public void Animate(AutomatedDraw drawConstructor)
        {
            drawConstructor.draw(FrameSize, SpriteSheet, new Rectangle(FrameSize.Right * CurrentFrame + 1, FrameSize.Top, FrameSize.Width, FrameSize.Height), Color.White);
        }
    }
}
