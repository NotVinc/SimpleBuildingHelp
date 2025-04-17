using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public switchBuildEditoType type;
    public List<GameObject> prefabsToSwitchBetween;
    private GameObject currentObject;
    int currentIndex = 0;
    private void Awake()
    {
        Destroy(this);
    }
    public enum switchBuildEditoType
    {
        Random,
        RandomWithRotation,
        CountingUpwards
    }

    public void SwitchPrefab()
    {
        DestroyImmediate(currentObject);
        currentObject = Instantiate(prefabsToSwitchBetween[GetIndex()], this.transform.position, this.transform.rotation);
        currentObject.transform.parent = this.transform;
        currentObject.name = "Model (DO NOT MOVE THIS OBJECT ALONE!)";

        if (type == switchBuildEditoType.RandomWithRotation) currentObject.transform.rotation = new Quaternion(
            currentObject.transform.rotation.x, Random.Range(0, 360), currentObject.transform.rotation.z, currentObject.transform.rotation.w);
    }

    int GetIndex()
    {
        switch (type)
        {
            case switchBuildEditoType.Random: return Random.Range(0, prefabsToSwitchBetween.Count);
            case switchBuildEditoType.CountingUpwards: return CountingUpIndex();
            case switchBuildEditoType.RandomWithRotation: return Random.Range(0, prefabsToSwitchBetween.Count);
            default: return 0;
        }
    }

    int CountingUpIndex()
    {
        currentIndex++;
        if (currentIndex >= prefabsToSwitchBetween.Count)
        {
            currentIndex = 0;
        }
        return currentIndex;
    }
}
