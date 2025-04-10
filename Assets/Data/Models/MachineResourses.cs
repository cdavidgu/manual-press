using System;
using System.Collections.Generic;
using Controllers;
using SQLite4Unity3d;
using UnityEngine;

namespace Models
{
    public class MachineResources
    {
        // public string[] R1 { get; set; }
        public List<R1> R1 { get; set; }
        public string[] R2 { get; set; }

    }

    public class R1
    {
        public string Description { get; set; }
        public string Item { get; set; }
    }
}