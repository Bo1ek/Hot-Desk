﻿namespace SoftwareMind.Application.Common.Exceptions;
public class DeskAlreadyReservedException : Exception
{
    public DeskAlreadyReservedException(int deskId) : base($"Desk with id {deskId} is already reserved")
    {
         
    }
}
