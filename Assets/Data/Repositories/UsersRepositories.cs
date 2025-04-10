using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using SQLite4Unity3d;
using Models;

namespace DataRepositories
{
    public class UsersRepositories : IUsersRepository
    {
        private SQLiteConnection _connection;
        public UsersRepositories(){
            
           _connection = DataServiceClient.Instance.DataServiceClients();

        }
        
        public IEnumerable<Users> GetAll(){
            return _connection.Table<Users>();
        }

        public IEnumerable<Users> GetById(int id){
            return _connection.Table<Users>().Where(x => x.Id == id);
        }

        public Users GetByPass(string Name, string Pass){
            return _connection.Table<Users>().Where(x => x.Name == Name && x.Pass == Pass).FirstOrDefault();
        }

        public Users Create(Users _Users){
            var p = new Users{
                    Name = _Users.Name,
                    Email = _Users.Email,
                    Pass = _Users.Pass,
            };
            _connection.Insert (p);
            return p;
        }

        public int UpdateById(Users _Users) {
            var p = new Users{
                    Name = _Users.Name,
                    Email = _Users.Email,
                    Pass = _Users.Pass,
            };
            int rowsAffected =  _connection.Update (p);
            return rowsAffected;
        } 
    }

}

