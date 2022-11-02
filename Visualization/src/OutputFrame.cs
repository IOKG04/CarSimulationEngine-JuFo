using System;

namespace Visualization;

public struct OutputFrame{
    public Int64 numberOfCars;
    public Car[] cars;

    public OutputFrame(Int64 _noC, Car[] _cars){
	numberOfCars = _noC;
	cars = _cars;
    }

    public static OutputFrame FromFile(byte[] bytes){
	if(bytes.Length < 8) throw new Exception("OutputFrame.FromFile bytes isn't large enough");

	Int64 numberOfCars = BitConverter.ToInt64(bytes, 0);
	Car[] cars = new Car[numberOfCars];
	int c = 8;

	for(Int64 i = 0; i < numberOfCars; i++){
	    cars[i] = new Car();
	    cars[i].color = BitConverter.ToInt32(bytes, c); c += sizeof(Int32);
	    cars[i].positionX = BitConverter.ToDouble(bytes, c); c += sizeof(Double);
	    cars[i].positionY = BitConverter.ToDouble(bytes, c); c += sizeof(Double);
	    cars[i].velocityX = BitConverter.ToDouble(bytes, c); c += sizeof(Double);
	    cars[i].velocityY = BitConverter.ToDouble(bytes, c); c += sizeof(Double);
	    cars[i].accelleration = BitConverter.ToDouble(bytes, c); c += sizeof(Double);
	    cars[i].steering = BitConverter.ToDouble(bytes, c); c += sizeof(Double);
	    cars[i].sizeX = BitConverter.ToDouble(bytes, c); c += sizeof(Double);
	    cars[i].sizeY = BitConverter.ToDouble(bytes, c); c += sizeof(Double);
	    cars[i].rotation = BitConverter.ToDouble(bytes, c); c += sizeof(Double);
	}

	return new OutputFrame(numberOfCars, cars);
    }
}

public struct Car{
    public Int32 color;
    public Double positionX;
    public Double positionY;
    public Double velocityX;
    public Double velocityY;
    public Double accelleration;
    public Double steering;
    public Double sizeX;
    public Double sizeY;
    public Double rotation;

    public Car(Int32 _c, Double _pX, Double _pY, Double _vX, Double _vY, Double _a, Double _s, Double _sX, Double _sY, Double _r){
	color = _c;
	positionX = _pX;
	positionY = _pY;
	velocityX = _vX;
	velocityY = _vY;
	accelleration = _a;
	steering = _s;
	sizeX = _sX;
	sizeY = _sY;
	rotation = _r;
    }
}
