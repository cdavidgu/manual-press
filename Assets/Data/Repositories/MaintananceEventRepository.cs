using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using SQLite4Unity3d;
using Models;

namespace DataRepositories
{
    public class MaintananceEventRepository : IMaintananceEventRepository
    {
        private SQLiteConnection _connection;
        public MaintananceEventRepository(){
            
           _connection = DataServiceClient.Instance.DataServiceClients();

        }
        
        public IEnumerable<MaintananceEvents> GetAll(){
            return _connection.Table<MaintananceEvents>();
        }

        public IEnumerable<MaintananceEvents> GetById(int id){
            return _connection.Table<MaintananceEvents>().Where(x => x.Id == id);
        }

        public MaintananceEvents Create(MaintananceEvents _MaintanceEvent){
            var p = new MaintananceEvents{
                    Dates = _MaintanceEvent.Dates,
                    UserId=_MaintanceEvent.UserId,
                    Machine = _MaintanceEvent.Machine,
                    Description = _MaintanceEvent.Description,
                    Duration = _MaintanceEvent.Duration,
                    MTypeId = _MaintanceEvent.MTypeId,
                    Images = _MaintanceEvent.Images,
            };
            _connection.Insert (p);
            return p;
        }

        public int UpdateById(MaintananceEvents _MaintanceEvent) {
            var p = new MaintananceEvents{
                    Dates = _MaintanceEvent.Dates,
                    UserId=_MaintanceEvent.UserId,
                    Machine = _MaintanceEvent.Machine,
                    Description = _MaintanceEvent.Description,
                    Duration = _MaintanceEvent.Duration,
                    MTypeId = _MaintanceEvent.MTypeId,
                    Images = _MaintanceEvent.Images,
            };
            int rowsAffected =  _connection.Update (p);
            return rowsAffected;
        } 
    }

}

