using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallaxStarter
{
    class Bullet
    {
        Texture2D texture;
        Vector2 origin = new Vector2(16, 16);
        Player sender;
        Rectangle sourceRect = new Rectangle
        {
            X = 0,
            Y = 0,
            Width = 32,
            Height = 32
        };

        public Vector2 Position { get; set; }

        public float Speed { get; set; } = 600;

        public Bullet( Texture2D t, Player p )
        {
            texture = t;
            this.Position = p.Position;
            this.Position += new Vector2( 25, 0 );
        }

        /// <summary>
        /// Draws the bullet sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Render the bullet
            spriteBatch.Draw(texture, Position, sourceRect, Color.White, 0, origin, 1f, SpriteEffects.None, 0.7f);
        }

        public void Update(GameTime gameTime)
        {
            // This subtraction controls the speed, higher number to increase speed
            Vector2 direction = Vector2.Zero;
            direction.X = 1;

            Position += (float)gameTime.ElapsedGameTime.TotalSeconds * Speed * direction;
        }
    }
}
