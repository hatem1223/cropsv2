using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities;

namespace CROPS.Projects
{
    public class Account : IEntity
    {
        [Key]
        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsActive { get; set; }

        public int? ParentId { get; set; }

        public string Descriptions { get; set; }

        [NotMapped]
        public int Id { get => this.AccountId; set => this.AccountId = this.Id; }

        public bool IsTransient()
        {
            return Convert.ToInt32(this.Id) <= 0;
        }
    }
}
