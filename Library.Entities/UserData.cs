using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class UserData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Todo: Guardar o hash?
        public DateTime DateOfBirth { get; set; }
        public Guid IdentificationNumber { get; set; }
        public Guid IdentificationType { get; set; }
        public Guid Gender { get; set; }
        public Guid Nationality { get; set; }
        public Guid Occupation { get; set; }
        public Guid EducationLevel { get; set; }
        public 
        public User User { get; set; }
    }
}
