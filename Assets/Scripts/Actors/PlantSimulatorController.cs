using Unity.VisualScripting;
using UnityEngine;
using TMPro;
public class PlantSimulatorController : MonoBehaviour
{
    public struct EnvironmentParameters
    {
        public float lightIntensity;
        public bool bIndirectLight;
        public float tempurature;
        public int repotFrequency;
        public int waterFrequency;
        public float humidity;
    }

    public EnvironmentParameters environment;
    public Plant currPlant;

    int tick = 0;

    // TEMP TEMP TEMP
    PlantBehavior testPlant = new PlantBehavior();

    // UI
    public TMP_Text DebugDisplay;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Tick
        {
            Tick(ref testPlant);
        }

        if (Input.GetKeyDown(KeyCode.W)) // Tick
        {
            Water(ref testPlant);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Repot(ref testPlant);
        }
    }

    public void RunSimulation(int duration)
    {
        for (int i = 0; i < duration; i++)
        {
            if (environment.waterFrequency <= 0)
            {
                if (Random.Range(0.0f, 1.0f) < 0.75f) Water(ref testPlant);
            }
            else
            {
                if (i % environment.waterFrequency == 0) Water(ref testPlant);
            }
            if (environment.repotFrequency <= 0)
            {
                if (Random.Range(0.0f, 1.0f) < 0.25f) Repot(ref testPlant);
            }
            else
            {
                if (i % environment.repotFrequency == 0) Water(ref testPlant);
            }

            Tick(ref testPlant);

            if (testPlant.state.currSurfaceHealth <= 0.0f)
            {
                return;
            }
        }
    }

    public void ResetSimulator()
    {
        testPlant = new PlantBehavior();
        tick = 0;

        // Set environment
        environment.lightIntensity = 0.5f;
        environment.bIndirectLight = false;
        environment.tempurature = 65.0f; // Ferenheit
        environment.repotFrequency = -1;
        environment.waterFrequency = -1;
        environment.humidity = 0.5f;
    }

    public void LoadPlant(Plant plant)
    {
        currPlant = plant;
    }

    public void Water(ref PlantBehavior behavior)
    {
        behavior.state.currSoilDryness = 0.0f;
        string output = PrintPlantState(behavior, tick);
        Debug.Log(output);
        DebugDisplay.text = output;
    }

    public void Repot(ref PlantBehavior behavior)
    {
        behavior.state.currSoilNutrients = 1.0f;
        behavior.state.currSurfaceHealth *= 0.8f; // 20% health lost on repot
        string output = PrintPlantState(behavior, tick);
        Debug.Log(output);
        DebugDisplay.text = output;
    }

    // Update given plant behavior
    // Time step is 1 day
    public float Tick(ref PlantBehavior behavior)
    {
        behavior.prevState = behavior.state;

        float plantVol = behavior.state.currPlantWetness * currPlant.plantWaterVolume;
        float storedVol = behavior.state.currStoredWater * currPlant.plantWaterStorageVolume;

        // -- Soil --
        // To calculate soil dryness, subtract water consumption per day from total soil water
        float soilVol = (1.0f - behavior.state.currSoilDryness) * currPlant.potVolume;
        float waterAdd = 0.0f; // Added later to plant water storage
        float nutrientPercent = 0.0f;
        if (soilVol > 0.0f)
        {
            waterAdd = Mathf.Min(soilVol, (currPlant.plantWaterVolume - plantVol) + (currPlant.plantWaterStorageVolume - storedVol));
            nutrientPercent = Mathf.Min(waterAdd, currPlant.waterConsumptionPerDay) / (currPlant.plantWaterVolume + currPlant.plantWaterStorageVolume);
            soilVol -= waterAdd;
        }

        behavior.state.currSoilDryness = 1.0f - (soilVol / currPlant.potVolume);

        // To calculate nutrients, decrease by same percentage as water decreased by
        float soilNutrients = behavior.state.currSoilNutrients;
        behavior.state.currSoilNutrients = Mathf.Clamp(soilNutrients - (nutrientPercent * 0.01f), 0.0f, 1.0f);

        // -- Water --
        // Subtracting water for consumption
        float waterSubtract = currPlant.waterConsumptionPerDay * (1.0f - (behavior.state.currSurfaceHealth * 0.5f));
        float remainder = Mathf.Abs(Mathf.Min(plantVol - waterSubtract, 0.0f));
        plantVol = Mathf.Max(plantVol - waterSubtract, 0.0f);

        waterSubtract = remainder;

        remainder = Mathf.Abs(Mathf.Min(storedVol - waterSubtract, 0.0f));
        storedVol = Mathf.Max(storedVol - waterSubtract, 0.0f);

        waterSubtract = remainder;

        // Adding water from soil
        remainder = Mathf.Max(plantVol + waterAdd - currPlant.plantWaterVolume, 0.0f);
        plantVol = Mathf.Min(plantVol + waterAdd, currPlant.plantWaterVolume);

        waterAdd = remainder;

        remainder = Mathf.Max(storedVol + waterAdd - currPlant.plantWaterStorageVolume, 0.0f);
        storedVol = Mathf.Min(storedVol + waterAdd, currPlant.plantWaterStorageVolume);

        waterAdd = remainder;

        // Transfer water from storage to plant
        float diff = currPlant.plantWaterVolume - plantVol;
        float passthrough = Mathf.Min(diff, storedVol);
        plantVol += passthrough;
        storedVol -= passthrough;

        // Overwatering
        //float waterDiff = Mathf.Max(waterAdd - waterSubtract, 0.0f);
        //float waterAddPercentage = plantVol / currPlant.waterConsumptionPerDay;
        //if (behavior.state.currSoilDryness < 0.5f)
        //{
        //    // I believe this is inaccurate right now
        //    behavior.state.currSurfaceHealth = Mathf.Max(behavior.state.currSurfaceHealth - currPlant.decay * (waterDiff / plantVol), 0.0f);
        //}
        //// Underwatering
        //else if (waterAddPercentage < 1.0f)
        //{
        //    // I believe this is also inaccurate right now
        //    behavior.state.currSurfaceHealth = Mathf.Max(behavior.state.currSurfaceHealth - currPlant.decay * waterAddPercentage, 0.0f);
        //}

        behavior.state.currPlantWetness = plantVol / currPlant.plantWaterVolume;
        behavior.state.currStoredWater = storedVol / currPlant.plantWaterStorageVolume;

        // -- Environment --
        float lightDiff = Mathf.Abs(currPlant.expectedLightIntensity - environment.lightIntensity);
        float directDiff = (environment.bIndirectLight == currPlant.bExpectedIndirectLight) ? 0.0f : 1.0f;
        float tempuratureDiff = (environment.tempurature >= currPlant.expectedTempuratureRange.x && environment.tempurature <= currPlant.expectedTempuratureRange.y) ? 0.0f : Mathf.Min(Mathf.Abs(environment.tempurature - (currPlant.expectedTempuratureRange.x + currPlant.expectedTempuratureRange.y) * 0.5f) / 30.0f, 1.0f);
        float humidityDiff = Mathf.Abs(currPlant.expectedHumidity - environment.humidity);

        // -- Surface health --
        // Needs to factor in humidity, light, and whatnot
        behavior.state.currSurfaceHealth = Mathf.Clamp(behavior.state.currSurfaceHealth + (((behavior.state.currPlantWetness * behavior.state.currSoilNutrients * 2.0f) - 1.0f) * currPlant.decay) + (0.25f * -lightDiff * currPlant.decay) + (0.1f * -directDiff * currPlant.decay) + (0.4f * -tempuratureDiff * currPlant.decay) + (0.6f * -humidityDiff * currPlant.decay), 0.0f, 1.0f);

        // -- Plant height --
        behavior.state.currPlantHeight += (currPlant.growthRate * behavior.state.currSoilNutrients * behavior.state.currPlantWetness * behavior.state.currSurfaceHealth) * Mathf.Pow(1.0f - (behavior.state.currPlantHeight / currPlant.expectedPlantHeight.y), 2.0f);

        tick++;

        string output = PrintPlantState(behavior, tick);
        Debug.Log(output);
        DebugDisplay.text = output;

        return behavior.GetHealth(behavior.prevState);
    }

    public string PrintPlantState(PlantBehavior behavior, int tick)
    {
        return "-- Plant state on tick " + tick + " --" +
                "\nSoil Dryness: " + behavior.state.currSoilDryness.ToString("F2") +
                "\nSoil Nutrients: " + behavior.state.currSoilNutrients.ToString("F2") +
                "\nPlant Surface Health: " + behavior.state.currSurfaceHealth.ToString("F2") +
                "\nPlant Leaf Wetness: " + behavior.state.currPlantWetness.ToString("F2") +
                "\nPlant Stored Water: " + behavior.state.currStoredWater.ToString("F2") +
                "\nPlant Height: " + behavior.state.currPlantHeight.ToString("F2") +

                "\n\nPlant Health Score: " + behavior.GetHealth(behavior.prevState).ToString("F2") +

                "\n\n----";
    }

    public EnvironmentParameters GetEnvironment()
    {
        return environment;
    }
}
