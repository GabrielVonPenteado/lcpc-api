using System;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public abstract class BaseModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; private set; }
        public void SetDeletedAt()
        {
            DeletedAt = DateTime.UtcNow;
        }
        public void Restore()
        {
            DeletedAt = null;
        }
    }
}
