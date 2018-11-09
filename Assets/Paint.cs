using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class Paint : MonoBehaviour {
    public Material material;
    public float tolerance = 0.1f;
    public GameObject linePrefab;

    VRTK_InteractableObject interactableObject;
    LineRenderer lineRenderer;
    bool painting;
    Vector3 tipPosition;

    GameObject currentLine;
    List<GameObject> lines = new List<GameObject>();

    void OnEnable() {
        // Save resources by getting components only once
        if (interactableObject == null) {
            interactableObject = GetComponent<VRTK_InteractableObject>();
        }

        // Register our delegate functions
        interactableObject.InteractableObjectUsed += BrushUsed;
        interactableObject.InteractableObjectUnused += BrushUnused;
    }

    void OnDisable() {
        // Unregister our delegate functions
        interactableObject.InteractableObjectUsed -= BrushUsed;
        interactableObject.InteractableObjectUnused -= BrushUnused;
    }
    
    void Update () {
        if (painting == false) {
            return;
        }

        if (lineRenderer == null) {
            return;
        }

        tipPosition = this.transform.position + new Vector3(0, 0.25f, 0);

        // Add current position of brush's tip to the lineRenderer's points
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, tipPosition);
    }

    void BrushUsed (object sender, InteractableObjectEventArgs e) {
        painting = true;
        CreateLine();
    }

    void BrushUnused (object sender, InteractableObjectEventArgs e) {
        painting = false;

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

        if (lineRenderer == null) {
            lineRenderer = currentLine.GetComponent<LineRenderer>();
        }

        lineRenderer.material = material;
        lineRenderer.widthMultiplier = 0.2f;

        lines.Add(currentLine);
    }
}