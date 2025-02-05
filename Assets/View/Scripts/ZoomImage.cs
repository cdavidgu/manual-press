using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomImage : MonoBehaviour
{
    private Vector3 initialScale;

    [SerializeField]
    private float zoomSpeed = 1f;
    [SerializeField]
    private float maxZoom = 10f;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch T0 = Input.GetTouch(0);
            Touch T1 = Input.GetTouch(1);

            Vector3 T0_Cambio = T0.position - T0.deltaPosition;
            Vector3 T1_Cambio = T1.position - T1.deltaPosition;

            float Magnitud_anterior = T0_Cambio.magnitude - T1_Cambio.magnitude;
            float Magnitud_toque = T0.position.magnitude - T1.position.magnitude;

            float Diferencia = Magnitud_anterior - Magnitud_toque;

            var delta = Vector3.one * (Diferencia * zoomSpeed);

            var desiredScale = transform.localScale + delta;

            desiredScale = ClampDesiredScale(desiredScale);

            transform.localScale = desiredScale;
        }
    }
     private Vector3 ClampDesiredScale(Vector3 desiredScale)
        {
            desiredScale = Vector3.Max(initialScale, desiredScale);
            desiredScale = Vector3.Min(initialScale * maxZoom, desiredScale);
            return desiredScale;
        }
}
