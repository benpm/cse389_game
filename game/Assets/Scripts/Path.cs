using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Path : MonoBehaviour
{
    List<Vector3> controlPoints = new List<Vector3>();
    List<Vector3> controlDirs = new List<Vector3>();
    List<float> controlDists = new List<float>();
    List<Vector3Int> cardinals = new List<Vector3Int>() { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
    readonly int MAX_PATH_LEN = 1000;
    readonly float turnRadius = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Tilemap tilemap = GameObject.Find("FFG").GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int pos = Vector3Int.zero;

        //Find the starting track tile position
        bool done = false;
        for (pos.x = bounds.xMin; pos.x < bounds.xMax; pos.x++)
        {
            for (pos.y = bounds.yMin; pos.y < bounds.yMax; pos.y++)
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
        
        // Add initial control point
        controlPoints.Add(tilemap.GetCellCenterWorld(pos) - Vector3.right * 0.5f);
        controlDists.Add(0);

        //Construct the path by following track tiles
        Vector3Int dir = Vector3Int.zero;
        Vector3Int lastdir = Vector3Int.zero;
        float pathlength = 0;
        while (pathlength < MAX_PATH_LEN)
        {
            foreach (Vector3Int tmp in cardinals)
            {
                if (tilemap.GetTile(pos + tmp) && tmp != -lastdir)
                {
                    dir = tmp;
                    break;
                }
            }

            if (pathlength == 0)
            {
                lastdir = dir;
                controlDirs.Add(dir);
            }

            // Path is turning
            if (dir != lastdir)
            {
                Vector3 tilepos = tilemap.GetCellCenterWorld(pos);
                controlPoints.Add(tilepos -  0.5f * new Vector3(lastdir.x, lastdir.y, lastdir.z));
                controlPoints.Add(tilepos + 0.5f * new Vector3(dir.x, dir.y, dir.z));

                controlDists.Add(pathlength);
                pathlength += Mathf.PI / 4;
                controlDists.Add(pathlength);

                controlDirs.Add(dir);
                controlDirs.Add(dir);
            }
            else
                pathlength += 1;
            
            pos += dir;
            lastdir = dir;
        }

        controlPoints.Add(tilemap.GetCellCenterWorld(pos));
        controlDirs.Add(lastdir);
        controlDists.Add(pathlength);
    }

    public Vector3 distanceToPosition(float distance)
    {
        distance = Mathf.Max(distance, 0);
        int i = controlIndexAfter(distance);
        
        Vector3 p0 = controlPoints[i];
        Vector3 p1 = controlPoints[i + 1];
        Vector3 newDir = controlDirs[i];
        float distancePastP0 = distance - controlDists[i];

        Vector3 pos = Vector3.zero;
        if ((p0.x != p1.x) && (p0.y != p1.y))
        {
            Vector3 oldDir = controlDirs[i - 1];
            Vector3 vec = Vector3.RotateTowards(-newDir, oldDir, distancePastP0 / turnRadius, 0) * turnRadius;
            pos += turnRadius * newDir + vec;
        }
        else
            pos = distancePastP0 * newDir;

        return p0 + pos;
    }

    public float distanceToAngle(float distance)
    {
        int i = controlIndexAfter(distance);

        Vector3 p0 = controlPoints[i];
        Vector3 p1 = controlPoints[i + 1];
        Vector3 newDir = controlDirs[i];


        if ((p0.x != p1.x) && (p0.y != p1.y))
        {
            float distancePastP0 = distance - controlDists[i];
            Vector3 oldDir = controlDirs[i - 1];
            Vector3 vec = Vector3.RotateTowards(-newDir, oldDir, distancePastP0 / turnRadius, 0) * turnRadius;
            return Vector3.SignedAngle(Vector3.down, vec, Vector3.forward);
        }

        return Vector3.Angle(Vector3.right, newDir);
    }

    private int controlIndexAfter(float distance)
    {
        for (int i = 0; i < (controlDists.Count - 2); i++)
            if (distance < controlDists[i + 1])
                return i;

        return controlDists.Count - 2;
    }
}
