using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUMAGICS2
{
    class Animation
    {
        public int Duration { get; set; }
        public int CurrentFrame { get; set; } = 0;
        public Texture2D texture { get; private set; }
        public Texture2D texturef { get; private set; }
        public Vector2 Position { get; set; }
        public Animation(Texture2D _texture, Texture2D _texturef, int _duration)
        {
            texture = _texture;
            texturef = _texturef;
            Duration = _duration;
        }

        public void Draw(SpriteBatch spriteBatch,Vector2 Position)
        {
            spriteBatch.Draw(texture, Position, Color.White);
            CurrentFrame++;
        }

        public void DrawFlip(SpriteBatch spriteBatch, Vector2 Position)
        {
            spriteBatch.Draw(texturef, Position, Color.White);
            CurrentFrame++;
        }
    }
}
