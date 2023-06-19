using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using org.mariuszgromada.math.mxparser;

public class Grapher : MonoBehaviour
{
    public float xRange;
    public float yRange;
    [SerializeField] float precission;
    [SerializeField] float asymptoteMin;

    [SerializeField] Transform fSlices;
    [SerializeField] Transform gSlices;
    [SerializeField] GameObject slicePrefab;

    [HideInInspector] public bool canGraph;

    EdgeCollider2D sliceColl;
    LineRenderer sliceGraphic;
    string fExpression;
    string gExpression;

    public void FGraph(string expression)
    {
        if (fExpression == expression)
            return;
        fExpression = expression;
        Graph(expression, true);
    }

    public void GGraph(string expression)
    {
        if (gExpression == expression)
            return;
        gExpression = expression;
        Graph(expression, false);
    }

    public void UnGraph()
    {
        for (int i = 0; i < fSlices.childCount; i++)
        {
            Destroy(fSlices.GetChild(i).gameObject);
        }
    
        for (int i = 0; i<gSlices.childCount; i++)
        {
            Destroy(gSlices.GetChild(i).gameObject);
        }
    }

    void Graph(string expression, bool func)
    {
        if (!canGraph)
            return;

        if (func)
        {
            for (int i = 0; i < fSlices.childCount; i++)
            {
                Destroy(fSlices.GetChild(i).gameObject);
            }
        }
        else
        {
            for (int i = 0; i < gSlices.childCount; i++)
            {
                Destroy(gSlices.GetChild(i).gameObject);
            }
        }

        AddSlice(func);

        var function = new Function("f", expression, "x");

        var pointList = new List<Vector2>();

        bool outOfRange = false;
        for (float x = (-xRange / 2); x < (xRange/2); x+=precission)
        {
            var eval = function.calculate(x);

            if (eval < (yRange / 2) && eval > -(yRange / 2))
                outOfRange = false;
            else
            {
                if (!outOfRange)
                {
                    sliceColl.points = pointList.ToArray();
                    sliceGraphic.positionCount = pointList.Count;
                    sliceGraphic.SetPositions(Vec2ToVec3(pointList));

                    pointList.Clear();
                    AddSlice(func);
                    outOfRange = true;
                }
                continue;
            }

            pointList.Add(new Vector2(x, (float)eval));
        }

        if (!outOfRange)
        {
            sliceColl.points = pointList.ToArray();
            sliceGraphic.positionCount = pointList.Count;
            sliceGraphic.SetPositions(Vec2ToVec3(pointList));
        }
    }

    void AddSlice(bool func)
    {
        var slice = Instantiate(slicePrefab, transform.position, Quaternion.identity, func == true ? fSlices : gSlices);
        sliceColl = slice.GetComponent<EdgeCollider2D>();
        sliceGraphic = slice.GetComponent<LineRenderer>();
    }

    Vector3[] Vec2ToVec3(List<Vector2> vectorList)
    {
        var result = new Vector3[vectorList.Count];
        for (int i = 0; i < vectorList.Count; i++)
        {
            result[i] = vectorList[i];
        }

        return result;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(xRange, yRange, 0));
    }
}