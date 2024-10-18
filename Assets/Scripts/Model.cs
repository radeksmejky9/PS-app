using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public class ModelData
{
    private readonly string PIPE_PATTERN;
    private readonly string CONNECTION_PATTERN;
    private readonly Category[] categories;

    private GameObject model;
    private List<Pipe> pipes;
    private List<Fitting> fittings;
    private List<Category> filter;

    public GameObject Model { get => model; private set => model = value; }
    public List<Pipe> Pipes
    {
        get => pipes;
        private set => pipes = value;
    }

    public List<Fitting> Fittings
    {
        get => fittings;
        private set => fittings = value;
    }

    public List<Category> Filter { get => filter; set => filter = value; }

    public ModelData(
        GameObject model,
        string pipe_pattern = @"(?:Pipe Types|DuctSegment):([^:]+):",
        string connection_pattern = "FlowFitting",
        Category[] categories = null)
    {
        this.Model = model;
        this.PIPE_PATTERN = pipe_pattern;
        this.CONNECTION_PATTERN = connection_pattern;
        this.Filter = new List<Category>();
        this.categories = categories ?? new Category[] { };
        InitModel();
    }
    private void InitModel()
    {
        Match match;

        foreach (Transform element in model.GetComponentsInChildren<Transform>(true))
        {
            match = Regex.Match(element.name, PIPE_PATTERN);
            if (!match.Success) continue;
            CreatePipe(match, element);
        }
        foreach (Transform element in model.GetComponentsInChildren<Transform>(true))
        {
            match = Regex.Match(element.name, CONNECTION_PATTERN);
            if (!match.Success) continue;
            CreateFitting(match, element);
        }
    }

    private void CreatePipe(Match match, Transform element)
    {
        string pipeType = match.Groups[1].Value.Replace(" ", "");
        Pipe pipe = element.gameObject.AddComponent<Pipe>();
        pipe.gameObject.AddComponent<BoxCollider>();

        if (Array.Exists(categories, category => string.Equals(category.ToString(), pipeType, StringComparison.OrdinalIgnoreCase)))
        {
            pipe.Category = LinkCategoryToName(categories, pipeType);

            if (pipe.Category == null)
            {
                pipe.Category = LinkCategoryToName(categories, "Undefined");
            }
        }
        else
        {
            pipe.Category = LinkCategoryToName(categories, "Undefined");
        }
        pipes.Add(pipe);
    }

    private void CreateFitting(Match match, Transform element)
    {
        Fitting fitting = element.gameObject.AddComponent<Fitting>();
        Pipe closestPipe = fitting.transform.GetClosestPipe(pipes.ToArray());
        if (closestPipe != null)
        {
            fitting.Category = closestPipe.Category;
        }
        else
        {
            fitting.Category = LinkCategoryToName(categories, "Undefined");
        }
    }

    private static Category LinkCategoryToName(Category[] categories, string categoryName)
    {
        foreach (var category in categories)
        {
            if (category.name.Equals(categoryName, StringComparison.OrdinalIgnoreCase))
            {
                return category;
            }
        }
        return null;
    }
}
