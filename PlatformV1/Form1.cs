using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlatformV1
{
    public partial class Form1 : Form
    {

        //variables for the player
        int playerX = 100;
        int playerY = 300;


        //player size
        int playerWidth = 40;
        int playerHeight = 40;

        int playerSpeed = 5;

        //vertical movement
        int jump = -15;
        int gravity = 1;
        int velocity = 0;

        //boolean variables
        bool moveLeft = false;
        bool moveRight = false;
        bool isJumping = false;

        //game object
        Rectangle ground;
        Rectangle platform1;
        Rectangle platform2;

        //game timer
        Timer gameTimer = new Timer();

        public Form1()
        {
            InitializeComponent();

            //create level
            ground = new Rectangle(0, 350, 800, 100);
            platform1 = new Rectangle(300, 280, 120, 20);
            platform2 = new Rectangle(500, 220, 120, 20);
            
            gameTimer.Interval = 16;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            //input event
            this.KeyDown += KeyisDown;
            this.KeyUp += KeyisUp;

        }


        private void KeyisDown (object sender, KeyEventArgs e)
        {
            //left
            if(e. KeyCode == Keys.A)
                moveLeft = true;

            //right
            if (e.KeyCode == Keys.D)
                moveRight = true;

            if(e.KeyCode == Keys.Space && !isJumping)
            {
                velocity = jump;
                isJumping = true;
            }


        }

        private void KeyisUp (object sender, KeyEventArgs e)
        {
            //left
            if(e. KeyCode == Keys.A)
                moveLeft = false;

            //right
            if (e. KeyCode == Keys.D)
                moveRight = false;
        }



        private void GameLoop(object sender, EventArgs e)
        {
            UpdatePlayer();
            Invalidate();
        }

        private void UpdatePlayer()
        {
            if(moveLeft)
                playerX -= playerSpeed;

            if(moveRight)
                playerX += playerSpeed;

            velocity += gravity;
            playerY += velocity;

            //player hitbox
            Rectangle playerRec = new Rectangle(playerX, playerY, playerWidth, playerHeight);

            //collision ground
            if (playerRec.IntersectsWith(ground) && velocity >= 0)
            {
                playerY = ground.Y - playerHeight;
                velocity = 0;
                isJumping = false;
            }

            if (playerRec.IntersectsWith(platform1) && velocity >= 0)
            {
                playerY = platform1.Y - playerHeight;
                velocity = 0;
                isJumping = false;
            }

            if (playerRec.IntersectsWith(platform2) && velocity >= 0)
            {
                playerY = platform2.Y - playerHeight;
                velocity = 0;
                isJumping = false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            //draw ground
            g.FillRectangle(Brushes.Orange, ground);
            //draw platforms
            g.FillRectangle(Brushes.Tomato, platform1);
            g.FillRectangle(Brushes.Yellow, platform2);

            //draw palyer
            g.FillRectangle(Brushes.Green, playerX, playerY, playerWidth, playerHeight);
        }

    }
    

}
