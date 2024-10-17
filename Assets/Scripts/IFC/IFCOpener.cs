
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;
using Xbim.Ifc;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.SharedBldgServiceElements;

public class IFCOpener
{
    public static Dictionary<string, List<MeshRenderer>> GetPipes(List<MeshRenderer> children, List<string> wordsToMatch)
    {
        string pattern = @"(?:Pipe Types|DuctSegment):([^:]+):";

        Dictionary<string, List<MeshRenderer>> sortedChildren = new Dictionary<string, List<MeshRenderer>>();

        foreach (var child in children)
        {
            Match match = Regex.Match(child.gameObject.name, pattern);
            if (!match.Success) continue;

            string pipeType = match.Groups[1].Value.Replace(" ", "");
            if (Array.Exists(wordsToMatch.ToArray(), word => string.Equals(word, pipeType, StringComparison.OrdinalIgnoreCase)))
            {
                if (sortedChildren.ContainsKey(pipeType))
                    sortedChildren[pipeType].Add(child);
                else
                    sortedChildren[pipeType] = new List<MeshRenderer>() { child };
            }
            else
            {
                if (sortedChildren.ContainsKey("Undefined"))
                    sortedChildren["Undefined"].Add(child);
                else
                    sortedChildren["Undefined"] = new List<MeshRenderer>() { child };
            }
        }

        return sortedChildren;
    }

    public static List<GameObject> GetConnections(List<MeshRenderer> children)
    {
        List<GameObject> connectionChildren = new List<GameObject>();
        string[] wordsToMatch = new string[] { "FlowFitting" };
        for (int i = children.Count - 1; i >= 0; i--)
        {
            if (wordsToMatch.Any(children[i].name.Contains))
                connectionChildren.Add(children[i].gameObject);
        }

        return connectionChildren;
    }

    public static List<GameObject> GetSegments(List<MeshRenderer> children)
    {
        List<GameObject> connectionChildren = new List<GameObject>();
        string[] wordsToMatch = new string[] { "FlowSegment" };
        for (int i = children.Count - 1; i >= 0; i--)
        {
            if (wordsToMatch.Any(children[i].name.Contains))
                connectionChildren.Add(children[i].gameObject);
        }

        return connectionChildren;
    }

    public static List<MeshRenderer> CleanModel(List<MeshRenderer> children)
    {
        string[] wordsToMatch = new string[] { "FlowSegment", "FlowFitting" };
        for (int i = children.Count - 1; i >= 0; i--)
        {
            if (!wordsToMatch.Any(children[i].name.Contains))
                GameObject.Destroy(children[i].gameObject);
        }

        return children;
    }
}




