using UnityEngine;

public class UnlockField : MonoBehaviour
{
    public Material darkMaterial;
    private Material originalMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalMaterial = GetComponent<Renderer>().material;
        GetComponent<Renderer>().material = darkMaterial;
    }

    public void UnlockFieldFunc()
    {
        GetComponent<Renderer>().material = originalMaterial;
    }
}
