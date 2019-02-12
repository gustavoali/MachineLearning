using System;

namespace ClusterNumeric
{
    class ClusterProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Empieza la demo del algoritmo k-means clustering");
            Console.WriteLine();

            double[][] rawData = new double[10][];
            rawData[0] = new double[] { 73, 72.6 };
            rawData[1] = new double[] { 61, 54.4 };
            rawData[2] = new double[] { 67, 99.9 };
            rawData[3] = new double[] { 68, 97.3 };
            rawData[4] = new double[] { 62, 59 };
            rawData[5] = new double[] { 75, 81.6 };
            rawData[6] = new double[] { 74, 77.1 };
            rawData[7] = new double[] { 66, 97.3 };
            rawData[8] = new double[] { 68, 93.3 };
            rawData[9] = new double[] { 61, 59 };

            Console.WriteLine();
            Console.WriteLine("Datos sin agrupar");
            Console.WriteLine();
            Console.WriteLine("ID    Altura (pulg.)  Peso (kg.)");
            Console.WriteLine("------------------------");
            Console.WriteLine();

            ShowData(rawData, 1);

            int numClusters = 3;

            Console.WriteLine();
            Console.WriteLine("Estableciendo la cantidad de agrupamientos en " + numClusters);
            
            Console.WriteLine("Comenzando el agrupamiento usando el algoritmo k-means");

            Clusterer c = new Clusterer(numClusters);
            int[] clustering = c.Cluster(rawData);

            Console.WriteLine("Agrupamiento completo");
            Console.WriteLine("Agrupamiento final:");

            ShowVector(clustering);

            Console.WriteLine("Datos por agrupamiento:\n");

            ShowClustered(rawData, clustering, numClusters, 1);

            Console.WriteLine("\nEnd k-means clustering demo\n");
            Console.ReadLine();
        }

        static void ShowData(double[][] data, int decimals, bool indexs, bool newLine)
        {
            for (int i = 0; i < data.Length; ++i)
            {
                if (indexs)
                    Console.Write(i.ToString().PadLeft(3) + " ");
                for (int j = 0; j < data[i].Length; ++j)
                {
                    double v = data[i][j];
                    Console.Write(v.ToString("F" + decimals) + " ");
                }
                Console.WriteLine("");
            }
            if (newLine)
                Console.WriteLine("");
        }
 
        static void ShowData(double[][] data, int decimals)
        {
            ShowData(data, decimals, true, true);
        }
        static void ShowVector(int[] vector, bool newLine) { }
        static void ShowVector(int[] vector)
        {
            ShowVector(vector, true);
        }
        static void ShowClustered(double[][] data, int[] clustering, int numClusters, int decimals) { }
        public class Clusterer
        {
            public Clusterer(int numClusters) { }
            public int[] Cluster(double[][] data)
            {
                return new int[10];
            }
        }
    }
}

