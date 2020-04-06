using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace ParallaxStarter
{
    public class Player : ISprite
    {
        /// <summary>
        /// A spritesheet containing a helicopter image
        /// </summary>
        Texture2D sprite1;
        Texture2D sprite2;
        Texture2D sprite1Shoot;
        Texture2D sprite2Shoot;
        Texture2D spriteCurrent;
        Texture2D spriteBullet;
        int spriteChooser;

        /// <summary>
        /// The portion of the spritesheet that is the helicopter
        /// </summary>
        Rectangle sourceRect = new Rectangle
        {
            X = 0,
            Y = 0,
            Width = 64,
            Height = 64
        };

        /// <summary>
        /// The origin of the helicopter sprite
        /// </summary>
        Vector2 origin = new Vector2(32, 32);

        /// <summary>
        /// The angle the helicopter should tilt
        /// </summary>
        float angle = 0;

        /// <summary>
        /// The player's position in the world
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// How fast the player moves
        /// </summary>
        public float Speed { get; set; } = 200;

        /// <summary>
        /// Bullets fired by this player
        /// </summary>
        private List<Bullet> bullets = new List<Bullet>();

        // Delay Timer Initializations
        private int _animationDelay = 5;
        private int _animationTimer = 0;
        private int _shootDelay = 7;
        private int _shootTimer = 0;

        /// <summary>
        /// Constructs a player
        /// </summary>
        /// <param name="spritesheet">The player's spritesheet</param>
        public Player(Texture2D spritesheet, Texture2D sprite2, Texture2D spriteShoot, Texture2D spriteShoot2, Texture2D spriteBullet )
        {
            this.sprite1 = spritesheet;
            this.sprite2 = sprite2;
            this.sprite1Shoot = spriteShoot;
            this.sprite2Shoot = spriteShoot2;
            this.spriteBullet = spriteBullet;
            spriteChooser = 0;
            this.Position = new Vector2(200, 200);
        }

        /// <summary>
        /// Updates the player position based on GamePad or Keyboard input
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {
            Vector2 direction = Vector2.Zero;

            _animationTimer++;
            if ( _animationTimer > _animationDelay )
            {
                _animationTimer = 0;
                if( spriteChooser == 0 )
                {
                    spriteCurrent = sprite1;
                    spriteChooser = 1;
                }
                else if( spriteChooser == 1 )
                {
                    spriteCurrent = sprite2;
                    spriteChooser = 0;
                }
                else if( spriteChooser == 2 )
                {
                    spriteCurrent = sprite1Shoot;
                    spriteChooser = 3;
                }
                else if( spriteChooser == 3 )
                {
                    spriteCurrent = sprite2Shoot;
                    spriteChooser = 2;
                }
            }
            
            // Use GamePad for input
            var gamePad = GamePad.GetState(0);

            // The thumbstick value is a vector2 with X & Y between [-1f and 1f] and 0 if no GamePad is available
            direction.X = gamePad.ThumbSticks.Left.X;

            // We need to inverty the Y axis
            direction.Y = -gamePad.ThumbSticks.Left.Y;

            // Override with keyboard input
            var keyboard = Keyboard.GetState();
            if(keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
            {
                direction.X -= 1;
            }
            if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D)) 
            {
                direction.X += 1;
            }
            if(keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
            {
                direction.Y -= 1;
            }
            if(keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
            {
                direction.Y += 1;
            }
            if(keyboard.IsKeyDown(Keys.Z))
            {
                if( spriteChooser < 2 )
                {
                    spriteChooser = 2;
                }
                if( _shootTimer > _shootDelay )
                {
                    _shootTimer = 0;
                    bullets.Add( new Bullet( spriteBullet, this ) );
                }
                _shootTimer++;
            }
            else if ( spriteChooser >= 2 )
            {
                spriteChooser = 0;
            }

            // Caclulate the tilt of the player
            angle = 0.1f * direction.X;

            // Move the player
            Position += (float)gameTime.ElapsedGameTime.TotalSeconds * Speed * direction;

            // Update bullets
            foreach( Bullet b in bullets )
            {
                b.Update( gameTime );
            }

            for( int i = 0; i < bullets.Count; i++ )
            {
                if( bullets[ i ].Position.X > 1000 )
                {
                    bullets.RemoveAt( i );
                }
            }
        }

        /// <summary>
        /// Draws the player sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Render the player, rotating about the rotors
            spriteBatch.Draw(spriteCurrent, Position, sourceRect, Color.White, angle, origin, 1f, SpriteEffects.None, 0.7f);

            foreach( Bullet b in bullets )
            {
                b.Draw( spriteBatch, gameTime );
            }
        }
    }
}
