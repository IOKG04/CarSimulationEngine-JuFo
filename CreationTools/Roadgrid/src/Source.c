#include <stdio.h>
#include <stdlib.h>

// point struct
struct _Point{
	double		x;
	double		y;
};
typedef struct _Point Point;

// roadPoint struct
struct _RoadPoint{
	long long	index;
	char		is_corner;
};
typedef struct _RoadPoint RoadPoint;

// road struct
struct _Road{
	double		speedlimit;
	long long	lanes;
	long long	number_of_points;
	RoadPoint    *points;
};
typedef struct _Road Road;


// main function
int main(int argslength, char **args){
	if(argslength < 2){
		printf("ERROR 01: Not enough arguments:\n\tUsage: [file name]\tSaves the output to [file name]\n");
		return 1;
	}

	// create point array
	printf("Number of points (long long):\t\t");
	long long number_of_points;
	scanf("%lli", &number_of_points);
	Point *points = malloc(number_of_points * sizeof(Point));
	printf("\n");

	// loop through points
	for(long long i = 0; i < number_of_points; ++i){
		// get point information
		printf("Position of point %lli (double; x, y):\n", i);
		printf("\t"); scanf("%lf", &(points[i].x));
		printf("\t"); scanf("%lf", &(points[i].y));
	}

	// create road array
	printf("Number of roads (long long):\t\t");
	long long number_of_roads;
	scanf("%lli", &number_of_roads);
	Road *roads = malloc(number_of_roads * sizeof(Road));
	printf("\n");

	// loop through roads
	for(long long i = 0; i < number_of_roads; ++i){
		// create roadPoint array
		printf("\nNumber of points road %lli goes over (long long):\t", i);
		scanf("%lli", &(roads[i].number_of_points));
		roads[i].points = malloc(roads[i].number_of_points * sizeof(RoadPoint));

		// loop through roadPoints
		for(long long j = 0; j < roads[i].number_of_points; ++j){
			// get roadPoint information
			printf("\tIndex of point %lli in road %lli (long long):\t", j, i);
			scanf("%lli", &(roads[i].points[j].index));
			printf("\tIs point %lli in road %lli a corner (1 or 0):\t", j, i);
			scanf("%c", &(roads[i].points[j].is_corner));
			roads[i].points[j].is_corner -= 48;
		}

		// TODO: add lanes and speedlimit
	}

	// free and return
	free(points);
	for(long long i = 0; i < number_of_roads; ++i) free(roads[i].points);
	free(roads);
	return 0;
}
