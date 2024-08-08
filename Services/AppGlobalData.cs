﻿
namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class AppGlobalData : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ChatSessionModel> _sessions =
            new ObservableCollection<ChatSessionModel>();

        [ObservableProperty]
        private ChatSessionModel _selectedSession;
    }
}
