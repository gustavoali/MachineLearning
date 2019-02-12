using System;
using System.Collections.Generic;
namespace NaiveBayes
{
    class BayesProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nBegin Naive Bayes classification demo");
            Console.WriteLine("Goal is to predict (liberal/conservative) from job, " +
            "sex and income\n");
            string[][] rawData = new string[30][];
            rawData[0] = new string[] { "analyst", "male", "high", "conservative" };
            rawData[1] = new string[] { "barista", "female", "low", "liberal" };
            rawData[2] = new string[] { "cook", "male", "medium", "conservative" };
            rawData[3] = new string[] { "doctor", "female", "medium", "conservative" };
            rawData[4] = new string[] { "analyst", "female", "low", "liberal" };
            rawData[5] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[6] = new string[] { "analyst", "male", "medium", "conservative" };
            rawData[7] = new string[] { "cook", "female", "low", "liberal" };
            rawData[8] = new string[] { "doctor", "female", "medium", "liberal" };
            rawData[9] = new string[] { "cook", "female", "low", "liberal" };
            rawData[10] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[11] = new string[] { "cook", "female", "high", "liberal" };
            rawData[12] = new string[] { "barista", "female", "medium", "liberal" };
            rawData[13] = new string[] { "analyst", "male", "low", "liberal" };
            rawData[14] = new string[] { "doctor", "female", "high", "conservative" };
            rawData[15] = new string[] { "barista", "female", "medium", "conservative" };
            rawData[16] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[17] = new string[] { "barista", "male", "high", "conservative" };
            rawData[18] = new string[] { "doctor", "female", "medium", "liberal" };
            rawData[19] = new string[] { "analyst", "male", "low", "liberal" };
            rawData[20] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[21] = new string[] { "cook", "male", "medium", "conservative" };
            rawData[22] = new string[] { "doctor", "female", "high", "conservative" };
            rawData[23] = new string[] { "analyst", "male", "high", "conservative" };
            rawData[24] = new string[] { "barista", "female", "medium", "liberal" };
            rawData[25] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[26] = new string[] { "analyst", "female", "medium", "conservative" };
            rawData[27] = new string[] { "analyst", "male", "medium", "conservative" };
            rawData[28] = new string[] { "doctor", "female", "medium", "liberal" };
            rawData[29] = new string[] { "barista", "male", "medium", "conservative" };
            Console.WriteLine("The raw data is: \n");
            ShowData(rawData, 5, true);
            Console.WriteLine("Splitting data into 80%-20% train and test sets");
            string[][] trainData;
            string[][] testData;
            MakeTrainTest(rawData, 15, out trainData, out testData); // seed = 15 is nice
            Console.WriteLine("Done \n");
            Console.WriteLine("Training data: \n");
            ShowData(trainData, 5, true);
            Console.WriteLine("Test data: \n");
            ShowData(testData, 5, true);
            Console.WriteLine("Creating Naive Bayes classifier object");
            Console.WriteLine("Training classifier using training data");
            BayesClassifier bc = new BayesClassifier();
            bc.Train(trainData); // compute key count data structures
            Console.WriteLine("Done \n");
            double trainAccuracy = bc.Accuracy(trainData);
            Console.WriteLine("Accuracy of model on train data = " +
            trainAccuracy.ToString("F4"));
            double testAccuracy = bc.Accuracy(testData);
            Console.WriteLine("Accuracy of model on test data = " +
            testAccuracy.ToString("F4"));
            Console.WriteLine("\nPredicting politics for job = barista, sex = female, "
            + "income = medium \n");
            string[] features = new string[] { "barista", "female", "medium" };
            string liberal = "liberal";
            double pLiberal = bc.Probability(liberal, features);
            Console.WriteLine("Probability of liberal = " +
            pLiberal.ToString("F4"));
            string conservative = "conservative";
            double pConservative = bc.Probability(conservative, features);
            Console.WriteLine("Probability of conservative = " +
            pConservative.ToString("F4"));
            Console.WriteLine("\nEnd Naive Bayes classification demo\n");
            Console.ReadLine();
        } // Main
        static void MakeTrainTest(string[][] allData, int seed,
        out string[][] trainData, out string[][] testData)
        {
            Random rnd = new Random(seed);
            int totRows = allData.Length;
            int numTrainRows = (int)(totRows * 0.80);
            int numTestRows = totRows - numTrainRows;
            trainData = new string[numTrainRows][];
            testData = new string[numTestRows][];
            string[][] copy = new string[allData.Length][]; // ref copy of all data
            for (int i = 0; i < copy.Length; ++i)
                copy[i] = allData[i];
            for (int i = 0; i < copy.Length; ++i) // scramble order
            {
                int r = rnd.Next(i, copy.Length);
                string[] tmp = copy[r];
                copy[r] = copy[i];
                copy[i] = tmp;
            }
            for (int i = 0; i < numTrainRows; ++i)
                trainData[i] = copy[i];
            for (int i = 0; i < numTestRows; ++i)
                testData[i] = copy[i + numTrainRows];
        } // MakeTrainTest
        static void ShowData(string[][] rawData, int numRows, bool indices)
        {
            for (int i = 0; i < numRows; ++i)
            {
                if (indices == true)
                    Console.Write("[" + i.ToString().PadLeft(2) + "] ");
                for (int j = 0; j < rawData[i].Length; ++j)
                {
                    string s = rawData[i][j];
                    Console.Write(s.PadLeft(14) + " ");
                }
                Console.WriteLine("");
            }
            if (numRows != rawData.Length - 1)
                Console.WriteLine(". . .");
            int lastRow = rawData.Length - 1;
            if (indices == true)
                Console.Write("[" + lastRow.ToString().PadLeft(2) + "] ");
            for (int j = 0; j < rawData[lastRow].Length; ++j)
            {
                string s = rawData[lastRow][j];
                Console.Write(s.PadLeft(14) + " ");
            }
            Console.WriteLine("\n");
        }
        static double[] MakeIntervals(double[] data, int numBins) // bin numeric data
        {
            double max = data[0]; // find min & max
            double min = data[0];
            for (int i = 0; i < data.Length; ++i)
            {
                if (data[i] < min) min = data[i];
                if (data[i] > max) max = data[i];
            }
            double width = (max - min) / numBins; // compute width
            double[] intervals = new double[numBins * 2]; // intervals
            intervals[0] = min;
            intervals[1] = min + width;
            for (int i = 2; i < intervals.Length - 1; i += 2)
            {
                intervals[i] = intervals[i - 1];
                intervals[i + 1] = intervals[i] + width;
            }
            intervals[0] = double.MinValue; // outliers
            intervals[intervals.Length - 1] = double.MaxValue;
            return intervals;
        }
        static int Bin(double x, double[] intervals)
        {
            for (int i = 0; i < intervals.Length - 1; i += 2)
            {
                if (x >= intervals[i] && x < intervals[i + 1])
                    return i / 2;
            }
            return -1; // error
        }
    } // Program
    public class BayesClassifier
    {
        private Dictionary<string, int>[] stringToInt; // "male" -> 0, etc.
        private int[][][] jointCounts; // [feature][value][dependent]
        private int[] dependentCounts;
        public BayesClassifier()
        {
            this.stringToInt = null; // need training data to know size
            this.jointCounts = null; // need training data to know size
            this.dependentCounts = null; // need training data to know size
        }
        public void Train(string[][] trainData)
        {
            // 1. scan training data and construct one dictionary per column
            int numRows = trainData.Length;
            int numCols = trainData[0].Length;
            this.stringToInt = new Dictionary<string, int>[numCols]; // allocate array
            for (int col = 0; col < numCols; ++col) // including y-values
            {
                stringToInt[col] = new Dictionary<string, int>(); // instantiate Dictionary
                int idx = 0;
                for (int row = 0; row < numRows; ++row) // each row of curr column
                {
                    string s = trainData[row][col];
                    if (stringToInt[col].ContainsKey(s) == false) // first time seen
                    {
                        stringToInt[col].Add(s, idx); // ex: analyst -> 0
                        ++idx;
                    }
                } // each row
            } // each col
              // 2. scan and count using stringToInt Dictionary
            this.jointCounts = new int[numCols - 1][][]; // do not include the y-value
                                                         // a. allocate second dim
            for (int c = 0; c < numCols - 1; ++c) // each feature column but not y-column
            {
                int count = this.stringToInt[c].Count; // number possible values for column
                jointCounts[c] = new int[count][];
            }
            // b. allocate last dimension = always 2 for binary classification
            for (int i = 0; i < jointCounts.Length; ++i)
                for (int j = 0; j < jointCounts[i].Length; ++j)
                {
                    //int numDependent = stringToInt[stringToInt.Length - 1].Count;

                    //jointCounts[i][j] = new int[numDependent];
                    jointCounts[i][j] = new int[2]; // binary classification
                }
            // c. init joint counts with 1 for Laplacian smoothing
            for (int i = 0; i < jointCounts.Length; ++i)
                for (int j = 0; j < jointCounts[i].Length; ++j)
                    for (int k = 0; k < jointCounts[i][j].Length; ++k)
                        jointCounts[i][j][k] = 1;
            // d. compute joint counts
            for (int i = 0; i < numRows; ++i)
            {
                string yString = trainData[i][numCols - 1]; // dependent value
                int depIndex = stringToInt[numCols - 1][yString]; // corresponding index
                for (int j = 0; j < numCols - 1; ++j)
                {
                    int attIndex = j;
                    string xString = trainData[i][j]; // an attribute value like "male"
                    int valIndex = stringToInt[j][xString]; // corresponding integer like 0
                    ++jointCounts[attIndex][valIndex][depIndex];
                }
            }
            // 3. scan and count number of each of the 2 dependent values
            this.dependentCounts = new int[2]; // binary
            for (int i = 0; i < dependentCounts.Length; ++i) // Laplacian init
                dependentCounts[i] = numCols - 1; // numCols - 1 = num features
            for (int i = 0; i < trainData.Length; ++i)
            {
                string yString = trainData[i][numCols - 1]; // conservative or liberal
                int yIndex = stringToInt[numCols - 1][yString]; // 0 or 1
                ++dependentCounts[yIndex];
            }
            return; // the trained 'model' is jointCounts and dependentCounts
        } // Train
        public double Probability(string yValue, string[] xValues)
        {
            int numFeatures = xValues.Length; // ex: 3 (job, sex, income)
            double[][] conditionals = new double[2][]; // binary
            for (int i = 0; i < 2; ++i)
                conditionals[i] = new double[numFeatures]; // ex: P('doctor' | conservative)
            double[] unconditionals = new double[2]; // ex: P('conservative'), P('liberal')
                                                     // convert strings to ints
            int y = this.stringToInt[numFeatures][yValue];
            int[] x = new int[numFeatures];
            for (int i = 0; i < numFeatures; ++i)
            {
                string s = xValues[i];
                x[i] = this.stringToInt[i][s];
            }

            // compute conditionals
            for (int k = 0; k < 2; ++k) // each y-value
            {
                for (int i = 0; i < numFeatures; ++i)
                {
                    int attIndex = i;
                    int valIndex = x[i];
                    int depIndex = k;
                    conditionals[k][i] =
                    (jointCounts[attIndex][valIndex][depIndex] * 1.0) /
                    dependentCounts[depIndex];
                }
            }
            // compute unconditionals
            int totalDependent = 0; // ex: count(conservative) + count(liberal)
            for (int k = 0; k < 2; ++k)
                totalDependent += this.dependentCounts[k];
            for (int k = 0; k < 2; ++k)
                unconditionals[k] = (dependentCounts[k] * 1.0) / totalDependent;
            // compute partials
            double[] partials = new double[2];
            for (int k = 0; k < 2; ++k)
            {
                partials[k] = 1.0; // because we are multiplying
                for (int i = 0; i < numFeatures; ++i)
                    partials[k] *= conditionals[k][i];
                partials[k] *= unconditionals[k];
            }
            // evidence = sum of partials
            double evidence = 0.0;
            for (int k = 0; k < 2; ++k)
                evidence += partials[k];
            return partials[y] / evidence;
        } // Probability
        public double Accuracy(string[][] data)
        {
            int numCorrect = 0;
            int numWrong = 0;
            int numRows = data.Length;
            int numCols = data[0].Length;
            for (int i = 0; i < numRows; ++i) // row
            {
                string yValue = data[i][numCols - 1]; // assumes y in last column
                string[] xValues = new string[numCols - 1];
                Array.Copy(data[i], xValues, numCols - 1);
                double p = this.Probability(yValue, xValues);
                if (p > 0.50)
                    ++numCorrect;
                else
                    ++numWrong;
            }
            return (numCorrect * 1.0) / (numCorrect + numWrong);
        }
    } // class BayesClassifier
} // ns