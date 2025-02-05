using SQLite4Unity3d;

namespace Models
{
    public class MaintananceActivity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }  
        public int MachineId { get; set; }  
        public int MTypeId { get; set; }

        public override string ToString ()
        {
            return string.Format ("[MaintananceActivity: Id={0}, Description={1}, MachineId={2}, MTypeId={3}", Id, Description, MachineId,MTypeId);
        }
    }
}