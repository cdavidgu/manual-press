using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using SQLite4Unity3d;
using Models;

namespace DataRepositories
{
    public class ReplacementRepositories : IReplacementRepository
    {
        private SQLiteConnection _connection;
        public ReplacementRepositories(){
            
           _connection = DataServiceClient.Instance.DataServiceClients();

        }
        
        public IEnumerable<Replacement> GetAll(){
            return _connection.Table<Replacement>();
        }

        public IEnumerable<Replacement> GetById(int id){
            return _connection.Table<Replacement>().Where(x => x.Id == id);
        }

        public Replacement Create(Replacement _Replacement){
            var p = new Replacement{
                    Name = _Replacement.Name,
                    Code = _Replacement.Code,
                    Image = _Replacement.Image,
                    Datasheet = _Replacement.Datasheet, 
                    UserManual = _Replacement.UserManual,
            };
            _connection.Insert (p);
            return p;
        }

        public int UpdateById(Replacement _Replacement) {
            var p = new Replacement{
                    Name = _Replacement.Name,
                    Code = _Replacement.Code,
                    Image = _Replacement.Image,
                    Datasheet = _Replacement.Datasheet, 
                    UserManual = _Replacement.UserManual,
            };
            int rowsAffected =  _connection.Update (p);
            return rowsAffected;
        } 
    }

}

