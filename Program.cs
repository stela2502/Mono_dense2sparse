
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
        }

        private static void ReadCSV(string PATH)
        {
            string OPATH = PATH.Substring(0, PATH.LastIndexOf('.'));
            System.IO.StreamReader file = File.OpenText(PATH);
            string line = null;
            StreamWriter Genefile = File.CreateText(OPATH + "Genes.txt");
            StreamWriter Mfile = File.CreateText(OPATH + "Matrix.txt");
            var i = 0;
            Mfile.WriteLine("rowID" + " " + "colID" + " " + "Value");
            while ((line = file.ReadLine()) != null)
            {
                string[] Xval = line.Split(new char[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (i == 0)
                {
                    //public static System.IO.StreamReader Cellfile (OPATH+"Cells.txt" );
                    File.WriteAllLines(OPATH + "Cells.txt",
                                       Xval.Skip(1).ToArray());
                }
                else
                {
                    Genefile.WriteLine(Xval[0]);
                    //Console.WriteLine("got  gene " + Xval[0]);
                    for (int a = 1; a < Xval.Length; a++)
                    {
                        if (int.Parse(Xval[a]) != 0)
                        {
                            Mfile.WriteLine(i + " " + a + " " + Xval[a]);
                        }
                    }
                }
                i++;
            }
            Console.WriteLine("Your dense table " + PATH + " has been split into a prefix " + OPATH + " +Genes.txt, + Matrix.txt and +Cells.txt");
            Genefile.Close();
            Mfile.Close();
            file.Close();
        }
    }
}