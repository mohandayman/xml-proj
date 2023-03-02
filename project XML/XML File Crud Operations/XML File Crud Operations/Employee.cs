using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_File_Crud_Operations
{
    public class Employee
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public Employee() { }
        public Employee(string name, string phone, string address, string email)
        {
            Name = name;
            Phone = phone;
            Address = address;
            Email = email;
        }
    }
}
