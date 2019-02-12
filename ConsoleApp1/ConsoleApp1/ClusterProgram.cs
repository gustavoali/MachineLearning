using System;
using System.Linq;

namespace ClusterNumeric
{
    class ClusterProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Empieza la demo del algoritmo k-means clustering\n");

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

            Console.WriteLine("\nDatos sin agrupar\n");
            Console.WriteLine("ID    Altura (pulg.)  Peso (kg.)");
            Console.WriteLine("------------------------\n");

            ShowData(rawData, 1);

            int numClusters = 3;

            Console.WriteLine("\nEstableciendo la cantidad de grupos en " + numClusters);
            
            Console.WriteLine("Comenzando el agrupamiento usando el algoritmo k-means");

            Clusterer c = new Clusterer(numClusters);
            int[] clustering = c.Cluster(rawData);

            Console.WriteLine("Proceso de agrupamiento finalizado\n");
            Console.WriteLine("Agrupamiento final:");

            ShowVector(clustering);

            Console.WriteLine("Datos por grupo:\n");

            ShowClustered(rawData, clustering, numClusters, 1);

            Console.WriteLine("\nFinalización de la demo de agrupamiento con el algoritmo k-means\n");
            Console.ReadLine();
        }

        static void ShowData(double[][] data, int decimals, bool indexs, bool newLine)
        {
            for (int i = 0; i < data.Length; ++i)
            {
                if (indexs)
                    Console.Write(i.ToString().PadLeft(0) + string.Concat(Enumerable.Repeat(" ",10)));
                for (int j = 0; j < data[i].Length; ++j)
                {
                    double v = data[i][j];
                    Console.Write(v.ToString("F" + decimals) + string.Concat(Enumerable.Repeat(" ", 10)));
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

        static void ShowVector(int[] vector, bool newLine)
        {
            for (int i = 0; i < vector.Length; ++i)
                Console.Write(vector[i] + " ");
            if (newLine == true)
                Console.WriteLine("\n");
        }
        static void ShowVector(int[] vector)
        {
            ShowVector(vector, true);
        }

        static void ShowClustered(double[][] data, int[] clustering, int numClusters, int decimals)
        {
            for (int k = 0; k < numClusters; ++k)
            {
                Console.WriteLine("===================");
                for (int i = 0; i < data.Length; ++i)
                {
                    int clusterID = clustering[i];
                    if (clusterID != k) continue;
                    Console.Write(i.ToString().PadLeft(3) + " ");
                    for (int j = 0; j < data[i].Length; ++j)
                    {
                        double v = data[i][j];
                        Console.Write(v.ToString("F" + decimals) + " ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("===================");
            } // k
        }

        public class Clusterer
        {
            private int numClusters;
            private int[] clustering;
            private double[][] centroids;
            private Random rnd;

            public Clusterer(int numClusters)
            {
                this.numClusters = numClusters;
                this.centroids = new double[numClusters][];
                this.rnd = new Random(0);
            }

            public int[] Cluster(double[][] data)
            {
                int numTuples = data.Length;
                int numValues = data[0].Length;
                this.clustering = new int[numTuples];
                for (int k = 0; k < numClusters; ++k)
                    this.centroids[k] = new double[numValues];
                InitRandom(data);
                Console.WriteLine("\nAgrupamiento aleatorio inicial:");
                for (int i = 0; i < clustering.Length; ++i)
                    Console.Write(clustering[i] + " ");
                Console.WriteLine("\n");
                bool changed = true; // change in clustering?
                int maxCount = numTuples * 10; // sanity check
                int ct = 0;
                while (changed == true && ct < maxCount)
                {
                    ++ct;
                    UpdateCentroids(data);
                    changed = UpdateClustering(data);
                }
                int[] result = new int[numTuples];
                Array.Copy(this.clustering, result, clustering.Length);
                return result;
            }

            private void InitRandom(double[][] data)
            {
                int numTuples = data.Length;
                int clusterID = 0;
                for (int i = 0; i < numTuples; ++i)
                {
                    clustering[i] = clusterID++;
                    if (clusterID == numClusters)
                        clusterID = 0;
                }
                for (int i = 0; i < numTuples; ++i)
                {
                    int r = rnd.Next(i, clustering.Length);
                    int tmp = clustering[r];
                    clustering[r] = clustering[i];
                    clustering[i] = tmp;
                }
            }

            private void UpdateCentroids(double[][] data)
            {
                int[] clusterCounts = new int[numClusters];
                for (int i = 0; i < data.Length; ++i)
                {
                    int clusterID = clustering[i];
                    ++clusterCounts[clusterID];
                }
                // zero-out this.centroids so it can be used as scratch
                for (int k = 0; k < centroids.Length; ++k)
                    for (int j = 0; j < centroids[k].Length; ++j)
                        centroids[k][j] = 0.0;
                for (int i = 0; i < data.Length; ++i)
                {
                    int clusterID = clustering[i];
                    for (int j = 0; j < data[i].Length; ++j)
                        centroids[clusterID][j] += data[i][j]; // accumulate sum
                }
                for (int k = 0; k < centroids.Length; ++k)
                    for (int j = 0; j < centroids[k].Length; ++j)
                        centroids[k][j] /= clusterCounts[k]; // danger?
            }

            private bool UpdateClustering(double[][] data)
            {
                // (re)assign each tuple to a cluster (closest centroid)
                // returns false if no tuple assignments change OR
                // if the reassignment would result in a clustering where
                // one or more clusters have no tuples.
                bool changed = false; // did any tuple change cluster?
                int[] newClustering = new int[clustering.Length]; // proposed result
                Array.Copy(clustering, newClustering, clustering.Length);
                double[] distances = new double[numClusters]; // from tuple to centroids
                for (int i = 0; i < data.Length; ++i) // walk through each tuple
                {
                    for (int k = 0; k < numClusters; ++k)
                        distances[k] = Distance(data[i], centroids[k]);
                    int newClusterID = MinIndex(distances); // find closest centroid
                    if (newClusterID != newClustering[i])
                    {
                        changed = true; // note a new clustering
                        newClustering[i] = newClusterID; // accept update
                    }
                }
                if (changed == false)
                    return false; // no change so bail
                                  // check proposed clustering cluster counts
                int[] clusterCounts = new int[numClusters];
                for (int i = 0; i < data.Length; ++i)
                {
                    int clusterID = newClustering[i];
                    ++clusterCounts[clusterID];
                }
                for (int k = 0; k < numClusters; ++k)
                    if (clusterCounts[k] == 0)
                        return false;

                // bad clustering
                // alternative: place a random data item into empty cluster
                // for (int k = 0; k < numClusters; ++k)
                // {
                // if (clusterCounts[k] == 0) // cluster k has no items
                // {
                // for (int t = 0; t < data.Length; ++t) // find a tuple to put into cluster k
                // {}
                // int cid = newClustering[t]; // cluster of t
                // int ct = clusterCounts[cid]; // how many items are there?
                // if (ct >= 2) // t is in a cluster w/ 2 or more items
                // {
                // newClustering[t] = k; // place t into cluster k
                // ++clusterCounts[k]; // k now has a data item
                // --clusterCounts[cid]; // cluster that used to have t
                // break; // check next cluster
                // }
                // } // t
                // } // cluster count of 0
                // } // k
                Array.Copy(newClustering, clustering, newClustering.Length); // update
                return true; // good clustering and at least one change
            }

            private static double Distance(double[] tuple, double[] centroid)
            {
                // Euclidean distance between two vectors for UpdateClustering()
                double sumSquaredDiffs = 0.0;
                for (int j = 0; j < tuple.Length; ++j)
                    sumSquaredDiffs += (tuple[j] - centroid[j]) * (tuple[j] - centroid[j]);
                return Math.Sqrt(sumSquaredDiffs);
            }

            private static int MinIndex(double[] distances)
            {
                // helper for UpdateClustering() to find closest centroid
                int indexOfMin = 0;
                double smallDist = distances[0];
                for (int k = 1; k < distances.Length; ++k)
                {
                    if (distances[k] < smallDist)
                    {
                        smallDist = distances[k];
                        indexOfMin = k;
                    }
                }
                return indexOfMin;
            }

            static double[][] LoadData(string dataFile, int numRows, int numCols, char delimit)
            {
                System.IO.FileStream ifs = new System.IO.FileStream(dataFile, System.IO.FileMode.Open);
                System.IO.StreamReader sr = new System.IO.StreamReader(ifs);
                string line = "";
                string[] tokens = null;
                int i = 0;
                double[][] result = new double[numRows][];
                while ((line = sr.ReadLine()) != null)
                {
                    result[i] = new double[numCols];
                    tokens = line.Split(delimit);
                    for (int j = 0; j < numCols; ++j)
                        result[i][j] = double.Parse(tokens[j]);
                    ++i;
                }
                sr.Close();
                ifs.Close();
                return result;
            }
        }
    }
}

