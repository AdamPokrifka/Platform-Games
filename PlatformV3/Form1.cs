using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlatformV3
{
    public partial class Form1 : Form
    {

        //game loop
        private Timer gameTimer = new Timer();

        //player
        private int playerX = 100;
        private int playerY = 300;
        private int playerW = 40;
        private int playerH = 40;

        private int playerSpd = 5;





        //moving platforms
        private List<int> HplatformSpd = new List<int>();
        private List<int> VplatformSpd = new List<int>();

        //clear
        private int clear = 0;

        //physics - gravity, jump force, velocity vertical
        private int gravity = 1;
        private int force = -15;
        private int velocity = 0;


        //bool
        private bool moveLeft = false;
        private bool moveRight = false;
        private bool moveUp = false; //jumping

        //level objects
        private Rectangle ground;
        private List<Rectangle> Hplatform = new List<Rectangle>();
        private List<Rectangle> Vplatform = new List<Rectangle>();

        //coin
        private List<Rectangle> coins = new List<Rectangle>();

        //scoring system
        private int score = 0;

        //enemies
        private List<Rectangle> enemies = new List<Rectangle>();
        private List<int> enemiesSpd = new List<int>();


        //lives
        private int lives = 3;
        private int newlives = 0;

        //hud
        private Font hudFont = new Font("Arial", 16);

        //invincibility
        private bool invincible = false;
        private int invincibleTime = 0;
        private int invincibleDuration = 60;


        //timer
        //private Stopwatch levelTimer = new Stopwatch();
        //private long bestTime = long.MaxValue;

        private int timeFrames = 0;
        private double timeSeconds = 0;

        //private double bestTime = 1000;
        

        public Form1()
        {
            InitializeComponent();

            CreateLevel1();
            
            



            //game loop setup
            gameTimer.Interval = 16;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();


        }

        private void CreateLevel1()
        {
            ground = new Rectangle(0, 350, 800, 100);

            //platforms
            Hplatform.Clear();
            Hplatform.Add(new Rectangle(150, 280, 120, 20));
            Hplatform.Add(new Rectangle(330, 240, 120, 20));
            Hplatform.Add(new Rectangle(520, 200, 120, 20));
            Hplatform.Add(new Rectangle(630, 300, 100, 20));
            Hplatform.Add(new Rectangle(400, 160, 140, 20));

           

            //coin
            coins.Clear();
            coins.Add(new Rectangle(180, 250, 20, 20));
            coins.Add(new Rectangle(360, 210, 20, 20));
            coins.Add(new Rectangle(550, 170, 20, 20));
            coins.Add(new Rectangle(680, 270, 20, 20));
            coins.Add(new Rectangle(450, 130, 20, 20));


            //enemies
            enemies.Clear();
            enemiesSpd.Clear();

            //enemies on ground
            enemies.Add(new Rectangle(600, ground.Y - 35, 35, 35));
            enemiesSpd.Add(2);

            //enemies on platform
            enemies.Add(new Rectangle(170, 280 - 35, 35, 35));
            enemiesSpd.Add(2);

            HplatformSpd.Add(0);
            VplatformSpd.Add(0);

            timeFrames = 0;
            timeSeconds = 0;

        }


        private void GameLoop(object sender, EventArgs e)
        {
            UpdateGame();
            Invalidate();

        }

        private void UpdateGame()
        {
            timeSeconds = timeFrames / 60.0;
            timeFrames++;

            //Invincible countdown
            if (invincible == true)
            {

                invincibleTime--;
                if (invincibleTime <= 0)
                    invincible = false;
            }


            //horizontal movement
            if (moveLeft)
                playerX -= playerSpd;


            if (moveRight)
                playerX += playerSpd;

            //keep player on screen
            if (playerX < 0)
                playerX = 0;


            if (playerX + playerW > this.ClientSize.Width)
                playerX = this.ClientSize.Width - playerW;


            //moving platform horizontal
            for (int i = 0; i < Hplatform.Count; i++)
            {
                Rectangle e = Hplatform[0];
                e.X += HplatformSpd[0];

                if (e.X <= 0 || e.X + e.Height >= this.ClientSize.Height)
                    HplatformSpd[0] = -HplatformSpd[0];

                Hplatform[0] = e;

            }


            //moving platform vertical
            for (int i = 0; i < Vplatform.Count; i++)
            {
                Rectangle e = Vplatform[0];
                e.Y += VplatformSpd[0];

                if (e.Y <= 0 || e.Y + e.Height >= this.ground.Y)
                    VplatformSpd[0] = -VplatformSpd[0];

                Vplatform[0] = e;

            }

            //gravity and physics(vertical)
            velocity += gravity;
            playerY += velocity;


            Rectangle playerRec = new Rectangle(playerX, playerY, playerW, playerH);

            //collision with ground = velocity >= 0
            if (playerRec.IntersectsWith(ground) && velocity >= 0)
            {
                playerY = ground.Y - playerH;
                velocity = 0;
                moveUp = false;
                playerRec = new Rectangle(playerX, playerY, playerW, playerH);

            }

            foreach (Rectangle plat in Hplatform)
            {
                if (playerRec.IntersectsWith(plat) && velocity >= 0)
                {
                    playerY = plat.Y - playerH;
                    velocity = 0;
                    moveUp = false;
                    playerRec = new Rectangle(playerX, playerY, playerW, playerH);
                }
            }

            foreach (Rectangle plat in Vplatform)
            {
                if (playerRec.IntersectsWith(plat) && velocity >= 0)
                {
                    playerY = plat.Y - playerH;
                    velocity = 0;
                    moveUp = false;
                    playerRec = new Rectangle(playerX, playerY, playerW, playerH);
                }
            }
            
            //coin collection
            for (int i = coins.Count - 1; i >= 0; i--)
            {
                if (playerRec.IntersectsWith(coins[i]))
                {
                    coins.RemoveAt(i);
                    score = score + 10;

                }
            }

            //Enemies Movement
            for (int i = 0; i < enemies.Count; i++)
            {
                Rectangle e = enemies[i];
                e.X += enemiesSpd[i];

                if (e.X <= 0 || e.X + e.Width >= this.ClientSize.Width)
                    enemiesSpd[i] = -enemiesSpd[i];

                enemies[i] = e;

            }

            //player vs enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Rectangle e = enemies[i];
                if (playerRec.IntersectsWith(e))
                {
                    bool stomp = velocity > 0 && (playerY + playerH - velocity) <= e.Y;

                    if (stomp)
                    {
                        enemies.RemoveAt(i);
                        enemiesSpd.RemoveAt(i);

                        velocity = force / 2;

                        score = score + 50;
                    }

                    else 
                    
                    {
                        if (invincible == false)
                        {
                            lives--;
                            ResetPlayer();

                            invincible = true;
                            invincibleTime = invincibleDuration;

                            if (lives <= 0)
                                GameOver();

                            break;
                        }
                    }
                }
            }
        }

        private void ResetPlayer()
        {

            playerX = 100;
            playerY = 300;
            velocity = 0;
            moveLeft = false;
            moveRight = false;
            moveUp = false;

        }

        private void GameOver()
        {
            gameTimer.Stop();
            MessageBox.Show("Game Over!");

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.DarkOliveGreen, ground);

            foreach (Rectangle plat in Hplatform)
                g.FillRectangle(Brushes.Coral, plat);

            foreach (Rectangle plat in Vplatform)
                g.FillRectangle(Brushes.Coral, plat);

            foreach (Rectangle c in coins)
                g.FillEllipse(Brushes.Gold, c);

            foreach (Rectangle enemy in enemies)
                g.FillEllipse(Brushes.Chartreuse, enemy);

            g.FillRectangle(Brushes.Orange, playerX, playerY, playerW, playerH);

            g.DrawString("Score:" + score, hudFont, Brushes.Green, 10, 10); //30

            g.DrawString("Lives:" + lives, hudFont, Brushes.Green, 10, 30); //50

            g.DrawString("Time:" + timeSeconds.ToString("0.00"), hudFont, Brushes.Green, 10, 70);


            //invincible flashing
            if(invincible && (invincibleTime % 10 < 5))
            {
                g.FillRectangle(Brushes.AliceBlue,playerX, playerY, playerW, playerH);
            }
            else
            {
                g.FillRectangle(Brushes.Orange, playerX, playerY, playerW, playerH);
            }

            //finishing levels
            if (clear == 0)
            {

                if (coins.Count == 0 && enemies.Count == 0)
                {
                    ResetPlayer();
                    enemies.Clear();
                    coins.Clear();
                    Hplatform.Clear();
                    Vplatform.Clear();
                    HplatformSpd.Clear();
                    VplatformSpd.Clear();
                    CreateLevel2();

                    clear = 1;

                    newlives = lives;
                   
                }
            }

            if (clear == 1)
            {
                

                if (coins.Count == 0 && enemies.Count == 0)
                {

                    if (timeSeconds < Properties.Settings.Default.bestTime)
                    {
                        Properties.Settings.Default.bestTime = timeSeconds;
                        Properties.Settings.Default.Save();
                    }

                    ResetPlayer();
                    enemies.Clear();
                    coins.Clear();
                    Hplatform.Clear();

                    Vplatform.Clear();
                    gameTimer.Stop();
                    g.DrawString("Best Time: "+Properties.Settings.Default.bestTime.ToString("0.00"), hudFont, Brushes.Purple, 10, 100);
                    MessageBox.Show("Your Time: "+timeSeconds+"\nBest Time: "+Properties.Settings.Default.bestTime.ToString("0.00"));
                }
            }
        }

        private void KeyisDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
                moveLeft = true;
            if (e.KeyCode == Keys.D)
                moveRight = true;
            if (e.KeyCode == Keys.Space && !moveUp)
            {
                velocity = force;
                moveUp = true;
            }

            if (e.KeyCode == Keys.P)
            {
                PauseGame();
            }

            if (e.KeyCode == Keys.U)
            {
                UnpauseGame();
            }

            if (e.KeyCode == Keys.R)
            {
                
                
                if(clear == 0)
                {
                    RestartL1();
                }
                else
                {
                    RestartL2();
                }
              
            }
        }

        private void KeyisUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
                moveLeft = false;
            if (e.KeyCode == Keys.D)
                moveRight = false;

        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            gameTimer.Stop();
            MessageBox.Show("You Quit!");
        }

        void StartGame()
        {
            menuPanel.Visible = false;

            gameTimer.Start();

            UpdateGame();
            Invalidate();

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartGame();
            UpdateGame();
        }


        //level 2
        private void CreateLevel2()
        {

            ground = new Rectangle(0, 350, 800, 100);

            //platforms
            Hplatform.Clear();
            Hplatform.Add(new Rectangle(75, 100, 120, 20));
            Hplatform.Add(new Rectangle(700, 280, 120, 20));
            Hplatform.Add(new Rectangle(400, 300, 120, 20));
            Hplatform.Add(new Rectangle(600, 60, 140, 20));

            Vplatform.Add(new Rectangle(100, 160, 140, 20));

            //coin
            coins.Clear();
            coins.Add(new Rectangle(75, 60, 20, 20));
            coins.Add(new Rectangle(400, 70, 20, 20));
            coins.Add(new Rectangle(450, 275, 20, 20));
            coins.Add(new Rectangle(700, 30, 20, 20));
            coins.Add(new Rectangle(760, 320, 20, 20));


            //enemies
            enemies.Clear();
            enemiesSpd.Clear();

            //enemies on ground
            enemies.Add(new Rectangle(600, ground.Y - 35, 35, 35));
            enemiesSpd.Add(2);

            //enemies on platform 1
            enemies.Add(new Rectangle(100, 50 - 35, 35, 35));
            enemiesSpd.Add(2);

            HplatformSpd.Add(2);
            VplatformSpd.Add(2);

        }

        void PauseGame()
        {
            gameTimer.Stop();
        }

        void UnpauseGame()
        {
            gameTimer.Start();
        }

        void RestartL1()
        {
            Vplatform.Clear();
            VplatformSpd.Clear();
            Hplatform.Clear();
            HplatformSpd.Clear();
            StartGame();
            CreateLevel1();
            ResetPlayer();
            score = 0;
            lives = 3;
        }

        void RestartL2()
        {

            Vplatform.Clear();
            VplatformSpd.Clear();
            Hplatform.Clear();
            HplatformSpd.Clear();
            StartGame();
            CreateLevel2();
            ResetPlayer();
            score = 150;
            lives = newlives;
            

        }
    }
}

