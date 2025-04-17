# Unity Placement System

A small editor‑focused Unity component that lets you cycle or randomly choose among a list of prefabs and place them under a GameObject in the Scene view. You can switch between three modes (Random, RandomWithRotation, CountingUpwards) and preview new instances right from the Inspector.

---

## Table of Contents

- [Features](#features)  
- [Installation](#installation)  
- [Usage](#usage)  
  - [Setup in the Scene](#setup-in-the-scene)  
  - [Inspector Controls](#inspector-controls)  
- [API & Code Overview](#api--code-overview)  
  - [`PlacementSystem`](#placementsystem)  
  - [`PlacementSystemEditor`](#placementsystemeditor)  
- [Switch Modes](#switch-modes)  
- [Example Workflow](#example-workflow)  
- [Notes](#notes)  
- [License](#license)  

---

## Features

- **Three switch modes**:  
  - Random  
  - Random with random Y‑axis rotation  
  - Counting upwards through the list  
- **Live Editor button** to instantly spawn/destroy prefabs in the Scene  
- **Custom Inspector** with a toggleable prefab list for easy editing  
- **Hierarchical parenting**: spawned objects are children of the PlacementSystem GameObject  

---

## Installation

1. Copy both scripts into your project under `Assets/Scripts/PlacementSystem/` or a similar folder:  
   - `PlacementSystem.cs`  
   - `PlacementSystemEditor.cs`  

2. In Unity, let the Editor compile. No extra packages are required.

---

## Usage

### Setup in the Scene

1. **Create an empty GameObject** in your Scene (e.g. _“PrefabSpawner”_).  
2. **Add the `PlacementSystem` component** to it (via **Add Component → PlacementSystem**).  
3. In the Inspector, choose a **Switch Mode** from the dropdown:  
   - **Random**  
   - **RandomWithRotation**  
   - **CountingUpwards**  
4. Click **Setup Settings** to reveal the **Prefabs To Switch Between** list.  
5. Populate the list with your desired prefab assets (drag & drop).  
6. Click **Finish Editing** to hide the list again.

### Inspector Controls

Once your list is set up:

- **Get Random** / **Get Random + Random Y‑Rotation** / **Switch Object**:  
  - Instantiates a new prefab (destroying the previous) at the GameObject’s own transform.  
  - Automatically names it `"Model (DO NOT MOVE THIS OBJECT ALONE!)"` and parents it.  
- **Setup Settings** / **Finish Editing**:  
  - Toggles visibility of the prefab list in the Inspector.  

---

## API & Code Overview

### `PlacementSystem`  
_Attached at runtime or in Editor play mode._

```csharp
public class PlacementSystem : MonoBehaviour
{
    public enum switchBuildEditoType {
        Random,
        RandomWithRotation,
        CountingUpwards
    }

    public switchBuildEditoType type;
    public List<GameObject> prefabsToSwitchBetween;
    public GameObject currentObject;

    private int currentIndex = 0;

    private void Awake() {
        // Prevent runtime updates: remove this component when the Scene plays
        Destroy(this);
    }

    public void SwitchPrefab() {
        // Remove previous preview
        DestroyImmediate(currentObject);

        // Instantiate next prefab
        int idx = GetIndex();
        currentObject = Instantiate(
          prefabsToSwitchBetween[idx],
          transform.position,
          transform.rotation,
          transform
        );
        currentObject.name = "Model (DO NOT MOVE THIS OBJECT ALONE!)";

        // Apply random Y‑axis rotation if requested
        if (type == switchBuildEditoType.RandomWithRotation) {
            currentObject.transform.rotation = Quaternion.Euler(
              currentObject.transform.rotation.eulerAngles.x,
              Random.Range(0, 360),
              currentObject.transform.rotation.eulerAngles.z
            );
        }
    }

    private int GetIndex() {
        switch (type) {
            case switchBuildEditoType.Random:
            case switchBuildEditoType.RandomWithRotation:
                return Random.Range(0, prefabsToSwitchBetween.Count);
            case switchBuildEditoType.CountingUpwards:
                return CountingUpIndex();
            default:
                return 0;
        }
    }

    private int CountingUpIndex() {
        currentIndex = (currentIndex + 1) % prefabsToSwitchBetween.Count;
        return currentIndex;
    }
}
