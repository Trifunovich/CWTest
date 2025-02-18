﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using CWTest.Core.DataManipulation;
using DataAccess.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("DataAccess.Tests")]
namespace DataAccess.Sql.Models
{
    public abstract class SqlDataModelBase : DataModelAbstractionBase, ISqlDataModelBase
    {
        private IDAbstraction _idAbstraction;

        [NotMapped]
        public override IDAbstraction IdAbstraction => _idAbstraction ??= new GuidAbstraction(Id);

        public override void SetId(IDAbstraction idAbstraction)
        {
            _idAbstraction = idAbstraction;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public override int InternalId { get; set; }

        [Timestamp]
        public override byte[] Timestamp { get; set; }

        public override string ToString()
        {
            return $"{GetType().Name}: [IdAbstraction: {IdAbstraction} {base.ToString()}]";
        }
    }
}
