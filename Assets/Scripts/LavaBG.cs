using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBG : MonoBehaviour
{
    private Renderer myrenderer;
    public float offsetValue;
    // Start is called before the first frame update
    void Start()
    {
        myrenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        myrenderer.material.mainTextureOffset -= new Vector2(offsetValue * Time.deltaTime, 0);
    }
}
