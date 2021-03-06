﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class Doctor
    {
        public Doctor(int id, string name, string color)
        {
            Id = id;
            Name = name;
            Color = color;
        }
        public Doctor()
        {
            PackageIds = new List<int>();
            NurseIds = new List<int>();
            Color = "#FFEEEEEE";
            Id = 0;
            ShortName = "";
            Name = "";
        }
        public Doctor(string shortName)
        {
            PackageIds = new List<int>();
            NurseIds = new List<int>();
            Color = "#FFFFFFFF";
            Id = 0;
            ShortName = shortName;
            Name = "";
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public string Signature { get; set; }
        [DataMember]
        public List<int> PackageIds { get; set; }
        [DataMember]
        public List<int> NurseIds { get; set; }
        public List<ResultMessage> Validate()
        {
            List<ResultMessage> em = new List<ResultMessage>();
            if (string.IsNullOrEmpty(Name)) em.Add(new ResultMessage(2, "Наименование:", OperationErrors.EmptyString));
            if (string.IsNullOrEmpty(ShortName)) em.Add(new ResultMessage(2, "Краткое наименование:", OperationErrors.EmptyString));
            return em;
        }
    }
}
