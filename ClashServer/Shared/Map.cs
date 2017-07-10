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


    private string TEAM1_KING_TOWER = "0";
    private string TEAM1_LEFT_TOWER = "1";
    private string TEAM1_RIGHT_TOWER = "2";
    private string TEAM0_KING_TOWER = "3";
    private string TEAM0_LEFT_TOWER = "4";
    private string TEAM0_RIGHT_TOWER = "5";


    GridCell[,] grids;

	public Map()
	{
		width = DEFAULT_MAP_WIDTH;
		height = DEFAULT_MAP_HEIGHT;

		grids = new GridCell[width, height];

	}


    public void Init()
    {        
        readMapTextFile(mapString);
    }

    private void readMapTextFile(string mapString)
    {

        // From textReaderText, create a continuous paragraph 
        // with two spaces between each sentence.
        string aLine, aParagraph = null;
        StringReader strReader = new StringReader(mapString);

        var tower0KPositions = new List<Vector3>();
        var tower0LPositions = new List<Vector3>();
        var tower0RPositions = new List<Vector3>();
        var tower1KPositions = new List<Vector3>();
        var tower1LPositions = new List<Vector3>();
        var tower1RPositions = new List<Vector3>();


        int row, col = 0;
        while(true)
        {
            aLine = strReader.ReadLine();
            if(aLine != null)
            {
                var list = aLine.Split(',');
                col = 0;
                foreach (var sub in list)
                {
                    /*
                    switch
*/
                    col++;
                    
                }

            }
            else
            {
                break;
            }
            row++;
        }

        Util.LogError(aParagraph);
     //   StreamReader sr = new StreamReader(mapString);
        /*
        while(!mapString.EndOfStream)
        {
            string line = sr.ReadLine( );
            // Do Something with the input. 

            Util.LogError(line);
        }

        sr.Close( );  
        */
    }
}

