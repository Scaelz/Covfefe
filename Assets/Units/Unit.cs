using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    protected IMovable moveScript;
    protected IStressable stressScript;
    [SerializeField] Texture[] textures;
    [SerializeField] SkinnedMeshRenderer renderer;

    protected void Initialize()
    {
        moveScript = GetComponent<IMovable>();
        stressScript = GetComponent<IStressable>();
        GetRandomTexture();
    }

    void GetRandomTexture()
    {
        renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
    }
}
