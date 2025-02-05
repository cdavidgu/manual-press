using SQLite4Unity3d;

namespace Models
{
    public class MaintenanceType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }  
        public string Code { get; set; }  

        public override string ToString ()
        {
            return string.Format ("[MaintenanceType: Id={0}, Name={1}, Code={2}]", Id, Name, Code);
        }
    }
}