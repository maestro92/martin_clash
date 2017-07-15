using System;

using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public struct GridCoord
{
    public int x;
    public int y;

    public static GridCoord GetOne(int x, int y)
    {
        GridCoord coord = new GridCoord();
        coord.x = x;
        coord.y = y;
        return coord;
    }
}

public class Map
{

    public string mapDataString =  
  @"_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,0,0,0,0,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,0,0,0,0,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,0,0,0,0,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,0,0,0,0,_,_,_,_,_,_,_
    _,_,1,1,1,_,_,_,_,_,_,_,_,2,2,2,_,_
    _,_,1,1,1,_,_,_,_,_,_,_,_,2,2,2,_,_
    _,_,1,1,1,_,_,_,_,_,_,_,_,2,2,2,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    w,w,b,b,b,w,w,w,w,w,w,w,w,b,b,b,w,w
    w,w,b,b,b,w,w,w,w,w,w,w,w,b,b,b,w,w
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_
    _,_,3,3,3,_,_,_,_,_,_,_,_,4,4,4,_,_
    _,_,3,3,3,_,_,_,_,_,_,_,_,4,4,4,_,_
    _,_,3,3,3,_,_,_,_,_,_,_,_,4,4,4,_,_
    _,_,_,_,_,_,_,5,5,5,5,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,5,5,5,5,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,5,5,5,5,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,5,5,5,5,_,_,_,_,_,_,_
    _,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_,_";



    public string vectorFieldString =  
  @"0,0,0,0,0,0,0,0,0,180,180,180,180,180,180,180,180,180
    0,0,0,0,0,0,0,0,0,180,180,180,180,180,180,180,180,180
    0,0,0,0,0,0,0,0,0,180,180,180,180,180,180,180,180,180
    0,0,0,0,0,0,0,0,0,180,180,180,180,180,180,180,180,180
    0,0,0,0,0,0,0,0,0,180,180,180,180,180,180,180,180,180
    0,0,0,0,0,90,90,90,90,90,90,90,90,180,180,180,180,180
    0,0,0,0,0,90,90,90,90,90,90,90,90,180,180,180,180,180
    0,0,0,0,0,90,90,90,90,90,90,90,90,180,180,180,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    0,0,90,90,90,180,180,180,180,0,0,0,0,90,90,90,180,180
    90,90,90,90,90,90,90,90,90,90,90,90,90,90,90,90,90,90
    90,90,90,90,90,90,90,90,90,90,90,90,90,90,90,90,90,90
    90,90,90,90,90,90,90,90,90,90,90,90,90,90,90,90,90,90
    90,90,180,180,180,90,90,90,90,90,90,90,90,0,0,0,90,90
    90,90,180,180,180,90,90,90,90,90,90,90,90,0,0,0,90,90
    90,90,180,180,180,90,90,90,90,90,90,90,90,0,0,0,90,90
    90,90,180,180,180,90,90,90,90,90,90,90,90,0,0,0,90,90
    90,90,180,180,180,90,90,180,180,0,0,90,90,0,0,0,90,90";
        

    public const int DEFAULT_MAP_WIDTH = 18;    //  9
	public const int DEFAULT_MAP_HEIGHT = 34;   //  17

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


    public Vector3 max;
    public Vector3 min;

    public Vector3[,] vectorFields;

    public Action<List<Vector3>> OnTowerPositionsReady;

    GridCell[,] grids;

    public int[,] gridPos;

	public Map()
	{
		width = DEFAULT_MAP_WIDTH;
		height = DEFAULT_MAP_HEIGHT;

        halfWidth = width / 2;
        halfHeight = height / 2;

		grids = new GridCell[width, height];
        gridPos = new int[width, height];

        vectorFields = new Vector3[width, height];

        max = GridCoordToSimPos(width - 1, height - 1) + new Vector3(0.5f, 0.5f, 0f);
        min = GridCoordToSimPos(0, 0) - new Vector3(0.5f, 0.5f, 0f);

        Util.LogError("Max is " + max.ToString());
        Util.LogError("Min is " + min.ToString());

    }
        
