using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarketForecastMartingale
{
    public partial class Form1 : Form
    {
        List<bool> randomWalkProfits = new List<bool>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // IDEA - use anti-martingale as it is right now and once it reaches 6 it doubles to 12 at 9 then to 24 at 12, 48 at 15, 96 at 18
            // 6 at 180, 9 at 360, 12 at 720, 15 at 1440, 18 at 2880
            int counter = 1;
            string line;
            double totalTicks = 0;
            double lowestTicks = 10000;
            double lowestCounter = 0;
            int loseStreak = 0;
            int totalLoseStreak = 0;
            int totalCounter = 0;
            int numTwenties = 0;
            // Read the file and display it line by line.
            // NQ = 1940 -> 4089
            // YM = -> 
            System.IO.StreamReader file =
               new System.IO.StreamReader(@"E:\Visual Studio 2012\Projects\MarketForecastMartingale\MarketForecastMartingale\NQ.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("20"))
                {
                    if (counter <= 7)
                    {
                        totalTicks += counter * 20;
                        //totalTicks -= counter * 5.29;
                    }
                    if (loseStreak > 0)
                    {
                        totalLoseStreak += loseStreak;
                        totalCounter++;
                    }
                    loseStreak = 0;
                    /*
                    if (counter >= 8 && numTwenties == 3)
                    {
                        counter = 1;
                        numTwenties = 0;
                    }
                    else if (counter <= 7)
                    {
                        counter = 1;
                        numTwenties = 0;
                    }
                    numTwenties++;
                     */
                    counter = 1;
                }
                else if (line.Contains("6") && counter <= 7)
                {
                    totalTicks -= counter * 6;
                    //totalTicks -= counter * 5.29;
                    counter += 1;
                    if (totalTicks < lowestTicks)
                    {
                        lowestTicks = totalTicks;
                    }
                    loseStreak++;
                }

                if (counter > 10)
                {
                    lowestCounter++;
                }
                
            }
            MessageBox.Show(totalTicks.ToString());
            //MessageBox.Show(lowestTicks.ToString());
            //MessageBox.Show(lowestCounter.ToString());
            //MessageBox.Show(counter.ToString());
            file.Close();
        }

        private int fxn(int counter)
        {
            if (counter <= 6)
            {
                return counter;
            }
            int numIncBy3 = 0;
            for (int i = 7; i <= counter; i++)
            {
                if (i % 3 == 0)
                {
                    numIncBy3++;
                }
            }
            int count = 6;
            int total = 6;
            int lastSquare = 6;
            for (int i = 0; i <= numIncBy3; i++)
            {
                int square = lastSquare * 2;
                if ((counter - count) < 3)
                {
                    for (int j = 0; j < counter % 3; j++)
                    {
                        count++;
                        double squareDiff = square - lastSquare;
                        total += (int)(((double)1 / (double)3) * squareDiff);
                    }
                }
                else
                {
                    for (int j = 0; j < 3; j++)
                    {
                        count++;
                        double squareDiff = square - lastSquare;
                        total += (int)(((double)1 / (double)3) * squareDiff);
                    }
                }
                lastSquare = square;
            }
            return total;
        }

        private int fxn2(int counter)
        {
            if (counter <= 60)
            {
                return counter;
            }
            int numIncBy3 = 0;
            for (int i = 70; i <= counter; i += 10)
            {
                if (i % 30 == 0)
                {
                    numIncBy3++;
                }
            }
            int count = 60;
            int total = 60;
            int lastSquare = 60;
            for (int i = 0; i <= numIncBy3; i++)
            {
                int square = lastSquare * 2;
                if ((counter - count) < 30)
                {
                    for (int j = 0; j < counter % 3; j++)
                    {
                        count += 10;
                        double squareDiff = square - lastSquare;
                        total += (int)(((double)1 / (double)3) * squareDiff);
                    }
                }
                else
                {
                    for (int j = 0; j < 3; j++)
                    {
                        count += 10;
                        double squareDiff = square - lastSquare;
                        total += (int)(((double)1 / (double)3) * squareDiff);
                    }
                }
                lastSquare = square;
            }
            return total;
        }


        bool[] randomWalk(int size)
        {
            bool[] retArr = new bool[size];
            System.Random rand = new System.Random();
            int priceAt = 0;
            for (int i = 0; i < size; i++)
            {
                while (true)
                {
                    if (rand.Next(1500) <= 762)
                    {
                        priceAt++;
                    }
                    else
                    {
                        priceAt--;
                    }
                    if (priceAt >= 30)
                    {
                        retArr[i] = true;
                        break;
                    }
                    else if (priceAt <= -9)
                    {
                        retArr[i] = false;
                        break;
                    }
                }
                priceAt = 0;
            }
            return retArr;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            bool[] arr = randomWalk(1000000);
            int loseStreak = 0;
            int totalLoseStreak = 0;
            int totalCounter = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i])
                {
                    if (loseStreak > 0)
                    {
                        totalLoseStreak += loseStreak;
                        totalCounter++;
                    }
                    loseStreak = 0;
                }
                else
                {
                    loseStreak++;
                }
            }
            
            MessageBox.Show(((double)totalLoseStreak / (double)totalCounter).ToString());

            /*
            double totalTicks = 0;
            double totalDownstreak = 0;
            double downstreakNum = 0;
            double lowestTicks = 0;
            double lowestTicksTotal = 0;
            int highestTicks = 0;
            for (int xxx = 0; xxx < 1000; xxx++)
            {
                bool[] arr = randomWalk(80);
                int counter = 1;
                int i = 0;
                
                double currTicks = 0;
                lowestTicks = 0;
                double lowestCounter = 0;
                
                while (i < arr.Length)
                {

                    if (arr[i])
                    {
                        if (counter > 9)
                        {
                            highestTicks++;
                        }
                            downstreakNum++;
                            totalDownstreak += counter - 1;

                        if (counter <= 5)
                        {
                            currTicks += counter * 30 * 5;
                            totalTicks += counter * 30 * 5;
                            totalTicks -= counter * 3.5;
                        }
                        counter = 1;
                    }
                    else
                    {
                        if (counter > lowestCounter)
                        {
                            lowestCounter = counter;
                        }
                        if (counter <= 5)
                        {
                            currTicks -= counter * 9 * 5;
                            totalTicks -= counter * 9 * 5;
                            totalTicks -= counter * 3.5;
                        }
                        if (currTicks < lowestTicks)
                        {
                            lowestTicks = currTicks;
                        }
                        counter += 1;
                    }

                    i++;
                }
                lowestTicksTotal += lowestTicks;
            }
            // 450
            MessageBox.Show((totalTicks/1000).ToString());
            //MessageBox.Show((lowestTicksTotal / 1000).ToString());
            //MessageBox.Show((highestTicks/1000).ToString());
            MessageBox.Show((totalDownstreak / downstreakNum).ToString());
            //MessageBox.Show(lowestTicks.ToString());
            //MessageBox.Show(counter.ToString());
             */
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int totalProfit = 0;
            int totalLowest = 0;
            Random rand = new Random(); //reuse this if you are generating many
            for (int j = 0; j < 100000; j++)
            {
                int lowest = 10000;
                for (int i = 0; i < 37; i++) // one month of trade sequences
                {
                    double u1 = rand.NextDouble(); //these are uniform(0,1) random doubles
                    double u2 = rand.NextDouble();
                    double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                 Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                    double randNormal = 2.8356164383561643835616438356164 + 3.03862666632 * randStdNormal;
                    int randNormInt = (int)Math.Round(randNormal, 0);
                    totalProfit += profitLossFromSeq(randNormInt);
                    if (profitLossFromSeq(randNormInt) < lowest)
                        lowest = profitLossFromSeq(randNormInt);
                }
                totalLowest += lowest;
            }
            MessageBox.Show((totalProfit / 100000).ToString());
            MessageBox.Show((totalLowest / 100000).ToString());

            // Results
            // 10 for -6
            // 185 for -7 <-- what I was going to use going in to trading
            // 350 for -8
            // 475 for -9
            // 552 for -10
            // 600 for -11
            // 620 for -12
            // 630 for -13
            // 630 for -14
            // 630 for -15
            // 630 for -16
        }

        private int profitLossFromSeq(int sequenceLength)
        {   
            if (sequenceLength <= 0)
                return 20;
            if (sequenceLength == 1)
                return 33;
            if (sequenceLength == 2)
                return 40;
            if (sequenceLength == 3)
                return 41;
            if (sequenceLength == 4)
                return 31;
            if (sequenceLength == 5)
                return 19;
            if (sequenceLength == 6)
                return 0;
            if (sequenceLength == 7)
                return -22;
            if (sequenceLength == 8)
                return -55;
            if (sequenceLength == 9)
                return -95;
            /*
            if (sequenceLength == 10)
                return -140;
            if (sequenceLength == 11)
                return -192;
            if (sequenceLength == 12)
                return -250;
             
           if (sequenceLength == 13)
               return -314;
           if (sequenceLength == 14)
               return -384;
           if (sequenceLength == 15)
               return -460;
            
           double count = 0;
           for (int i = 1; i <= sequenceLength; i++)
           {
               count -= 6.5 * i;
           }
             count += 20 * (sequenceLength + 1);
             *  */
            return (0 - 6 - 6 * 2 - 6 * 3 - 6 * 4 - 6 * 5 - 6 * 6 - 6 * 7 - 6 * 8 - 6 * 9 - 6 * 10);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int result = 0;
            string line;
            System.IO.StreamReader file =
               new System.IO.StreamReader(@"E:\Visual Studio 2012\Projects\MarketForecastMartingale\MarketForecastMartingale\NQ.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("+20"))
                {
                    result += 18;
                }
                else if (line.Contains("-6"))
                {
                    result -= 6;
                }
            }
            MessageBox.Show(result.ToString());
        }

    }
}


