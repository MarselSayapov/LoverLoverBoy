﻿namespace Domain.Entities;

public class RefreshToken
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public Guid UserId { get; set; }
}