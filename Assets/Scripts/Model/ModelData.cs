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

    public GameObject Model { get => model; set => model = value; }
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
        string pipe_pattern = @"(?:Pipe Types|DuctSegment|PipeSegment\b)(?<!type\b.*)",
        string connection_pattern = "FlowFitting",
        List<Category> categories = null)
    {
        this.PIPE_PATTERN = pipe_pattern;
        this.CONNECTION_PATTERN = connection_pattern;
        this.categories = categories ?? new List<Category> { };
        this.Filter = this.categories.ToList();
    }

    public void BuildElement(GameObject element)
    {
        Transform elementTransform = element.transform;

        switch (element.name)
        {
            case var name when Regex.IsMatch(name, PIPE_PATTERN, RegexOptions.IgnoreCase):
                CreatePipe(Regex.Match(name, @":([^:]+):", RegexOptions.IgnoreCase), elementTransform);
                break;

            case var name when Regex.IsMatch(name, CONNECTION_PATTERN, RegexOptions.IgnoreCase):
                CreateFitting(Regex.Match(name, @":([^:]+):", RegexOptions.IgnoreCase), elementTransform);
                break;

            case var name when Regex.IsMatch(name, "Wall", RegexOptions.IgnoreCase):
                CreateGenericCategory<Wall>(Regex.Match(name, "Waål"), elementTransform, "Wall");
                break;

            case var name when Regex.IsMatch(name, "Door", RegexOptions.IgnoreCase):
                CreateGenericCategory<Door>(Regex.Match(name, "Door"), elementTransform, "Door");
                break;

            case var name when Regex.IsMatch(name, "Window", RegexOptions.IgnoreCase):
                CreateGenericCategory<Window>(Regex.Match(name, "Window"), elementTransform, "Window");
                break;

            default:
                Match match = Regex.Match(element.name, string.Empty, RegexOptions.IgnoreCase);
                CreateGenericCategory<ModelElement>(match, elementTransform, "Environment");
                break;
        }
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
        Pipe pipe = element.gameObject.AddComponent<Pipe>();
        pipe.gameObject.AddComponent<BoxCollider>();

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
}
