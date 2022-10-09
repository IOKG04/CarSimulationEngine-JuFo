using System;

namespace Visualization;

public struct Roadgrid{
    public Int64 numberOfPoints;
    public Point[] points;
    public Int64 numberOfRoads;
    public Road[] roads;

    public Roadgrid(Point[] _points, Road[] _roads){
	numberOfPoints = _points.Length;
	points = _points;
	numberOfRoads = _roads.Length;
	roads  = _roads;
    }

    public static Roadgrid FromFile(byte[] bytes){
	Int32 currentByte = 0;

	//Get Points
	Point[] points = new Point[BitConverter.ToInt64(bytes, currentByte)];
	currentByte += sizeof(Int64);
	for(Int64 i = 0; i < points.LongLength; i++){
	    double x = BitConverter.ToDouble(bytes, currentByte);
	    currentByte += sizeof(double);
	    double y = BitConverter.ToDouble(bytes, currentByte);
	    currentByte += sizeof(double);
	    points[i] = new Point(x, y);
	}

	//GetRoads
	Road[] roads = new Road[BitConverter.ToInt64(bytes, currentByte)];
	currentByte += sizeof(Int64);
	for(Int64 i = 0; i < roads.LongLength; i++){
	    double speedLimit = BitConverter.ToDouble(bytes, currentByte);
	    currentByte += sizeof(double);
	    Int64 lanes = BitConverter.ToInt64(bytes, currentByte);
	    currentByte += sizeof(Int64);
	    RoadPoint[] roadPoints = new RoadPoint[BitConverter.ToInt64(bytes, currentByte)];
	    currentByte += sizeof(Int64);
	    for(Int64 j = 0; j < roadPoints.LongLength; j++){
		Int64 index = BitConverter.ToInt64(bytes, currentByte);
		currentByte += sizeof(Int64);
		Int64 isCorner = BitConverter.ToInt64(bytes, currentByte);
		currentByte += sizeof(Int64);
		roadPoints[j] = new RoadPoint(index, isCorner);
	    }
	    roads[i] = new Road(speedLimit, lanes, roadPoints);
	}

	return new Roadgrid(points, roads);
    }
}

public struct Point{
    public double x, y;

    public Point(double _x, double _y){
	x = _x;
	y = _y;
    }
}

public struct Road{
    public double speedLimit;
    public Int64 lanes;
    public Int64 numberOfPoints;
    public RoadPoint[] points;

    public Road(double _speedLimit, Int64 _lanes, RoadPoint[] _points){
	speedLimit = _speedLimit;
	lanes = _lanes;
	numberOfPoints = _points.Length;
	points = _points;
    }
}

public struct RoadPoint{
    public Int64 index, isCorner;

    public RoadPoint(Int64 _index, Int64 _isCorner){
	index = _index;
	isCorner = _isCorner;
    }
}
