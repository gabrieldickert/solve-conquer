using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [Range(0.1f, 1.0f)]
    public float fadeSpeed = 1f;    // How fast alpha value decreases.
    public Color fadeColor = new Color(0, 0, 0, 0);
    //briges are basically inverted barriers
    public bool isBridge = false;
    public List<int> activatedByTriggerId = new List<int>();

    private Renderer BarrierRenderer;
    private Material m_Material;    // Used to store material reference.
    private Color m_Color;            // Used to store color reference.
    private List<int> activeTriggers = new List<int>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.Debug.Log("Barrier: Start");

        // Get reference to object's material.
        m_Material = GetComponent<Renderer>().material;

        // Get material's starting color value.
        m_Color = m_Material.color;

        //Initialize Barrier (isBridge ? disable collision and fade color out : enable collision and fade color in)
        ToggleCollision(!isBridge);
        ToggleFade(isBridge);

        EventsManager.instance.PressurePlateEnable += HandlePressurePlateEnabled;
        EventsManager.instance.PressurePlateDisable += HandlePressurePlateDisabled;
        EventsManager.instance.SwitchEnable += HandlePressurePlateEnabled;
        EventsManager.instance.SwitchDisable += HandlePressurePlateDisabled;

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void HandlePressurePlateEnabled(int id)
    {
        //UnityEngine.Debug.Log("Barrier: HandlePressurePlateEnabled");
        if (activatedByTriggerId.Contains(id))
        {
            if(activeTriggers.Count == 0)
            {
                ToggleCollision(isBridge);
                ToggleFade(!isBridge);
            }
            if(!activeTriggers.Contains(id))
            {
                activeTriggers.Add(id);
            }
            
        }

    }

    private void HandlePressurePlateDisabled(int id)
    {
        //UnityEngine.Debug.Log("Barrier: HandlePressurePlateDisabled");
        if (activatedByTriggerId.Contains(id))
        {
            if(activeTriggers.Contains(id))
            {
                activeTriggers.Remove(id);
            }
            if (activeTriggers.Count == 0)
            {
                ToggleCollision(!isBridge);
                ToggleFade(isBridge);
            }
        }

    }

    // This method fades only the alpha.
    IEnumerator AlphaFade()
    {
        // Alpha start value.
        float alpha = 1.0f;

        // Loop until aplha is below zero (completely invisalbe)
        while (alpha > 0.0f)
        {
            // Reduce alpha by fadeSpeed amount.
            alpha -= fadeSpeed * Time.deltaTime;

            // Create a new color using original color RGB values combined
            // with new alpha value. We have to do this because we can't 
            // change the alpha value of the original color directly.
            m_Material.color = new Color(m_Color.r, m_Color.g, m_Color.b, alpha);

            yield return null;
        }
    }

    // This method fades from original color to "fadeColor"
    IEnumerator ColorFade()
    {
        // Lerp start value.
        float change = 0.0f;

        // Loop until lerp value is 1 (fully changed)
        while (change < 1.0f)
        {
            // Reduce change value by fadeSpeed amount.
            change += fadeSpeed * Time.deltaTime;

            m_Material.color = Color.Lerp(m_Color, fadeColor, change);

            yield return null;
        }
    }
  
    IEnumerator ColorGrow()
    {
        // Lerp start value.
        float change = 1.0f;

        // Loop until lerp value is 1 (fully changed)
        while (change > 0.0f)
        {
            // Reduce change value by fadeSpeed amount.
            change -= fadeSpeed * Time.deltaTime;

            m_Material.color = Color.Lerp(m_Color, fadeColor, change);

            yield return null;
        }
    }

    private void ToggleCollision(bool hasCollision)
    {
        //UnityEngine.Debug.Log("Barrier: ToggleCollision: " + hasCollision);
        this.GetComponent<MeshCollider>().enabled = hasCollision;
    }

    private void ToggleFade(bool isFadeOut)
    {
        if(isFadeOut)
        {
            StartCoroutine(ColorFade());
        } else
        {
            StartCoroutine(ColorGrow());
        }
    }
}
