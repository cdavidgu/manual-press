using System.Collections.Generic;
using Models;

public interface IMaintananceEventRepository {
    IEnumerable<MaintananceEvents> GetAll();
    IEnumerable<MaintananceEvents> GetById(int id);
    MaintananceEvents Create(MaintananceEvents _MaintananceEvents);
    int UpdateById(MaintananceEvents _MaintananceEvents);
}