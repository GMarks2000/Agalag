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
        Graphics g;
        SolidBrush drawBrush = new SolidBrush(Color.White);

        int shipX;//player x value
        int shipY = 400;//player y value
        bool playerOk = true;//tracks whether player is alive

        int bulletModulator = 10;//when equal to 10,  bullet will be ready to fire.

        long tracker = 0;//tracks the  number of timer repetitions passed.

        Random rand = new Random();

        double enemySpawnRate = 1; //controls the number of enemies to spawn at each interval      

        List<int> bulletXValues = new List<int>(new int[] {});//list for bullet Xes
        List<int> bulletYValues = new List<int>(new int[] {});//list for bullet Ys
        

        List<int> enemyXValues = new List<int>(new int[] { });//list for enemy Xes
        List<int> enemyYValues = new List<int>(new int[] { });//list for enemy Ys
        

        List<int> enemyBulletXValues = new List<int>(new int[] { });//list for bullet Xes
        List<int> enemyBulletYValues = new List<int>(new int[] { });//list for bullet Ys
        


        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown, spaceDown;//track whether keys are held down

        public Form1()
        {
            InitializeComponent();
            g = this.CreateGraphics();
            gameTimer.Enabled = true;
            gameTimer.Start();

            shipX = this.Width / 2;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
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
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
                default:
                    break;
            }
        }

        //function to draw the player ship
        void drawShip(int x, int y)
        {
            Point[] triangle1Points = { new Point(x, y + 35), new Point(x + 5, y + 10), new Point(x + 10, y + 35) };//array for the points of triangle 1
            Point[] triangle2Points = { new Point(x + 20, y + 15), new Point(x + 25, y), new Point(x + 30, y + 15) };//array for the points of triangle 2
            Point[] triangle3Points = { new Point(x + 40, y + 35), new Point(x + 45, y + 10), new Point(x + 50, y + 35) };//array for the points of triangle 3

            drawBrush.Color = Color.White;

            g.FillRectangle(drawBrush, x, y + 35, 50, 10);//draws ship base
            g.FillRectangle(drawBrush, x + 20, y + 15, 10, 20);//draws ship spine

            drawBrush.Color = Color.DarkRed;

            g.FillPolygon(drawBrush, triangle1Points);//draws left triangle
            g.FillPolygon(drawBrush, triangle2Points);//draws central triangle
            g.FillPolygon(drawBrush, triangle3Points);//draws right triangle

            g.FillRectangle(drawBrush, x+10, y + 38, 30, 4);//draws ship base detail
            g.FillRectangle(drawBrush, x + 23, y + 20, 4, 20);//draws ship spine detail
        }

        void drawEnemy (int x, int y)
        {
            Point[] shipBodyPoints = { new Point(x, y + 15), new Point(x + 15, y + 55), new Point(x + 30, y + 15) };//array for the points of the ship's body

            drawBrush.Color = Color.Gray;

            g.FillRectangle(drawBrush, x, y, 10, 15);//draws left thruster
            g.FillRectangle(drawBrush, x + 20, y, 10, 15);//draws right thruster

            drawBrush.Color = Color.Gold;

            g.FillPolygon(drawBrush, shipBodyPoints);//draws ship's body
            drawBrush.Color = Color.DarkBlue;

            g.FillEllipse(drawBrush, x + 10, y + 20, 10, 15);
        }

        //timer tick method
        private void timer1_Tick(object sender, EventArgs e)
        {
            //checks to see if any keys have been pressed and adjusts the X or Y value
            //for the rectangle appropriately
            if (leftArrowDown == true && shipX > 5)
            {
                shipX-= 6;
            }
            if (downArrowDown == true && shipY <this.Height - 88)
            {
               shipY+= 6;
            }
            if (rightArrowDown == true && shipX < this.Width - 70)
            {
                shipX+= 6;
            }
            if (upArrowDown == true && shipY > 5)
            {
                shipY-= 6;
            }
            if (spaceDown == true && bulletModulator == 10 && playerOk )//fires shots only if bulletModulator has reched 10(100 ms have passed)
            {
                bulletXValues.Add(shipX + 24);
                bulletYValues.Add(shipY);

                bulletModulator = 0;
            }
            
            if (bulletModulator < 10) { bulletModulator++; }//cases bulletmodulator to incement if a shot is not ready. This will cause a shot to be fired every 100 ms

            for  (int i = 0; i < bulletXValues.Count(); i++)
            {
                //causes bullets to ascend
                if (bulletYValues[i] < 0)
                {
                    bulletXValues.Remove(bulletXValues[i]);
                    bulletYValues.Remove(bulletYValues[i]);

                }
                else
                {
                    bulletYValues[i] -= 10;
                }
            }
            for (int i = 0; i < enemyXValues.Count(); i++)
            {
                if (enemyYValues[i] > this.Height)
                {
                    enemyXValues.Remove(enemyXValues[i]);
                    enemyYValues.Remove(enemyYValues[i]);
                }
                else {
                    enemyYValues[i] += 2;//causes enemies to descend
                }
            }
            for (int i = 0; i < enemyBulletXValues.Count(); i++)
            {
                if (enemyBulletYValues[i] < 0)
                {
                    enemyBulletXValues.Remove(enemyBulletXValues[i]);
                    enemyBulletYValues.Remove(enemyBulletYValues[i]);

                }
                else
                {
                    enemyBulletYValues[i] += 7;//causes enemy bullets to descend
                }
            }


            if (tracker % 200 == 0) {
                //enemy spawn rate increases every 200 frames
                double screenDiv = this.Width / enemySpawnRate;//used to evenly distribute enemies across screen
                for (int i = 0; i < enemySpawnRate; i += 1)
                {                   
                    enemyXValues.Add(i * Convert.ToInt16(screenDiv)+ Convert.ToInt16(screenDiv)/2 - 30);
                    enemyYValues.Add(0 + (rand.Next(-50, 51)));
                }
                enemySpawnRate += 1;
            }

            if (tracker % 50 == 0)//enemies fire every 50 frames
            {
                for (int i = 0; i < enemyXValues.Count(); i++)
                {
                    enemyBulletXValues.Add(enemyXValues[i] + 15);
                    enemyBulletYValues.Add(enemyYValues[i] + 55);
                }
            }

            //collisions detection for bullets and enemies
            for (int i = 0; i < bulletXValues.Count; i++)//nested for loops to check all bullets against all enemies
            {
                for (int j = 0; j < enemyXValues.Count(); j++)
                {   
                    //*******IMPORTANT******** TEMPORARY TRY/CATCH TO STOP CRASHING! MUST FIX!*************************
                    try
                    { 
                        if (Math.Sqrt(Math.Pow(bulletXValues[i] - enemyXValues[j], 2) + Math.Pow(bulletYValues[i] - enemyYValues[j], 2)) < 30)//uses distance formula
                        {
                            bulletXValues.Remove(bulletXValues[i]);
                            bulletYValues.Remove(bulletYValues[i]);//removes collided bullets

                            enemyXValues.Remove(enemyXValues[j]);
                            enemyYValues.Remove(enemyYValues[j]);
                        }
                    }
                    catch { }
                }
            }

            for (int i = 0; i < enemyBulletXValues.Count(); i++)
            {
                if (Math.Abs(enemyBulletXValues[i] - (shipX + 25)) < 25 && Math.Abs (enemyBulletYValues[i] - (shipY + 20)) < 20)
                {
                    playerOk = false; //kills player on hit
                }
            }
            tracker++;
            Refresh();          
        }

        //paint method
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (playerOk)
            {
                drawShip(shipX, shipY);//draws ship
            }

            drawBrush.Color = Color.Orange;

            for (int i = 0; i < bulletXValues.Count(); i++)
            {
                g.FillRectangle(drawBrush, bulletXValues[i], bulletYValues[i], 3, 10);//draws player shots
            }
            for (int i = 0; i < enemyXValues.Count(); i++)
            {   
               drawEnemy(enemyXValues[i], enemyYValues[i]);//draws enemies               
            }
            drawBrush.Color = Color.Red;
            for (int i = 0; i < enemyBulletXValues.Count(); i++)
            {
                g.FillRectangle(drawBrush, enemyBulletXValues[i], enemyBulletYValues[i], 3, 10);//draws player shots
            }
        }
    }
}
