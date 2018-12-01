using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour {
    public GameObject Plane = null;
    public float MaxSpawnRadius = 1500;
    public float MinSpawnRadius = 1000;

    public enum PlaneState {
        Ready,
        FlyIn,
        FlyOut
    }

    StoryMapPoint _currentPoint = null;

}
