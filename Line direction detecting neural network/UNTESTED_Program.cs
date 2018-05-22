using System;
using System.IO;

namespace Line_direction_detecting_neural_network
{
    class Program
    {
        int[,,] straightLineFilters = new int[3, 3, 513];
        int[,,] diagonalLineFilters = new int[3, 3, 513];
        int[] straightSignificance = new int[513];
        int[] diagonalSignificance = new int[513];
        int numOfImages = 10;
        int straightLineScore = 0;
        int diagonalLineScore = 0;
        string finalDirection = "";
        static void Main(string[] args)
        {
            Console.WriteLine("TreacherousToast's line orientation detecting neural network v0.2");
            Console.WriteLine("If you are using this algorithm, please leave a link to its GitHub page");
            Console.WriteLine();
            for (int i = 0; i < 513; i++)
            {
                straightSignificance[i] = 1;
                diagonalSignificance[i] = 1;
            }
            for (int i = 0; i < 100; i++)
            {
                straightLineFilters[1, 1, i] = 2;
                straightLineFilters[0, 1, i] = 2;
                straightLineFilters[2, 1, i] = 2;
                straightLineFilters[0, 0, i] = 2;
                straightLineFilters[1, 0, i] = 2;
                straightLineFilters[2, 0, i] = 2;
                straightLineFilters[0, 2, i] = 2;
                straightLineFilters[1, 2, i] = 2;
                straightLineFilters[2, 2, i] = 2;
            }
            for (int i = 0; i < 100; i++)
            {
                diagonalLineFilters[1, 1, i] = 2;
                diagonalLineFilters[0, 1, i] = 2;
                diagonalLineFilters[2, 1, i] = 2;
                diagonalLineFilters[0, 0, i] = 2;
                diagonalLineFilters[1, 0, i] = 2;
                diagonalLineFilters[2, 0, i] = 2;
                diagonalLineFilters[0, 2, i] = 2;
                diagonalLineFilters[1, 2, i] = 2;
                diagonalLineFilters[2, 2, i] = 2;
            }            
            trainStraightLineFilters();
            trainDiagonalLineFilters();
            Console.WriteLine("Direction | Diagonal score | Straight score");
            for (int i = 0; i < 10; i++)
            {
                checkImage(i+".txt");
                printToScreen();
            }
        }
        public void printToScreen()
        {
            Console.WriteLine(finalDirection + "\t"+diagonalLineScore+"\t\t"+straightLineScore);
            straightLineScore = 0;
            diagonalLineScore = 0;
        }
        
        public void trainStraightLineFilters()
        {
            int amountOfArrayWrittenTo = 0;
            for (int i = 0; i < (numOfImages / 2); i++)
            {
                string[] rawImage = File.ReadAllLines(i + ".txt");
                int[,] unpackedImage = new int[6, 6];
                for (int a = 0; a < 6; a++)
                {
                    string currLine = rawImage[a];
                    for (int j = 0; j < 6; j++)
                    {
                        unpackedImage[j, a] = Convert.ToInt32(currLine[j]);
                    }
                }
                bool filterExists = false;
                for (int a = 1; a < 5; a++)
                {
                    for (int b = 1; b < 5; b++)
                    {
                        filterExists = false;
                        for (int c = 0; c < 513; c++)
                        {
                            if (straightLineFilters[1,1, c] == unpackedImage[b, a]&&straightLineFilters[1,2, c] == unpackedImage[b, a + 1]&&straightLineFilters[1,0, c] == unpackedImage[b, a - 1]&&straightLineFilters[2,1, c] == unpackedImage[b + 1, a]&&straightLineFilters[0,1, c] == unpackedImage[b - 1, a]&&straightLineFilters[2,2, c] == unpackedImage[b + 1, a + 1]&&straightLineFilters[0,0, c] == unpackedImage[b - 1, a - 1]&&straightLineFilters[2,0, c] == unpackedImage[b + 1, a - 1]&&straightLineFilters[0,2, c] == unpackedImage[b - 1, a + 1]){
                                straightSignificance[c]++;
                                filterExists = true;
                                break;
                            }
                        }
                        if (filterExists == false)
                        {
                            straightLineFilters[1,1, amountOfArrayWrittenTo] = unpackedImage[b, a];
                            straightLineFilters[1,2, amountOfArrayWrittenTo] = unpackedImage[b, a + 1];
                            straightLineFilters[1,0, amountOfArrayWrittenTo] = unpackedImage[b, a - 1];
                            straightLineFilters[2,1, amountOfArrayWrittenTo] = unpackedImage[b + 1, a];
                            straightLineFilters[0,1, amountOfArrayWrittenTo] = unpackedImage[b - 1, a];
                            straightLineFilters[2,2, amountOfArrayWrittenTo] = unpackedImage[b + 1, a + 1];
                            straightLineFilters[0,0, amountOfArrayWrittenTo] = unpackedImage[b - 1, a - 1];
                            straightLineFilters[2,0, amountOfArrayWrittenTo] = unpackedImage[b + 1, a - 1];
                            straightLineFilters[0,2, amountOfArrayWrittenTo] = unpackedImage[b - 1, a + 1];
                            amountOfArrayWrittenTo++;
                        }
                    }
                }
            }
        }
        
