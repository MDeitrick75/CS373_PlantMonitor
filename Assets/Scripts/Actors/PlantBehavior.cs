using UnityEngine;

public class PlantBehavior
{
    public struct PlantState
    {
        public float currSoilDryness; // 0.0 is good, 1.0 is bad
        public float currSurfaceHealth; // 0.0 is bad, 1.0 is good
        public float currStoredWater; // 0.0 is bad, 1.0 is good
        public float currPlantHeight; // In inches
        public float currPlantWetness; // 0.0 is dry, 1.0 is full, if full, additional water is put in storage
        public float currSoilNutrients; // 0.0 needs a repot, 1.0 is max
    }

    public PlantState state;

    public PlantState GetState()
    {
        return state;
    }

    public float GetHealth()
    {
        // Not implemented
        return 0.0f;
    }

    public PlantBehavior()
    {
        // Defaults to best state
        state.currSoilDryness = 0.0f;
        state.currSurfaceHealth = 1.0f;
        state.currStoredWater = 1.0f;
        state.currPlantHeight = 0.0f;
        state.currPlantWetness = 1.0f;
        state.currSoilNutrients = 1.0f;
    }
}
