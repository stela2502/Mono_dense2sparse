using System;
using System.IO;
using System.Linq;

namespace Mono_dense2sparse
{
    class Program
    {
        static void Main(string[] args)
        {
            if ( args.Length != 2 ) {
                Console.WriteLine("usage: \nMono_dense2sparse.exe <denseMatrix> <outputPrefix>");
                return;
            }

            var PATH = args[0];
            var OPATH = args[1];
            System.IO.StreamReader file = File.OpenText( PATH );
            string line= null;
            StreamWriter Genefile = File.CreateText(OPATH + "Genes.txt");
            StreamWriter Mfile = File.CreateText( OPATH+"Matrix.txt" );
            var i = 0;
            Mfile.WriteLine("rowID" + " " + "colID" + " " + "Value");
            while( ( line = file.ReadLine()) != null) {
                string[] Xval = line.Split("\t", StringSplitOptions.None);
                if (i == 0)
                {
                    //public static System.IO.StreamReader Cellfile (OPATH+"Cells.txt" );
                    File.WriteAllLines(OPATH + "Cells.txt",
                                       Xval.Skip(1).ToArray());
                }
                else
                {
                    Genefile.WriteLine(Xval[0]);

                    for (int a = 1; a < Xval.Length; a++)
                    {
                        if ( Int32.Parse(Xval[a]) != 0 ){
                            Mfile.WriteLine(i + " " + a + " " + Xval[a]);
                        }
                    }
                }
                i ++;
            }
            Console.WriteLine("Hello World! sould have produced the files in data/Sparse");
        }
    }
}
