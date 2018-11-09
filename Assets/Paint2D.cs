using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class Paint2D : MonoBehaviour {
    public Material material;
    public float tolerance = 0.05f;
    public GameObject linePrefab;

    VRTK_InteractableObject interactableObject;
    Transform brushTip;
    LineRenderer lineRenderer;
    bool painting;
    public bool touching;
    Vector3 tipPosition;

    GameObject currentLine;
    List<GameObject> lines = new List<GameObject>();

    void OnEnable() {
        // Save resources by getting components only once
        if (interactableObject == null) {
            interactableObject = GetComponent<VRTK_InteractableObject>();
        }

        if (brushTip == null) {
            brushTip = transform.Find("Tip");
        }

        // Register our delegate functions
        interactableObject.InteractableObjectUsed += OnBrushUsed;
        interactableObject.InteractableObjectUnused += OnBrushUnused;
    }

    void OnDisable() {
        // Unregister our delegate functions
        interactableObject.InteractableObjectUsed -= OnBrushUsed;
        interactableObject.InteractableObjectUnused -= OnBrushUnused;
    }
    
    void Update () {
        if (painting == false || touching == false) {
            return;
        }

        if (lineRenderer == null) {
            return;
        }

        tipPosition = brushTip.position;
        tipPosition.z = 0.94f;

        // Add current position of brush's tip to the lineRenderer's points
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, tipPosition);
    }

    void OnBrushUsed (object sender, InteractableObjectEventArgs e) {
        painting = true;
        Debug.Log("use");

        // Other actions
        if (touching == true) {
            CreateLine();
            Debug.Log("New line");
        }
    }

    void OnBrushUnused (object sender, InteractableObjectEventArgs e) {
        painting = false;
        Debug.Log("unuse");

        // Other actions
        if (lineRenderer == null) {
            return;
        }

        // Reduce the amount of points in the line to improve performance
        lineRenderer.Simplify(tolerance);
    }

    void CreateLine () {
        // https://docs.unity3d.com/ScriptReference/LineRenderer.SetPositions.html
        
        // Create new empty gameObject and give it a line renderer component
        // We need a new gameObject for each individual line (otherwise we'd get one continuous line)
        currentLine = Instantiate(linePrefab, tipPosition, Quaternion.identity);

        
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        

        lineRenderer.material = material;
        lineRenderer.widthMultiplier = 0.05f;

        lines.Add(currentLine);
    }
}