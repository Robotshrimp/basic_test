using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System;

namespace GameJom
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static double ScreenSizeAdjustment = 1;
        public static Point calculationScreenSize = new Point(3840, 2160);
        public static Rectangle ScreenBounds;
        public static int GameState = 1;
        public static bool Paused = false;
        public static MouseState mouseState;
        int XMousePos;
        int YMousePos;
        public static Texture2D BasicTexture;
        Rectangle Player = new Rectangle(0, 0, 96, 96);
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            double num = (double)graphics.PreferredBackBufferWidth / 3840;
            if (num > (double)graphics.PreferredBackBufferHeight / 2160)
            {
                num = (double)graphics.PreferredBackBufferHeight / 2160;
            }

            // use this for calculations outside of drawing

            ScreenBounds = new Rectangle(
                (int)((graphics.PreferredBackBufferWidth - 3840 * num) / 2), 
                (int)((graphics.PreferredBackBufferHeight - 2160 * num) / 2), 
                (int)(3840 * num), (int)(2160 * num));
            ScreenSizeAdjustment = num;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            BasicTexture = Content.Load<Texture2D>("BasicShape");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (Keyboard.GetState().IsKeyDown(Keys.W))
                Player.Y -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                Player.Y += 10;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                Player.X -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                Player.X += 10;
            LinearAlgebruh.MatrixTransform(new float[,] { { 0, 1 }, { 1, 0 } }, new float[] { 1, 0 });
            base.Update(gameTime);


            base.Draw(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            if (GameState == 2)
            {
                GraphicsDevice.Clear(Color.Black);
            }

            AutomatedDraw MaiCamera = new AutomatedDraw(ScreenBounds, Player.Center, Color.Red, GameState == 2, Parallax.ParallaxZoom(40));
            MaiCamera.draw(new Rectangle(0, 0, 1000, 1000), BasicTexture);
            AutomatedDraw paralxDraw = new AutomatedDraw(ScreenBounds, new Point(Player.X + Player.Width / 2, Player.Y + Player.Height / 2), Color.White, GameState == 2, Parallax.ParallaxZoom(30));
            paralxDraw.draw(new Rectangle(-1000, 0, 1000, 1000), BasicTexture);
            paralxDraw.draw(new Rectangle(1000, 0, 1000, 1000), BasicTexture, Color.Purple);
            AutomatedDraw paralaxDraw = new AutomatedDraw(ScreenBounds, new Point(Player.X + Player.Width / 2, Player.Y + Player.Height / 2), Color.Lime, GameState == 2, Parallax.ParallaxZoom(20));
            paralaxDraw.draw(new Rectangle(0, 0, 1000, 1000), BasicTexture);
            AutomatedDraw MainCamera = new AutomatedDraw(ScreenBounds, new Point(Player.X + Player.Width / 2, Player.Y + Player.Height / 2),  Color.Yellow, GameState == 2, Parallax.ParallaxZoom(10));
            MainCamera.draw(new Rectangle(0,0, 1000, 1000), BasicTexture);
            MainCamera.draw(new Rectangle(-1500, -1500, 100, 100), BasicTexture);
            MainCamera.draw(Player, BasicTexture, Color.Blue);
            
            AutomatedDraw Base = new AutomatedDraw(ScreenBounds, Color.White);

            Button button = new Button(Base, GameState == 1);
            button.ButtonUpdate(new Rectangle(300 , 300, 1000, 300), BasicTexture);
            if (button.PressedLeft)
            {
                GameState = 2;
            }


            
            TextFont test = new TextFont(BasicTexture);
            test.printCharacter(new Rectangle(0, 0, 1000, 100), 'a', Color.White);


            PrintManager printTest = new PrintManager(test, 20, Color.White, new Point(60, 100), MainCamera);
            printTest.Print(test, "bobama 10 $%#@!^&*()", new Point(1000, 1000));






            spriteBatch.Begin();
            spriteBatch.Draw(BasicTexture, new Rectangle(mouseState.X, mouseState.Y, 30, 40), Color.White);
            spriteBatch.Draw(BasicTexture, new Rectangle(0, 0, calculationScreenSize.X, ScreenBounds.Top), Color.Black);
            spriteBatch.Draw(BasicTexture, new Rectangle(0, ScreenBounds.Bottom, calculationScreenSize.X, ScreenBounds.Top), Color.Black);
            //spriteBatch.Draw(BasicTexture, new Rectangle(2000, 2000, 50, 50), Color.Black);
            spriteBatch.End();
            /*
            _3D_Because_Why_Not._3D_Renderer _3DEngine = new _3D_Because_Why_Not._3D_Renderer(180);
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                _3DEngine.UpdateLocation(new Vector3(-1, 0, 0));
                //_3DEngine.UpdateDirection(new Vector2(0.02f, 0));
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                _3DEngine.UpdateLocation(new Vector3(1, 0, 0));
                //_3DEngine.UpdateDirection(new Vector2(-0.02f, 0));
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                _3DEngine.UpdateLocation(new Vector3(0, 0, -1));
                //_3DEngine.UpdateDirection(new Vector2(0, 0.02f));
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                _3DEngine.UpdateLocation(new Vector3(0, 0, 1));
                //_3DEngine.UpdateDirection(new Vector2(0, -0.02f));
            int depth = 10;
            int depth2 = 12;
            Vector3 corner1_1 = new Vector3(1, 1, depth);
            Vector3 corner1_2 = new Vector3(-1, 1, depth);
            Vector3 corner1_3 = new Vector3(-1, -1, depth);
            Vector3 corner1_4 = new Vector3(1, -1, depth);
            Vector3 corner2_1 = new Vector3(1, 1, depth2);
            Vector3 corner2_2 = new Vector3(-1, 1, depth2);
            Vector3 corner2_3 = new Vector3(-1, -1, depth2);
            Vector3 corner2_4 = new Vector3(1, -1, depth2);
            _3DEngine.renderLine(corner1_1, corner2_1, 3).DrawLine();
            _3DEngine.renderLine(corner1_2, corner2_2, 3).DrawLine();
            _3DEngine.renderLine(corner1_4, corner2_4, 3).DrawLine();
            //_3DEngine.renderLine(corner1_3, corner2_3, 3).DrawLine();

            _3DEngine.renderLine(corner1_1, corner1_2, 3).DrawLine();
            _3DEngine.renderLine(corner1_1, corner1_4, 3).DrawLine();
            _3DEngine.renderLine(corner1_3, corner1_2, 3).DrawLine();
            _3DEngine.renderLine(corner1_3, corner1_4, 3).DrawLine();

            _3DEngine.renderLine(corner2_1, corner2_2, 3).DrawLine();
            _3DEngine.renderLine(corner2_1, corner2_4, 3).DrawLine();
            _3DEngine.renderLine(corner2_3, corner2_2, 3).DrawLine();
            _3DEngine.renderLine(corner2_3, corner2_4, 3).DrawLine();
            AutomatedLine line = new AutomatedLine(new AutomatedDraw());
            line.DrawLine(_3DEngine.renderLine(corner1_3, corner2_3, 3));
            */
            //LineClass linedraw = new LineClass(new Vector(2000, 2000), new Vector(100, 100), 3);
            //linedraw.DrawLine();
            //Base.draw(new Rectangle(300, 300, 1000, 300), PlayerTexture);
            base.Draw(gameTime);
        }
    }
}
