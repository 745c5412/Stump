﻿// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using NLog;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Enums;

namespace Stump.Tools.Proxy
{
    abstract class DerivedConnexion
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ServerConnexion Server
        { get; protected set; }

        public Client Client
        { get; protected set; }

        public Message LastClientMessage
        { get; private set; }

        public Message LastServerMessage
        { get; private set; }

        public bool Analyse
        { get; set; }

        public bool Debug
        { get; set; }

        protected bool isBind = false;

        public DerivedConnexion(Client client)
        {
            Client = client;

            /* Set Default Values */
            Analyse = true;
            Debug = false;

            /* Handle Client Message */
            Client.onMessageReceived += new Client.MessageReceived(Client_onMessageReceived);
            Client.onDisconnected += new Tools.Proxy.Client.Disconnected(Client_onDisconnected);

            /* Handle Server Message */
            Server.OnConnected += new ServerConnexion.Connected(Server_OnConnected);
            Server.onMessageReceived += new ServerConnexion.MessageReceived(ServerConnexion_onMessageReceived);
            Server.OnDisconnected += new ServerConnexion.Disconnected(Server_OnDisconnected);
        }

        public void BindToServer(IPEndPoint serverEndPoint)
        {
            Server = new ServerConnexion(serverEndPoint);

            Server.Connect();
        }

        void Server_OnConnected()
        {
            isBind = true;
        }

        void ServerConnexion_onMessageReceived(DofusProtocol.Messages.Message message)
        {
            /* Set last Message */
            LastServerMessage = message;

            /* Only if Analyse is activate */
            if (Analyse)
            {
                if (!Proxy.HandlerManager.Dispatch(message, this))
                    Client.Send(message);
            }
            else
                Client.Send(message);
        }

        void Server_OnDisconnected()
        {    
            isBind = false;
        }

        void Client_onMessageReceived(DofusProtocol.Messages.Message message)
        {
            /* Set last Message */
            LastClientMessage = message;

            /* Only if Analyse is activate */
            if (Analyse)
            {
                if (!Proxy.HandlerManager.Dispatch(message, this))
                    Server.Send(message);
            }
            else
                Server.Send(message);
        }

        void Client_onDisconnected()
        {
            /* Do the Same */
            Server.Disconnect();
        }

        public void SendClientChatMessage(string message)
        {
            Client.Send(new ChatServerMessage().initChatServerMessage((uint)ChatActivableChannelsEnum.CHANNEL_ADMIN, message, 0, "", 0, "PROXY", 0));
        }

    }
}
