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

        public static int WindowWidth = 1240;
        public static int WindowHeight = 640;

        static int time = 0;
        int wave = 0;
        int FramesThisPeroid = 0;
        int FPS = 0;
        TimeSpan ElapsedTime = TimeSpan.Zero;

        static int checkpoint = 0;

        Player thePlayer;
        Texture2D PlayerTexture;
        Texture2D EnemyTexture;
        Texture2D BulletTexture;
        Texture2D EnemyTexture1;

        Texture2D hudBox;
        Texture2D hudBoxSelected;

        static public Texture2D bgWaterFrontTexture;
        static public Texture2D bgWaterMiddleTexture;
        static public Texture2D bgWaterBackTexture;
        static public Texture2D sheildText;
        public static Texture2D explosionTex;
        static public Texture2D powerTex;

        List<Background> bgArray = new List<Background>();

        static List<PowerUpgrade> powerList = new List<PowerUpgrade>();

        public static List<BaseEnemy> enemyArray = new List<BaseEnemy>();

        public static BulletManager bManager;
        static public Random aRandom = new Random();

        int powerLevel = -1;

        private static Vector2 VectorZero = new Vector2(0, 0);
        MouseState mouse;
        SpriteFont DebugFont;
        public static SpriteFont Size8;

        //HUD
        float HUDDepth = 1f;
        Vector2 HUDStartLocation = new Vector2(450, 20);
        Vector2 HUDDrawNow = new Vector2(0, 0);
        int spacing = 75;
        Rectangle CurrentBoxLocation = new Rectangle(0, 0, 70, 16);

        public static List<Explosion> explosionList = new List<Explosion>();

        public static Vector2 vectorZero
        {
            get { return VectorZero; }
        }
            
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
            graphics.ToggleFullScreen();
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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            PlayerTexture = Content.Load<Texture2D>(@"sprPlayer");
            EnemyTexture = Content.Load<Texture2D>(@"sprEnemyBase");
            BulletTexture = Content.Load < Texture2D>(@"sprBullet");
            bgWaterFrontTexture = Content.Load<Texture2D>(@"bgWaterFront");
            bgWaterMiddleTexture = Content.Load<Texture2D>(@"bgWaterMiddle");
            bgWaterBackTexture = Content.Load<Texture2D>(@"bgWaterBack");
            hudBox = Content.Load<Texture2D>(@"sprHudBox");
            hudBoxSelected = Content.Load<Texture2D>(@"sprHudBoxSelected");
            powerTex = Content.Load<Texture2D>(@"sprPowerUp");
            sheildText = Content.Load<Texture2D>(@"sprSheild");
            DebugFont = Content.Load<SpriteFont>(@"debugFont");
            Size8 = Content.Load<SpriteFont>(@"FontSize8");
            explosionTex = Content.Load<Texture2D>(@"sprExplosion");
            EnemyTexture1 = Content.Load<Texture2D>(@"sprPlane1");

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

            mouse = Mouse.GetState();
            time++;
        
            thePlayer.Update(gameTime, enemyArray, powerList, bManager.bullets);

            for (int i = 0; i < enemyArray.Count; i++ )
            {

                enemyArray[i].Update(gameTime, thePlayer.GetCollosionRect);
            }

            createWaves(time);

            foreach(Explosion aExplosion in explosionList)
            {
                if (aExplosion.active == true)
                {
                    aExplosion.Update(gameTime);
                }
            }

            for (int i = 0; i < explosionList.Count; i++)
            {
                if (explosionList[i].active == false)
                {
                    explosionList.Remove(explosionList[i]);
                }
            }

            foreach (Background aBg in bgArray)
            {
                aBg.update(gameTime);
            }

            foreach (PowerUpgrade aUpgrade in powerList)
            {
                aUpgrade.Update(gameTime);
            }

            bManager.Update(gameTime, enemyArray);


            powerLevel = thePlayer.PowerupsCollected -1;

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

            if (powerList.Count > 0)
            {
                for (int i = powerList.Count - 1; i >= 0; i--)
                {
                    if (powerList[i].Collected == true)
                    {
                        powerList.RemoveAt(i);
                    }
                }
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

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //spriteBatch.DrawString(DebugFont, "Mouse X: " + mouse.X, new Vector2(mouse.X, mouse.Y), Color.White);
            //spriteBatch.DrawString(DebugFont, "Mouse Y: " + mouse.Y, new Vector2(mouse.X, mouse.Y + 30), Color.White);


            foreach (Explosion aExplosion in explosionList)
            {
                //if (aExplosion.active == true)
                //{
                    aExplosion.Draw(spriteBatch);
                //}
            }

            foreach (Background aBg in bgArray)
            {
                aBg.Draw(spriteBatch);
            }

            thePlayer.Draw(spriteBatch);

            foreach(BaseEnemy aEnemy in enemyArray)
            {
                aEnemy.Draw(spriteBatch);
            }

            foreach (PowerUpgrade aPower in powerList)
            {
                aPower.Draw(spriteBatch);
            }

            bManager.Draw(spriteBatch);

            DrawHud(thePlayer.PlayerShield, thePlayer.PlayerShots);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetupGameObjects(Texture2D playertexture, Texture2D bullettexture)
        {
            thePlayer = new Player(new Vector2(100, 320), playertexture);
            bManager = new BulletManager(bullettexture);

            bgArray.Add(new Background(new Vector2(0, 150), bgWaterBackTexture, 1,true,1));
            bgArray.Add(new Background(new Vector2(1600, 150), bgWaterBackTexture, 1,true,1));
            bgArray.Add(new Background(new Vector2(800, 150), bgWaterBackTexture, 1, true, 1));

            bgArray.Add(new Background(new Vector2(0, 150), bgWaterMiddleTexture, 2,false,0.95f));
            bgArray.Add(new Background(new Vector2(1595, 150), bgWaterMiddleTexture, 2, false, 0.95f));
            bgArray.Add(new Background(new Vector2(800, 150), bgWaterMiddleTexture, 2,false,0.95f));

            bgArray.Add(new Background(new Vector2(0, 150), bgWaterFrontTexture, 3,false,0.1f));
            bgArray.Add(new Background(new Vector2(1595, 150), bgWaterFrontTexture, 3,false,0.1f));
            bgArray.Add(new Background(new Vector2(800, 150), bgWaterFrontTexture, 3, false, 0.1f));


            /*enemyArray.Add(new BaseEnemy(new Vector2(800, 320), EnemyTexture, 3));
            enemyArray.Add(new BaseEnemy(new Vector2(900, 320), EnemyTexture, 3));
            enemyArray.Add(new BaseEnemy(new Vector2(1000, 320), EnemyTexture, 3));*/
        }

        public void createWaves(int time)
        {

            if (time == 10)
            {
                //enemyArray.Add(new DownShotEnemy(new Vector2(WindowWidth, 320), EnemyTexture, 1, 1));
                //enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth, 220), EnemyTexture, 1, 1));
                enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth, 220), EnemyTexture, 4, 1));
                enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 100, 220), EnemyTexture, 4, 1));
                enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 200, 220), EnemyTexture, 4, 1));
                enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 300, 220), EnemyTexture, 4, 1));
                enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 400, 220), EnemyTexture, 4, 1));
                enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+500, 220), EnemyTexture, 4, 1));


                //enemyArray.Add(new Boss(new Vector2(1000, WindowHeight / 2), PlayerTexture));
            }

           if (time == 400)
            {
                enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth, 420), EnemyTexture, 5, 2));
                enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+100, 420), EnemyTexture, 5, 2));
                enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+200, 420), EnemyTexture, 5, 2));
                enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+300, 420), EnemyTexture, 5, 2));
                enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+400, 420), EnemyTexture, 5, 2));
                enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+500, 420), EnemyTexture, 5, 2));
            }

           if (time == 800)
           {
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth, 500), EnemyTexture1, 0,2));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+500, 35), EnemyTexture1, 0,2));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth, 120), EnemyTexture, 3,2));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+100, 120), EnemyTexture, 3,2));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+200, 120), EnemyTexture, 3,2));
           }

           if (time == 1100)
           {
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth, 400), EnemyTexture1, 1));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+500, 35), EnemyTexture1, 1));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth, 420), EnemyTexture, 6));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+100, 420), EnemyTexture, 6));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+200, 420), EnemyTexture, 6));
               checkpoint = 1100;
           }

           if (time == 1300)
           {
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth, 0), EnemyTexture, 7));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+100, 0), EnemyTexture, 7));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+200, 0), EnemyTexture, 7));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+300, 0), EnemyTexture, 7));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+400, 0), EnemyTexture, 7));
           }

           if (time == 1500)
           {
               enemyArray.Add(new DownShotEnemy(new Vector2(WindowWidth, 0), EnemyTexture, 8));
               enemyArray.Add(new DownShotEnemy(new Vector2(WindowWidth + 100, 0), EnemyTexture, 8));
               enemyArray.Add(new DownShotEnemy(new Vector2(WindowWidth + 200, 0), EnemyTexture, 8));
               enemyArray.Add(new DownShotEnemy(new Vector2(WindowWidth + 300, 0), EnemyTexture, 8));
               enemyArray.Add(new DownShotEnemy(new Vector2(WindowWidth + 400, 0), EnemyTexture, 8));
           }

           if (time == 1800)
           {
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth, 400), EnemyTexture, 2));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+100, 450), EnemyTexture, 3));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+200, 534), EnemyTexture, 4));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+300, 400), EnemyTexture, 5));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth+400, 350), EnemyTexture, 6));
           }

           if (time == 2000)
           {
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth, 428), EnemyTexture, 9));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 100, 428), EnemyTexture1, 9));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 200, 428), EnemyTexture1, 9));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 300, 428), EnemyTexture1, 9));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 400, 428), EnemyTexture1, 9));
           }

           if (time == 2300)
           {
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth, 0), EnemyTexture, 7,4));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 100, 0), EnemyTexture, 7,4));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 200, 0), EnemyTexture1, 7,4));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 300, 0), EnemyTexture, 7,4));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 400, 0), EnemyTexture1, 7,4));
               checkpoint = 2300;
           }

           if (time == 2500)
           {
               enemyArray.Add(new TripleShotEnemy(new Vector2(WindowWidth, 500), EnemyTexture1, 0));
               enemyArray.Add(new TripleShotEnemy(new Vector2(WindowWidth + 500, 35), EnemyTexture1, 0));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth, 120), EnemyTexture, 3, 3));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 100, 120), EnemyTexture, 3, 3));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 200, 120), EnemyTexture, 3, 3));
           }

           if (time == 2600)
           {
               enemyArray.Add(new TripleShotEnemy(new Vector2(WindowWidth, 100), EnemyTexture1, 0));
               enemyArray.Add(new TripleShotEnemy(new Vector2(WindowWidth + 500, 35), EnemyTexture1, 0));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth, 428), EnemyTexture, 9));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 100, 428), EnemyTexture1, 9));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 200, 428), EnemyTexture1, 9));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 300, 428), EnemyTexture1, 9));
               enemyArray.Add(new BaseEnemy(new Vector2(WindowWidth + 400, 428), EnemyTexture1, 9));
           }

           if (time == 2900)
           {
               float baseLocation = 300;
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth, baseLocation), EnemyTexture, 1));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth+150, baseLocation-50), EnemyTexture, 1));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 150, baseLocation +50), EnemyTexture, 1));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 300, baseLocation + 100), EnemyTexture, 1));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 300, baseLocation - 100), EnemyTexture, 1));
           }

           if (time == 3000)
           {
               float baseLocation = 100;
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth, baseLocation), EnemyTexture, 1));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 150, baseLocation - 50), EnemyTexture, 1));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 150, baseLocation + 50), EnemyTexture, 1));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 300, baseLocation + 100), EnemyTexture, 1));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 300, baseLocation - 100), EnemyTexture, 1));
           }

           if (time == 3100)
           {
               float baseLocation = 300;
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth, baseLocation), EnemyTexture, 1));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 150, baseLocation - 50), EnemyTexture, 1));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 150, baseLocation + 50), EnemyTexture, 1));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 300, baseLocation + 100), EnemyTexture, 1));
               enemyArray.Add(new SingleShotEnemy(new Vector2(WindowWidth + 300, baseLocation - 100), EnemyTexture, 1));
           }
        }

        public static void addPowerUp(Vector2 pos)
        {
            powerList.Add(new PowerUpgrade(pos, powerTex));
        }

        public static void CheckPoint()
        {
            enemyArray.Clear();
            bManager.bullets.Clear();
            time = checkpoint;
            explosionList.Clear();
        }

        public void DrawHud(int playershield, int numshots)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i == powerLevel)
                {
                    //spriteBatch.Draw(hudBoxSelected, new Vector2(250 + (i * 50 + 30), 20), Color.White);
                    CurrentBoxLocation.X = (int)HUDStartLocation.X + (i * spacing + 30);
                    CurrentBoxLocation.Y = (int)HUDStartLocation.Y;
                    spriteBatch.Draw(hudBoxSelected, CurrentBoxLocation, null, Color.White, 0f, Game1.vectorZero, SpriteEffects.None, HUDDepth);
                }
                else
                {
                    //spriteBatch.Draw(hudBox, new Vector2(250 + (i * 50 + 30), 20), Color.White);
                    CurrentBoxLocation.X =  (int)HUDStartLocation.X + (i * spacing + 30);
                    CurrentBoxLocation.Y = (int)HUDStartLocation.Y;
                    spriteBatch.Draw(hudBox, CurrentBoxLocation, null,  Color.White, 0f, Game1.vectorZero, SpriteEffects.None, HUDDepth);
                }

                HUDDrawNow.X = HUDStartLocation.X + (i * spacing + 35);
                HUDDrawNow.Y = HUDStartLocation.Y;

                switch (i)
                {
                    case 0: HUDDrawNow.X += 3; 
                        spriteBatch.DrawString(Size8, "Speed Up", HUDDrawNow, Color.Black, 0f, vectorZero, 1f, SpriteEffects.None, HUDDepth - 0.01f);
                        break;
                    case 1: HUDDrawNow.X -= 2;
                        spriteBatch.DrawString(Size8, "Shot Rate", HUDDrawNow, Color.Black, 0f, vectorZero, 1f, SpriteEffects.None, HUDDepth - 0.01f);
                        break;
                    case 2:
                        if (playershield < 3)
                        {
                            HUDDrawNow.X += 7;
                            spriteBatch.DrawString(Size8, "Shield", HUDDrawNow, Color.Black, 0f, vectorZero, 1f, SpriteEffects.None, HUDDepth - 0.01f);
                        }
                        break;
                    case 3:
                        if (numshots < 3)
                        {
                            HUDDrawNow.X += 8;
                            if (numshots == 1)
                            {
                                spriteBatch.DrawString(Size8, "Double", HUDDrawNow, Color.Black, 0f, vectorZero, 1f, SpriteEffects.None, HUDDepth - 0.01f);
                            }
                            else
                            {
                                spriteBatch.DrawString(Size8, "Tripple", HUDDrawNow, Color.Black, 0f, vectorZero, 1f, SpriteEffects.None, HUDDepth - 0.01f);
                            }
                        }
                        break;
                    case 4: HUDDrawNow.X += 20; 
                        spriteBatch.DrawString(Size8, "Meh!", HUDDrawNow, Color.Black, 0f, vectorZero, 1f, SpriteEffects.None, HUDDepth - 0.01f);
                        break;

                }
            }
        }
    }
}
