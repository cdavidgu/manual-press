using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using SQLite4Unity3d;
using Models;

namespace DataRepositories
{
    public class ReplaceEventRepository : IReplaceEventRepository
    {
        private SQLiteConnection _connection;
        public ReplaceEventRepository(){
            
           _connection = DataServiceClient.Instance.DataServiceClients();

        }
        
        public IEnumerable<Replace_Event> GetAll(){
            return _connection.Table<Replace_Event>();
        }

        //IdMaintenaceEvent
        public IEnumerable<Replace_Event> GetByIdMaintenaceEvent(int id){
            return _connection.Table<Replace_Event>().Where(x => x.IdMaintenaceEvent == id);
        }
        //ReplacementId
        public IEnumerable<Replace_Event> GetByReplacementId(int id){
            return _connection.Table<Replace_Event>().Where(x => x.ReplacementId == id);
        }
        //MachineType
        public IEnumerable<Replace_Event> GetByMachineType(int id){
            return _connection.Table<Replace_Event>().Where(x => x.MachineType == id);
        }

        public Replace_Event Create(Replace_Event _ReplaceEvent){
            var p = new Replace_Event{
                    IdMaintenaceEvent = _ReplaceEvent.IdMaintenaceEvent,
                    ReplacementId = _ReplaceEvent.ReplacementId,
                    MachineType=_ReplaceEvent.MachineType,
            };
            _connection.Insert (p);
            return p;
        }

        public int UpdateById(Replace_Event _ReplaceEvent) {
           var p = new Replace_Event{
                    IdMaintenaceEvent = _ReplaceEvent.IdMaintenaceEvent,
                    ReplacementId = _ReplaceEvent.ReplacementId,
                    MachineType=_ReplaceEvent.MachineType,
            };
            int rowsAffected =  _connection.Update (p);
            return rowsAffected;
        } 
    }

}

