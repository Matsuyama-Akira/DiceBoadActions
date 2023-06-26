using NextSceneScript;
using System.Collections;
using UnityEngine;

public class MapCreateManager : MonoBehaviour
{
    [SerializeField] NextScene nextScene;
    void Start()
    {
    }
    void Update()
    {
        nextScene.ChengeScene("DiceBoadScene");
        Debug.Log("test");
    }
}
