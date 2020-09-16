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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D Player;
        private Texture2D level;
        private Rectangle _playerposition = new Rectangle(100, 100, 96, 48);
        private Rectangle level_rec = new Rectangle(0, 300, 1024, 1024);
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
        private int _notrightspeed = -20;
        
        private double _timesincelastacc = 0;
        private double _timesincelastfric = 0;
        private double _timesincelastmove = 0;
        private double _timesincelastairrescheck = 0;
        private double _timesincelastfallacc = 0;
        private double _timetilljumpslowdown = 0;
        private double _timesincelastjumpslowdown = 0;
            //movement variables
        //horozontal
        private int speed = 4;
        private int friction = 2;
        private int speedcap = 12;
        private int midaircap = 8;
        //vertical
        private int fallcap1 = 12;
        private int fallcap2 = 24;
        private int drag = 1;
        private int gravspeed = 6;
        private int jumpspeed = 12;
        //timer variables
        //vertical
        private double accdelay = 0.1;
        private double fricdelay = 0;
        private double jumpslowdowndelay = 0.1;
        private double jumpvariation = 0.25;
        //vertical
        private double falldelay = 0.1;
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
            _playerposition.Width = 96;
            _playerposition.Height = 192;
            tileMap = new int[,]
            {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {1,1,1,1,1},
            };
            
        }
            
            public void Draw(SpriteBatch spriteBatch)
            {
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    for (int x = 0; x < tileMap.GetLength(1); x++)
                    {

                        spriteBatch.Draw(
                            Player,
                            new Vector2(x * tileWidth, y * tileHeight),
                            Color.White);
                    }
                }
            }
            
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
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
                _playerposition.X -= _notrightspeed;
                _playerposition.Y += _fallspeed;
                _timesincelastmove = 0;
            }
            
            #region COLLISION
            if (_playerposition.Bottom > 288)
            {
                _isgrounded = true;
                _fallspeed = 0;
            }
            else
            {
                _isgrounded = false;
            }
            if (_playerposition.Intersects(level_rec))
            {
                _isgrounded = true;
                _fallspeed = 0;
            }
            if (collision_checker[0].Intersects(level_rec))
            {
                amount_colliding[0] = Rectangle.Intersect(collision_checker[0], level_rec);
            }
            if (collision_checker[1].Intersects(level_rec))
            {
                amount_colliding[1] = Rectangle.Intersect(collision_checker[1], level_rec);
            }
            if (collision_checker[2].Intersects(level_rec))
            {
                amount_colliding[2] = Rectangle.Intersect(collision_checker[2], level_rec);
            }
            if (collision_checker[3].Intersects(level_rec))
            {
                amount_colliding[3] = Rectangle.Intersect(collision_checker[3], level_rec);
            }


            #endregion
            #region HOROZONTAL MOVEMENT
            if ((Keyboard.GetState().IsKeyDown(Keys.A) 
                & (_notrightspeed < speedcap))
                & (_timesincelastacc > accdelay)
                & _isgrounded == true)
            {
                _notrightspeed += speed;
                _timesincelastacc = 0;
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.A)
                & (_notrightspeed < midaircap))
                & (_timesincelastacc > accdelay)
                & _isgrounded == false)
            {
                _notrightspeed += speed/2;
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.D) 
                & (_notrightspeed > -speedcap))
                & (_timesincelastacc > accdelay)
                & _isgrounded == true)
            {
                _notrightspeed -= speed;
                _timesincelastacc = 0;
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.D)
                & (_notrightspeed > -midaircap))
                & (_timesincelastacc > accdelay)
                & _isgrounded == false)
            {
                _notrightspeed -= speed/2;
                _timesincelastacc = 0;
            }
            #endregion
            #region JUMP
            if (_isgrounded == true
                & Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _fallspeed -= jumpspeed;
                _timetilljumpslowdown = 0;
            }
            if (_isgrounded == true
                & Keyboard.GetState().IsKeyDown(Keys.Space)
                & Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _notrightspeed -= 8;
            }
            if (_isgrounded == true
                & Keyboard.GetState().IsKeyDown(Keys.Space)
                & Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _notrightspeed += 8;
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
            if ((!Keyboard.GetState().IsKeyDown(Keys.A) 
                & (_timesincelastfric >= fricdelay)
                & (_notrightspeed > 0)) 
                | ((_notrightspeed > speedcap) 
                & (_timesincelastfric >= fricdelay)))
            {
                _notrightspeed -= friction;
                _timesincelastfric = 0;
            }
            if ((!Keyboard.GetState().IsKeyDown(Keys.D) 
                & (_timesincelastfric >= fricdelay)
                & (_notrightspeed < 0)) 
                | ((_notrightspeed < -speedcap)
                & (_timesincelastfric >= fricdelay)))
            {
                _notrightspeed += friction;
                _timesincelastfric = 0;
            }
            #endregion
            //--------------------------------------------//
            //                                            //
            //               JUMP PREVENTER               //
            //                                            //
            //--------------------------------------------//
            #region X
            if (-_notrightspeed >= 0)
            {
                collision_checker[0].X = _playerposition.X;
                collision_checker[1].X = _playerposition.X;
                collision_checker[2].X = _playerposition.X + _playerposition.Width;
                collision_checker[3].X = _playerposition.X + _playerposition.Width;
            }
            if (-_notrightspeed < 0)
            {
                collision_checker[0].X = _playerposition.X - _notrightspeed;
                collision_checker[1].X = _playerposition.X - _notrightspeed;
                collision_checker[2].X = _playerposition.X + _playerposition.Width - _notrightspeed;
                collision_checker[3].X = _playerposition.X + _playerposition.Width - _notrightspeed;
            }
            #endregion 
            #region Y
            if (_fallspeed >= 0)
            {
                collision_checker[0].Y = _playerposition.Y;
                collision_checker[2].Y = _playerposition.Y;
                collision_checker[1].Y = _playerposition.Y + _playerposition.Height;
                collision_checker[3].Y = _playerposition.Y + _playerposition.Height;
            }
            if (_fallspeed < 0)
            {
                collision_checker[0].Y = _playerposition.Y + _fallspeed;
                collision_checker[2].Y = _playerposition.Y + _fallspeed;
                collision_checker[1].Y = _playerposition.Y + _playerposition.Height + _fallspeed;
                collision_checker[3].Y = _playerposition.Y + _playerposition.Height + _fallspeed;
            }
            #endregion
            #region width
            if (-_notrightspeed >= 0)
            {
                collision_checker[0].Width = -_notrightspeed;
                collision_checker[1].Width = -_notrightspeed;
                collision_checker[2].Width = -_notrightspeed;
                collision_checker[3].Width = -_notrightspeed;
            }
            if (-_notrightspeed < 0)
            {
                collision_checker[0].Width = _notrightspeed;
                collision_checker[1].Width = _notrightspeed;
                collision_checker[2].Width = _notrightspeed;
                collision_checker[3].Width = _notrightspeed;
            }
            #endregion
            #region height
            if (_fallspeed >= 0)
            {
                collision_checker[0].Height = _fallspeed;
                collision_checker[1].Height = _fallspeed;
                collision_checker[2].Height = _fallspeed;
                collision_checker[3].Height = _fallspeed;
            }
            if (_fallspeed < 0)
            {
                collision_checker[0].Height = -_fallspeed;
                collision_checker[1].Height = -_fallspeed;
                collision_checker[2].Height = -_fallspeed;
                collision_checker[3].Height = -_fallspeed;
            }
            #endregion
            

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
            spriteBatch.Draw(Player, collision_checker[0], Color.Transparent);
            spriteBatch.Draw(Player, collision_checker[1], Color.Transparent);
            spriteBatch.Draw(Player, collision_checker[2], Color.Transparent);
            spriteBatch.Draw(Player, collision_checker[3], Color.Transparent);
            #endregion
            spriteBatch.Draw(Player, _playerposition, Color.Black);
            spriteBatch.Draw(Player, level_rec, Color.Black);


            spriteBatch.End();

            // TODO: Add your drawing code here
            graphics.IsFullScreen = true;
            base.Draw(gameTime);

        }
    }
}
