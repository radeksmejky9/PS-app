using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


public class ModelData
{
    private readonly string PIPE_PATTERN;
    private readonly string CONNECTION_PATTERN;
    private readonly List<Category> categories;
    private readonly List<ModelElement> elements = new List<ModelElement>();
    private readonly HashSet<Category> usedCategories = new HashSet<Category>();

    private GameObject model;
    private List<Category> filter;

    public GameObject Model { get => model; private set => model = value; }
    public List<Category> Filter { get => filter; set => filter = value; }
    public HashSet<Category> UsedCategories => usedCategories;
    public List<T> GetElementsOfType<T>() where T : ModelElement
    {
        return elements.OfType<T>().ToList();
    }
    public List<T> GetElementsOfCategory<T>(Category category) where T : ModelElement
    {
        return elements.OfType<T>().Where(e => e.Category == category).ToList();
    }

    public ModelData(
        GameObject model,
        string pipe_pattern = @"(?:Pipe Types|DuctSegment|PipeSegment\b)(?<!type\b.*)",
        string connection_pattern = "FlowFitting",
        List<Category> categories = null)
    {
        this.Model = model;
        this.PIPE_PATTERN = pipe_pattern;
        this.CONNECTION_PATTERN = connection_pattern;
        this.categories = categories ?? new List<Category> { };
        this.Filter = this.categories.ToList();
        InitModel();
    }
    private void InitModel()
    {
        var elements = model.GetComponentsInChildren<Transform>(true).Where(t => t != model.transform).ToList();

        ProcessElements(elements, PIPE_PATTERN, CreatePipe);
        ProcessElements(elements, CONNECTION_PATTERN, CreateFitting);
        ProcessElements(elements, "Wall", CreateGenericCategory<Wall>, "Wall");
        ProcessElements(elements, "Window", CreateGenericCategory<Window>, "Window");
        ProcessElements(elements, "Door", CreateGenericCategory<Door>, "Door");
        ProcessElements(elements, string.Empty, CreateGenericCategory<ModelElement>, "Environment");
    }

    private void CreateGenericCategory<T>(Match match, Transform element, string categoryType) where T : ModelElement
    {
        T modelElement = element.gameObject.AddComponent<T>();
        AddCategory(modelElement, categoryType);
        usedCategories.Add(modelElement.Category);
        elements.Add(modelElement);
    }

    private void CreatePipe(Match match, Transform element)
    {
        match = Regex.Match(element.name, @":([^:]+):", RegexOptions.IgnoreCase);
        Pipe pipe = element.gameObject.AddComponent<Pipe>();
        pipe.gameObject.AddComponent<BoxCollider>();

        if (!match.Success)
        {
            AddCategory(pipe, "Undefined");
            return;
        }

        string pipeType = match.Groups[1].Value.Replace(" ", "");

        if (categories.Exists(category => string.Equals(category.ToString(), pipeType, StringComparison.OrdinalIgnoreCase)))
        {
            AddCategory(pipe, pipeType);
        }
        else
        {
            AddCategory(pipe, "Undefined");
        }
    }

    private void CreateFitting(Match match, Transform element)
    {
        Fitting fitting = element.gameObject.AddComponent<Fitting>();
        Pipe closestPipe = fitting.transform.GetClosestPipe(GetElementsOfType<Pipe>().ToArray());
        if (closestPipe != null)
        {
            AddCategory(fitting, category: closestPipe.Category);
        }
        else
        {
            AddCategory(fitting, "Undefined");

        }
    }
    private Category FindCategory(string categoryName)
    {
        return categories.FirstOrDefault(c => c.name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
    }
    private void AddCategory<T>(T element, string categoryName = "", Category category = null) where T : ModelElement
    {
        element.Category = category ?? FindCategory(categoryName) ?? FindCategory("Undefined");
        usedCategories.Add(element.Category);
        elements.Add(element);
    }
    private static void ProcessElements(List<Transform> elements, string pattern, Action<Match, Transform, string> creationMethod, string categoryType)
    {
        for (int i = elements.Count - 1; i >= 0; i--)
        {
            Transform element = elements[i];
            Match match = Regex.Match(element.name, pattern, RegexOptions.IgnoreCase);
            if (!match.Success) continue;

            creationMethod(match, element, categoryType);
            elements.RemoveAt(i);
        }
    }

    private static void ProcessElements(List<Transform> elements, string pattern, Action<Match, Transform> creationMethod)
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
}
