using System;
using UnityEngine;


public class EyeRaycaster : MonoBehaviour
{
   
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
    [SerializeField]
    private ReticleView m_Reticle;                 // The reticle, if applicable.
    [SerializeField]
    private bool m_ShowDebugRay;                   // Optionally show the debug ray.
    [SerializeField]
    private float m_DebugRayLength = 5f;           // Debug ray length.
    [SerializeField]
    private float m_DebugRayDuration = 1f;         // How long the Debug ray will remain visible.
    
    [SerializeField]
    public float m_RayLength = 200f;              // How far into the scene the ray is cast.

    private void Update()
    {
        EyeRaycast();          
    }

    private void EyeRaycast()
    {
        // Show the debug ray if required
        if (m_ShowDebugRay)
        {
            Debug.DrawRay(firePoint.position, firePoint.forward * m_DebugRayLength, Color.blue, m_DebugRayDuration);
        }

        // Create a ray that points forwards from the camera.
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hit;

        // Do the raycast forweards to see if we hit an interactive item
        if (Physics.Raycast(ray, out hit, m_RayLength, m_ExclusionLayers))
        {
            if (m_Reticle)
                m_Reticle.SetPosition(hit);
        }
        else
        {
            // Position the reticle at default distance.
            if (m_Reticle)
                m_Reticle.SetPosition();
        }
    }
}