using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;


public class ModelData
{
    private string PIPE_PATTERN;
    private string CONNECTION_PATTERN;
    private Category[] categories;

    private GameObject model;
    private List<Category> filter;
    private List<ModelElement> elements = new List<ModelElement>();

    public GameObject Model { get => model; private set => model = value; }
    public List<ModelElement> Elements
    {
        get => elements;
    }
    public List<Pipe> Pipes
    {
        get => elements.OfType<Pipe>().ToList();
    }

    public List<Fitting> Fittings
    {
        get => elements.OfType<Fitting>().ToList();
    }

    public List<ModelElement> EnvironmentElements
    {
        get => elements.Where(e => e is not Pipe && e is not Fitting).ToList();
    }

    public List<Category> Filter { get => filter; set => filter = value; }

    public ModelData(
        GameObject model,
        string pipe_pattern = @"(?:Pipe Types|DuctSegment)(?<!type\b.*):([^:]+):",
        string connection_pattern = "FlowFitting",
        Category[] categories = null)
    {
        this.Model = model;
        this.PIPE_PATTERN = pipe_pattern;
        this.CONNECTION_PATTERN = connection_pattern;
        this.categories = categories ?? new Category[] { };
        this.Filter = this.categories.ToList();
        InitModel();
    }
    private void InitModel()
    {
        List<Transform> elements = model.GetComponentsInChildren<Transform>(true).ToList();
        elements.Remove(model.transform);


        ProcessElements(elements, PIPE_PATTERN, (match, element) => CreatePipe(match, element));
        ProcessElements(elements, CONNECTION_PATTERN, (match, element) => CreateFitting(match, element));
        ProcessElements(elements, string.Empty, (match, element) => CreateEnvironmentElement(match, element));
    }
    void ProcessElements(List<Transform> elements, string pattern, Action<Match, Transform> creationMethod)
    {
        for (int i = elements.Count - 1; i >= 0; i--)
        {
            Transform element = elements[i];
            Match match = Regex.Match(element.name, pattern, RegexOptions.IgnoreCase);
            if (!match.Success) continue;

            creationMethod(match, element);
            elements.RemoveAt(i);
        }
    }

    private void CreateEnvironmentElement(Match match, Transform element)
    {
        ModelElement modelElement = element.AddComponent<ModelElement>();
        modelElement.Category = LinkCategoryToName(categories, "Environment");
        elements.Add(modelElement);
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
        elements.Add(pipe);
    }

    private void CreateFitting(Match match, Transform element)
    {
        Fitting fitting = element.gameObject.AddComponent<Fitting>();
        Pipe closestPipe = fitting.transform.GetClosestPipe(Pipes.ToArray());
        if (closestPipe != null)
        {
            fitting.Category = closestPipe.Category;
        }
        else
        {
            fitting.Category = LinkCategoryToName(categories, "Undefined");
        }
        elements.Add(fitting);
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
