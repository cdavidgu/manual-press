using System;
using System.Collections.Generic;
using SQLite4Unity3d;

namespace Models
{
    public class Replacement
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Name { get;set; }
        public string Code { get; set; }  
        public string Image { get; set; }  
        public string Datasheet { get; set; }  
        public string UserManual { get; set; }  

        public override string ToString ()
        {
            return string.Format (
                "[Replacement: Id={0}, Name={1}, Code={2}, Image={3},Datasheet={4}"+
                ",UserManual={5}]",Id,Name,Code,Image,Datasheet,UserManual);
        }
    }
}