using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDesk.Domain.Common;

namespace HelpDesk.Domain.Entities
{
    public class Admin: AuditableEntity
    {
        [Key]
        [Required]
        public int AdminId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
