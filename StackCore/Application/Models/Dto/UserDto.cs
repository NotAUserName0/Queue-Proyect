using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public UserDto(int id, string name, int age, string email, string password)
        {
            Id = id;
            Name = name;
            Age = age;
            Email = email;
            Password = password;
        }
    }
}
