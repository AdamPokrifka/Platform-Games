using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlatformV2
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


        //physics - gravity, jump force, velocity vertical
        private int gravity = 1;
        private int force = -15;
        private int velocity = 0;


        //bool
        private bool moveLeft = false;
        private bool moveRight = false;
        private bool moveUp = false; //jumping bardia belabardi the bronks are you jumping?

        //level objects
        private Rectangle ground;
        private List<Rectangle> platform = new List<Rectangle>();

        //coin
        private List<Rectangle> coins = new List<Rectangle>();

        //scoring system
        private int score = 0;

        //enemies
        private List<Rectangle> enemies = new List<Rectangle>();
        private List<int> enemiesSpd = new List<int>();


        //lives
        private int lives = 3;

        //hud
        private Font hudFont = new Font("Arial", 16);


        public Form1()
        {
            InitializeComponent();

            CreateLevel();

            //game loop setup
            gameTimer.Interval = 16;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();


        }

        private void CreateLevel()
        {
            ground = new Rectangle(0, 350, 800, 100);

            //platforms
            platform.Clear();
            platform.Add(new Rectangle(150, 200, 120, 20));
            platform.Add(new Rectangle(330, 240, 120, 20));
            platform.Add(new Rectangle(520, 200, 120, 20));
            platform.Add(new Rectangle(650, 300, 100, 20));
            platform.Add(new Rectangle(400, 160, 140, 20));


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

            //enemies on platform 1
            enemies.Add(new Rectangle(170, 280 - 35, 35, 35));
            enemiesSpd.Add(2);

        }


        private void GameLoop(object sender, EventArgs e)
        {
            UpdateGame();
            Invalidate();

        }
        
        private void UpdateGame()
        {
            //horizontal movement
            if (moveLeft)
                playerX -= playerSpd;


            if (moveRight)
                playerX += playerSpd;

            //keep player on screen
            if(playerX < 0)
                playerX = 0;


            if (playerX + playerW > this.ClientSize.Width)
                playerX = this.ClientSize.Width - playerW;



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

            foreach(Rectangle plat in platform)
            {
                if
            }



        }



    }



}
