﻿namespace ModelTracKer.Dto
{
    public class RegisterUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; 
        public int Role { get; set; }
    }
}
