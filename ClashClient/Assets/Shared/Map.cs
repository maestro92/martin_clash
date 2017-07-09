using System;

public class Map
{
	public const int DEFAULT_MAP_WIDTH = 20;
	public const int DEFAULT_MAP_HEIGHT = 34;

	public int width;
	public int height;



	GridCell[,] grids;

	public Map()
	{
		width = DEFAULT_MAP_WIDTH;
		height = DEFAULT_MAP_HEIGHT;

		grids = new GridCell[width, height];

	}





}

