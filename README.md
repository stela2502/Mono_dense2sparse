# Mono_dense2sparse

This is an extremely basic .NET program that converts a dense tab separated table with row and column names into a Marix, Cells and Genes text file.

# Compile

## Linux

On Ubuntu 18.04 you need the dotnet package and I recommend Microsoft Code to interact with the source:

```sudo apt install dotnet-sdk-3.0```
```sudo snap install code```

You then can compile the program using the dotnet tool:

```dotnet build```

And Run with 
```dotnet run <dll>```

## Windows 10

On windows I have not much idear of how to do this, but Visual Studio is the tools of choice.

2021-03-26: I have been able to try this on windows 10 and Visual Studio 2019.

You should be able to use the compiled program in the exe folder - you need all files in the exe folder to run the tool.


# Function

Mono_dense2sparse will remove all '0' values in the table and create a sparse representation of the remaining information:

The tools will convert a comma or tab separated text table into a <prefix>.Martix.txt triplet file and
a #prefix>.Genes.txt and <prefix>.Cells.txt file containing the row resp. column names.

The data is 1 indexed like in R not 0 like in c#.

# DANGER

Be extremely careful with a decimal separator ',' data. This data will be corruped using this tool.
