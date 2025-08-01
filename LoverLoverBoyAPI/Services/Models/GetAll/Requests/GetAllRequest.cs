﻿namespace Services.Models.GetAll.Requests;

public class GetAllRequest
{
    private int _pageNumber = 1;
    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 10 ? int.MaxValue : value;
    }

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }
}