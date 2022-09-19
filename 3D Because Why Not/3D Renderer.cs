using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameJom._3D_Because_Why_Not
{
    class _3D_Renderer
    {
        static GraphicsDeviceManager graphics = Game1.graphics;
        static Vector3 CameraLocation = new Vector3(0, 0, 0);
        static Vector2 CameraDirection = new Vector2(0, 0);



        //legacy code, not used
        int fovHorizontal;
        int fovVertical;
        int horizontalFovRequirement = 1;
        int verticalFovRequirement = 1; 
        static int RotationSizeHorizontal;
        //

         /* if these values ever diviates from 1: the person configuring the settings is clinically insane and should 
          * be put away in a maximum security mental hostpital or be put down to protect the rest of society, i have
          * made a severe and continious lapse in my judgement and i will not and do not expect god to be merciful*/

        public _3D_Renderer(int FovHorozontal = 100, int FovVertical = 0)
        {
            fovHorizontal = FovHorozontal;
            if (FovVertical <= 0)
            {
                fovVertical = (int)(FovHorozontal * (2160.0 / 3840.0));
            }


            // legacy code, not used
            horizontalFovRequirement = (int)(fovHorizontal / 360) + 1;
            verticalFovRequirement = (int)(fovHorizontal / 360) + 1;
            RotationSizeHorizontal = (int)(graphics.PreferredBackBufferWidth * (360.0 / fovHorizontal));
            //

        }

        

        public void UpdateLocation(Vector3 Movement)
        {
            CameraLocation += Movement;
        }
        public void UpdateDirection(Vector2 Rotation)
        {
            CameraDirection += Rotation;
            if (CameraDirection.X >= Math.PI)
            {
                CameraDirection.X -= (float)(2 * Math.PI);
            }
            else if(CameraDirection.X < -Math.PI)
            {
                CameraDirection.X += (float)(2 * Math.PI);
            }
            if (CameraDirection.Y >= Math.PI)
            {
                CameraDirection.Y -= (float)(2 * Math.PI);
            }
            else if (CameraDirection.Y < -Math.PI)
            {
                CameraDirection.Y += (float)(2 * Math.PI);
            }
        }
        private Point ScreenProjection(Vector2 Angle)
        {
            float radFOVX = ((float)fovHorizontal / 360) * (float)Math.PI;
            float radFOVY = ((float)fovVertical / 360) * (float)Math.PI;
            Point screenOffset = new Point(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2); // sets 0,0 to center of screen
            Point projectionLocation = new Point((int)(screenOffset.X * (Angle.X / radFOVX)), (int)(screenOffset.Y * (Angle.Y / radFOVY))); // gives angle coordinates an x,y coornet scaled with fov. screen offset is used because i'm a lazy bastard who is too lazy to get the value straight from the source, it has no computational value and probably weakens the code
            return new Microsoft.Xna.Framework.Point(projectionLocation.X + screenOffset.X, -projectionLocation.Y + screenOffset.Y);// screen offset is applied to center around middle of screen
        }
        public Vector2 CoordnetConvert(Vector3 coordnetLocation)
        {
            coordnetLocation -= CameraLocation;
            Vector2 angleLocation = TrigFun.Angle3(coordnetLocation) - CameraDirection;
            return angleLocation;
        }

        public LineClass renderLine(Vector3 start, Vector3 end, int thiccness = 1)
        {
            return new LineClass(ScreenProjection(CoordnetConvert(start)), ScreenProjection(CoordnetConvert(end)), thiccness);
        }
    }
}
