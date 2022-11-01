using System;
using System.IO;
using SixLabors.ImageSharp;
using Rgba32 = SixLabors.ImageSharp.PixelFormats.Rgba32;
using Visualization;

namespace Visualization;

public class Program{
    public static Image<Rgba32> outputImage;
    public static string outputPath, rgPath, sfPath;
    public static bool simulationFrame;
    public static int minArgPos, halfPixelsPerLane;
    public static double minX, maxX, minY, maxY, bufferSize;
    public static Roadgrid rg;

    public static int Main(string[] args){
	if(args.Length < 1 || ArgPos("-h", args) != -1 || ArgPos("--help", args) != -1){
	    //Print help message
	    Console.WriteLine("Usage:");
	    Console.WriteLine(" Visualization [options] [RG_file] [SF_file]\tVisualizes [RG_file] and [SF_file] with [options] and stores the result in [SF_file].png");
	    Console.WriteLine(" Visualization [options] -r [RG_file]\t\tVisualizes [RG_file] with [options] and stores the result in [RG_file].png");
	    Console.WriteLine("\nOptions:");
	    Console.WriteLine(" -r\t--roadgrid\t\tOnly visualizes a RG_file");
	    Console.WriteLine(" -o\t--output [File]\t\tStores the result in [File] instead of the default location");
	    Console.WriteLine(" -l\t--lanesize [Size]\t\tAmount of pixels to use per lane on a road");
	    //Possibly add dimensions option
	    //Possibly add bufferSize option
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

	//Set outputPath
	halfPixelsPerLane = 8;
	if(ArgPos("-l", args) != -1){halfPixelsPerLane = int.Parse(args[ArgPos("-l", args) + 1]) / 2; minArgPos += 2;}
	if(ArgPos("--lanesize", args) != -1){halfPixelsPerLane = int.Parse(args[ArgPos("--lanesize", args) + 1]) / 2; minArgPos += 2;}

	//Set file paths
	rgPath = args[minArgPos];
	if(!File.Exists(rgPath)){Console.WriteLine("RG_file doesn't exist"); return 3;}
	sfPath = simulationFrame ? args[minArgPos + 1] : "";
	if(simulationFrame && File.Exists(sfPath)){Console.WriteLine("SF_file doesn't exist"); return 4;}

	//Create and initialize outputImage
	outputImage = new Image<Rgba32>(4096, 4096);
	for(int x = 0; x < outputImage.Width; x++){for(int y = 0; y < outputImage.Height; y++){outputImage[x, y] = Rgba32.ParseHex("000000");}}

	//Load rg and sf
	rg = Roadgrid.FromFile(File.ReadAllBytes(rgPath));
	if(rg.points.LongLength < 1 || rg.roads.LongLength < 1){Console.WriteLine("Roadgrid doesn't have enough points or roads"); return 2;}
	//TODO: Load sf
	
	//Set mbufferSize, minX, maxX, minY and maxY
	minX = rg.points[0].x;
	maxX = rg.points[0].x;
	minY = rg.points[0].y;
	maxY = rg.points[0].y;
	for(Int64 i = 1; i < rg.points.LongLength; i++){
	    if(rg.points[i].x < minX) minX = rg.points[i].x;
	    if(rg.points[i].x > maxX) maxX = rg.points[i].x;
	    if(rg.points[i].y < minY) minY = rg.points[i].y;
	    if(rg.points[i].y > maxY) maxY = rg.points[i].y;
	}
	bufferSize = 1.0; //FOR WHATEVER REASON NOT STABLE WITH OTHER VALUE... WHY??? TODO: FIX!!!
	minX -= bufferSize;
	maxX += bufferSize;
	minY -= bufferSize;
	maxY += bufferSize;

	//Draw roadgrid
	double factorX = outputImage.Width / (maxX - minX);
	double factorY = outputImage.Height / (maxY - minY);
	//Loop through roads and points
	for(int r = 0; r < rg.numberOfRoads; r++){
	    //Console.WriteLine("Road: " + r); //Debuging
	    for(int p = 0; p < rg.roads[r].numberOfPoints - 1; p++){
		int x1 = (int)((rg.points[rg.roads[r].points[p].index].x - minX) * factorX);
		int y1 = (int)((rg.points[rg.roads[r].points[p].index].y - minY) * factorY);
		int x2 = (int)((rg.points[rg.roads[r].points[p + 1].index].x - minX) * factorX);
		int y2 = (int)((rg.points[rg.roads[r].points[p + 1].index].y - minY) * factorY);
		/*Debuging
		Rgba32 color = Rgba32.ParseHex("ffffff");
		color.B /= (byte)(r + 1);
		color.R /= (byte)(p + 1);
		Console.WriteLine("\t" + x1 + " " + y1 + " till " + x2 + " " + y2 + " with color: " + color.ToHex());
		DrawLine(x1, y1, x2, y2, color, (int)rg.roads[r].lanes * 8);
		*/
		DrawLine(x1, y1, x2, y2, Rgba32.ParseHex("404040"), (int)rg.roads[r].lanes * 8);
	    }
	}
	
	//Close if !simulationFrame
	if(!simulationFrame){outputImage.Save(outputPath); return 0;}

	//Draw simulationFrame
	//TODO: Draw simulationFrame

	//Save image and return
	outputImage.Save(outputPath);
	return 0;
    }

    public static int DrawLine(int x1, int y1, int x2, int y2, Rgba32 c){
	//TODO: Make function
	//Calcluate steps
	int dx = x2 - x1;
	int dy = y2 - y1;
	float stepx, stepy;
	int steps;
	if(Math.Abs(dx) > Math.Abs(dy)){
	    stepx = 1 * Math.Sign(dx);
	    stepy = dy / Math.Abs(dx);
	    steps = (int)Math.Abs(dx);
	}
	else{
	    stepx = dx / Math.Abs(dy);
	    stepy = 1 * Math.Sign(dy);
	    steps = (int)Math.Abs(dy);
	}

	for(int i = 0; i < steps; i++){
	    outputImage[(int)(x1 + (stepx * i)), (int)(y1 + (stepy * i))] = c;
	}

	return 0;
    }

    public static int DrawLine(int x1, int y1, int x2, int y2, Rgba32 c, int variation){
	//Calcluate distances
	int xd = (int)Math.Abs(x1 - x2);
	int yd = (int)Math.Abs(y1 - y2);
	
	//Draw lines
	if(xd >= yd){
	    //Draw lines for Y variation
	    for(int yp = -variation; yp < variation; yp++){DrawLine(x1, y1 + yp, x2, y2 + yp, c);}
	}
	else{
	    //Draw lines for X variation
	    for(int xp = -variation; xp < variation; xp++){DrawLine(x1 + xp, y1, x2 + xp, y2, c);}
	}

	return 0;
    }

    public static int ArgPos(string arg, string[] args){
	for(int i = 0; i < args.Length; i++){
	    if(arg == args[i]) return i;
	}
	return -1;
    }
}
