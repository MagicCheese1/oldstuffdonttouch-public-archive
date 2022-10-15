using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUMAGICS2.Sprites
{
    public class Sprite
    {
        protected Texture2D _texture;

        public Vector2 Position;
        public Vector2 Velocity;
        public Color Colour = Color.White;
        public float Speed;
        public float DSpeed;
        public Input Input;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width - 5, _texture.Height - 5);
            }
        }

        public Sprite(Texture2D texture)
        {
            DSpeed = Speed;
            _texture = texture;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Colour);
        }

        #region Collision
        protected bool IsTouchingLeft(Sprite sprite)
        {
            return this.Rectangle.Right + this.Velocity.X > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Left &&
              this.Rectangle.Bottom > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(Sprite sprite)
        {
            return this.Rectangle.Left + this.Velocity.X < sprite.Rectangle.Right &&
              this.Rectangle.Right > sprite.Rectangle.Right &&
              this.Rectangle.Bottom > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(Sprite sprite)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Top &&
              this.Rectangle.Right > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsTouchingBottom(Sprite sprite)
        {
            return this.Rectangle.Top + this.Velocity.Y < sprite.Rectangle.Bottom &&
              this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
              this.Rectangle.Right > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Right;
        }

        public bool IsColliding(Sprite sprite)
        {            
            if (IsTouchingBottom(sprite) || IsTouchingLeft(sprite) || IsTouchingTop(sprite) || IsTouchingRight(sprite))
                return true;
            else
                return false;
        }

        #endregion
    }
}



//protected bool IsColliding(Sprite sprite)
//{
//    if (IsTouchingBottom(sprite) || IsTouchingTop(sprite) || IsTouchingLeft(sprite) || IsTouchingRight(sprite))
//        return true;
//    else
//        return false;
//}
