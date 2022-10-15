using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CUMAGICS2.Sprites
{
    public class Player : Sprite
    {
        public bool flip { get; private set; }
        public float Stamina { get; private set; } = 100;
        public float MaxStamina { get; } = 100;
        float angle = 0;
        readonly Texture2D _flipTexture;
        readonly GameWindow _window;

        public Player(Texture2D texture, Texture2D flipTexture,GameWindow window) : base(texture)
        {
            _flipTexture = flipTexture;
            _window = window;
        }

        public override void Update(GameTime gameTime)
        {
            //Sprint / Stamina
            if (Keyboard.GetState().IsKeyDown(Input.Sprint))
            {
                if(Stamina > 0)
                {
                    if ((Stamina - 0.5f) > 0)
                    {
                        Speed += 2f;
                        Stamina -= 0.5f;
                    }
                }
            }
            else
            {
                if(Stamina < 100f)
                    Stamina += 0.2f;
            }

            //Movement
            Move();

            //Screen border collison
            if (Position.X > _window.ClientBounds.Width - _texture.Width)
                Position.X = _window.ClientBounds.Width - _texture.Width;
            if (Position.Y > _window.ClientBounds.Height - _texture.Height)
                Position.Y = _window.ClientBounds.Height - _texture.Height;
            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;

            //Calculate new position using the Velocity
            Position += Velocity;
            
            //Set the Velocity to zero So the player stops when no key is being pressed
            Velocity = Vector2.Zero;
            Speed = DSpeed;
        }

        private void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Left))
            {
                Velocity.X = -Speed;
                angle = 0;
                flip = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
            {
                Velocity.X = Speed;
                angle = 0;
                flip = false;
            }
            if (Keyboard.GetState().IsKeyDown(Input.Up))
            {
                Velocity.Y = -Speed;
                angle = -(float)Math.PI / 2.0f;
                //flip = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
            {
                Velocity.Y = Speed;
                angle = (float)Math.PI / 2.0f;
               // flip = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Vector2 origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
            if (!flip) //If the Flip value is false draw normal player
                spriteBatch.Draw(_texture, Position, Color.White);
            else //If Flip value is true draw player Fliped
                spriteBatch.Draw(_flipTexture, Position, Color.White);
        }
    }
}
