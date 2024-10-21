using UnityEngine;

[CreateAssetMenu(fileName = "NewCategoryGroup", menuName = "Utilities/Category/CategoryGroup")]
public class CategoryGroup : ScriptableObject
{
    public override string ToString()
    {
        return this.name;
    }
}