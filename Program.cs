
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace Mono_dense2sparse
{
    class Program
    {

        public static ( int, int, int) GetInfo(string PATH)
        {
            string OPATH = PATH.Substring(0, PATH.LastIndexOf('.'));
            System.IO.StreamReader file = File.OpenText(PATH);
            string line = null;
            int i = 0;
            int nData = 0;
            int nGene = 0;
            int nCell = 0;
            
            while ((line = file.ReadLine()) != null)
            {
                string[] Xval = line.Split(new char[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (i == 0)
                {
                    nCell = Xval.ToArray().Length-1;
                }
                else
                {
                    for (int a = 1; a < Xval.Length; a++)
                    {
                        int val = 0;
                        try {
                            val = int.Parse(Xval[a]);
                        }
                        catch (Exception err ) {
                            // crap - likely a float then...
                            val = (int) float.Parse(Xval[a]);
                        }

                        if ( val != 0)
                        {
                            nData++;
                        }
                    }
                    nGene++;
                }
                i++;
            }
            file.Close();
            return (nGene, nCell, nData);
        }

        private static bool  ReadCSV_float(string PATH)
        {
            int i = 0;
            int nData = 0;
            int nGene = 0;
            int nCell = 0;
            (nGene, nCell, nData) = Program.GetInfo(PATH);

            string OPATH = PATH.Substring(0, PATH.LastIndexOf('.'));
            OPATH = Path.Combine( OPATH, "filtered_feature_bc_matrix" );
            System.IO.StreamReader file = File.OpenText(PATH);
            string line = null;
            if (! Directory.Exists(OPATH))
            {
                DirectoryInfo di = Directory.CreateDirectory(OPATH);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(OPATH));
            }
            StreamWriter Genefile = File.CreateText(Path.Combine(OPATH, "features.tsv"));
            StreamWriter Mfile = File.CreateText(Path.Combine(OPATH, "matrix.mtx"));
           
            Mfile.WriteLine("%%MatrixMarket matrix coordinate integer general");
            Mfile.WriteLine("%");
            
            //Mfile.WriteLine("%" + nGene + " " + nCell + " " + nData);
            //Mfile.WriteLine("%" + nCell + " " + nGene + " " + nData);
            Mfile.WriteLine( nGene + " " + nCell + " " + nData);
            nGene = nCell = nData = 0;
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
                    StreamWriter Cellfile = File.CreateText(Path.Combine(OPATH, "barcodes.tsv"));
                    nCell  = Xval.ToArray().Length ;
                    for (int a = 1; a < (nCell); a++ ){ // the cell naames are in columns 1..cellID+1 as C# is 0 indexed.
                        Cellfile.WriteLine( Xval[a] );
                    }
                    Cellfile.Close();

                }
                else
                {
                    Genefile.WriteLine(Xval[0] + "\t" + Xval[0] +"\t"+ "Gene Expression");
                    //Console.WriteLine("got  gene " + Xval[0]);
                    for (int a = 1; a < Xval.Length; a++)
                    {
                        int val = 0;
                        val = (int) float.Parse(Xval[a]);
                        if ( val != 0)
                        {
                            Mfile.WriteLine(i + " " + a + " " + val );
                            nData ++;
                        }
                    }
                    nGene++;
                }
                i++;
            }

            Console.WriteLine("Your dense table " + PATH + " has been split into a prefix " + OPATH + " features.tsv, barcodes.tsv and matrix.mtx");
            Console.WriteLine("gzip these files to read into loompy");
            Genefile.Close();
            Mfile.Close();
            file.Close();

            return true;
        }

        private static bool ReadCSV(string PATH)
        {
            int i = 0;
            int nData = 0;
            int nGene = 0;
            int nCell = 0;
            (nGene, nCell, nData) = Program.GetInfo(PATH);

            string OPATH = PATH.Substring(0, PATH.LastIndexOf('.'));
            OPATH = Path.Combine( OPATH, "filtered_feature_bc_matrix" );
            System.IO.StreamReader file = File.OpenText(PATH);
            string line = null;
            if (! Directory.Exists(OPATH))
            {
                DirectoryInfo di = Directory.CreateDirectory(OPATH);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(OPATH));
            }
            StreamWriter Genefile = File.CreateText(Path.Combine(OPATH, "features.tsv"));
            StreamWriter Mfile = File.CreateText(Path.Combine(OPATH, "matrix.mtx"));
           
            Mfile.WriteLine("%%MatrixMarket matrix coordinate integer general");
            Mfile.WriteLine("%");
            
            //Mfile.WriteLine("%" + nGene + " " + nCell + " " + nData);
            //Mfile.WriteLine("%" + nCell + " " + nGene + " " + nData);
            Mfile.WriteLine( nGene + " " + nCell + " " + nData);
            nGene = nCell = nData = 0;
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
                    StreamWriter Cellfile = File.CreateText(Path.Combine(OPATH, "barcodes.tsv"));
                    nCell  = Xval.ToArray().Length;
                    for (int a = 1; a < (nCell); a++ ){
                        Cellfile.WriteLine( Xval[a] );
                    }
                    Cellfile.Close();

                }
                else
                {
                    Genefile.WriteLine(Xval[0] + "\t" + Xval[0] +"\t"+ "Gene Expression");
                    //Console.WriteLine("got  gene " + Xval[0]);
                    for (int a = 1; a < Xval.Length; a++)
                    {
                        int val = 0;
                        try {
                            val = int.Parse(Xval[a]);
                        }
                        catch ( Exception err ) {
                            // crap - likely a float then...
                            return false;
                        }

                        if ( val != 0)
                        {
                            Mfile.WriteLine(i + " " + a + " " + val );
                            nData ++;
                        }
                    }
                    nGene++;
                }
                i++;
            }

            

            Console.WriteLine("Your dense table " + PATH + " has been split into a prefix " + OPATH + " features.tsv, barcodes.tsv and matrix.mtx");
            Console.WriteLine("gzip these files to read into loompy");
            Genefile.Close();
            Mfile.Close();
            file.Close();

            return true;
        }
        static void Main(string[] args)
        {
            //if (args.Length != 2)
            //{
            //    Console.WriteLine("usage: \nMono_dense2sparse.exe <denseMatrix> <outputPrefix>");
            //    return;
            //}
            Console.WriteLine("Mono_dense2sparse converts all dense comma or tab separated files (*.csv) in the input path to MatrixMarket integer tables as used by CellRanger");
            Console.WriteLine("Enter input folder path");
            string inputFolderPath = Console.ReadLine();
            string[] files = Directory.GetFiles(inputFolderPath, "*.csv");
            int i = 0;
            bool res = false;
            foreach (string path in files)
            {
                Console.WriteLine("Processing file " + path);
                res = ReadCSV(path);
                if (! res ){
                    res = ReadCSV_float(path);
                }
                i++;
            }
            Console.WriteLine("Processed "+i+" files fond in " + inputFolderPath);
            System.Environment.Exit(1);
        }
    }
}