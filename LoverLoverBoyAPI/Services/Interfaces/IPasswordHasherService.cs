﻿namespace Services.Interfaces;

public interface IPasswordHasherService
{
    public string HashPassword(string password);
    public bool VerifyPassword(string hashedPassword, string password);
}