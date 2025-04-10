using System;
using System.Collections.Generic;
using SQLite4Unity3d;

namespace Models
{
    public class MaintananceEvents
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public DateTime Dates { get;set; }
        public int UserId { get; set; }  
        public string Machine { get; set; }  
        public string Description { get; set; }  
        public string Duration { get; set; }  
        public int MTypeId { get; set; }  
        public string Images {get; set;}

        public override string ToString ()
        {
            return string.Format (
                "[MaintananceEvents: Id={0}, Dates={1}, UserId={2}, Machine={3},Description={4}"+
                ", Duration={5}, MTypeId={6}, Images={7}]", 
                Id, Dates, UserId,Machine,Description,Duration,MTypeId,Images);
        }
    }
}