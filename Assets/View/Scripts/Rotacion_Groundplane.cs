using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotacion_Groundplane : MonoBehaviour
{


    public Slider sliderRotacion;
    public GameObject Mordaza;
    

    public void Rotacion()
    {
        Mordaza.transform.rotation = Quaternion.Euler(0, sliderRotacion.value, 0);
        
    }
}
