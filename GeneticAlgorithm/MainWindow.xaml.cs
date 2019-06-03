using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GeneticAlgorithm
{
    public partial class MainWindow : Window
    {
        string target = "Ez a cél szöveg amit megkell találni";
        string characters = "aábcdeéfghijklmnoóöőpqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,.|!#$%&/()=? ";
        //int target = 500;

        int populationSize = 200;
        float mutationRate = 0.01f;
        int elitism = 5;
        Random random = new Random();
        GeneticAlgorithm<char> ga; // <--- Mivel T típusú ezért itt kell módosítani először.
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
        }
        public void Timer()
        {                                                  //Gének száma
            ga = new GeneticAlgorithm<char>(populationSize, target.Length, random, getRandomChar, FitnessFunction, elitism, mutationRate);

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 0);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!(ga.BestFitness == 1))
            {
                ga.NewGeneration(); //Indit

                //Sum_FrontEnd();
                Letter_FrontEnd();

                string s = "";
                for (int i = 0; i < ga.BestGenes.Length; i++)
                {
                    s += ga.BestGenes[i];
                }
                listBox.Items.Add(s + "  Gen: " + ga.Generation + ", " + " Fitness: " + ga.BestFitness.ToString());

                Border border = (Border)VisualTreeHelper.GetChild(listBox, 0);
                ScrollViewer vmi = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                vmi.ScrollToBottom();

                generationText.Content = ga.Generation.ToString();
                fitnessText.Content = ga.BestFitness.ToString();
            }
            else
            {
                dispatcherTimer.Stop();
            }
        }

        private char getRandomChar() // Ezt és...-->
        {
            //return Sum_getRandomItem();
            return Letters_getRandomItem();
        }
        private float FitnessFunction(int index) //--> ezt módosítod, és bármire jó az AI
        {
            float score = 0;
            //score = Sum_FitnessFunction(index);
            score = Letters_FitnessFunction(index);
            return score;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Timer();
        }

        private char Letters_getRandomItem()
        {
            int i = random.Next(characters.Length);
            return characters[i];
        }

        private float Letters_FitnessFunction(int index)
        {
            DNA<char> dna = ga.Population[index]; // <--- T típióusra módosítani
            float score = 0;

            for (int i = 0; i < dna.Genes.Length; i++)
            {
                if (dna.Genes[i] == target[i])
                {
                    score += 1;
                }

            }
            score /= target.Length;
            score = (float)(Math.Pow(2, score) - 1) / (2 - 1);
            return score;
        }

        private void Letter_FrontEnd()
        {
            targetText.Content = target;
            string s = "";
            for (int i = 0; i < ga.BestGenes.Length; i++)
            {
                s += ga.BestGenes[i];
            }
            actualText.Content = s;
        }

        //Sum
        //private void Sum_FrontEnd()
        //{
        //    targetText.Content = target;
        //    string actualString = "";
        //    int CorrectSum = 0;
        //    foreach (int item in ga.BestGenes)
        //    {
        //        CorrectSum += item;
        //    }
        //    for (int i = 0; i < ga.BestGenes.Length; i++)
        //    {
        //        actualString += ga.BestGenes[i] + "+";
        //    }
        //    actualString += "Sum= " + CorrectSum;
        //    actualText.Content = actualString;
        //}
        //private int Sum_getRandomItem()
        //{
        //    return random.Next(500);
        //}

        //private float Sum_FitnessFunction(int index)
        //{
        //    DNA<int> dna = ga.Population[index];

        //    float score = 0;
        //    int sum = 0;
        //    for (int i = 0; i < dna.Genes.Length; i++)
        //    {
        //        sum += dna.Genes[i];
        //    }
        //    float vmi = (float)Math.Abs(target - sum) / 10f;
        //    score = 1 - vmi;
        //    return score;
        //}

    }
}


