using System;
using System.Collections.Generic;
using SQLite4Unity3d;

namespace Models
{
    public class Replace_Event
    {
        public int IdMaintenaceEvent { get;set; }
        public int ReplacementId { get; set; }  
        public int MachineType { get; set; }  

        public override string ToString ()
        {
            return string.Format (
                "[Replace_Event: IdMaintenaceEvent={0}, ReplacementId={1}, MachineType={2}]", 
                IdMaintenaceEvent,ReplacementId,MachineType);
        }
    }
}