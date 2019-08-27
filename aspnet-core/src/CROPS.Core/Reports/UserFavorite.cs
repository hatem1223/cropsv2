using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CROPS.Reports
{
    public class UserFavorite : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string ReportId { get; set; }

        public string DashboardId { get; set; }

        public string UserId { get; set; }

        public bool IsTransient()
        {
            throw new NotImplementedException();
        }
    }
}
