using UnityEngine;

public class CategoryState : MonoBehaviour
{
    public static CategoryState Instance { get; private set; }  
    
    public string txtCategory;
    public string customJsonPath; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
