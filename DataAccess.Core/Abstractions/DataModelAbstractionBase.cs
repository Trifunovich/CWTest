using System;
using System.ComponentModel.DataAnnotations;
using CWTest.Core.DataManipulation;

namespace DataAccess.Core.Abstractions
{
    public abstract class DataModelAbstractionBase : IDataModel
    {
        protected DataModelAbstractionBase()
        {
            CreatedOn = DateTime.Now;
        }

        public abstract IDAbstraction IdAbstraction { get; }

        [Required]
        public virtual int InternalId { get; set; }

        [MaxLength(100, ErrorMessage = "Label too long")]
        public virtual string Label { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual byte[] Timestamp { get; set; }

        public abstract void SetId(IDAbstraction idAbstraction);


        public override string ToString()
        {
            return $"{InternalId}, {Label}, {CreatedOn}, {Timestamp}, {IsActive}";
        }
    }
}