        public void trainDiagonalLineFilters()
        {
            int amountOfArrayWrittenTo = 0;
            for (int i = (numOfImages/2); i < numOfImages; i++)
            {
                string[] rawImage = File.ReadAllLines(i + ".txt");
                int[,] unpackedImage = new int[6, 6];
                for (int a = 0; a < 6; a++)
                {
                    string currLine = rawImage[a];
                    for (int j = 0; j < 6; j++)
                    {
                        unpackedImage[j, a] = Convert.ToInt32(currLine[j]);
                    }
                }
                bool filterExists = false;
                for (int a = 1; a < 5; a++)
                {
                    for (int b = 1; b < 5; b++)
                    {
                        filterExists = false;
                        for (int c = 0; c < 513; c++)
                        {
                            if (diagonalLineFilters[1,1, c] == unpackedImage[b, a]&&diagonalLineFilters[1,2, c] == unpackedImage[b, a + 1]&&diagonalLineFilters[1,0, c] == unpackedImage[b, a - 1]&&diagonalLineFilters[2,1, c] == unpackedImage[b + 1, a]&&diagonalLineFilters[0,1, c] == unpackedImage[b - 1, a]&&diagonalLineFilters[2,2, c] == unpackedImage[b + 1, a + 1]&&diagonalLineFilters[0,0, c] == unpackedImage[b - 1, a - 1]&&diagonalLineFilters[2,0, c] == unpackedImage[b + 1, a - 1]&&diagonalLineFilters[0,2, c] == unpackedImage[b - 1, a + 1]){
                                diagonalSignificance[c]++;
                                filterExists = true;
                                break;
                            }
                        }
                        if (filterExists == false)
                        {
                            diagonalLineFilters[1,1, amountOfArrayWrittenTo] = unpackedImage[b, a];
                            diagonalLineFilters[1,2, amountOfArrayWrittenTo] = unpackedImage[b, a + 1];
                            diagonalLineFilters[1,0, amountOfArrayWrittenTo] = unpackedImage[b, a - 1];
                            diagonalLineFilters[2,1, amountOfArrayWrittenTo] = unpackedImage[b + 1, a];
                            diagonalLineFilters[0,1, amountOfArrayWrittenTo] = unpackedImage[b - 1, a];
                            diagonalLineFilters[2,2, amountOfArrayWrittenTo] = unpackedImage[b + 1, a + 1];
                            diagonalLineFilters[0,0, amountOfArrayWrittenTo] = unpackedImage[b - 1, a - 1];
                            diagonalLineFilters[2,0, amountOfArrayWrittenTo] = unpackedImage[b + 1, a - 1];
                            diagonalLineFilters[0,2, amountOfArrayWrittenTo] = unpackedImage[b - 1, a + 1];
                            amountOfArrayWrittenTo++;
                        }
                    }
                }

            }
        }        
        
        public void checkImage(string fileName)
        {
            string[] rawImage = File.ReadAllLines(fileName);
            int[,] unpackedImage = new int[6, 6];
            for (int a = 0; a < 6; a++)
                {
                    string currLine = rawImage[a];
                    for (int j = 0; j < 6; j++)
                    {
                        unpackedImage[j, a] = Convert.ToInt32(currLine[j]);
                    }
                }
            for (int i = 1; i < 5; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    for (int a = 0; a < 513; a++)
                    {
                        if (unpackedImage[j, i] == straightLineFilters[1, 1, a] && unpackedImage[j, i + 1] == straightLineFilters[1, 2, a] && unpackedImage[j, i - 1] == straightLineFilters[1, 0, a] && unpackedImage[j + 1, i] == straightLineFilters[2, 1, a] && unpackedImage[j - 1, i] == straightLineFilters[0, 1, a] && unpackedImage[j + 1, i + 1] == straightLineFilters[2, 2, a] && unpackedImage[j - 1, i - 1] == straightLineFilters[0, 0, a] && unpackedImage[j + 1, i - 1] == straightLineFilters[2, 0, a] && unpackedImage[j - 1, i + 1] == straightLineFilters[0, 2, a])
                        {
                            straightLineScore+=straightSignificance[a];
                        }
                        if (unpackedImage[j, i] == diagonalLineFilters[1, 1, a] && unpackedImage[j, i + 1] == diagonalLineFilters[1, 2, a] && unpackedImage[j, i - 1] == diagonalLineFilters[1, 0, a] && unpackedImage[j + 1, i] == diagonalLineFilters[2, 1, a] && unpackedImage[j - 1, i] == diagonalLineFilters[0, 1, a] && unpackedImage[j + 1, i + 1] == diagonalLineFilters[2, 2, a] && unpackedImage[j - 1, i - 1] == diagonalLineFilters[0, 0, a] && unpackedImage[j + 1, i - 1] == diagonalLineFilters[2, 0, a] && unpackedImage[j - 1, i + 1] == diagonalLineFilters[0, 2, a])
                        {
                            diagonalLineScore+=diagonalSignificance[a];
                        }
                    }
                }
            }
            if (straightLineScore > diagonalLineScore)
            {
                finalDirection = "Straight";
            }
            else if (straightLineScore < diagonalLineScore)
            {
                finalDirection = "Diagonal";
            }
        }
    }
} 
