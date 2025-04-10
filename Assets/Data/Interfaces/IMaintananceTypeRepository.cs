using System.Collections.Generic;
using Models;

public interface IMaintananceTypeRepository {
    IEnumerable<MaintenanceType> GetAll();
    IEnumerable<MaintenanceType> GetById(int id);
    MaintenanceType Create(MaintenanceType _MaintanceType);
    int UpdateById(MaintenanceType _MaintanceType);
}