using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant", menuName = "ScriptableObjects/Plant")]
public class Plant : ScriptableObject
{
    // Given
    public float growthRate;
    public float plantWaterVolume;
    public float plantWaterStorageVolume;
    public float potVolume;
    public float waterConsumptionPerDay; // Percentage of pot volume

    // Expected
    public Vector2 expectedPlantHeight;
    public float expectedLightIntensity;
    public bool bExpectedIndirectLight;
    public Vector2 expectedTempuratureRange;
    public int expectedRepotFrequency;
    public int expectedWaterFrequency;
    public float expectedHumidity;
}
