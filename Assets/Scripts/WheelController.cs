using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using DefaultNamespace;
using Random = UnityEngine.Random;

public class WheelController : MonoBehaviour
{
    [Header("Wheel Components")] [SerializeField]
    private List<Transform> wheelContainers;

    [SerializeField] private GameObject slicePrefab;
    [SerializeField] private Transform slicesParent;

    [Header("Spin Settings")] [SerializeField]
    private float spinDuration = 3f;

    [SerializeField] private int minRotations = 3;
    [SerializeField] private int maxRotations = 5;
    [SerializeField] private Ease spinEase = Ease.OutCubic;

    private WheelConfigSO currentConfig;
    private List<WheelSlice> slices = new List<WheelSlice>();
    private bool isSpinning = false;

    public void SetupWheel(WheelConfigSO config)
    {
        currentConfig = config;
        ClearWheel();
        GenerateSlices();
    }

    private void ClearWheel()
    {
        foreach (var slice in slices)
        {
            if (slice != null && slice.gameObject != null)
            {
                Destroy(slice.gameObject);
            }
        }

        slices.Clear();
    }

    private void GenerateSlices()
    {
        if (currentConfig == null || currentConfig.slices == null || currentConfig.slices.Count == 0)
        {
            return;
        }

        wheelContainers.ForEach(a => a.parent.gameObject.SetActive(false));
        GetWheelContainer().parent.gameObject.SetActive(true);
        int sliceCount = currentConfig.slices.Count;
        float anglePerSlice = 360f / sliceCount;
        Transform slicePositionsContainer = GetWheelContainer().transform.GetChild(0);
        List<Transform> sliceContainers = new List<Transform>();
        for (int i = 0; i < slicePositionsContainer.childCount; i++)
        {
            sliceContainers.Add(slicePositionsContainer.GetChild(i));
        }

        for (int i = 0; i < sliceCount; i++)
        {
            GameObject sliceObj = Instantiate(slicePrefab, sliceContainers[i]);
            WheelSlice slice = sliceObj.GetComponent<WheelSlice>();

            if (slice != null)
            {
                slice.Initialize(currentConfig.slices[i]);
                slices.Add(slice);

                // Position slice
                float angle = i * anglePerSlice;
                sliceObj.transform.localRotation = Quaternion.Euler(0, 0, -angle);
            }
        }
    }

    public void Spin()
    {
        if (isSpinning)
        {
            return;
        }

        isSpinning = true;
        Transform wheelContainer = GetWheelContainer();
        // Calculate random final angle
        int rotations = Random.Range(minRotations, maxRotations + 1);
        int selectedSliceIndex = Random.Range(0, slices.Count);

        float anglePerSlice = 360f / slices.Count;
        float targetAngle = (rotations * 360f) + selectedSliceIndex * anglePerSlice;

        // Reset rotation
        wheelContainer.localRotation = Quaternion.identity;

        // Spin animation
        wheelContainer.DORotate(new Vector3(0, 0, targetAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(spinEase)
            .OnComplete(() => OnSpinComplete(selectedSliceIndex));
    }

    private Transform GetWheelContainer()
    {
        return currentConfig.zoneType switch
        {
            ZoneType.Regular => wheelContainers[0],
            ZoneType.Safe => wheelContainers[1],
            ZoneType.Super => wheelContainers[2],
            _ => wheelContainers[0],
        };
    }

    private void OnSpinComplete(int sliceIndex)
    {
        isSpinning = false;
        GameEvents.OnWheelSpinComplete?.Invoke(sliceIndex);
    }

    public SliceData GetSliceData(int index)
    {
        if (index >= 0 && index < currentConfig.slices.Count)
        {
            return currentConfig.slices[index];
        }

        return null;
    }

    public bool IsSpinning()
    {
        return isSpinning;
    }
}