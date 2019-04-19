using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameManager : ScriptableObject {

    [SerializeField]
    public Dictionary<string, Transform> GameText;

    private void Awake()
    {
        GameText = new Dictionary<string, Transform>();
    }


}
