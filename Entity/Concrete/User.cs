﻿using System.ComponentModel.DataAnnotations;
using Entity.Identity;
using Object;

namespace Entity.Concrete
{
    public class User
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the phoneNumber
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the discount is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        public LibraryStatusEnum LibraryStatus { get; set; }
        public Guid IdentityId { get; set; }
        public ICollection<TakeOfBook> TakeOfBooks { get; set; }
        public static object Identity { get; set; }
    }
}
