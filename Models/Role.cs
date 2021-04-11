using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkAppReactAPI.Models
{
    public class Role
    {

        [Key]
        public Guid Id { set; get; }
        [Required]
        public string FunctionCode { set; get; }
        [Required]
        [MaxLength(250)]
        public string Name { set; get; }
        public bool isSearch { get; set; }
        public bool isInsert { set; get; }
        public bool isUpdate { set; get; }
        public bool isDelete { set; get; }
        public bool isExport { set; get; }
        public bool isImport { set; get; }

        public virtual ICollection<UserRole> UserRoles { set; get; }

    }
}