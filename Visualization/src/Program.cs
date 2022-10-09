using System;
using SixLabors.ImageSharp;
using PF = SixLabors.ImageSharp.PixelFormats;
using Visualization;

namespace Visualization;

public class Program{
    public static Image<PF.Rgba32> outputImage;
    public static string outputPath, rgPath, sfPath;
    public static bool simulationFrame;
    public static int minArgPos;

    public static int Main(string[] args){
	if(args.Length < 1 || ArgPos("-h", args) != -1 || ArgPos("--help", args) != -1){
	    //Print help message
	    Console.WriteLine("Usage:");
	    Console.WriteLine(" Visualization [options] [RG_file] [SF_file]\tVisualizes [RG_file] and [SF_file] with [options] and stores the result in [SF_file].png");
	    Console.WriteLine(" Visualization [options] -r [RG_file]\t\tVisualizes [RG_file] with [options] and stores the result in [RG_file].png");
	    Console.WriteLine("\nOptions:");
	    Console.WriteLine(" -r\t--roadgrid\t\tOnly visualizes a RG_file");
	    Console.WriteLine(" -o\t--output [File]\tStores the result in [File] instead of the default location");
	    Console.WriteLine("\nDictionary");
	    Console.WriteLine(" RG_file\tRoadgrid file");
	    Console.WriteLine(" SF_file\tSimulation frame file");
	    return 1;
	}

	minArgPos = 0;

	//Set simulationFrame
	simulationFrame = true;
	if(ArgPos("-r", args) != -1 || ArgPos("--roadgrid", args) != -1){simulationFrame = false; minArgPos++;}

	//Set outputPath
	outputPath = simulationFrame ? args[minArgPos + 1] + ".png" : args[minArgPos] + ".png";
	if(ArgPos("-o", args) != -1){outputPath = args[ArgPos("-o", args) + 1]; minArgPos += 2;}
	if(ArgPos("--output", args) != -1){outputPath = args[ArgPos("--output", args) + 1]; minArgPos += 2;}

	rgPath = args[minArgPos];
	sfPath = simulationFrame ? args[minArgPos + 1] : "";

	return 0;
    }

    public static int ArgPos(string arg, string[] args){
	for(int i = 0; i < args.Length; i++){
	    if(arg == args[i]) return i;
	}
	return -1;
    }
}
