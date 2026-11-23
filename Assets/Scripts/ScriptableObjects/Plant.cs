using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant", menuName = "ScriptableObjects/Plant")]
public class Plant : ScriptableObject
{
    [Header("Visuals")]
    public Sprite baseSprite;   // Pot and stem
    public Sprite soilSprite;   // White/grayscale soil for tinting
    public Sprite leafSprite;   // White/grayscale leaves for tinting

    [Header("Colors")]
    public Color healthyLeafColor = Color.green;
    public Color thirstyLeafColor = new Color(0.85f, 0.8f, 0.3f); // Yellowish tint
    public Color wetSoilColor = new Color(0.25f, 0.16f, 0.1f);    // Dark brown
    public Color drySoilColor = new Color(0.65f, 0.52f, 0.38f);   // Light tan

    [Header("Given")]
    public float growthRate;
    public float plantWaterVolume;
    public float plantWaterStorageVolume;
    public float potVolume;
    public float waterConsumptionPerDay; // Percentage of pot volume
    public float decay = 0.1f;

    [Header("Expected")]
    public Vector2 expectedPlantHeight;
    public float expectedLightIntensity;
    public bool bExpectedIndirectLight;
    public Vector2 expectedTempuratureRange;
    public int expectedRepotFrequency;
    public int expectedWaterFrequency;
    public float expectedHumidity;
}
