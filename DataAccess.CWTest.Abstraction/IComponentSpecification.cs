using DataAccess.Core.Abstractions;

namespace DataAccess.CWTest.Abstraction
{
    public enum AssignmentType
    {
        Unknown, Controller, CircuitCard, AteMaintenance, Drive
    }


    public interface IComponentSpecification : IHierarchicalModel
    {
        public AssignmentType Assignment { get; set; }
        public bool InitializationTest { get; set; }
        public string SerialNumber { get; set; }
        public bool HasSerialNumber { get; }
    }
}