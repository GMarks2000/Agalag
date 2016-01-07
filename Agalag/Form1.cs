using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agalag
{
    public partial class Form1 : Form
    {       
        SolidBrush drawBrush = new SolidBrush(Color.White);
        Font gameFont = new Font("Courier New", 12);

        int shipX;//player x value
        int shipY = 400;//player y value
        bool playerOk = true;//tracks whether player is alive

        string fireMode = "spread"; //will track the players mode of shooting

        int bulletModulator = 10;//when equal to 10,  bullet will be ready to fire.

        long tracker = 0;//tracks the  number of timer repetitions passed.

        Random rand = new Random();

        double enemySpawnRate = 1; //controls the number of enemies to spawn at each interval      

        int[] starXValues = new int[200];
        double[] starYValues = new double[200];
        int[] starSizeValues = new int[200];

        List<int> bulletXValues = new List<int>(new int[] {});//list for bullet Xes
        List<int> bulletYValues = new List<int>(new int[] {});//list for bullet Ys
        List<string> bulletTypeValues = new List<string>(new string[] { });//list for bullet types

        List<int> enemyXValues = new List<int>(new int[] { });//list for enemy Xes
        List<int> enemyYValues = new List<int>(new int[] { });//list for enemy Ys
        List<string> enemyTypeValues = new List<string>(new string[] { });//list for enemy types
        List<int> enemyHealths = new List<int>(new int[] { });//list for enemy health bars
        List<int> enemyStartXes = new List<int>(new int[] { });//list for enemy start xes. useful for sine patterns in dynamic enemy types.                  
         
        List<int> enemyBulletXValues = new List<int>(new int[] { });//list for enemy bullet Xes
        List<int> enemyBulletYValues = new List<int>(new int[] { });//list for bullet Ys

        List<int> enemyDynamicBulletXValues = new List<int>(new int[] { });//list for dynamic type enemy bullet Xes
        List<int> enemyDynamicBulletYValues = new List<int>(new int[] { });//list for dynamic type bullet Ys
        List<int> enemyDynamicBulletXIncreases = new List<int>(new int[] { });//list for dynamic type enemy bullet Xes
        List<int> enemyDynamicBulletYIncreases = new List<int>(new int[] { });//list for dynamic type bullet Ys

        List<int> enemyHeavyBulletXValues = new List<int>(new int[] { });//list for heavy type enemy bullet Xes
        List<int> enemyHeavyBulletYValues = new List<int>(new int[] { });//list for heavy type bullet Ys

        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown, spaceDown;//track whether keys are held down

        public Form1()
        {
            InitializeComponent();
            gameTimer.Enabled = true;
            gameTimer.Start();

            shipX = this.Width / 2;//initialises player to midcsreen

            for (int i = 0; i < 199; i++)//randomises star locations and sizes
            {
                starXValues[i] = rand.Next(0, this.Width - 1);
                starYValues[i] = rand.Next(0, this.Height - 1);
                starSizeValues[i] = rand.Next(3, 6);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    upArrowDown = true;
                    break;
                case Keys.A:
                    leftArrowDown = true;
                    break;
                case Keys.S:
                    downArrowDown = true;
                    break;
                case Keys.D:
                    rightArrowDown = true;
                    break;               
                case Keys.Space:
                    spaceDown = true;
                    break;
                default:
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    leftArrowDown = false;
                    break;
                case Keys.S:
                    downArrowDown = false;
                    break;
                case Keys.D:
                    rightArrowDown = false;
                    break;
                case Keys.W:
                    upArrowDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
                default:
                    break;
            }
        }       

        //timer tick method
        private void timer1_Tick(object sender, EventArgs e)
        {
            //checks to see if any keys have been pressed and adjusts the X or Y value
            //for the rectangle appropriately
            if (spaceDown == true && bulletModulator == 10 && playerOk)//fires shots only if bulletModulator has reached 10
            {
                switch (fireMode)//determines which type of bullet to add based on current firing mode. This is changed via randomly appearing powerups.
                {
                    case "single": //fires a single shot. Start case.
                        bulletXValues.Add(shipX + 24);
                        bulletYValues.Add(shipY);
                        bulletTypeValues.Add("light");
                        bulletModulator = 0;
                        break;
                    case "double": //fires two parallel shots. Shots same as in single
                        bulletXValues.Add(shipX + 4);
                        bulletYValues.Add(shipY);
                        bulletTypeValues.Add("light");
                        bulletXValues.Add(shipX + 44);
                        bulletYValues.Add(shipY);
                        bulletTypeValues.Add("light");
                        bulletModulator = 0;
                        break;
                    case "spread"://fires three shots that spread out
                        bulletXValues.Add(shipX + 4);
                        bulletYValues.Add(shipY);
                        bulletTypeValues.Add("spread left");
                        bulletXValues.Add(shipX + 24);
                        bulletYValues.Add(shipY);
                        bulletTypeValues.Add("spread center");
                        bulletXValues.Add(shipX + 44);
                        bulletYValues.Add(shipY);
                        bulletTypeValues.Add("spread right");
                        bulletModulator = 0;
                        break;
                    case "heavy": //fires a single heavy shot.
                        bulletXValues.Add(shipX + 24);
                        bulletYValues.Add(shipY);
                        bulletTypeValues.Add("heavy");
                        bulletModulator = 0;
                        break;
                    default:
                        break;
                }
            }

            if (leftArrowDown == true && shipX > 5)
            {
                shipX-= 6;
            }
            if (rightArrowDown == true && shipX < this.Width - 70)
            {
                shipX += 6;
            }
            if (downArrowDown == true && shipY <this.Height - 88)
            {
               shipY+= 6;
            }

            if (upArrowDown == true && shipY > 5)
            {
                shipY-= 6;
            }
           
            
            if (bulletModulator < 10) { bulletModulator++; }//cases bullet modulator to incement if a shot is not ready. This will cause a shot to be fired every 100 ms

            for  (int i = 0; i < bulletXValues.Count(); i++)
            {
                //causes bullets to ascend
                if (bulletYValues[i] < 0)
                {
                    bulletXValues.Remove(bulletXValues[i]);
                    bulletYValues.Remove(bulletYValues[i]);
                    bulletTypeValues.Remove(bulletTypeValues[i]);
                }
                else
                {
                    switch (bulletTypeValues[i]) {//switch to determine how player bullets will move
                        case "light":
                            bulletYValues[i] -= 10;
                            break;
                        case "heavy":
                            bulletYValues[i] -= 5;
                            break;
                        case "spread left":
                            bulletYValues[i] -= 10;
                            bulletXValues[i] -= 5;
                            break;
                        case "spread center":
                            bulletYValues[i] -= 10;
                            break;
                        case "spread right":
                            bulletYValues[i] -= 10;
                            bulletXValues[i] += 5;
                            break;
                    }                   
                }
            }

            for (int i = 0; i < 199; i++)
            {
                starYValues[i] += 0.3;
                if (starYValues[i] > this.Height) { starYValues[i] = 0; }//causes stars to "snake" back up to top when they go offscreen.
            }

            for (int i = 0; i < enemyXValues.Count(); i++)
            {
                if (enemyYValues[i] > this.Height)//removes offscreen enemies
                {
                    enemyXValues.Remove(enemyXValues[i]);
                    enemyYValues.Remove(enemyYValues[i]);
                    enemyTypeValues.Remove(enemyTypeValues[i]);
                    enemyStartXes.Remove(enemyStartXes[i]);
                }
                else {
                    switch (enemyTypeValues[i]) 
                    {
                        case "light":
                            enemyYValues[i] += 2;//moves light enemies down
                            break;
                        case "dynamic":
                            enemyYValues[i] += 3;//moves dynamic enemies down
                            enemyXValues[i] = enemyStartXes[i] + Convert.ToInt16( 100*Math.Sin(0.01 *enemyYValues[i]));//causes dynamic enemies to sway in a sinusoidal wave
                            break;
                        case "heavy":
                            enemyYValues[i] += 1;//causes heavy enemies to slowly move down
                            break;
                    }
                }
            }
            for (int i = 0; i < enemyBulletXValues.Count(); i++)
            {
                if (enemyBulletYValues[i] < 0)
                {
                    enemyBulletXValues.Remove(enemyBulletXValues[i]);//removes offscreen enemy bullets
                    enemyBulletYValues.Remove(enemyBulletYValues[i]);
                }
                else
                {
                    enemyBulletYValues[i] += 7;//causes enemy bullets to descend
                    if (Math.Abs(enemyBulletXValues[i] - (shipX + 25)) < 25 && Math.Abs(enemyBulletYValues[i] - (shipY + 20)) < 20)
                    {
                        playerOk = false; //kills player on hit
                    }
                }
                
            }

            for (int i = 0; i < enemyDynamicBulletXValues.Count(); i++)//for loop for dynamic bullets. Seperate due to their differet behavior
            {
                if (enemyDynamicBulletYValues[i] < 0)
                {
                    enemyDynamicBulletXValues.Remove(enemyDynamicBulletXValues[i]);//removes offscreen enemy bullets
                    enemyDynamicBulletYValues.Remove(enemyDynamicBulletYValues[i]);
                    enemyDynamicBulletXIncreases.Remove(enemyDynamicBulletXIncreases[i]);
                    enemyDynamicBulletYIncreases.Remove(enemyDynamicBulletYIncreases[i]);
                }
                else
                {   
                    //moves enemy shots as determined by the player's position during their creation.
                    enemyDynamicBulletXValues[i] += enemyDynamicBulletXIncreases[i];
                    enemyDynamicBulletYValues[i] += enemyDynamicBulletYIncreases[i];
                    if (enemyDynamicBulletYIncreases[i] < 5) { enemyDynamicBulletYIncreases[i] = 5; }//sets minimum vertical movement
                }
                try
                {
                    if (Math.Abs(enemyDynamicBulletXValues[i] - (shipX + 25)) < 25 && Math.Abs(enemyDynamicBulletYValues[i] - (shipY + 20)) < 20)
                    {
                        playerOk = false; //kills player on hit
                    }
                }
                catch { }
            }

            if (tracker % 200 == 0) {
                //enemies spawn every 200 frames
                if (tracker <= 800)
                {
                    double screenDiv = this.Width / enemySpawnRate;//used to evenly distribute enemies across screen
                    for (int i = 0; i < enemySpawnRate; i += 1)
                    {
                        int startX = (i * Convert.ToInt16(screenDiv) + Convert.ToInt16(screenDiv) / 2 - 30);
                        enemyXValues.Add(startX);
                        enemyYValues.Add(-50 + (rand.Next(-50, 51)));//randomises Y to a degree
                        enemyTypeValues.Add("heavy");
                        enemyStartXes.Add(startX);
                    }
                    enemySpawnRate += 1;
                    if (enemySpawnRate == 5) { enemySpawnRate = 0; } //resets spawn rate once next bracket is reached
                }else if (tracker > 800 && tracker <= 1800)
                {
                    double screenDiv = this.Width / enemySpawnRate;//used to evenly distribute enemies across screen
                    for (int i = 0; i < enemySpawnRate; i += 1)
                    {
                        int startX = (i * Convert.ToInt16(screenDiv) + Convert.ToInt16(screenDiv) / 2 - 30);
                        enemyXValues.Add(startX);
                        enemyYValues.Add(-50 + (rand.Next(-50, 51)));//randomises Y to a degree
                        enemyTypeValues.Add("dynamic");
                        enemyStartXes.Add(startX);//used to allow a sine wave pattern.
                    }
                    enemySpawnRate += 1;
                }
            }

            if (tracker % 50 == 0)//light enemies fire every 50 frames
            {
                for (int i = 0; i < enemyXValues.Count(); i++)
                {
                    if (enemyTypeValues[i] == "light")
                    {
                        enemyBulletXValues.Add(enemyXValues[i] + 15);
                        enemyBulletYValues.Add(enemyYValues[i] + 55);
                    }
                }
            }
            if (tracker % 25 == 0)
            {
                for (int i = 0; i < enemyXValues.Count(); i++)
                {
                    if (enemyTypeValues[i] == "dynamic")
                    {
                        enemyDynamicBulletXValues.Add(enemyXValues[i] + 15);
                        enemyDynamicBulletYValues.Add(enemyYValues[i] + 55);
                        //sends shot towards the player
                        enemyDynamicBulletXIncreases.Add((shipX - enemyXValues[i]) / 50);
                        enemyDynamicBulletYIncreases.Add((shipY - enemyYValues[i]) / 50);
                    }
                }
            }

            //collisions detection for bullets and enemies
            for (int i = 0; i < bulletXValues.Count; i++)//nested for loops to check all bullets against all enemies
            {
                for (int j = 0; j < enemyXValues.Count(); j++)
                {
                    try {
                        if (enemyXValues.IndexOf(enemyXValues[j]) != -1 && bulletXValues.IndexOf(bulletXValues[i]) != -1)
                        {
                            switch (enemyTypeValues[j])//determines hitbox based on enemy type
                            {

                                case "light":
                                    if (Math.Sqrt(Math.Pow(bulletXValues[i] - enemyXValues[j], 2) + Math.Pow(bulletYValues[i] - enemyYValues[j], 2)) < 30)//uses distance formula to check for collision
                                    {
                                        removeBulletsAndEnemies(i, j);
                                    }
                                    break;
                                case "dynamic":
                                    if ((Math.Sqrt(Math.Pow(bulletXValues[i] - enemyXValues[j], 2) + Math.Pow(bulletYValues[i] - enemyYValues[j], 2)) < 60))//uses distance formula to check for collision
                                    {
                                        removeBulletsAndEnemies(i, j);
                                    }
                                   break;
                            }
                        }
                    }
                    catch { }
                }
            }
       
            
            tracker++;
            Refresh();          
        }

        //paint method
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawBrush.Color = Color.White;
            
            for (int i = 0; i < 199; i++)
            {
                float starY = Convert.ToInt16(starYValues[i]);
                e.Graphics.FillEllipse(drawBrush, starXValues[i], starY, starSizeValues[i], starSizeValues[i]);
            }


            if (playerOk)
            {
                Point[] triangle1Points = { new Point(shipX, shipY + 35), new Point(shipX + 5, shipY + 10), new Point(shipX + 10, shipY + 35) };//array for the points of triangle 1
                Point[] triangle2Points = { new Point(shipX + 20, shipY + 15), new Point(shipX + 25, shipY), new Point(shipX + 30, shipY + 15) };//array for the points of triangle 2
                Point[] triangle3Points = { new Point(shipX + 40, shipY + 35), new Point(shipX + 45, shipY + 10), new Point(shipX + 50, shipY + 35) };//array for the points of triangle 3
               

                e.Graphics.FillRectangle(drawBrush, shipX, shipY + 35, 50, 10);//draws ship base
                e.Graphics.FillRectangle(drawBrush, shipX + 20, shipY + 15, 10, 20);//draws ship spine

                drawBrush.Color = Color.DarkRed;

                e.Graphics.FillPolygon(drawBrush, triangle1Points);//draws left triangle
                e.Graphics.FillPolygon(drawBrush, triangle2Points);//draws central triangle
                e.Graphics.FillPolygon(drawBrush, triangle3Points);//draws right triangle

                e.Graphics.FillRectangle(drawBrush, shipX + 10, shipY + 38, 30, 4);//draws ship base detail
                e.Graphics.FillRectangle(drawBrush, shipX + 23, shipY + 20, 4, 20);//draws ship spine detail
            }

            drawBrush.Color = Color.Orange;

            for (int i = 0; i < bulletXValues.Count(); i++)
            { switch (bulletTypeValues[i]) {//switch statement to determine which bullet shape to draw
                    case "light":
                         e.Graphics.FillRectangle(drawBrush, bulletXValues[i], bulletYValues[i], 3, 10);//draws player shots
                         break;
                    case "spread left":
                        e.Graphics.FillEllipse(drawBrush, bulletXValues[i], bulletYValues[i], 3, 6);//draws player shots
                        break;
                    case "spread center":
                        e.Graphics.FillEllipse(drawBrush, bulletXValues[i], bulletYValues[i], 3, 6);//draws player shots
                        break;
                    case "spread right":
                        e.Graphics.FillEllipse(drawBrush, bulletXValues[i], bulletYValues[i], 3, 6);//draws player shots
                        break;
                    case "heavy":
                        e.Graphics.FillEllipse(drawBrush, bulletXValues[i], bulletYValues[i], 10, 10);//draws player shots
                        break;
                }
            }
            for (int i = 0; i < enemyXValues.Count(); i++)
            {   switch (enemyTypeValues[i])//determines which type of enemy to draw
                {
                    case "light"://draws a light enemy
                        Point[] shipBodyPoints = { new Point(enemyXValues[i], enemyYValues[i] + 15), new Point(enemyXValues[i] + 15, enemyYValues[i] + 55), new Point(enemyXValues[i] + 30, enemyYValues[i] + 15) };//array for the points of the ship's body

                        drawBrush.Color = Color.Gray;

                        e.Graphics.FillRectangle(drawBrush, enemyXValues[i], enemyYValues[i], 10, 15);//draws left thruster
                        e.Graphics.FillRectangle(drawBrush, enemyXValues[i] + 20, enemyYValues[i], 10, 15);//draws right thruster

                        drawBrush.Color = Color.Gold;

                        e.Graphics.FillPolygon(drawBrush, shipBodyPoints);//draws ship's body
                        drawBrush.Color = Color.DarkBlue;

                        e.Graphics.FillEllipse(drawBrush, enemyXValues[i] + 10, enemyYValues[i] + 20, 10, 15);
                        break;
                    case "dynamic"://draws dynamic enemy
                        drawBrush.Color = Color.Brown;
                        e.Graphics.FillEllipse(drawBrush, enemyXValues[i], enemyYValues[i], 5, 40);//draws left "wing"
                        e.Graphics.FillRectangle(drawBrush, enemyXValues[i] + 3, enemyYValues[i] + 17, 20, 6);//draws left connector
                        e.Graphics.FillEllipse(drawBrush, enemyXValues[i] + 20, enemyYValues[i], 20, 40);//draws ships body
                        e.Graphics.FillRectangle(drawBrush, enemyXValues[i] + 37, enemyYValues[i] + 17, 20, 6);//draws right connector
                        e.Graphics.FillEllipse(drawBrush, enemyXValues[i] + 55, enemyYValues[i], 5, 40);//draws right "wing"

                        drawBrush.Color = Color.Red;
                        e.Graphics.FillEllipse(drawBrush, enemyXValues[i] + 25, enemyYValues[i] + 12, 10, 16);

                        break;
                    case "heavy":
                        drawBrush.Color = Color.White;
                        Point[] heavyShipBodyPoints = { new Point(enemyXValues[i], enemyYValues[i]), new Point(enemyXValues[i] + 15, enemyYValues[i] + 40), new Point(enemyXValues[i] + 15, enemyYValues[i] + 80), new Point(enemyXValues[i] + 45, enemyYValues[i] + 80), new Point(enemyXValues[i] + 45, enemyYValues[i] + 40), new Point(enemyXValues[i] + 60, enemyYValues[i]) };
                        Point[] triangleDetailPoints = { new Point(enemyXValues[i] + 25, enemyYValues[i] + 50), new Point(enemyXValues[i] + 30, enemyYValues[i] + 80), new Point(enemyXValues[i] + 35, enemyYValues[i] + 50) };

                        e.Graphics.FillPolygon(drawBrush, heavyShipBodyPoints);//draws ships body

                        drawBrush.Color = Color.Gray;
                        e.Graphics.FillRectangle(drawBrush, enemyXValues[i] + 15, enemyYValues[i] + 80, 5, 10);//draws left cannon
                        e.Graphics.FillRectangle(drawBrush, enemyXValues[i] + 40, enemyYValues[i] + 80, 5, 10);//draws right cannon

                        e.Graphics.FillRectangle(drawBrush, enemyXValues[i], enemyYValues[i]-10, 10, 10);//draws left thruster
                        e.Graphics.FillRectangle(drawBrush, enemyXValues[i] + 50, enemyYValues[i] - 10, 10, 10);//draws right thruster

                        drawBrush.Color = Color.Blue;
                        e.Graphics.FillRectangle(drawBrush, enemyXValues[i] + 17, enemyYValues[i] + 40, 12, 7);//draws left window
                        e.Graphics.FillRectangle(drawBrush, enemyXValues[i] + 31, enemyYValues[i] + 40, 12, 7);//draws right window

                        drawBrush.Color = Color.Red;
                        e.Graphics.FillPolygon(drawBrush, triangleDetailPoints);//draws ship's central detail
                        
                        break;
                }
            }
            drawBrush.Color = Color.Red;
            for (int i = 0; i < enemyBulletXValues.Count(); i++)
            {
                e.Graphics.FillRectangle(drawBrush, enemyBulletXValues[i], enemyBulletYValues[i], 3, 10);//draws player shots
            }
            for (int i = 0; i < enemyDynamicBulletXValues.Count(); i++)
            {
                e.Graphics.FillRectangle(drawBrush, enemyDynamicBulletXValues[i], enemyDynamicBulletYValues[i], 3, 10);//draws player shots
            }
        }
        void removeBulletsAndEnemies(int i, int j)//this function removes enemies and bullets from their arrays when they die
        {
            //removes collided bullets
            bulletXValues.Remove(bulletXValues[i]);
            bulletYValues.Remove(bulletYValues[i]);
            bulletTypeValues.Remove(bulletTypeValues[i]);

            //removes collided enemies
            enemyXValues.Remove(enemyXValues[j]);
            enemyYValues.Remove(enemyYValues[j]);
            enemyStartXes.Remove(enemyStartXes[j]);
            enemyTypeValues.Remove(enemyTypeValues[j]);
        }
    }
}
