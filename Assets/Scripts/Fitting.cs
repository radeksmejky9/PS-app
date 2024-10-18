using UnityEngine;

public class Fitting : MonoBehaviour
{
    private Category category;

    public Category Category
    {
        get => category;
        set
        {
            category = value;
            this.GetComponent<MeshRenderer>().material = category.material;
        }
    }

}