using System; using System.Drawing;
using System.IO;
using SixLabors.ImageSharp;
using PF = SixLabors.ImageSharp.PixelFormats;
using Visualization;

namespace Visualization;

public class Program{
    public static Image<PF.Rgba32> outputImage;
    public static string outputPath, rgPath, sfPath;
    public static bool simulationFrame;
    public static int minArgPos;
    public static double minX, maxX, minY, maxY, bufferSize;
    public static Roadgrid rg;

    public static int Main(string[] args){
	if(args.Length < 1 || ArgPos("-h", args) != -1 || ArgPos("--help", args) != -1){
	    //Print help message
	    Console.WriteLine("Usage:");
	    Console.WriteLine(" Visualization [options] [RG_file] [SF_file]\tVisualizes [RG_file] and [SF_file] with [options] and stores the result in [SF_file].png");
	    Console.WriteLine(" Visualization [options] -r [RG_file]\t\tVisualizes [RG_file] with [options] and stores the result in [RG_file].png");
	    Console.WriteLine("\nOptions:"); Console.WriteLine(" -r\t--roadgrid\t\tOnly visualizes a RG_file");
	    Console.WriteLine(" -o\t--output [File]\t\tStores the result in [File] instead of the default location");
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
	if(ArgPos("-o", args) != -1){outputPath = args[ArgPos("-o", args) + 1]; minArgPos += 2;} if(ArgPos("--output", args) != -1){outputPath = args[ArgPos("--output", args) + 1]; minArgPos += 2;}

	//Set file paths
	rgPath = args[minArgPos];
	if(!File.Exists(rgPath)){Console.WriteLine("RG_file doesn't exist"); return 3;}
	sfPath = simulationFrame ? args[minArgPos + 1] : "";
	if(simulationFrame && File.Exists(sfPath)){Console.WriteLine("SF_file doesn't exist"); return 4;}

	//Create and initialize outputImage
	outputImage = new Image<PF.Rgba32>(4096, 4096);
	for(int x = 0; x < outputImage.Width; x++){for(int y = 0; y < outputImage.Height; y++){outputImage[x, y] = PF.Rgba32.ParseHex("000000");}}

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
		PF.Rgba32 color = PF.Rgba32.ParseHex("ffffff");
		color.B /= (byte)(r + 1);
		color.R /= (byte)(p + 1);
		Console.WriteLine("\t" + x1 + " " + y1 + " till " + x2 + " " + y2 + " with color: " + color.ToHex());
		DrawLine(x1, y1, x2, y2, color, (int)rg.roads[r].lanes * 8);
		*/
		DrawLine(x1, y1, x2, y2, PF.Rgba32.ParseHex("404040"), (int)rg.roads[r].lanes * 8);
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

    public static int DrawLine(int x1, int y1, int x2, int y2, PF.Rgba32 c){
	//WHAT IS THIS FIX AND WHY DID I HAVE TO DO IT AT ALL???
	if(!(y1 > y2 && !(x1 >= x2))){
	    if(x1 > x2){int a = x1; x1 = x2; x2 = a;}
	    if(y1 > y2){int a = y1; y1 = y2; y2 = a;}
	}
	//TODO: Maybe fix, so fix doesn't need to fix

	//Calcluate steps
	int xd = x2 - x1;
	int yd = y2 - y1;
	double xs, ys;
	int amount;
	if(Math.Abs(xd) >= Math.Abs(yd)){
	    xs = xd / Math.Abs(xd);
	    ys = ((double)yd) / Math.Abs(xd);
	    amount = xd;
	}
	else{
	    xs = ((double)xd) / Math.Abs(yd);
	    ys = yd / Math.Abs(yd);
	    amount = yd;
	}

	//Step through steps
	double x = x1; double y = y1;
	for(int i = 0; i < amount; i++){
	    try{
		outputImage[(int)x, (int)y] = c;
		x += xs;
		y += ys;
	    }catch{}
	}

	return 0;
    }

    public static int DrawLine(int x1, int y1, int x2, int y2, PF.Rgba32 c, int variation){
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
