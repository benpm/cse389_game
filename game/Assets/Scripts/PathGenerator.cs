using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using PathCreation;

public class PathGenerator : MonoBehaviour
{
    Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        PathCreator path = GetComponent<PathCreator>();
        tilemap = GameObject.Find("FFG").GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int pos = new Vector3Int(bounds.xMin, bounds.yMin, 0);

        //Find the starting track tile position
        bool done = false;
        for (; pos.x < bounds.xMax; pos.x += 1)
        {
            for (pos.y = bounds.yMin; pos.y < bounds.yMax; pos.y += 1)
            {
                if (tilemap.GetTile(pos))
                {
                    done = true;
                    break;
                }
            }
            if (done)
                break;
        }

        //Construct the path by following track tiles
        Vector3Int northdir = new Vector3Int(0, 1, 0);
        Vector3Int southdir = new Vector3Int(0, -1, 0);
        Vector3Int westdir = new Vector3Int(-1, 0, 0);
        Vector3Int eastdir = new Vector3Int(1, 0, 0);
        Vector3Int dir = Vector3Int.zero;
        Vector3Int lastdir = Vector3Int.zero;
        int pathlength = 0;
        while (pathlength < 1000)
        {
            TileBase north = tilemap.GetTile(pos + northdir);
            TileBase south = tilemap.GetTile(pos + southdir);
            TileBase west = tilemap.GetTile(pos + westdir);
            TileBase east = tilemap.GetTile(pos + eastdir);

            //Determine which direction to continue in
            if (east && eastdir != Vector3Int.zero - lastdir)
                dir = eastdir;
            else if (south && southdir != Vector3Int.zero - lastdir)
                dir = southdir;
            else if (north && northdir != Vector3Int.zero - lastdir)
                dir = northdir;
            else if (west && westdir != Vector3Int.zero - lastdir)
                dir = westdir;
            else
            {
                //Path has ended
                path.bezierPath.AddSegmentToEnd(tilemap.GetCellCenterWorld(pos));
                break;
            }
            
            //If path continues, we should add path segments
            if (dir != lastdir)
            {
                Vector3 tilepos = tilemap.GetCellCenterWorld(pos);
                Vector3 midpoint = Vector3.Lerp(tilepos - lastdir, tilepos + dir, 0.5f);
                midpoint = Vector3.Lerp(midpoint, tilepos, 0.5f);
                path.bezierPath.AddSegmentToEnd(tilepos - lastdir);
                path.bezierPath.AddSegmentToEnd(midpoint);
                path.bezierPath.AddSegmentToEnd(tilepos + dir);
            }
            pos += dir;
            lastdir = dir;
            pathlength += 1;
        }
    }
}
