using UnityEngine;

public class PlantInstance : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Plant plantData;

    [Header("Renderers")]
    [SerializeField] private SpriteRenderer baseRenderer;
    [SerializeField] private SpriteRenderer soilRenderer;
    [SerializeField] private SpriteRenderer leafRenderer;

    private float currentPlantWater;
    private float currentSoilWater;

    private void Start()
    {
        if (plantData != null)
        {
            InitializePlant(plantData);
        }
    }

    public void InitializePlant(Plant data)
    {
        plantData = data;
        currentPlantWater = data.plantWaterVolume;
        currentSoilWater = data.potVolume; // Assumes pot starts watered; adjust as needed

        if (baseRenderer != null) baseRenderer.sprite = data.baseSprite;
        if (soilRenderer != null) soilRenderer.sprite = data.soilSprite;
        if (leafRenderer != null) leafRenderer.sprite = data.leafSprite;

        UpdateVisualColors();
    }

    public void SetWaterLevels(float plantWater, float soilWater)
    {
        currentPlantWater = plantWater;
        currentSoilWater = soilWater;
        UpdateVisualColors();
    }

    private void UpdateVisualColors()
    {
        if (plantData == null) return;

        if (leafRenderer != null)
        {
            float leafRatio = Mathf.Clamp01(currentPlantWater / plantData.plantWaterStorageVolume);
            leafRenderer.color = Color.Lerp(plantData.thirstyLeafColor, plantData.healthyLeafColor, leafRatio);
        }

        if (soilRenderer != null)
        {
            float soilRatio = Mathf.Clamp01(currentSoilWater / plantData.potVolume);
            soilRenderer.color = Color.Lerp(plantData.drySoilColor, plantData.wetSoilColor, soilRatio);
        }
    }
}
