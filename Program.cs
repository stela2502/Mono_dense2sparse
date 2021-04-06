
using System;
using System.IO;
using System.Linq;

namespace Mono_dense2sparse
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args.Length != 2)
            //{
            //    Console.WriteLine("usage: \nMono_dense2sparse.exe <denseMatrix> <outputPrefix>");
            //    return;
            //}
            Console.WriteLine("Mono_dense2sparse converts all dense comma or tab separated files in the input path to triplets txt files");
            Console.WriteLine("Enter input folder path");
            string inputFolderPath = Console.ReadLine();
            string[] files = Directory.GetFiles(inputFolderPath, "*.csv");
            foreach (string path in files)
            {
                Console.WriteLine("Processing file " + path);
                ReadCSV(path);
            }
            System.Environment.Exit(1);
        }

        private static void ReadCSV(string PATH)
        {
            string OPATH = PATH.Substring(0, PATH.LastIndexOf('.'));
            System.IO.StreamReader file = File.OpenText(PATH);
            string line = null;
            if (! Directory.Exists(OPATH))
            {
                DirectoryInfo di = Directory.CreateDirectory(OPATH);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(OPATH));
            }

            StreamWriter Genefile = File.CreateText(Path.Combine(OPATH, "features.tsv"));
            StreamWriter Mfile = File.CreateText(Path.Combine(OPATH, "matrix.mtx"));
            int i = 0;
            int nData = 0;
            int nGene = 0;
            int nCell = 0;
            Mfile.WriteLine("%%MatrixMarket matrix coordinate integer general");
            Mfile.WriteLine("%");
            Mfile.WriteLine("%MISSING INFO: 'gene count' 'cell count' 'value count'");
            while ((line = file.ReadLine()) != null)
            {
                string[] Xval = line.Split(new char[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (i == 0)
                {
                    //%%MatrixMarket matrix coordinate integer general
                    //%
                    //16938 65662 83159538
                    //2 240 1

                    //public static System.IO.StreamReader Cellfile (OPATH+"Cells.txt" );

                    File.WriteAllLines(Path.Combine(OPATH, "barcodes.tsv"),
                                       Xval.ToArray());
                    nGene  = Xval.ToArray().Length;

                }
                else
                {
                    Genefile.WriteLine(Xval[0] + "\t" + Xval[0] + "Gene Expression");
                    //Console.WriteLine("got  gene " + Xval[0]);
                    for (int a = 1; a < Xval.Length; a++)
                    {
                        if (int.Parse(Xval[a]) != 0)
                        {
                            Mfile.WriteLine(i + " " + a + " " + Xval[a]);
                            nData ++;
                        }
                    }
                    nCell++;
                }
                i++;
            }
            Console.WriteLine("Your dense table " + PATH + " has been split into a prefix " + OPATH + " +features.tsv, + barcodes.tsv and +Cells.txt");
            Console.WriteLine("replace the 3rd line in the matrix.mtx by: ");
            Console.WriteLine("%" + nGene + " " + nCell + " " + nData);
            Genefile.Close();
            Mfile.Close();
            file.Close();
        }
    }
}