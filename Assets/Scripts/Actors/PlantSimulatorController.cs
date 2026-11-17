using UnityEngine;

public class PlantSimulatorController : MonoBehaviour
{
    public struct EnvironmentParameters
    {
        public float lightIntensity;
        public bool bIndirectLight;
        public Vector2 tempuratureRange;
        public int repotFrequency;
        public int waterFrequency;
        public float humidity;
    }

    public EnvironmentParameters environment;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update given plant behavior
    // Time step is 1 day
    public void Tick(ref PlantBehavior behavior)
    {
        
    }

    public EnvironmentParameters GetEnvironment()
    {
        return environment;
    }
}
