﻿namespace FaceIO.Contracts.Location
{
    public class CreateLocationRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
