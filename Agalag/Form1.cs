using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Agalag
{
    public partial class Form1 : Form
    {       
        SolidBrush drawBrush = new SolidBrush(Color.White);
        Font gameFont = new Font("Courier New", 12);
        Font powerupFont = new Font("Impact", 16);
        Font titleFont = new Font("Liberation Mono", 30, FontStyle.Bold);

        Random rand = new Random();
        Stopwatch invincibilityWatch = new Stopwatch();//will track how long the player has been invincible
        Stopwatch speedWatch = new Stopwatch();//will track how long the player has been at double speed

        int playerX;//player x value
        int playerY = 400;//player y value        
        int playerHealth = 5;
        int playerLives = 1;
        int score = 0;
        int playerSpeed = 6;
        int bulletModulator = 10;//when equal to 10,  bullet will be ready to fire.
        int timeSincePowerup = 0;

        //p2 specific vars
        int player2X;//player x value
        int player2Y = 400;//player y value        
        int player2Health = 5;
        int player2Lives = 1;
        int p2BulletModulator = 10;

        bool gamePaused = false;
        bool playerFiring = false; //used to determine whether to play firing animation
        bool playerDying = false; //tracks whether  player is exploding
        bool gameOver = false; //tracks if game is ended
        bool playerOk = true;//tracks whether player is alive
        bool playerInvincible = false;
        bool playerFast = false;

        //p2 specific vars
        bool player2Ok = true;//tracks whether player is alive
        bool player2Firing = false;

        string fireMode = "single"; //will track the players mode of shooting
        string gameState = "title";

        string p2FireMode = "single"; //will track the players mode of shooting

        double enemySpawnRate = 1; //controls the number of enemies to spawn at each interval    

        long tracker = 0;//tracks the  number of timer repetitions passed.
                
        int[] starXValues = new int[200];
        double[] starYValues = new double[200];
        int[] starSizeValues = new int[200];

        //arrays for highscore names and numbers
        List<int> highScores= new List<int>(new int[] {0,0,0,0,0,0,0,0,0,0 });//list for high scores
        List<string> highScoreNames = new List<string>(new string[] {"AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", });//list for hs names

        List<int> bulletXValues = new List<int>(new int[] {});//list for bullet Xes
        List<int> bulletYValues = new List<int>(new int[] {});//list for bullet Ys
        List<string> bulletTypeValues = new List<string>(new string[] { });//list for bullet types

        List<int> p2BulletXValues = new List<int>(new int[] { });//list for bullet Xes
        List<int> p2BulletYValues = new List<int>(new int[] { });//list for bullet Ys
        List<string> p2BulletTypeValues = new List<string>(new string[] { });//list for bullet types

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

        List<int> powerupXValues = new List<int>(new int[] { });//list for powerup xes
        List<int> powerupYValues = new List<int>(new int[] { });//list for powerup ys

        List<int> explosionXValues = new List<int>(new int[] { });//list for explosion xes
        List<int> explosionYValues = new List<int>(new int[] { });//list for explosion ys
        List<int> explosionSizeValues = new List<int>(new int[] { });//list for explosion sizes
        List<int> explosionOpacityValues = new List<int>(new int[] { });//list for explosion opacities


        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown, spaceDown, jDown, kDown, lDown, iDown, shiftDown;//track whether keys are held down

        public Form1()
        {
            InitializeComponent();
           
            playerX = this.Width / 2;//initializes player to midcsreen

            entryBox.Visible = false;
            enterNameButton.Visible = false;

            for (int i = 0; i < 199; i++)//randomises star locations and sizes
            {
                starXValues[i] = rand.Next(0, this.Width - 1);
                starYValues[i] = rand.Next(0, this.Height - 1);
                starSizeValues[i] = rand.Next(3, 6);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState == "one player")
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
                    case Keys.P:
                        if (gamePaused == false)
                        {
                            gamePaused = true;
                            gameTimer.Enabled = false;
                            Refresh();
                        }
                        else
                        {
                            gamePaused = false;
                            gameTimer.Enabled = true;
                        }
                        break;
                    //returns to title from pause screen
                    case Keys.Escape:
                        if (gamePaused)
                        {
                            gameState = "title";
                            gameTimer.Enabled = false;
                            clearVariables();
                            Refresh();
                            changeTitleVisibility(true);
                        }
                        break;
                    //case for pressing enter after game over

                    default:
                        break;
                }
            }
            //TWO PLATER CASE
            else if (gameState == "two player")
            {
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
                    case Keys.J:
                        jDown = true;
                        break;
                    case Keys.K:
                        kDown = true;
                        break;
                    case Keys.L:
                        lDown = true;
                        break;
                    case Keys.I:
                        iDown = true;
                        break;
                    case Keys.RShiftKey:
                        shiftDown = true;
                        break;
                    case Keys.P:
                        if (gamePaused == false)
                        {   //pauses game
                            gamePaused = true;
                            gameTimer.Enabled = false;
                            Refresh();
                        }
                        else
                        {   //unpauses game
                            gamePaused = false;
                            gameTimer.Enabled = true;
                        }
                        break;
                    //returns to title from pause screen
                    case Keys.Escape:
                        if (gamePaused)
                        {
                            gameState = "title";
                            gameTimer.Enabled = false;
                            clearVariables();
                            Refresh();
                            changeTitleVisibility(true);
                        }
                        break;
                    //case for pressing enter after game over

                    default:
                        break;
                }
            }
            else if (gameState == "high scores")
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        //returns to title form high scores
                        if (gameState == "high scores")
                        {
                            gameState = "title";
                            gameTimer.Enabled = false;
                            Refresh();
                            changeTitleVisibility(true);
                        }
                        break;
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        { if (gameState == "one player")
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
                //TWO PLATER CASE
            }else if (gameState == "two player")
            {
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
                    case Keys.J:
                        jDown = false;
                        break;
                    case Keys.K:
                        downArrowDown = false;
                        break;
                    case Keys.L:
                        lDown = false;
                        break;
                    case Keys.I:
                        iDown = false;
                        break;
                    case Keys.RShiftKey:
                        shiftDown = false;
                        break;
                    default:
                        break;
                }
            }
        }

        //timer tick method
        private void timer1_Tick(object sender, EventArgs e)
        {

            if (gameState == "one player")
            {

                //adjusts player speed
                if (playerFast) { playerSpeed = 12; }
                else { playerSpeed = 6; }

                //******************PLAYER SHOT SPAWNS*************************************************
                if (spaceDown == true && bulletModulator == 10 && playerOk)//fires shots only if bulletModulator has reached 10
                {
                    playerFiring = true;
                    switch (fireMode)//determines which type of bullet to add based on current firing mode. This is changed via randomly appearing powerups.
                    {
                        case "single": //fires a single shot. Start case.
                            bulletXValues.Add(playerX + 24);
                            bulletYValues.Add(playerY);
                            bulletTypeValues.Add("light");
                            bulletModulator = 0;
                            break;
                        case "double": //fires two parallel shots. Shots same as in single
                            bulletXValues.Add(playerX + 4);
                            bulletYValues.Add(playerY);
                            bulletTypeValues.Add("light");
                            bulletXValues.Add(playerX + 44);
                            bulletYValues.Add(playerY);
                            bulletTypeValues.Add("light");
                            bulletModulator = 0;
                            break;
                        case "spread"://fires three shots that spread out
                            bulletXValues.Add(playerX + 4);
                            bulletYValues.Add(playerY);
                            bulletTypeValues.Add("spread left");
                            bulletXValues.Add(playerX + 24);
                            bulletYValues.Add(playerY);
                            bulletTypeValues.Add("spread center");
                            bulletXValues.Add(playerX + 44);
                            bulletYValues.Add(playerY);
                            bulletTypeValues.Add("spread right");
                            bulletModulator = 0;
                            break;
                        case "heavy": //fires a single heavy shot.
                            bulletXValues.Add(playerX + 20);
                            bulletYValues.Add(playerY - 15);
                            bulletTypeValues.Add("heavy");
                            bulletModulator = 0;
                            break;
                        default:
                            break;
                    }
                }
                else if (bulletModulator > 2) { playerFiring = false; }

                moveP1();

                if (bulletModulator < 10) { bulletModulator++; }//cases bullet modulator to incement if a shot is not ready. This will cause a shot to be fired every 100 ms

                //******************BULLETS************************************************
                for (int i = 0; i < bulletXValues.Count(); i++)
                {
                    //causes bullets to ascend
                    if (bulletYValues[i] < 0)
                    {
                        removeBullets(i);
                    }
                    else
                    {
                        switch (bulletTypeValues[i])
                        {//switch to determine how player bullets will move
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
                    for (int j = 0; j < enemyXValues.Count(); j++)//detects collisions with enemies
                    {
                        bool enemyHit = false;//tracks whether the enemy is hit to allow for the enemy switch to not impact the shot switch with less code.  
                        try
                        {
                            if (enemyXValues.IndexOf(enemyXValues[j]) != -1 && bulletXValues.IndexOf(bulletXValues[i]) != -1)
                            {
                                switch (enemyTypeValues[j])//determines hitbox based on enemy type
                                {
                                    case "light":
                                        if (calculateDistance(bulletXValues[i], enemyXValues[j], bulletYValues[i], enemyYValues[j]) < 30)//uses distance formula to check for collision
                                        {
                                            enemyHit = true;
                                        }
                                        break;
                                    case "dynamic":
                                        if (calculateDistance(bulletXValues[i], enemyXValues[j], bulletYValues[i], enemyYValues[j]) < 60)//uses distance formula to check for collision
                                        {
                                            enemyHit = true;
                                        }
                                        break;
                                    case "heavy":
                                        if (calculateDistance(bulletXValues[i], enemyXValues[j], bulletYValues[i], enemyYValues[j]) < 80)//uses distance formula to check for collision
                                        {
                                            enemyHit = true;
                                        }
                                        break;
                                }
                            }
                        }
                        catch { }
                        if (enemyHit)
                        {
                            switch (bulletTypeValues[i])//varies damage based on shot type
                            {
                                case "light":
                                    enemyHealths[j] -= 2;
                                    removeBullets(i);
                                    break;
                                case "heavy":
                                    enemyHealths[j] -= 3;
                                    removeBullets(i);
                                    break;
                                default: //covers spread shots
                                    enemyHealths[j] -= 1;
                                    removeBullets(i);
                                    break;
                            }
                        }
                    }
                }
                for (int i = 0; i < 199; i++)
                {
                    starYValues[i] += starSizeValues[i] * 0.05;
                    if (starYValues[i] > this.Height) { starYValues[i] = 0; }//causes stars to "snake" back up to top when they go offscreen.
                }

                //******************ENEMIES*************************************************
                for (int i = 0; i < enemyXValues.Count(); i++)
                {
                    if (enemyYValues[i] > this.Height)//removes offscreen enemies
                    {
                        removeEnemies(i);
                    }
                    else
                    {
                        int scoreMod = 0;//tracks the amount to add to the score on an enemy kill
                                         //tracks the amount to increase x and y by in an explosion to centralise explosion
                        int explosionXMod = 0;
                        int explosionYMod = 0;
                        switch (enemyTypeValues[i])
                        {
                            case "light":
                                enemyYValues[i] += 2;//moves light enemies down
                                scoreMod = 50;
                                explosionXMod = 15;
                                explosionYMod = 25;
                                break;
                            case "dynamic":
                                enemyYValues[i] += 3;//moves dynamic enemies down
                                enemyXValues[i] = enemyStartXes[i] + Convert.ToInt16(100 * Math.Sin(0.01 * enemyYValues[i]));//causes dynamic enemies to sway in a sinusoidal wave
                                scoreMod = 100;
                                explosionXMod = 30;
                                explosionYMod = 20;
                                break;
                            case "heavy":
                                enemyYValues[i] += 1;//causes heavy enemies to slowly move down
                                scoreMod = 150;
                                explosionXMod = 30;
                                explosionYMod = 40;
                                break;
                        }
                        //kills player and enemy on collision
                        if (calculateDistance(enemyXValues[i], playerX, enemyYValues[i], playerY) < 40)
                        {
                            if (playerInvincible == false) { playerHealth = 0; }

                            enemyHealths[i] = 0;
                        }
                        if (enemyHealths[i] <= 0)
                        {
                            //adds explosion to defeated enemies
                            explosionXValues.Add(enemyXValues[i] + explosionXMod);
                            explosionYValues.Add(enemyYValues[i] + explosionYMod);
                            explosionSizeValues.Add(0);
                            explosionOpacityValues.Add(0);

                            removeEnemies(i);
                            score += scoreMod;
                        }
                    }
                }

                //******************REGULAR ENEMY BULLETS*************************************************
                for (int i = 0; i < enemyBulletXValues.Count(); i++)
                {
                    if (enemyBulletYValues[i] < 0)
                    {
                        removeEnemyBullets(i);
                    }
                    else
                    {
                        enemyBulletYValues[i] += 7;//causes enemy bullets to descend
                        if (Math.Abs(enemyBulletXValues[i] - (playerX + 25)) < 25 && Math.Abs(enemyBulletYValues[i] - (playerY + 20)) < 20 && playerInvincible == false)
                        {
                            playerHealth -= 1;
                            removeEnemyBullets(i);
                        }
                    }
                }

                //******************DYNAMIC ENEMY BULLETS*************************************************
                for (int i = 0; i < enemyDynamicBulletXValues.Count(); i++)//for loop for dynamic bullets. Seperate due to their differet behavior
                {
                    if (enemyDynamicBulletYValues[i] < 0)
                    {
                        removeDynamicBullets(i);
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
                        if (Math.Abs(enemyDynamicBulletXValues[i] - (playerX + 25)) < 25 && Math.Abs(enemyDynamicBulletYValues[i] - (playerY + 20)) < 20 && playerInvincible == false)
                        {
                            playerHealth -= 2;
                            removeDynamicBullets(i);
                        }
                    }
                    catch { }
                }

                //******************HEAVY ENEMY BULLETS*************************************************
                for (int i = 0; i < enemyHeavyBulletXValues.Count(); i++)
                {
                    if (enemyHeavyBulletYValues[i] > this.Height)
                    {
                        removeHeavyBullets(i);
                    }
                    else
                    {
                        //moves enemy heavy shots down
                        enemyHeavyBulletYValues[i] += 4;
                        //causes heavy shots to "home" towards player
                        if (enemyHeavyBulletXValues[i] > playerX) { enemyHeavyBulletXValues[i]--; }
                        else if (enemyHeavyBulletXValues[i] < playerX) { enemyHeavyBulletXValues[i]++; }
                        try
                        {
                            if (Math.Abs(enemyHeavyBulletXValues[i] - (playerX + 25)) < 25 && Math.Abs(enemyHeavyBulletYValues[i] - (playerY + 20)) < 20 && playerInvincible == false)
                            {
                                playerHealth -= 3;
                                removeHeavyBullets(i);
                            }
                        }
                        catch { }
                    }
                }

                if (playerHealth <= 0 && gameOver == false)//player respawn
                {
                    if (playerDying == false)
                    {
                        explosionXValues.Add(playerX + 30);
                        explosionYValues.Add(playerY + 20);
                        explosionSizeValues.Add(0);
                        explosionOpacityValues.Add(0);
                        playerOk = false;
                        playerDying = true;
                    }

                    //waits for explosion to finish before stopping to respawn player
                    if (explosionXValues.Count() == 0)
                    {

                        gameTimer.Enabled = false;

                        for (int i = 0; i < 5; i++)//flashes player onscreen 5 times before restarting
                        {
                            if (playerLives != 1)
                            {
                                Refresh();
                                Thread.Sleep(250);
                                playerOk = false;
                                Refresh();
                                Thread.Sleep(250);
                                playerOk = true;
                                playerDying = false;
                            }

                        }
                        //respawns player with fresh stats
                        gameTimer.Enabled = true;
                        fireMode = "single";
                        playerInvincible = false;
                        playerFast = false;
                        playerLives--;
                        playerHealth = 5;
                        
                    }
                }

                if (playerLives == 0)//ends game on player death
                {
                    playerOk = false;
                    Refresh();
                    gameTimer.Enabled = false;

                    gameOver = true;

                    entryBox.Visible = true;
                    enterNameButton.Visible = true;
                     
                }

                int powerupRand = rand.Next(0, 501);//1 in 500 chance of a powerup spawn
                if (powerupRand == 500 && timeSincePowerup > 300)
                {
                    //starts powerup at a random x at the top of the screen
                    powerupXValues.Add(rand.Next(0, this.Width - 50));
                    powerupYValues.Add(0);
                    timeSincePowerup = 0;
                }
                else { timeSincePowerup++; }

                //******************POWERUPS*************************************************
                for (int i = 0; i < powerupXValues.Count(); i++)
                {
                    if (powerupYValues[i] > this.Height)//removes offscreen powerups
                    {
                        removePowerups(i);
                    }
                    else
                    {
                        powerupYValues[i]++;//moves powerup downn
                        if (calculateDistance(playerX, powerupXValues[i], playerY, powerupYValues[i]) < 50)
                        {
                            int randomEffect = rand.Next(1, 6);
                            switch (randomEffect)
                            {
                                case 1:
                                    fireMode = "double";
                                    break;
                                case 2:
                                    fireMode = "spread";
                                    break;
                                case 3:
                                    fireMode = "heavy";
                                    break;
                                case 4:
                                    invincibilityWatch.Start();
                                    playerInvincible = true;
                                    break;
                                case 5:
                                    speedWatch.Start();
                                    playerFast = true;
                                    break;
                            }
                            removePowerups(i);
                        }
                    }
                }
                //makes player vulnerable if they have been invincible for more than 15 seconds
                if (playerInvincible && invincibilityWatch.Elapsed.TotalHours >= 0.00416)//approx 15 secs invincibility
                {
                    invincibilityWatch.Stop();
                    invincibilityWatch.Reset();
                    playerInvincible = false;
                }
                //makes player normal if they have been fast for more than 15 seconds
                if (playerFast && speedWatch.Elapsed.TotalHours >= 0.00632)//approx 22 secs speed
                {
                    speedWatch.Stop();
                    speedWatch.Reset();
                    playerFast = false;
                }
                //******************EXPLOSIONS*************************************************
                for (int i = 0; i < explosionXValues.Count(); i++)
                {   //removes explosions that reach a certain size
                    if (explosionSizeValues[i] >= 100)
                    {
                        removeExplosions(i);
                    }
                    else
                    {
                        explosionSizeValues[i] += 4;//grows explosion
                        explosionOpacityValues[i]+= 8;

                        //recentralizes explosion
                        if (tracker % 2 == 0)
                        {
                            explosionXValues[i] -= 4;
                            explosionYValues[i] -= 4;
                        }
                    }
                }

                //******************ENEMY SPAWNING*************************************************
                if (tracker % 200 == 0)
                {
                    //enemies spawn every 200 frames
                    if (tracker <= 800)
                    {
                        double screenDiv = this.Width / enemySpawnRate;//used to evenly distribute enemies across screen
                        for (int i = 0; i < enemySpawnRate; i += 1)
                        {
                            int startX = (i * Convert.ToInt16(screenDiv) + Convert.ToInt16(screenDiv) / 2 - 30);
                            enemyXValues.Add(startX);
                            enemyYValues.Add(-50 + (rand.Next(-50, 51)));//randomises Y to a degree
                            enemyTypeValues.Add("light");
                            enemyHealths.Add(1);
                            enemyStartXes.Add(startX);
                        }
                        enemySpawnRate += 1;
                        if (enemySpawnRate == 6) { enemySpawnRate = 0; } //resets spawn rate once next bracket is reached
                    }
                    else if (tracker > 800 && tracker <= 1800)
                    {
                        double screenDiv = this.Width / enemySpawnRate;//used to evenly distribute enemies across screen
                        for (int i = 0; i < enemySpawnRate; i += 1)
                        {
                            int startX = (i * Convert.ToInt16(screenDiv) + Convert.ToInt16(screenDiv) / 2 - 30);
                            enemyXValues.Add(startX);
                            enemyYValues.Add(-50 + (rand.Next(-50, 51)));//randomises Y to a degree
                            enemyTypeValues.Add("dynamic");
                            enemyHealths.Add(3);
                            enemyStartXes.Add(startX);//used to allow a sine wave pattern.
                            if (enemySpawnRate == 5) { enemySpawnRate = 0; }
                        }
                        enemySpawnRate += 1;
                    }
                    else if (tracker > 2000 && tracker <= 3000)
                    {
                        double screenDiv = this.Width / enemySpawnRate;//used to evenly distribute enemies across screen
                        for (int i = 0; i < enemySpawnRate; i += 1)
                        {
                            int startX = (i * Convert.ToInt16(screenDiv) + Convert.ToInt16(screenDiv) / 2 - 30);
                            enemyXValues.Add(startX);
                            enemyYValues.Add(-50 + (rand.Next(-50, 51)));//randomises Y within 100 pixels
                            enemyTypeValues.Add("heavy");
                            enemyHealths.Add(5);
                            enemyStartXes.Add(startX);
                            if (enemySpawnRate == 5) { enemySpawnRate = 0; }
                        }
                        enemySpawnRate += 1;
                    }
                    else if (tracker > 3000 && tracker <= 5000)
                    {
                        double screenDiv = this.Width / enemySpawnRate;//used to evenly distribute enemies across screen
                        for (int i = 0; i < enemySpawnRate; i += 1)
                        {
                            int startX = (i * Convert.ToInt16(screenDiv) + Convert.ToInt16(screenDiv) / 2 - 30);
                            enemyXValues.Add(startX);
                            enemyYValues.Add(-50 + (rand.Next(-50, 51)));//randomises Y within 100 pixels

                            int typeRand = rand.Next(0, 2);//randomises enemy type

                            switch (typeRand)
                            {
                                case 0:
                                    enemyTypeValues.Add("light");
                                    enemyHealths.Add(1);
                                    break;
                                case 1:
                                    enemyTypeValues.Add("dynamic");
                                    enemyHealths.Add(3);
                                    break;
                            }

                            enemyStartXes.Add(startX);
                            if (enemySpawnRate == 7) { enemySpawnRate = 5; }
                        }
                        enemySpawnRate += 1;
                    }
                    else if (tracker > 5000 && tracker <= 7000)
                    {
                        double screenDiv = this.Width / enemySpawnRate;//used to evenly distribute enemies across screen
                        for (int i = 0; i < enemySpawnRate; i += 1)
                        {
                            int startX = (i * Convert.ToInt16(screenDiv) + Convert.ToInt16(screenDiv) / 2 - 30);
                            enemyXValues.Add(startX);
                            enemyYValues.Add(-50 + (rand.Next(-50, 51)));//randomises Y within 100 pixels

                            int typeRand = rand.Next(0, 2);//randomises enemy type

                            switch (typeRand)
                            {
                                case 0:
                                    enemyTypeValues.Add("light");
                                    enemyHealths.Add(1);
                                    break;
                                case 1:
                                    enemyTypeValues.Add("heavy");
                                    enemyHealths.Add(5);
                                    break;
                            }

                            enemyStartXes.Add(startX);
                            if (enemySpawnRate == 8) { enemySpawnRate = 6; }
                        }
                        enemySpawnRate += 1;
                    }
                    else if (tracker > 7000 && tracker <= 9000)
                    {
                        double screenDiv = this.Width / enemySpawnRate;//used to evenly distribute enemies across screen
                        for (int i = 0; i < enemySpawnRate; i += 1)
                        {
                            int startX = (i * Convert.ToInt16(screenDiv) + Convert.ToInt16(screenDiv) / 2 - 30);
                            enemyXValues.Add(startX);
                            enemyYValues.Add(-50 + (rand.Next(-50, 51)));//randomises Y within 100 pixels

                            int typeRand = rand.Next(0, 2);//randomises enemy type

                            switch (typeRand)
                            {
                                case 0:
                                    enemyTypeValues.Add("dynamic");
                                    enemyHealths.Add(3);
                                    break;
                                case 1:
                                    enemyTypeValues.Add("heavy");
                                    enemyHealths.Add(1);
                                    break;
                            }

                            enemyStartXes.Add(startX);
                            if (enemySpawnRate == 9) { enemySpawnRate = 7; }
                        }
                        enemySpawnRate += 1;
                    }
                    else if (tracker > 9000)
                    {
                        double screenDiv = this.Width / enemySpawnRate;//used to evenly distribute enemies across screen
                        for (int i = 0; i < enemySpawnRate; i += 1)
                        {
                            int startX = (i * Convert.ToInt16(screenDiv) + Convert.ToInt16(screenDiv) / 2 - 30);
                            enemyXValues.Add(startX);
                            enemyYValues.Add(-50 + (rand.Next(-50, 51)));//randomises Y within 100 pixels

                            int typeRand = rand.Next(0, 3);//randomises enemy type

                            switch (typeRand)
                            {
                                case 0:
                                    enemyTypeValues.Add("dynamic");
                                    enemyHealths.Add(3);
                                    break;
                                case 1:
                                    enemyTypeValues.Add("heavy");
                                    enemyHealths.Add(5);
                                    break;
                                case 2:
                                    enemyTypeValues.Add("light");
                                    enemyHealths.Add(1);
                                    break;
                            }

                            enemyStartXes.Add(startX);
                        }
                        enemySpawnRate += 1;
                    }
                }
                //******************ENEMY FIRE PATTERNS*************************************************
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
                            enemyDynamicBulletXIncreases.Add((playerX - enemyXValues[i]) / 50);
                            enemyDynamicBulletYIncreases.Add((playerY - enemyYValues[i]) / 50);
                        }
                    }
                }
                if (tracker % 75 == 0)
                {
                    for (int i = 0; i < enemyXValues.Count(); i++)
                    {
                        if (enemyTypeValues[i] == "heavy")
                        {
                            enemyHeavyBulletXValues.Add(enemyXValues[i] + 5);
                            enemyHeavyBulletYValues.Add(enemyYValues[i] + 80);

                            enemyHeavyBulletXValues.Add(enemyXValues[i] + 55);
                            enemyHeavyBulletYValues.Add(enemyYValues[i] + 80);
                        }
                    }
                }

                tracker++;
                Refresh();
            }else if (gameState == "two player")
                //**************P1 MOVEMENT*******************
            {
                moveP1();
                //*********************P2 MOVEMENT************************
                moveP2();

                //******************PLAYER SHOT SPAWNS*************************************************
                if (spaceDown == true && bulletModulator == 10 && playerOk)//fires shots only if bulletModulator has reached 10
                {
                    playerFiring = true;
                    switch (fireMode)//determines which type of bullet to add based on current firing mode. This is changed via randomly appearing powerups.
                    {
                        case "single": //fires a single shot. Start case.
                            bulletXValues.Add(playerX + 24);
                            bulletYValues.Add(playerY);
                            bulletTypeValues.Add("light");
                            bulletModulator = 0;
                            break;
                        case "double": //fires two parallel shots. Shots same as in single
                            bulletXValues.Add(playerX + 4);
                            bulletYValues.Add(playerY);
                            bulletTypeValues.Add("light");
                            bulletXValues.Add(playerX + 44);
                            bulletYValues.Add(playerY);
                            bulletTypeValues.Add("light");
                            bulletModulator = 0;
                            break;
                        case "spread"://fires three shots that spread out
                            bulletXValues.Add(playerX + 4);
                            bulletYValues.Add(playerY);
                            bulletTypeValues.Add("spread left");
                            bulletXValues.Add(playerX + 24);
                            bulletYValues.Add(playerY);
                            bulletTypeValues.Add("spread center");
                            bulletXValues.Add(playerX + 44);
                            bulletYValues.Add(playerY);
                            bulletTypeValues.Add("spread right");
                            bulletModulator = 0;
                            break;
                        case "heavy": //fires a single heavy shot.
                            bulletXValues.Add(playerX + 20);
                            bulletYValues.Add(playerY - 15);
                            bulletTypeValues.Add("heavy");
                            bulletModulator = 0;
                            break;
                        default:
                            break;
                    }
                }
                else if (p2BulletModulator > 2)
                { player2Firing = false; }

                if (shiftDown == true && bulletModulator == 10 && player2Ok)//fires shots only if bulletModulator has reached 10
                {
                    player2Firing = true;
                    switch (fireMode)//determines which type of bullet to add based on current firing mode. This is changed via randomly appearing powerups.
                    {
                        case "single": //fires a single shot. Start case.
                            p2BulletXValues.Add(player2X + 24);
                            p2BulletYValues.Add(player2Y);
                            p2BulletTypeValues.Add("light");
                            p2BulletModulator = 0;
                            break;
                        case "double": //fires two parallel shots. Shots same as in single
                            p2BulletXValues.Add(player2X + 4);
                            p2BulletYValues.Add(player2Y);
                            p2BulletTypeValues.Add("light");
                            p2BulletXValues.Add(player2X + 44);
                            bulletYValues.Add(player2Y);
                            p2BulletTypeValues.Add("light");
                            p2BulletModulator = 0;
                            break;
                        case "spread"://fires three shots that spread out
                            p2BulletXValues.Add(player2X + 4);
                            p2BulletYValues.Add(player2Y);
                            p2BulletTypeValues.Add("spread left");
                            p2BulletXValues.Add(player2X + 24);
                            p2BulletYValues.Add(player2Y);
                            p2BulletTypeValues.Add("spread center");
                            p2BulletXValues.Add(player2X + 44);
                            p2BulletYValues.Add(player2Y);
                            p2BulletTypeValues.Add("spread right");
                            p2BulletModulator = 0;
                            break;
                        case "heavy": //fires a single heavy shot.
                            p2BulletXValues.Add(player2X + 20);
                            p2BulletYValues.Add(player2Y - 15);
                            p2BulletTypeValues.Add("heavy");
                            p2BulletModulator = 0;
                            break;
                        default:
                            break;
                    }
                }
                else if (p2BulletModulator > 2) { player2Firing = false; }

                //*************PLAYER 1 BULLETS******************
                for (int i = 0; i < bulletXValues.Count();)
                {
                    if (bulletYValues[i] < 0)//removes offscreen shots
                    {
                        removeBullets(i);
                    }
                    else
                    {
                        switch (bulletTypeValues[i])
                        {//switch to determine how player bullets will move
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

            }
            else if (gameState == "high scores") { Refresh(); }
        }

        //paint method
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "one player")
            {
                drawBrush.Color = Color.White;

                for (int i = 0; i < 199; i++)
                {
                    float starY = Convert.ToInt16(starYValues[i]);
                    e.Graphics.FillEllipse(drawBrush, starXValues[i], starY, starSizeValues[i], starSizeValues[i]);
                }


                //draws explosions
                for (int i = 0; i < explosionXValues.Count(); i++)
                {   //explosions become translucent over time.
                    drawBrush.Color = Color.FromArgb(255 - explosionOpacityValues[i], 202 - explosionOpacityValues[i] / 1, 0);
                    e.Graphics.FillEllipse(drawBrush, explosionXValues[i], explosionYValues[i], explosionSizeValues[i], explosionSizeValues[i]);
                    drawBrush.Color = Color.FromArgb(255 - explosionOpacityValues[i], 0, 0);
                    e.Graphics.FillEllipse(drawBrush, explosionXValues[i] + explosionSizeValues[i] / 4, explosionYValues[i] + explosionSizeValues[i] / 4, explosionSizeValues[i] / 2, explosionSizeValues[i] / 2);
                }

                if (playerOk)
                {
                    drawBrush.Color = Color.White;

                    if (playerInvincible)
                    {
                        drawBrush.Color = Color.Gold;
                    }

                    Point[] triangle1Points = { new Point(playerX, playerY + 35), new Point(playerX + 5, playerY + 10), new Point(playerX + 10, playerY + 35) };//array for the points of triangle 1
                    Point[] triangle2Points = { new Point(playerX + 20, playerY + 15), new Point(playerX + 25, playerY), new Point(playerX + 30, playerY + 15) };//array for the points of triangle 2
                    Point[] triangle3Points = { new Point(playerX + 40, playerY + 35), new Point(playerX + 45, playerY + 10), new Point(playerX + 50, playerY + 35) };//array for the points of triangle 3


                    e.Graphics.FillRectangle(drawBrush, playerX, playerY + 35, 50, 10);//draws ship base
                    e.Graphics.FillRectangle(drawBrush, playerX + 20, playerY + 15, 10, 20);//draws ship spine

                    drawBrush.Color = Color.DarkRed;

                    e.Graphics.FillPolygon(drawBrush, triangle1Points);//draws left triangle
                    e.Graphics.FillPolygon(drawBrush, triangle2Points);//draws central triangle
                    e.Graphics.FillPolygon(drawBrush, triangle3Points);//draws right triangle

                    e.Graphics.FillRectangle(drawBrush, playerX + 10, playerY + 38, 30, 4);//draws ship base detail
                    e.Graphics.FillRectangle(drawBrush, playerX + 23, playerY + 20, 4, 20);//draws ship spine detail
                }

                drawBrush.Color = Color.Orange;

                if (playerFiring)//draws shooting effect
                {
                    if (fireMode == "single" || fireMode == "heavy" || fireMode == "spread")
                    {
                        e.Graphics.FillEllipse(drawBrush, playerX + 22, playerY - 15, 6, 20);
                    }
                    if (fireMode == "double" || fireMode == "spread")
                    {
                        e.Graphics.FillEllipse(drawBrush, playerX + 2, playerY - 5, 6, 20);
                        e.Graphics.FillEllipse(drawBrush, playerX + 42, playerY - 5, 6, 20);
                    }
                }

                for (int i = 0; i < bulletXValues.Count(); i++)
                {
                    switch (bulletTypeValues[i])
                    {//switch statement to determine which bullet shape to draw
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
                {
                    switch (enemyTypeValues[i])//determines which type of enemy to draw
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

                            e.Graphics.FillRectangle(drawBrush, enemyXValues[i], enemyYValues[i] - 10, 10, 10);//draws left thruster
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
                    e.Graphics.FillRectangle(drawBrush, enemyBulletXValues[i], enemyBulletYValues[i], 3, 10);//draws enemy light shots
                }
                for (int i = 0; i < enemyDynamicBulletXValues.Count(); i++)
                {
                    e.Graphics.FillRectangle(drawBrush, enemyDynamicBulletXValues[i], enemyDynamicBulletYValues[i], 3, 10);//draws enemy dynamic shots
                }
                for (int i = 0; i < enemyHeavyBulletXValues.Count(); i++)
                {
                    e.Graphics.FillEllipse(drawBrush, enemyHeavyBulletXValues[i], enemyHeavyBulletYValues[i], 10, 10);//draws enemy heavy shots
                }


                for (int i = 0; i < powerupXValues.Count(); i++)
                {
                    drawBrush.Color = Color.Blue;
                    e.Graphics.FillEllipse(drawBrush, powerupXValues[i], powerupYValues[i], 30, 30);
                    drawBrush.Color = Color.White;
                    e.Graphics.DrawString("P", powerupFont, drawBrush, powerupXValues[i] + 7, powerupYValues[i] + 2);
                }

                drawBrush.Color = Color.White;

                //text labels for player stats
                e.Graphics.DrawString("Lives", gameFont, drawBrush, 20, 20);
                e.Graphics.DrawString("Health", gameFont, drawBrush, 20, 50);
                e.Graphics.DrawString("Score:" + score, gameFont, drawBrush, 20, 80);
                if (playerFast) { e.Graphics.DrawString("Double Speed!", gameFont, drawBrush, 20, 110); }
                if (playerInvincible) { e.Graphics.DrawString("Invincible!", gameFont, drawBrush, 20, 140); }
                drawBrush.Color = Color.Red;

                for (int i = 0; i < playerLives; i++)
                {
                    e.Graphics.FillEllipse(drawBrush, 80 + i * 20, 20, 20, 20);//draws lives display
                }
                for (int i = 0; i < playerHealth; i++)
                {
                    e.Graphics.FillRectangle(drawBrush, 85 + i * 15, 50, 10, 20);//draws health display
                }

                //draws shot display
                drawBrush.Color = Color.DarkBlue;
                e.Graphics.FillEllipse(drawBrush, 20, 180, 30, 30);
                drawBrush.Color = Color.Orange;
                switch (fireMode)
                {
                    case "single":
                        e.Graphics.FillRectangle(drawBrush, 30, 185, 10, 20);
                        break;
                    case "double":
                        e.Graphics.FillRectangle(drawBrush, 27, 190, 5, 10);
                        e.Graphics.FillRectangle(drawBrush, 40, 190, 5, 10);
                        break;
                    case "spread":
                        e.Graphics.FillEllipse(drawBrush, 33, 185, 6, 6);
                        e.Graphics.FillEllipse(drawBrush, 26, 192, 6, 6);
                        e.Graphics.FillEllipse(drawBrush, 41, 192, 6, 6);
                        break;
                    case "heavy":
                        e.Graphics.FillEllipse(drawBrush, 25, 185, 20, 20);
                        break;
                }

                //draws pause text
                drawBrush.Color = Color.White;

                if (gamePaused)
                {
                    e.Graphics.DrawString("Game Paused", titleFont, drawBrush, this.Width / 2 - 150, this.Height / 2 - 100);
                    e.Graphics.DrawString("Press P to Unpause or Esc to Exit", gameFont, drawBrush, this.Width / 2 - 165, this.Height / 2);
                }
                drawBrush.Color = Color.Red;
                //draws game oover text
                if (gameOver)
                {
                    e.Graphics.DrawString("Game Over", titleFont, drawBrush, this.Width / 2 - 150, this.Height / 2 - 100);
                    drawBrush.Color = Color.White;
                    e.Graphics.DrawString("Score: " + score, titleFont, drawBrush, this.Width / 2 - 150, this.Height / 2 - 50);
                    e.Graphics.DrawString("Please enter your name", gameFont, drawBrush, this.Width / 2 - 145, this.Height / 2);
                        
                }
            }
            else if (gameState == "high scores")
            {
                drawBrush.Color = Color.White;
                e.Graphics.DrawString("High Scores", titleFont, drawBrush, this.Width / 2 - 160, 50);//draws title

                e.Graphics.FillRectangle(drawBrush, this.Width / 2 - 40, 100, 25, 700);//draws central divider

                for (int i = 0; i < 10; i++)
                {   //adjusts color based on position in high scores
                    if (i == 0) { drawBrush.Color = Color.Gold; }
                    else if (i == 1) { drawBrush.Color = Color.Silver; }
                    else if (i == 2) { drawBrush.Color = Color.Brown; }
                    else { drawBrush.Color = Color.White; }

                    e.Graphics.DrawString((i+1).ToString(), powerupFont, drawBrush, 350, 150 + i * 50);//draws place
                    e.Graphics.DrawString(highScoreNames[i], powerupFont, drawBrush, 550,  150 + i * 50);//draws names
                    e.Graphics.DrawString(Convert.ToString(highScores[i]), powerupFont, drawBrush, 750, 150 + i * 50);//draws scores
                }

                drawBrush.Color = Color.Red;
                e.Graphics.DrawString("Press [ESC] to Return", powerupFont, drawBrush, 1000, 200);//draws notice of how to return

            }
        }
        //*****************REMOVAL FUNCTIONS**********************
        void removeEnemies(int i)//
        {           
            //removes collided enemies
            enemyXValues.Remove(enemyXValues[i]);
            enemyYValues.Remove(enemyYValues[i]);
            enemyStartXes.Remove(enemyStartXes[i]);
            enemyHealths.Remove(enemyHealths[i]);
            enemyTypeValues.Remove(enemyTypeValues[i]);
        }
        void removeBullets(int i)
        {
            bulletXValues.Remove(bulletXValues[i]);
            bulletYValues.Remove(bulletYValues[i]);
            bulletTypeValues.Remove(bulletTypeValues[i]);
        }
        void removeEnemyBullets (int i)
        {
            enemyBulletXValues.Remove(enemyBulletXValues[i]);//removes offscreen enemy bullets
            enemyBulletYValues.Remove(enemyBulletYValues[i]);
        }
        void removeDynamicBullets(int i)
        {
            enemyDynamicBulletXValues.Remove(enemyDynamicBulletXValues[i]);//removes offscreen enemy bullets
            enemyDynamicBulletYValues.Remove(enemyDynamicBulletYValues[i]);
            enemyDynamicBulletXIncreases.Remove(enemyDynamicBulletXIncreases[i]);
            enemyDynamicBulletYIncreases.Remove(enemyDynamicBulletYIncreases[i]);
        }

        void removeHeavyBullets (int i)
        {
            enemyHeavyBulletXValues.Remove(enemyHeavyBulletXValues[i]);
            enemyHeavyBulletYValues.Remove(enemyHeavyBulletYValues[i]);
        }
        void removePowerups(int i)
        {
            powerupXValues.Remove(powerupXValues[i]);
            powerupYValues.Remove(powerupYValues[i]);
        }
        void removeExplosions(int i)
        {
            explosionXValues.Remove(explosionXValues[i]);
            explosionYValues.Remove(explosionYValues[i]);
            explosionSizeValues.Remove(explosionSizeValues[i]);
            explosionOpacityValues.Remove(explosionOpacityValues[i]);
        }

        //begins one player mode on the one player button click
        private void onePlayerButton_Click(object sender, EventArgs e)
        {
            if (gameState == "title")
            {
                changeTitleVisibility(false);
                gameState = "one player";
                gameTimer.Enabled = true;
                gameTimer.Start();
                this.Focus();
            }
        }

        
        //two player button click method
        private void twoPlayerButton_Click(object sender, EventArgs e)
        {
            gameState = "two player";
            changeTitleVisibility(false);
            gameTimer.Enabled = true;
            gameTimer.Start();
            this.Focus();
        }

        private void enterNameButton_Click(object sender, EventArgs e)
        {
            if (gameOver)
            {
                string name = entryBox.Text;

                //adjusts high scores
                for (int i = 0; i < 10; i++)
                {
                    if (score > highScores[i])
                    {
                        highScores.Insert(i, score);
                        highScoreNames.Insert(i, name);
                        break;//break is necessary to prevent all values from being changed to current score
                    }
                }
             
                gameState = "title";
                gameTimer.Enabled = false;
                clearVariables();
                Refresh();
                changeTitleVisibility(true);
                entryBox.Visible = false;
                enterNameButton.Visible = false;
            }
        }

        //high score button click method
        private void highScoreButton_Click(object sender, EventArgs e)
        {
            gameState = "high scores";
            changeTitleVisibility(false);
            gameTimer.Enabled = true;
            gameTimer.Start();
            this.Focus();
        }



        //function to cause the title forms to become invisible
        void changeTitleVisibility(bool state)
        {
            lowerTitleLabel.Visible = state;
            upperTitleLabel.Visible = state;
            nameLabel.Visible = state;
            onePlayerButton.Visible = state;
            twoPlayerButton.Visible = state;
            highScoreButton.Visible = state;
            explosionBox.Visible = state;
        }

        double calculateDistance(int x1, int x2, int y1, int  y2)//method to calculate distance for use in collision detection
        {
            return ((Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2))));
        }


        void moveP1()
        {
            if (leftArrowDown == true && playerX > 5)
            {
                playerX -= playerSpeed;
            }
            if (rightArrowDown == true && playerX < this.Width - 70)
            {
                playerX += playerSpeed;
            }
            if (downArrowDown == true && playerY < this.Height - 88)
            {
                playerY += playerSpeed;
            }
            if (upArrowDown == true && playerY > 5)
            {
                playerY -= playerSpeed;
            }
        }

        void moveP2()
        {
            if (jDown == true && player2X > 5)
                {
                playerX -= playerSpeed;
            }
                if (lDown == true && player2X < this.Width - 70)
                {
                player2X += playerSpeed;
            }
                if (kDown == true && player2Y < this.Height - 88)
                {
                playerY += playerSpeed;
            }
                if (iDown == true && player2Y > 5)
                {
                player2Y -= playerSpeed;
            }
        }


        //clears all game-relevant variables
        void clearVariables()
        {
            playerX = this.Width / 2;//player x value
            playerY = 400;//player y value        
            playerHealth = 5;
            playerLives = 3;
            score = 0;
            playerSpeed = 6;
            bulletModulator = 10;//when equal to 10,  bullet will be ready to fire.
            timeSincePowerup = 0;

            gamePaused = false;
            playerFiring = false; //used to determine whether to play firing animation
            playerDying = false; //tracks whether  player is exploding
            gameOver = false; //tracks if game is ended
            playerOk = true;//tracks whether player is alive
            playerInvincible = false;
            playerFast = false;

            fireMode = "single"; //will track the players mode of shooting
            gameState = "title";

           enemySpawnRate = 1; //controls the number of enemies to spawn at each interval    

            tracker = 0;//tracks the  number of timer repetitions passed.

            bulletXValues.Clear();
            bulletYValues.Clear();
            bulletTypeValues.Clear();

            enemyXValues.Clear();
            enemyYValues.Clear();
            enemyHealths.Clear();
            enemyStartXes.Clear();
            enemyTypeValues.Clear();

            enemyBulletXValues.Clear();
            enemyBulletYValues.Clear();

            enemyDynamicBulletXValues.Clear();
            enemyDynamicBulletYValues.Clear();
            enemyDynamicBulletXIncreases.Clear();
            enemyDynamicBulletYIncreases.Clear();

            enemyHeavyBulletXValues.Clear();
            enemyHeavyBulletYValues.Clear();

            powerupXValues.Clear();
            powerupYValues.Clear();

            explosionXValues.Clear();
            explosionYValues.Clear();
            explosionSizeValues.Clear();
            explosionOpacityValues.Clear();
        }
    }
}
