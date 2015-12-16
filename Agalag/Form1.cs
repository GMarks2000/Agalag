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
            gameTimer.Enabled = true;
            gameTimer.Start();

            shipX = this.Width / 2;
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

        //function to draw the player ship
        void drawShip(int x, int y)
        {
           
        }

        void drawEnemy (int x, int y)
        {
           
        }

        //timer tick method
        private void timer1_Tick(object sender, EventArgs e)
        {
            //checks to see if any keys have been pressed and adjusts the X or Y value
            //for the rectangle appropriately
            if (spaceDown == true && bulletModulator == 10 && playerOk)//fires shots only if bulletModulator has reched 10(100 ms have passed)
            {
                bulletXValues.Add(shipX + 24);
                bulletYValues.Add(shipY);

                bulletModulator = 0;
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

            e.Graphics.DrawString("up arrow: " + upArrowDown, new Font("Courier New", 12), drawBrush, 10, 10);
            if (playerOk)
            {
                Point[] triangle1Points = { new Point(shipX, shipY + 35), new Point(shipX + 5, shipY + 10), new Point(shipX + 10, shipY + 35) };//array for the points of triangle 1
                Point[] triangle2Points = { new Point(shipX + 20, shipY + 15), new Point(shipX + 25, shipY), new Point(shipX + 30, shipY + 15) };//array for the points of triangle 2
                Point[] triangle3Points = { new Point(shipX + 40, shipY + 35), new Point(shipX + 45, shipY + 10), new Point(shipX + 50, shipY + 35) };//array for the points of triangle 3

                drawBrush.Color = Color.White;

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
            {
                e.Graphics.FillRectangle(drawBrush, bulletXValues[i], bulletYValues[i], 3, 10);//draws player shots
            }
            for (int i = 0; i < enemyXValues.Count(); i++)
            {
                Point[] shipBodyPoints = { new Point(enemyXValues[i], enemyYValues[i] + 15), new Point(enemyXValues[i] + 15, enemyYValues[i] + 55), new Point(enemyXValues[i] + 30, enemyYValues[i] + 15) };//array for the points of the ship's body

                drawBrush.Color = Color.Gray;

                e.Graphics.FillRectangle(drawBrush, enemyXValues[i], enemyYValues[i], 10, 15);//draws left thruster
                e.Graphics.FillRectangle(drawBrush, enemyXValues[i] + 20, enemyYValues[i], 10, 15);//draws right thruster

                drawBrush.Color = Color.Gold;

                e.Graphics.FillPolygon(drawBrush, shipBodyPoints);//draws ship's body
                drawBrush.Color = Color.DarkBlue;

                e.Graphics.FillEllipse(drawBrush, enemyXValues[i] + 10, enemyYValues[i] + 20, 10, 15);
            }
            drawBrush.Color = Color.Red;
            for (int i = 0; i < enemyBulletXValues.Count(); i++)
            {
                e.Graphics.FillRectangle(drawBrush, enemyBulletXValues[i], enemyBulletYValues[i], 3, 10);//draws player shots
            }
        }
    }
}
