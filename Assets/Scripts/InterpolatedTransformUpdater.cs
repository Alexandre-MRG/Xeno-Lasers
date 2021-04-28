using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpolatedTransformUpdater : MonoBehaviour {

    private InterpolatedTransform m_interpolatedTransform;

    void Awake()
    {
        m_interpolatedTransform = GetComponent<InterpolatedTransform>();
    }

    void FixedUpdate()
    {
        m_interpolatedTransform.LateFixedUpdate();
    }
}
