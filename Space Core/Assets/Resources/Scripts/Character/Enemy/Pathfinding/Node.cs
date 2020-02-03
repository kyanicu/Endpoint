using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that contains the elements of behavior for a Node within a Graph Map
/// </summary>
public class Node : MonoBehaviour
{
    //declare variables
    public int NodeIndex;
    public List<Tuple<Node, PathType>> AdjacentNodes;
    public List<Node> Nodes;
    public List<PathType> Paths;
    public enum PathType { Walking, Jumping, Variable }

    /// <summary>
    /// Set the Adjacent Nodes list based off of the nodes and paths stored in the Paths and Nodes list
    /// </summary>
    private void Start()
    {
        AdjacentNodes = new List<Tuple<Node, PathType>>();
        for(int i = 0; i < Nodes.Count; i++)
        {
            AdjacentNodes.Add(new Tuple<Node, PathType>(Nodes[i], Paths[i]));
        }
    }
}
