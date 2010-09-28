﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using MemBus;
using MemBus.Configurators;
using Membus.WpfTwitterClient.Frame;
using Membus.WpfTwitterClient.Messages;
using StructureMap;

namespace Membus.WpfTwitterClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        private void getStarted(object sender, StartupEventArgs e)
        {
            ObjectFactory.Initialize(i=>i.AddRegistry<ClientRegistry>());
            ObjectFactory.Container.GetInstance<IBus>().Publish(new Bootstrap());
        }
    }
}
