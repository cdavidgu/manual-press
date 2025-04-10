using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using SQLite4Unity3d;
using Models;
using System.Linq;
using System.Dynamic;
using Newtonsoft.Json;

namespace DataRepositories
{
    public class MachineRepository : IMachineReposiory
    {
        private SQLiteConnection _connection;
        public MachineRepository()
        {
            _connection = DataServiceClient.Instance.DataServiceClients();
        }

        public List<Machine> GetAll()
        {
            List<Machine> _Machines = _connection.Table<Machine>().ToList();
            foreach (var IMachine in _Machines)
            {
                LoadResources(IMachine, "Datasheet");
                LoadResources(IMachine, "BluePrints");
                LoadResources(IMachine, "AssembleManual");
                LoadResources(IMachine, "InstalationManual");
                LoadResources(IMachine, "MCleaningManual");
                LoadResources(IMachine, "MReplacementManualDB");
                LoadResources(IMachine, "UserManual");
            }

            return _Machines;
        }

        private void LoadResources(Machine IMachine, string atributo)
        {
            MachineResources _Resourses = new MachineResources();
            if (atributo.Equals("Datasheet"))
            {
                _Resourses = JsonConvert.DeserializeObject<MachineResources>(IMachine.DatasheetDB);
                IMachine.DataSheet = new Dictionary<string, string>();
            }
            else if (atributo.Equals("BluePrints"))
            {
                _Resourses = JsonConvert.DeserializeObject<MachineResources>(IMachine.BluePrintsDB);
                IMachine.BluePrints = new Dictionary<string, string>();
            }
            if (atributo.Equals("AssembleManual"))
            {
                _Resourses = JsonConvert.DeserializeObject<MachineResources>(IMachine.AssembleManualDB);
                IMachine.AssembleManual = new Dictionary<string, string>();
            }
            else if (atributo.Equals("InstalationManual"))
            {
                _Resourses = JsonConvert.DeserializeObject<MachineResources>(IMachine.InstalationManualDB);
                IMachine.InstalationManual = new Dictionary<string, string>();
            }
            else if (atributo.Equals("MCleaningManual"))
            {
                _Resourses = JsonConvert.DeserializeObject<MachineResources>(IMachine.MCleaningManualDB);
                IMachine.MCleaningManual = new Dictionary<string, string>();
            }
            else if (atributo.Equals("MReplacementManualDB"))
            {
                _Resourses = JsonConvert.DeserializeObject<MachineResources>(IMachine.MReplacementManualDB);
                IMachine.MReplacementManual = new Dictionary<string, string>();
            }
            else if (atributo.Equals("UserManual"))
            {
                _Resourses = JsonConvert.DeserializeObject<MachineResources>(IMachine.UserManualDB);
                IMachine.UserManual = new Dictionary<string, string>();
            }

            if (_Resourses.R1 != null)
            {
                foreach (var item in _Resourses.R1)
                {
                    var Description = item.Description;
                    var element = item.Item;
                    //var NombreRecurso = item.Description.Split('.');
                    if (item.Item.Contains(".png") || item.Item.Contains(".jpg"))
                    {
                        if (atributo.Equals("Datasheet"))
                        {
                            IMachine.DataSheet.Add(IMachine.Url + "FichaTecnica/" + item.Item, item.Description);
                        }
                        else if (atributo.Equals("BluePrints"))
                        {
                            IMachine.BluePrints.Add(IMachine.Url + "Planos/" + item.Item, item.Description);
                        }
                        else if (atributo.Equals("AssembleManual"))
                        {
                            IMachine.AssembleManual.Add(IMachine.Url + "Infografias/Manual Ensamble/" + item.Item, item.Description);
                        }
                        else if (atributo.Equals("InstalationManual"))
                        {
                            IMachine.InstalationManual.Add(IMachine.Url + "Infografias/Manual Instalación/" + item.Item, item.Description);
                        }
                        else if (atributo.Equals("MCleaningManual"))
                        {
                            IMachine.MCleaningManual.Add(IMachine.Url + "Infografias/Manual Mantenimiento/Manual Mtto Limpieza/" + item.Item, item.Description);
                        }
                        else if (atributo.Equals("MReplacementManualDB"))
                        {
                            IMachine.MReplacementManual.Add(IMachine.Url + "Infografias/Manual Mantenimiento/Manual Mtto Cambio Repuestos/" + item.Item, item.Description);
                        }
                        else if (atributo.Equals("UserManual"))
                        {
                            IMachine.UserManual.Add(IMachine.Url + "Infografias/Manual de Usuario/" + item.Item, item.Description);
                        }
                    }
                    else
                    {
                        if (atributo.Equals("Datasheet"))
                        {
                            IMachine.DataSheet.Add(item.Item, item.Description);
                        }
                        else if (atributo.Equals("BluePrints"))
                        {
                            IMachine.BluePrints.Add(item.Item, item.Description);
                        }
                        else if (atributo.Equals("AssembleManual"))
                        {
                            IMachine.AssembleManual.Add(item.Item, item.Description);
                        }
                        else if (atributo.Equals("InstalationManual"))
                        {
                            IMachine.InstalationManual.Add(item.Item, item.Description);
                        }
                        else if (atributo.Equals("MCleaningManual"))
                        {
                            IMachine.MCleaningManual.Add(item.Item, item.Description);
                        }
                        else if (atributo.Equals("MReplacementManualDB"))
                        {
                            IMachine.MReplacementManual.Add(item.Item, item.Description);
                        }
                        else if (atributo.Equals("UserManual"))
                        {
                            IMachine.UserManual.Add(item.Item, item.Description);
                        }
                    }
                }
            }
        }

        public Machine GetById(int id)
        {
            return _connection.Table<Machine>().Where(x => x.Id == id).FirstOrDefault();
        }
    }
}

