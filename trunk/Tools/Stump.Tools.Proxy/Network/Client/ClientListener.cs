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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Pool;

namespace Stump.Tools.Proxy
{
    public sealed class ClientListener
    {

        public string Host = "localhost";

        public int Port = 5555;

        public int MaxConcurrentConnections = 2000;

        public int MaxPendingConnections = 100;

        /// <summary>
        /// Buffer size /!\ Advanced users /!\
        /// </summary>
        [Variable(DefinableByConfig = false, DefinableRunning = false)]
        public static int BufferSize = 8192;

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly SocketAsyncEventArgs m_acceptArgs = new SocketAsyncEventArgs();
        private readonly List<Client> m_clientList = new List<Client>();
        private readonly SemaphoreSlim m_clientSemaphore;
        public delegate void ClientConnexion(Client client);
        public event ClientConnexion onClientConnexion;

        private readonly IPEndPoint m_ipEndPoint;

        private readonly Socket m_listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                                            ProtocolType.Tcp);

        private readonly int m_maxConcurrentConnexion;

        private readonly SocketAsyncEventArgsPool m_readAsyncEventArgsPool;
        private readonly BufferManager m_readBufferManager;
        private readonly int m_readBufferSize;
        private readonly SocketAsyncEventArgsPool m_writeAsyncEventArgsPool;

        public ClientListener(string host, int port, int maxConcurrentConnexions, int maxPendingConnections)
        {  
            m_ipEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
            m_readBufferSize = BufferSize;
            Host = host;
            Port = port;
            MaxPendingConnections = maxPendingConnections;
            m_maxConcurrentConnexion = maxConcurrentConnexions;

            m_readBufferManager = new BufferManager(m_maxConcurrentConnexion*m_readBufferSize, m_readBufferSize);

            m_readAsyncEventArgsPool = new SocketAsyncEventArgsPool(m_maxConcurrentConnexion);
            m_writeAsyncEventArgsPool = new SocketAsyncEventArgsPool(m_maxConcurrentConnexion*3);

            m_clientSemaphore = new SemaphoreSlim(m_maxConcurrentConnexion, m_maxConcurrentConnexion);

            m_acceptArgs.Completed += Accept_Completed;

            Client.Initialize(ref m_writeAsyncEventArgsPool);
        }

        public List<Client> ClientList
        {
            get { return m_clientList; }
        }

        internal void Init()
        {
            m_readBufferManager.InitBuffer();

            SocketAsyncEventArgs args = null;

            // initialize read pool
            for (int i = 0; i < m_maxConcurrentConnexion; i++)
            {
                args = new SocketAsyncEventArgs();

                m_readBufferManager.SetBuffer(args);
                args.Completed += Receive_Completed;
                m_readAsyncEventArgsPool.Push(args);
            }

            // initialize write pool
            for (int i = 0; i < m_maxConcurrentConnexion*3; i++)
            {
                args = new SocketAsyncEventArgs();

                args.Completed += Send_Completed;
                m_writeAsyncEventArgsPool.Push(args);
            }
        }

        public void Start()
        {
            m_listenSocket.Bind(m_ipEndPoint);
            m_listenSocket.Listen(MaxPendingConnections);

            StartAccept();
        }

        public void Stop()
        {
            m_listenSocket.Close();
        }

        private void StartAccept()
        {
            m_acceptArgs.AcceptSocket = null;

            m_clientSemaphore.Wait();
            if (!m_listenSocket.AcceptAsync(m_acceptArgs))
            {
                ProcessAccept(m_acceptArgs);
            }
        }

        private void Accept_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }


        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            SocketAsyncEventArgs readAsyncEventArgs = m_readAsyncEventArgsPool.Pop();
            
            Client client = new Client(e.AcceptSocket);

            readAsyncEventArgs.UserToken = client;

            Console.WriteLine("Client connected <{0}>", client.IP);

            m_clientList.Add(client);

            if (onClientConnexion != null) onClientConnexion(client);

            if (!client.Socket.ReceiveAsync(readAsyncEventArgs))
            {
                ProcessReceive(readAsyncEventArgs);
            }

            StartAccept();
        }

        private void Receive_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Disconnect:
                    CloseClientSocket(e);
                    break;
                default:
                    break;
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                var client = e.UserToken as Client;

                if (client != null)
                {
                    client.ProcessReceive(e.Buffer, e.Offset, e.BytesTransferred);
                    bool willRaiseEvent = client.Socket.ReceiveAsync(e);

                    if (!willRaiseEvent)
                    {
                        ProcessReceive(e);
                    }
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        private void Send_Completed(object sender, SocketAsyncEventArgs e)
        {
            m_writeAsyncEventArgsPool.Push(e);
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            var client = e.UserToken as Client;

            if (client != null)
            {
                client.Disconnect();

                m_clientList.Remove(client);
            }
            m_clientSemaphore.Release();

            Console.WriteLine("Client disconnected");

            client = null;

            // Free the SocketAsyncEventArg so they can be reused by another client
            m_readAsyncEventArgsPool.Push(e);
        }
    }
}