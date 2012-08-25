using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MiniJamAirPlanes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int WindowWidth = 800;
        public static int WindowHeight = 640;

        int time = 0;
        int wave = 0;
        int FramesThisPeroid = 0;
        int FPS = 0;
        TimeSpan ElapsedTime = TimeSpan.Zero;

        Player thePlayer;
        Texture2D PlayerTexture;
        Texture2D EnemyTexture;
        Texture2D BulletTexture;
        private List<BaseEnemy> enemyArray = new List<BaseEnemy>();

        public static BulletManager bManager;
        Random aRandom = new Random();
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
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            //graphics.ToggleFullScreen();
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            PlayerTexture = Content.Load<Texture2D>(@"sprPlayer");
            EnemyTexture = Content.Load<Texture2D>(@"sprEnemyBase");
            BulletTexture = Content.Load < Texture2D>(@"Bullet");

            SetupGameObjects(PlayerTexture, BulletTexture);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            time++;
        
            thePlayer.Update(gameTime);

            for (int i = 0; i < enemyArray.Count; i++ )
            {

                enemyArray[i].Update(gameTime, thePlayer.GetCollosionRect);
            }

            createWaves(time);
          
            bManager.Update(gameTime, enemyArray);


            //FPS stuff
            ElapsedTime += gameTime.ElapsedGameTime;
            if (ElapsedTime > TimeSpan.FromSeconds(0.11))
            {
                ElapsedTime -= TimeSpan.FromSeconds(1);
                FPS = FramesThisPeroid;
                Window.Title = FPS.ToString();
                FramesThisPeroid = 0;
            }

            for (int i = enemyArray.Count - 1; i >= 0; i--)
            {
                if (enemyArray[i].destroyed == true)
                    enemyArray.RemoveAt(i);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            FramesThisPeroid++;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            thePlayer.Draw(spriteBatch);

            foreach(BaseEnemy aEnemy in enemyArray)
            {
                aEnemy.Draw(spriteBatch);
            }

            bManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetupGameObjects(Texture2D playertexture, Texture2D bullettexture)
        {
            thePlayer = new Player(new Vector2(100, 320), playertexture);
            bManager = new BulletManager(bullettexture);

            /*enemyArray.Add(new BaseEnemy(new Vector2(800, 320), EnemyTexture, 3));
            enemyArray.Add(new BaseEnemy(new Vector2(900, 320), EnemyTexture, 3));
            enemyArray.Add(new BaseEnemy(new Vector2(1000, 320), EnemyTexture, 3));*/
        }

        public void createWaves(int time)
        {

            if (time == 10)
            {
                enemyArray.Add(new BaseEnemy(new Vector2(800, 220), EnemyTexture, 4));
                enemyArray.Add(new BaseEnemy(new Vector2(900, 220), EnemyTexture, 4));
                enemyArray.Add(new BaseEnemy(new Vector2(1000, 220), EnemyTexture, 4));
                enemyArray.Add(new BaseEnemy(new Vector2(1100, 220), EnemyTexture, 4));
                enemyArray.Add(new BaseEnemy(new Vector2(1200, 220), EnemyTexture, 4));
                enemyArray.Add(new BaseEnemy(new Vector2(1300, 220), EnemyTexture, 4));
            }

           if (time == 400)
            {
                enemyArray.Add(new BaseEnemy(new Vector2(800, 420), EnemyTexture, 5));
                enemyArray.Add(new BaseEnemy(new Vector2(900, 420), EnemyTexture, 5));
                enemyArray.Add(new BaseEnemy(new Vector2(1000, 420), EnemyTexture,5));
                enemyArray.Add(new BaseEnemy(new Vector2(1100, 420), EnemyTexture,5));
                enemyArray.Add(new BaseEnemy(new Vector2(1200, 420), EnemyTexture,5));
                enemyArray.Add(new BaseEnemy(new Vector2(1300, 420), EnemyTexture,5));
            }

           if (time == 800)
           {
               enemyArray.Add(new BaseEnemy(new Vector2(800, 500), EnemyTexture, 0));
               enemyArray.Add(new BaseEnemy(new Vector2(1300, 35), EnemyTexture, 0));
               enemyArray.Add(new BaseEnemy(new Vector2(800, 120), EnemyTexture, 3));
               enemyArray.Add(new BaseEnemy(new Vector2(900, 120), EnemyTexture, 3));
               enemyArray.Add(new BaseEnemy(new Vector2(1000, 120), EnemyTexture, 3));
           }

           if (time == 1100)
           {
               enemyArray.Add(new BaseEnemy(new Vector2(800, 400), EnemyTexture, 1));
               enemyArray.Add(new BaseEnemy(new Vector2(1300, 35), EnemyTexture, 1));
               enemyArray.Add(new BaseEnemy(new Vector2(800, 420), EnemyTexture, 6));
               enemyArray.Add(new BaseEnemy(new Vector2(900, 420), EnemyTexture, 6));
               enemyArray.Add(new BaseEnemy(new Vector2(1000, 420), EnemyTexture, 6));
           }
        }
    }
}
