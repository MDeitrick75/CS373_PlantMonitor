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
    public Plant currPlant;

    // TEMP TEMP TEMP
    PlantBehavior testPlant;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Tick(ref testPlant);
        }
    }

    public void LoadPlant(Plant plant)
    {
        currPlant = plant;
    }

    // Update given plant behavior
    // Time step is 1 day
    public void Tick(ref PlantBehavior behavior)
    {
        // -- Water --
        float plantVol = behavior.state.currPlantWetness * currPlant.plantWaterVolume;
        float storedVol = behavior.state.currStoredWater * currPlant.plantWaterStorageVolume;
        if (behavior.state.currPlantWetness > 0.0f)
        {
            plantVol -= currPlant.waterConsumptionPerDay;

            if (plantVol < 0.0f)
            {
                storedVol += plantVol;
                plantVol = 0.0f;
            }
        }
        else if (behavior.state.currStoredWater > 0.0f)
        {
            storedVol -= currPlant.waterConsumptionPerDay;

            if (storedVol < 0.0f)
            {
                storedVol = 0.0f;
            }
        }

        behavior.state.currPlantWetness = plantVol / currPlant.plantWaterVolume;
        behavior.state.currStoredWater = storedVol / currPlant.plantWaterStorageVolume;

        Debug.Log("Plant Water: " + plantVol + " | Stored Water: " + storedVol);
    }

    public EnvironmentParameters GetEnvironment()
    {
        return environment;
    }
}
