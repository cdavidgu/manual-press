using System.Collections.Generic;
using Models;

public interface IMachineReposiory {
    List<Machine> GetAll();
    Machine GetById(int id);
}