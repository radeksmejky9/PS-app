using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCategory", menuName = "Utilities/Category/Category")]
public class Category : ScriptableObject
{
    public CategoryGroup categoryGroup;
    public Material material;

    public override string ToString()
    {
        return this.name;
    }
}