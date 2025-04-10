using System;
using System.Collections.Generic;
using Controllers;
using SQLite4Unity3d;
using UnityEngine;

namespace Models
{
    public class Machine
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MachineType { get; set; }
        public string Url { get; set; }
        public string AssembleManualDB { get; set; }
        public string InstalationManualDB { get; set; }
        public string MCleaningManualDB { get; set; }
        public string MReplacementManualDB { get; set; }
        public string UserManualDB { get; set; }
        public string DatasheetDB { get; set; }
        public string BluePrintsDB { get; set; }

        //Atributos no DB
        public Dictionary<string, string> AssembleManual { get; set; }
        public Dictionary<string, string> MCleaningManual { get; set; }
        public Dictionary<string, string> InstalationManual { get; set; }
        public Dictionary<string, string> MReplacementManual { get; set; }
        public Dictionary<string, string> UserManual { get; set; }
        public Dictionary<string, string> DataSheet { get; set; }
        public Dictionary<string, string> BluePrints { get; set; }


        public override string ToString()
        {
            return string.Format(
                "[Machine: Id={0}, Name={1}, Code={2}, MachineType={3},Url={4}" +
                ",AssembleManualDB={5}, InstalationManualDB={6}, MCleaningManualDB={7}, MReplacementManualDB={8}, UserManualDB={9}, DatasheetDB={10}, BluePrintsDB={11}]",
                Id, Name, Code, MachineType, Url, AssembleManualDB, InstalationManualDB, MCleaningManualDB, MReplacementManualDB, UserManualDB, DatasheetDB, BluePrintsDB);
        }
    }
}