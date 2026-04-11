using UnityEngine;

public class Collectible : MonoBehaviour //would change to abstract in larger project for subclasses, but ben mackey is scaling down code complexity!
{
    private GameObject childObj;

    public int ID;// random data, could put whatever you needed in 
    public string Name;//
    public bool spin = true;
    void Start()
    {
        if (transform.childCount > 0) childObj = transform.GetChild(0).gameObject;

        if (Name == string.Empty) Name = "None";
    }
    private void Spin()
    {
        childObj.transform.rotation *= Quaternion.Euler(Vector3.forward * Time.deltaTime * 100);
    }
    public void Collect()
    {
        gameObject.SetActive(false);
        //or whatever else it would do
    }
    void Update()
    {
        if (childObj != null) Spin();   
    }
}
