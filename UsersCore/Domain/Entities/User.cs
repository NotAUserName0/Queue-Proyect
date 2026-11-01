using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

        public UserEntity(string name, int age, string email, string password)
        {
            Name = name;
            Age = age;
            Email = email;
            Password = password;
            CreatedAt = DateTime.UtcNow;
        }

        public UserEntity(int id, string name, int age, string email, string password)
        {
            Id = id;
            Name = name;
            Age = age;
            Email = email;
            Password = password;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
