
using System.Collections.Generic;
using UnityEngine;

class UIExtension : UI
{

    /// <summary>
    /// Draws a grid of boxes and returns their positions.
    /// </summary>
    public static List<Rect> DrawAutoGrids(Rect rect, float gridWidth, float gridHeight, GUIStyle style)
    {
        List<Rect> rectGrids = new List<Rect>();

        float width = rect.width;
        float height = rect.height;

        //x, y grid count;
        int xCount = Mathf.FloorToInt(width/gridWidth);
        int yCount = Mathf.FloorToInt(height/gridHeight);

        //x,y gaps 
        float xTotalGaps = width - xCount*gridWidth;
        float yTotalGaps = height - yCount*gridHeight;

        //gap count;
        float xGap = xTotalGaps/(xCount+1);
        float yGap = yTotalGaps/(yCount+1);

        Vector2 topLeft = new Vector2(rect.x + xGap, rect.y + yGap);    //topleft to draw 1st grid;
        

        int id = 0;
        for (int i = 0; i < xCount; i++ )
        {
            for(int j = 0; j < yCount; j++)
            {
                
                float x = topLeft.x + (i* xGap) + (i * gridWidth);
                float y = topLeft.y + (j * yGap) + (j * gridHeight);
                Rect gridRect = new Rect(x, y, gridWidth, gridHeight);

                rectGrids.Add(gridRect);
                GUI.Box(gridRect, (++id).ToString(), style);
            }
        }
        return rectGrids;
    }

    public static List<Rect> DrawAutoGrids(Rect rect, float gridWidth, float gridHeight)
    {
        GUIStyle style = MyGUIBehavior.Instance.skin.box;   //style
        return DrawAutoGrids(rect, gridWidth, gridHeight, style);
    }
}