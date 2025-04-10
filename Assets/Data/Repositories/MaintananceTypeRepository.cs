using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using SQLite4Unity3d;
using Models;

namespace DataRepositories
{
    public class MaintananceTypeRepository :IMaintananceTypeRepository
    {
        private SQLiteConnection _connection;
        public MaintananceTypeRepository(){
            
            _connection = DataServiceClient.Instance.DataServiceClients();

        }
        
        public IEnumerable<MaintenanceType> GetAll(){
            return _connection.Table<MaintenanceType>();
        }

        public IEnumerable<MaintenanceType> GetById(int id){
            return _connection.Table<MaintenanceType>().Where(x => x.Id == id);
        }

        public MaintenanceType Create(MaintenanceType _MaintanceType){
            var p = new MaintenanceType{
                    Name = _MaintanceType.Name,
                    Code = _MaintanceType.Code,
            };
            _connection.Insert (p);
            return p;
        }

        public int UpdateById(MaintenanceType _MaintanceType) {
            var ret = new MaintenanceType{
                Name = _MaintanceType.Name,
                Code = _MaintanceType.Code,
            };
            int rowsAffected =  _connection.Update (ret);
            return rowsAffected;
        } 
    }

}

