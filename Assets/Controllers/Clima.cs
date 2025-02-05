using System.Collections.Generic;
using UnityEngine;
using Services;
using Models.ApiServices;
using Models;
using DataRepositories;

using UnityEngine.UI;

public class Clima : MonoBehaviour
{
  public Text DebugText;
  // [SerializeField]
  private string baseUrl = "api.openweathermap.org/data/2.5/weather?q=London"; //http://3.82.252.1
  
  void Start()
  {
      // envio de peticion get
      StartCoroutine(ApiServiceClient.Instance.HttpGet($"{baseUrl}", (r) => OnRequestComplete(r)));

      IMaintananceTypeRepository ds = new MaintananceTypeRepository();
      
      var _maintanceType = ds.GetAll();
      ToConsoleMaint (_maintanceType);


  }
  //Cuando el servicio responda, la informacion se imprime en consola
  void OnRequestComplete(Response response)
  {
      Debug.Log($"Status Code: {response.StatusCode}");
      Debug.Log($"Data: {response.Data}");
      Debug.Log($"Error: {response.Error}");
  }

  private void ToConsole(IEnumerable<Personas> personas){
    foreach (var person in personas) {
      Debug.Log (person.ToString());
    }
  }
  private void ToConsoleMaint(IEnumerable<MaintenanceType> MaintanceTypes){
    foreach (var person in MaintanceTypes) {
      Debug.Log (person.ToString());
    }
  }
}
