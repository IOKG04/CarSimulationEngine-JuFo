using System;
using System.IO;

namespace Visualization;

//TODO: Add Functions fo reading from byte[] / File

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
