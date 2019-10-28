using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public static class TransformGroupMenuItem
{
    [MenuItem( "Edit/Transform Group %g" )]
    private static void GroupSelected()
    {
        List<GameObject> items = new List<GameObject>();
        if( !Selection.activeTransform ) return;
        var go = new GameObject( Selection.activeTransform.name + "_Group" );
        Undo.RegisterCreatedObjectUndo( go, "Group Selected" );
        go.transform.SetParent( Selection.activeTransform.parent, false );

        //Sort items and add them to group
        foreach( var obj in Selection.gameObjects ) items.Add( obj );
        items = items.OrderBy( obj => obj.name ).ToList();
        go.transform.position = findAvgCenterPoint( items.ToArray() );
        foreach (var obj in items) Undo.SetTransformParent( obj.transform, go.transform, "Group Selected" );

        //Set Active object equal to newly created group
        Selection.activeGameObject = go;
    }

    //Find average center point of all objects for more accurate group object positioning
    private static Vector3 findAvgCenterPoint(GameObject[] go)
    {
        Vector3 point = new Vector3( 0, 0, 0 );
        int size = go.Length;
        foreach(GameObject obj in go )
        {
            point += obj.transform.position;
        }
        return (point /= size);
    }

    //Find center point of two farthest objects from an array
    /*private static Vector3 findDistCenterPoint( GameObject[] go )
    {
        if( go.Length == 2 ) return ( ( go[0].transform.position + go[1].transform.position ) / 2 );
        GameObject a, b, bestA = null, bestB = null;
        float dist = -1, bestDist = 0;
        for( int x = 0; x < go.Length; x++ )
        {
            a = go[x];
            for( int y = 0; y < go.Length; y++ )
            {
                b = go[y];
                if( a != b )
                {
                    dist = Vector3.Distance( a.transform.position, b.transform.position );
                    if( dist > bestDist )
                    {
                        bestDist = dist;
                        bestA = a;
                        bestB = b;
                    }
                }
            }
        }
        return ((bestA.transform.position + bestB.transform.position) / 2);
    }*/
}
