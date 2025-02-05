using System.Collections.Generic;
using Models;

public interface IReplacementRepository {
    IEnumerable<Replacement> GetAll();
    IEnumerable<Replacement> GetById(int id);
    Replacement Create(Replacement _Replacement);
    int UpdateById(Replacement _Replacement);
}