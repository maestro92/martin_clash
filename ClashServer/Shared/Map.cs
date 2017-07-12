using System;

using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Map
{

    public string mapString =  
    @"
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,0,0,0,0,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,0,0,0,0,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,0,0,0,0,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,0,0,0,0,_,_,_,_,_,_,_,_
    _,_,_,1,1,1,_,_,_,_,_,_,_,_,2,2,2,_,_,_
    _,_,_,1,1,1,_,_,_,_,_,_,_,_,2,2,2,_,_,_
    _,_,_,1,1,1,_,_,_,_,_,_,_,_,2,2,2,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    w,w,w,b,b,w,w,w,w,w,w,w,w,w,w,b,b,w,w,w
    w,w,w,b,b,w,w,w,w,w,w,w,w,w,w,b,b,w,w,w
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,3,3,3,_,_,_,_,_,_,_,_,4,4,4,_,_,_
    _,_,_,3,3,3,_,_,_,_,_,_,_,_,4,4,4,_,_,_
    _,_,_,3,3,3,_,_,_,_,_,_,_,_,4,4,4,_,_,_
    _,_,_,_,_,_,_,_,5,5,5,5,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,5,5,5,5,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,5,5,5,5,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,5,5,5,5,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_";

    public const int DEFAULT_MAP_WIDTH = 20;
	public const int DEFAULT_MAP_HEIGHT = 34;

	public int width;
	public int height;

    public int halfWidth;
    public int halfHeight;

    private const string TEAM1_KING_TOWER = "0";
    private const string TEAM1_LEFT_TOWER = "1";
    private const string TEAM1_RIGHT_TOWER = "2";
    private const string TEAM0_LEFT_TOWER = "3";
    private const string TEAM0_RIGHT_TOWER = "4";
    private const string TEAM0_KING_TOWER = "5";

    private const int TEAM0_L_INDEX = 0;
    private const int TEAM0_R_INDEX = 1;
    private const int TEAM0_K_INDEX = 2;
    private const int TEAM1_L_INDEX = 3;
    private const int TEAM1_R_INDEX = 4;
    private const int TEAM1_K_INDEX = 5;



    public Action<List<Vector3>> OnTowerPositionsReady;

    GridCell[,] grids;

	public Map()
	{
		width = DEFAULT_MAP_WIDTH;
		height = DEFAULT_MAP_HEIGHT;

        halfWidth = width / 2;
        halfHeight = height / 2;

		grids = new GridCell[width, height];

	}
        
    public void Init()
    {        
        readMapTextFile(mapString);
    }


    /*

    converting from gridCoord To SimPos is kind of tricky, 
    but think of it converting from one coordinate syste to another coordinate system

    and the conversion is just simply an offset of +0.5 - width/2

    we want the grid coordinates to indicate the grids, so the first grid should be x=0, y=0
    and we are going to use the x=0, and y=0 to convert to simulation position. 


         0,0    1,0    2,0    3,0    4,0    5,0    
        ______ ______ ______ ______ ______ ______
       |      |      |      |      |      |      |
       |______|______|______|______|______|______|
       |      |      |      |      |      |      |
       |______|______|______|______|______|______|
                                             

        then if you apply a +0.5 - width/2 offeset, you get to the simulation space

     -3,0   -2,0   -1,0    0,0    1,0    2,0    3,0   
        ______ ______ ______ ______ ______ ______
       |      |      |      |      |      |      |
       |______|______|______|______|______|______|
       |      |      |      |      |      |      |
       |______|______|______|______|______|______|



    */ 


    public Vector3 GridCoordToSimPos(int x, int y)
    {
        return new Vector3( x - halfWidth + 0.5f, y-halfHeight + 0.5f, 0f );
    }

    private void readMapTextFile(string mapString)
    {

        // From textReaderText, create a continuous paragraph 
        // with two spaces between each sentence.
        string aLine, aParagraph = null;
        StringReader strReader = new StringReader(mapString);


        var towerPositions = new List<List<Vector3>>();

        for (int i = 0; i < 6; i++)
        {
            towerPositions.Add(new List<Vector3>());
        }

        int y = 0, x = 0; 
        while(true)
        {
            aLine = strReader.ReadLine();
            if(aLine != null)
            {
                var list = aLine.Split(',');
                x = 0;
                foreach (var sub in list)
                {                    
                    switch (sub)
                    {
                        case TEAM1_KING_TOWER:
                            towerPositions[TEAM1_K_INDEX].Add(GridCoordToSimPos(x, y));
                            break;
                        case TEAM1_LEFT_TOWER:
                            towerPositions[TEAM1_L_INDEX].Add(GridCoordToSimPos(x, y));
                            break;
                        case TEAM1_RIGHT_TOWER:
                            towerPositions[TEAM1_R_INDEX].Add(GridCoordToSimPos(x, y));
                            break;
                        case TEAM0_KING_TOWER:
                            towerPositions[TEAM0_K_INDEX].Add(GridCoordToSimPos(x, y));
                            break;
                        case TEAM0_LEFT_TOWER:
                            towerPositions[TEAM0_L_INDEX].Add(GridCoordToSimPos(x, y));
                            break;
                        case TEAM0_RIGHT_TOWER:
                            towerPositions[TEAM0_R_INDEX].Add(GridCoordToSimPos(x, y));
                            break;
                        default:
//                            Console.WriteLine("Default case");
                            break;
                    }

                    x++;                    
                }
            }
            else
            {
                break;
            }
            y++;
        } 

        List<Vector3> centroids = new List<Vector3>();

        foreach (var list in towerPositions)
        {
            Vector3 sum = Vector3.zero;
            foreach (var pos in list)
            {
                sum += pos;
            }

            sum /= list.Count;
            centroids.Add(sum);
        }

        if (OnTowerPositionsReady != null)
        {
            OnTowerPositionsReady(centroids);
        }

    }
}

