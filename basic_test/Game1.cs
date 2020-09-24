using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace basic_test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        int[,] tileMap;
        
        int tileWidth = 96;
        int tileHeight = 96;
        bool is_crouching = false;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D Player;
        private Texture2D level;
        public Rectangle _player = new Rectangle(100, 100, 96, 48);
        private Rectangle level_rec = new Rectangle(0, 0, 0, 0);//(0, 300, 1024, 1024);
        private Rectangle[] collision_checker = 
        {
            new Rectangle(0, 0, 0, 0),
            new Rectangle(0, 0, 0, 0),
            new Rectangle(0, 0, 0, 0),
            new Rectangle(0, 0, 0, 0)
        };
        private Rectangle[] amount_colliding =
        {
            new Rectangle(),
            new Rectangle(),
            new Rectangle(),
            new Rectangle(),
        };
        private bool _isgrounded = false;
        private int _fallspeed = 0;
        private int _notrightspeed = -24;
        
        private double _timesincelastacc = 0;
        private double _timesincelastfric = 0;
        private double _timesincelastmove = 0;
        private double _timesincelastairrescheck = 0;
        private double _timesincelastfallacc = 0;
        private double _timetilljumpslowdown = 0;
        private double _timesincelastjumpslowdown = 0;
            //movement variables
        //horozontal
        private int speed = 6;
        private int friction = 3;
        private int speedcap = 18;
        private int midaircap = 12;
        //vertical
        private int fallcap1 = 36;
        private int fallcap2 = 48;
        private int drag = 1;
        private int gravspeed = 6;
        private int jumpspeed = 24;
        //timer variables
        //vertical
        private double accdelay = 0.1;
        private double fricdelay = 0;
        private double jumpslowdowndelay = 0.07;
        private double jumpvariation = 0.15;
        //vertical
        private double falldelay = 0.05;
        private double airresdelay = 0.1;
        //main movement
        private double movedelay = 0;
        public Game1()
        
        {
            
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 2160;
            graphics.PreferredBackBufferWidth = 3840;
            graphics.IsFullScreen = true;
            
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

            base.Initialize();
            _player.Width = 96;
            _player.Height = 192;
            tileMap = new int[,]
            {
                {0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,1,1,1,1},
                {1,1,1,1,1,1,1,1,1},
            };
            
        }
        #region squishy function
        static public void _Squish(int horozontalSquish, int verticalSquish, ref Rectangle _objectSquished)
        {

            _objectSquished.Width += horozontalSquish;
            _objectSquished.Height -= verticalSquish;
            _objectSquished.X -= horozontalSquish / 2;
            _objectSquished.Y += verticalSquish;
        }
        #endregion
        #region collision detector function
        static public void _collide(
            ref Rectangle colliding_object, 
            ref Rectangle player, 
            int tile_size, int[,] 
            mapOfTiles, bool isleft, 
            bool istop, int affected_varx, 
            int affected_vary)
        {
            int left_tile = colliding_object.Left / tile_size;
            int right_tile = colliding_object.Right / tile_size;
            int top_tile = colliding_object.Top / tile_size;
            int bottom_tile = colliding_object.Bottom / tile_size;
            int x_restricter = 0;
            int y_restricter = 0;
            bool isrelivant = false;
            bool isgoingleft = true;
            bool isgoingup = true;
            if (left_tile < 0)
            {
                left_tile = 0;
            }
            if (right_tile > mapOfTiles.GetLength(1))
            {
                right_tile = mapOfTiles.GetLength(1);
            }
            if (top_tile < 0)
            {
                top_tile = 0;
            }
            if (bottom_tile > mapOfTiles.GetLength(0))
            {
                bottom_tile = mapOfTiles.GetLength(0);
            }
            if (isleft == true)
            {
                if (istop == true
                    & (affected_vary < 0
                    | affected_varx > 0))
                {
                    isrelivant = true;
                }
                else if (affected_vary > 0)
                {
                    isrelivant = true;
                }
            }
            else
            {
                if (istop == true
                    & (affected_vary < 0
                    | affected_varx < 0))
                {
                    isrelivant = true;
                }
                else if (affected_vary > 0)
                {
                    isrelivant = true;
                }
            }
            if (isrelivant == true)
            {
                if (affected_varx > 0)
                {
                    if (affected_vary < 0)
                    {
                        isgoingup = false;
                    }
                }
                else
                {
                    isgoingleft = false;
                    if (affected_vary < 0)
                    {
                        isgoingup = false;
                    }
                }
            }
            if (isrelivant == true)
            { 
                for (int i = left_tile - 1; i <= right_tile; i++)
                {
                    for (int j = top_tile - 1; j <= bottom_tile; j++)
                    {
                        if (mapOfTiles[i, j] == 1)
                        {
                            if (isleft == true)
                            {
                                if (istop == true)
                                {
                                    if (isgoingleft == true)
                                    {
                                        if (isgoingup == true)
                                        {

                                        }
                                        else
                                        {
                                            
                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                                else
                                {
                                    if (isgoingleft == true)
                                    {
                                        if (isgoingup == true)
                                        {

                                        }
                                        else
                                        {

                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                            else
                            {

                                if (istop == true)
                                {
                                    if (isgoingleft == false)
                                    {
                                        if (isgoingup == true)
                                        {

                                        }
                                        else
                                        {

                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                                else
                                {
                                    if (isgoingleft == false)
                                    {
                                        if (isgoingup == true)
                                        {

                                        }
                                        else
                                        {

                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (isleft == true)
            {
                if (istop == true)
                {
                    
                }
                else
                {

                }
            }
            else
            {
                if (istop == true)
                {

                }
                else
                {

                }
            }
        }
        #endregion

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.play
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Player = Content.Load<Texture2D>("BasicShape");
            level = Content.Load<Texture2D>("level_test");
            // TODO: use this.Content to load your game content here
        }
        
        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() 
        {
            // TODO: Unload any non ContentManager content here
            // don't matter unless the game is biggggggggggggg
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //--------------------------------------------//
            //                                            //
            //                  MOVEMENT                  //
            //                                            //
            //--------------------------------------------//
            if (_timesincelastmove >= movedelay)
            {
                _player.X -= _notrightspeed;
                _player.Y += _fallspeed;
                _timesincelastmove = 0;
            }

            if (_player.Height > 192)
            {
                _player.Height -= 4;
                _player.Y += 4;
            }
            if (_player.Width < 96)
            {
                _player.Width += 4;
                _player.X -= 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S)
                & _isgrounded == true)
            {
                if (is_crouching == false)
                {
                    _player.X -= (120 - _player.Width)/2 ;
                    _player.Y += _player.Height - 96;
                }
                is_crouching = true;
                _player.Height = 96;
                _player.Width = 120;
            }
            else
            {
                if (_player.Height < 192)
                {
                    _player.Height += 8;
                    _player.Y -= 8;
                }
                if (_player.Width > 96)
                {
                    _player.Width -= 8;
                    _player.X += 4;
                }
                is_crouching = false;
            }

            #region COLLISION
            if (_player.Bottom > 2000)
            {
                _isgrounded = true;
                if (_fallspeed > 17)
                {
                    _Squish(_fallspeed, _fallspeed * 2, ref _player);
                }
                _fallspeed = 0;
            }
            else
            {
                _isgrounded = false;
            }
            if (_player.Intersects(level_rec))
            {
                _isgrounded = true;
                _fallspeed = 0;
            }
            /*
            int left_tile = _player.Left / tileHeight;
            int right_tile = _player.Right / tileHeight;
            int top_tile = _player.Top / tileHeight;
            int bottom_tile = _player.Bottom / tileHeight;

            if (left_tile < 0)
            {
                left_tile = 0;
            }
            if (right_tile > tileMap.GetLength(1))
            {
                right_tile = tileMap.GetLength(1);
            }
            if (top_tile < 0)
            {
                top_tile = 0;
            }
            if (bottom_tile > tileMap.GetLength(0))
            {
                bottom_tile = tileMap.GetLength(0);
            }

            bool any_collision = false;
            for (int i = left_tile - 1; i <= right_tile; i++)
            {
                for (int j = top_tile - 1; j <= bottom_tile; j++)
                {
                    if (tileMap[i,j] == 1)
                    {
                        any_collision = true;

                    }
                }
            }
            */
            #endregion
            #region HOROZONTAL MOVEMENT
            if (_timesincelastacc > accdelay)
            {
                if (_isgrounded == true
                & is_crouching == false)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.D)
                    & (_notrightspeed > -speedcap))
                    {
                        _notrightspeed -= speed;
                        _timesincelastacc = 0;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.A)
                    & (_notrightspeed < speedcap))
                    {
                        _notrightspeed += speed;
                        _timesincelastacc = 0;
                    }
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.D)
                    & (_notrightspeed > -midaircap))
                    {
                        _notrightspeed -= speed / 2;
                        _timesincelastacc = 0;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.A)
                    & (_notrightspeed < midaircap))
                    {
                        _notrightspeed += speed / 2;
                        _timesincelastacc = 0;
                    }
                }
            }
            #endregion
            #region JUMP
            if (_isgrounded == true
                & Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _fallspeed -= jumpspeed;
                _timetilljumpslowdown = 0;
                _player.Height += jumpspeed * 2;
                _player.Y -= jumpspeed * 2;
                _player.Width -= jumpspeed;
                _player.X += jumpspeed / 2;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    _notrightspeed -= 9;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    _notrightspeed += 9;
                }
            }
            #endregion
            #region GRAVITY
            if (_isgrounded == false 
                & _fallspeed >= -4 
                & _fallspeed < fallcap1
                & _timesincelastfallacc >= falldelay)
            {
                _fallspeed += gravspeed;
                _timesincelastfallacc = 0;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S)
                & _fallspeed >= -4
                & _fallspeed < fallcap2
                & _timesincelastfallacc >= falldelay
                & _isgrounded == false)
            {
                _fallspeed += gravspeed;
                _timesincelastfallacc = 0;
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.S) 
                & _fallspeed > fallcap1)
            {
                _fallspeed -= gravspeed;
            }
            if (_isgrounded == false)


            if (_fallspeed > fallcap2
                & _timesincelastairrescheck < airresdelay)
            {
                _fallspeed -= drag;
                _timesincelastairrescheck = 0;
            }
            if ((!Keyboard.GetState().IsKeyDown(Keys.Space)
                | _timetilljumpslowdown >= jumpvariation)
                & _fallspeed < 0
                & _timesincelastjumpslowdown >= jumpslowdowndelay)
            {
                _fallspeed += gravspeed;
                _timesincelastjumpslowdown = 0;
            }
#endregion
            #region FRICTION
            if (is_crouching == true)
            {
                _notrightspeed = 0;
            }
            if (_timesincelastfric >= fricdelay)
            {
                if((!Keyboard.GetState().IsKeyDown(Keys.A)
                & _notrightspeed > 0)
                | _notrightspeed > speedcap)
                {
                    _notrightspeed -= friction;
                    _timesincelastfric = 0;
                }
                if ((!Keyboard.GetState().IsKeyDown(Keys.D)
                & _notrightspeed < 0)
                | _notrightspeed < -speedcap)
                {
                    _notrightspeed += friction;
                    _timesincelastfric = 0;
                }
            }
            #endregion
            
            //--------------------------------------------//
            //                                            //
            //               JUMP PREVENTER               //
            //                                            //
            //--------------------------------------------//
            for (int x = 0;
                x < collision_checker.GetLength(0);
                x++)
            {
                collision_checker[x].X = _player.X;
                collision_checker[x].Y = _player.Y;
                collision_checker[x].Width = -_notrightspeed;
                collision_checker[x].Height = _fallspeed;
                if (x == 2 
                    || x == 3)
                {
                    collision_checker[x].X += _player.Width;
                }
                if (-_notrightspeed < 0)
                {
                    collision_checker[x].X -= _notrightspeed;
                    collision_checker[x].Width = _notrightspeed;
                }

                if (x == 1 
                    || x == 3)
                {
                    collision_checker[x].Y += _player.Height;
                }
                if (_fallspeed < 0)
                {
                    collision_checker[x].Y += _fallspeed;
                    collision_checker[x].Height = -_fallspeed;
                }
            }

            

            // TODO: Add your update logic here

            base.Update(gameTime);
            //--------------------------------------------//
            //                                            //
            //                   TIMERS                   //
            //                                            //
            //--------------------------------------------//
            _timesincelastacc += gameTime.ElapsedGameTime.TotalSeconds;
            _timesincelastfric += gameTime.ElapsedGameTime.TotalSeconds;
            _timesincelastmove += gameTime.ElapsedGameTime.TotalSeconds;
            _timesincelastfallacc += gameTime.ElapsedGameTime.TotalSeconds;
            _timesincelastairrescheck += gameTime.ElapsedGameTime.TotalSeconds;
            _timesincelastjumpslowdown += gameTime.ElapsedGameTime.TotalSeconds;
            if (_isgrounded == false)
            {
                _timetilljumpslowdown += gameTime.ElapsedGameTime.TotalSeconds;
            }
            
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();
            #region
            spriteBatch.Draw(Player, collision_checker[0], Color.Black);
            spriteBatch.Draw(Player, collision_checker[1], Color.Black);
            spriteBatch.Draw(Player, collision_checker[2], Color.Black);
            spriteBatch.Draw(Player, collision_checker[3], Color.Black);
            #endregion
            spriteBatch.Draw(Player, _player, Color.Black);
            //spriteBatch.Draw(Player, level_rec, Color.Black);
            for (int y = 0; y < tileMap.GetLength(0); y++)
            {
                for (int x = 0; x < tileMap.GetLength(1); x++)
                {

                    spriteBatch.Draw(
                        Player,
                        new Vector2(x * tileWidth, y * tileHeight),
                        Color.Black);
                }
            }

            spriteBatch.End();

            // TODO: Add your drawing code here
            graphics.IsFullScreen = true;
            base.Draw(gameTime);

        }
    }
}
