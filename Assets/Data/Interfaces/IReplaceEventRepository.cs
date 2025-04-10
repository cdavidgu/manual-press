using System.Collections.Generic;
using Models;

public interface IReplaceEventRepository {
    IEnumerable<Replace_Event> GetAll();
    IEnumerable<Replace_Event> GetByIdMaintenaceEvent(int id);
    IEnumerable<Replace_Event> GetByReplacementId(int id);
    IEnumerable<Replace_Event> GetByMachineType(int id);
    Replace_Event Create(Replace_Event _Replace_Event);
    int UpdateById(Replace_Event _Replace_Event);
}