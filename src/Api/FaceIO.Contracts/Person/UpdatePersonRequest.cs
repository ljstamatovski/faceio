﻿namespace FaceIO.Contracts.Person
{
    public class UpdatePersonRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}