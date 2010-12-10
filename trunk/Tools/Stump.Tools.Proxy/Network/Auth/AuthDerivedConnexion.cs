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
    class AuthDerivedConnexion : DerivedConnexion
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();


        public AuthDerivedConnexion(IPEndPoint serverEndPoint, Client client)
            :base(client)
        {
            base.BindToServer(serverEndPoint);
        }

        




    }
}
