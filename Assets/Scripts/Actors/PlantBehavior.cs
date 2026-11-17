using UnityEngine;

public class PlantBehavior
{
    public struct PlantState
    {
        float currSoilDryness; // 0.0 is good, 1.0 is bad
        float currSurfaceHealth; // 0.0 is bad, 1.0 is good
        float currStoredWater; // 0.0 is bad, 1.0 is good
        float currPlantHeight; // In inches
        float currPlantWetness; // 0.0 is dry, 1.0 is full, if full, additional water is put in storage
        float currSoilNutrients; // 0.0 needs a repot, 1.0 is max
    }
}
