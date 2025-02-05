using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direc_pag_web : MonoBehaviour
{
   public string URL;

   private void OnMouseDown ()
   {
		  Application.OpenURL (URL);
   }
}
