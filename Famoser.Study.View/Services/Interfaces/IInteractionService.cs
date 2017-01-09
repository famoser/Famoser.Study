﻿using System;
using System.Threading.Tasks;

namespace Famoser.Study.View.Services.Interfaces
{
    public interface IInteractionService
    {
        void OpenInBrowser(Uri uri);
        void CheckBeginInvokeOnUi(Action action);

        Task<bool> ConfirmMessage(string message);
    }
}