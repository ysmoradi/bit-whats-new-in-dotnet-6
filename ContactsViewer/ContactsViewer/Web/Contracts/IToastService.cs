﻿namespace ContactsViewer.App.Contracts
{
    public interface IToastService
    {
        Task ShowToast(string message);
    }
}
