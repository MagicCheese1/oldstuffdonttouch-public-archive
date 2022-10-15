using CUMAGICS2.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CUMAGICS2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player Player;
        Food Tomato;
        Texture2D foodTexture;
        SpriteFont Roboto;
        int Points;
        Animation anim;
        bool eating = false;
        
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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var playerTexture = Content.Load<Texture2D>("coolees");
            var playerFlipTexture = Content.Load<Texture2D>("cooleesf");
            var playerEatTexture = Content.Load<Texture2D>("cooleese");
            var playerEatFlipTexture = Content.Load<Texture2D>("cooleesef");
            Player = new Player(playerTexture, playerFlipTexture, Window)
            {
                Input = new Input()
                {
                    Left = Keys.A,
                    Right = Keys.D,
                    Up = Keys.W,
                    Down = Keys.S,
                    Sprint = Keys.LeftShift,
                },
                Position = new Vector2(100, 100),
                Speed = 3,
                DSpeed = 3
            };
            anim = new Animation(playerEatTexture, playerEatFlipTexture, 20);

            foodTexture = Content.Load<Texture2D>("food");
            Random random = new Random();
            Tomato = new Food(foodTexture)
            {
                Position = new Vector2(random.Next(random.Next(5,100), Window.ClientBounds.Width), random.Next(random.Next(5, 100), Window.ClientBounds.Height))
            };
            Roboto = Content.Load<SpriteFont>("Fonts/Roboto");
        }
        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
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
                Player.Update(gameTime);

            Random random = new Random();
            if (Player.IsColliding(Tomato))
            {
                eating = true;
                Draw(gameTime);
                Points++;
                Tomato = new Food(foodTexture)
                {
                    Position = new Vector2(random.Next(random.Next(5, 100), Window.ClientBounds.Width - 32), random.Next(random.Next(5, 100), Window.ClientBounds.Height - 32))
                };
                if (Player.IsColliding(Tomato))
                {
                    Tomato = new Food(foodTexture)
                    {
                        Position = new Vector2(random.Next(random.Next(5, 100), Window.ClientBounds.Width - 32), random.Next(random.Next(5, 100), Window.ClientBounds.Height - 32))
                    };
                }
            }

            //random.Next(5, Window.ClientBounds.Width);
            //random.Next(5, Window.ClientBounds.Height);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            Tomato.Draw(spriteBatch);
            if(!eating)
                Player.Draw(spriteBatch);
            else
            {
                if (anim.CurrentFrame <= anim.Duration)
                {
                    if(!Player.flip)
                        anim.Draw(spriteBatch, Player.Position);
                    else
                        anim.DrawFlip(spriteBatch, Player.Position);
                }
                else
                {
                    Player.Draw(spriteBatch);
                    anim.CurrentFrame = 0;
                    eating = false;
                }
            }
            var stamina = (int)Player.Stamina;
            spriteBatch.DrawString(Roboto, stamina.ToString(), new Vector2(25, 450-25), Color.Black);
            spriteBatch.DrawString(Roboto, Points.ToString(), new Vector2(25, 25), Color.Black);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
