﻿using System.Collections.Generic;
using ClientsController.Presenter;
using SignalR.Core.Model;
using SignalR.Core.View;

namespace ClientsController.View
{
    public interface IMainControllerView : IView<IMainControllerView>
    {
        string ServerIP { get; set; }
        string ServerPort { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Message { get; set; }
        List<User> Users { get; set; }
        bool ServerControlsEnable { get; set; }
        string ConnectionState { get; set; }

        void ActiveControls(bool active);

        new MainControllerPresenter Presenter { get; }

        void SetColumns();

        List<User> GetSelectedUsers();
    }
}
