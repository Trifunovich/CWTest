using System;
using CWTest.Core.DataManipulation;
using DataAccess.CWTest.Abstraction;
using DataServiceProvider.Core.DtoAbstraction;

namespace DataServiceProvider.TestBench.Dtos
{
    public class ComponentsDto : DtoBase, IComponentSpecification
    {
        public override void SetId(IDAbstraction idAbstraction)
        {
            IdAbstraction = idAbstraction;
        }

        public int ParentId { get; set; }
        public AssignmentType Assignment { get; set; }
        public bool InitializationTest { get; set; }
        public string SerialNumber { get; set; }
        public bool HasSerialNumber => !string.IsNullOrEmpty(SerialNumber);
    }
}
