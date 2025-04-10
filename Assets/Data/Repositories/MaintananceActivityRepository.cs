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
    public class MaintananceActivityRepository : IMaintananceActivity
    {
        private SQLiteConnection _connection;
        public MaintananceActivityRepository()
        {

            _connection = DataServiceClient.Instance.DataServiceClients();

        }

        public List<MaintananceActivity> GetByMachine(int MachineId) 
        {
            List<MaintananceActivity> _MaintenanceActivity = _connection.Table<MaintananceActivity>().Where(x => x.Id == MachineId).ToList();

            return _MaintenanceActivity;
        }

        public MaintananceActivity GetById(int id)
        {
            return _connection.Table<MaintananceActivity>().Where(x => x.Id == id).FirstOrDefault();
        }
    }
}

