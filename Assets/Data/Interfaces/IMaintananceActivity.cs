using System.Collections.Generic;
using Models;
public interface IMaintananceActivity {
    List<MaintananceActivity> GetByMachine(int MachineId);
    MaintananceActivity GetById(int id);
}