    public void Init()
    {        
        readMapTextFile(mapDataString);
        readVectorFieldMap(vectorFieldString);

        /*
        Util.LogError("\t\t>>>>>>> PrintGridField");
        PrintGridPosField();
        Util.LogError("\t\t>>>>>>> PrintVecField");
        PrintVecField();
        */
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


    public GridCoord SimPosToGridCoord(Vector3 simPos)
    {
        int x = (int)Mathf.Floor(simPos.x + (float)halfWidth);
        int y = (int)Mathf.Floor(simPos.y + (float)halfHeight);
               
        return GridCoord.GetOne(x, y);
    }



    public Vector3 ClampSimPos(Vector3 pos)
    {
        Vector3 clampedPos = pos;
        clampedPos.x = Mathf.Min(max.x, Mathf.Max(min.x, pos.x));
        clampedPos.y = Mathf.Min(max.y, Mathf.Max(min.y, pos.y));
        return clampedPos;
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

        int row = 0, col = 0; 
        while(true)
        {
            aLine = strReader.ReadLine();
            if(aLine != null)
            {
                var list = aLine.Split(',');
                col = 0;

                foreach (var sub in list)
                {          
                    int x = col;
                    int y = height - row - 1;

                    switch (sub)
                    {
                        case TEAM1_KING_TOWER:
                            towerPositions[TEAM1_K_INDEX].Add(GridCoordToSimPos(x, y));
                            gridPos[x, y] = 0;
                            break;
                        case TEAM1_LEFT_TOWER:
                            towerPositions[TEAM1_L_INDEX].Add(GridCoordToSimPos(x, y));
                            gridPos[x, y] = 1;
                            break;
                        case TEAM1_RIGHT_TOWER:
                            towerPositions[TEAM1_R_INDEX].Add(GridCoordToSimPos(x, y));
                            gridPos[x, y] = 2;
                            break;
                        case TEAM0_KING_TOWER:
                            towerPositions[TEAM0_K_INDEX].Add(GridCoordToSimPos(x, y));
                            gridPos[x, y] = 5;
                            break;
                        case TEAM0_LEFT_TOWER:
                            towerPositions[TEAM0_L_INDEX].Add(GridCoordToSimPos(x, y));
                            gridPos[x, y] = 3;
                            break;
                        case TEAM0_RIGHT_TOWER:
                            towerPositions[TEAM0_R_INDEX].Add(GridCoordToSimPos(x, y));
                            gridPos[x, y] = 4;
                            break;
                        default:
                            break;
                    }

                    col++;                    
                }
            }
            else
            {
                break;
            }
            row++;
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


    private void readVectorFieldMap(string vectFieldString)
    {

        // From textReaderText, create a continuous paragraph 
        // with two spaces between each sentence.
        string aLine, aParagraph = null;
        StringReader strReader = new StringReader(vectFieldString);

        int y = 0, x = 0; 
        while(true)
        {
            aLine = strReader.ReadLine();
            if(aLine != null)
            {
                var list = aLine.Split(',');
                x = 0;
       //         Util.LogError(aLine);

                foreach (var sub in list)
                {        
            //        Util.LogError(sub.ToString());

                    int degree = -1;

                    if (Int32.TryParse(sub, out degree))
                    {
                        // nothing
                    }
                    else
                    {
                        Util.LogError("Invalid degree at " + x.ToString() + " " + y.ToString() + ": degree is " + degree.ToString() + " sub is " + sub);
                    }

                    Vector3 dir = new Vector3(Mathf.Cos(degree * Util.DEGREE_TO_RADIAN), Mathf.Sin(degree * Util.DEGREE_TO_RADIAN), 0);

                    dir.Normalize();
       //             Util.LogError(x.ToString() + "  " + y.ToString());
       //             Util.LogError(x.ToString() + "  " + y.ToString());
                    switch (degree)
                    {
                        case 0:
                        case 90:
                        case 180:
                        case 270:
                            vectorFields[x, height - y - 1] = dir;
                            break;
                        default:
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
    }

    private void PrintVecField()
    {
        for (int y = height - 1; y>=0; y--)
        {  
            string s = "";

            for (int x = 0; x < width; x++)
            {
                s += vectorFields[x, y].ToString();
            }

            Util.LogError(s);
        }
    }



    private void PrintGridPosField()
    {
        for (int y = height - 1; y>=0; y--)
        {  
            string s = "";

            for (int x = 0; x < width; x++)
            {
                s += gridPos[x, y].ToString();
            }

            Util.LogError(s);
        }
    }




    public Vector3 GetVelocity(int x, int y)
    {
        return vectorFields[x, y];
    }

    public Vector3 GetVelocity(GridCoord coord)
    {
        if (coord.x < 0 || coord.y < 0 || coord.x >= width || coord.y >= height)
        {
            Util.LogError("Invalid coord at " + coord.x.ToString() + " " + coord.y.ToString());
        }

        return vectorFields[coord.x, coord.y];
    }
}

