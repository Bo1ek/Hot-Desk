﻿namespace SoftwareMind.Application.Common.Models;
public class Desk
{
    public int Id { get; set; }
    public bool IsAvailable { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }
}