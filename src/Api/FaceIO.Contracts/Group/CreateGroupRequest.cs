﻿namespace FaceIO.Contracts.Group
{
    public class CreateGroupRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}