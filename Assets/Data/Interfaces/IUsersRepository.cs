using System.Collections.Generic;
using Models;

public interface IUsersRepository {
    IEnumerable<Users> GetAll();
    IEnumerable<Users> GetById(int id);
    Users GetByPass(string Name, string Pass);
    Users Create(Users _Users);
    int UpdateById(Users _Users);
}