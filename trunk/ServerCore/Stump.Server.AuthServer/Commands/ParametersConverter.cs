// /*************************************************************************
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
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.AuthServer.Commands
{
    public static class ParametersConverter
    {
        public static Func<string, TriggerBase, RoleEnum> RoleConverter = (entry, trigger) =>
        {
            RoleEnum result;
            if (Enum.TryParse(entry, true, out result))
            {
                return result;
            }

            byte value;
            if (byte.TryParse(entry, out value))
                return (RoleEnum) Enum.ToObject(typeof (RoleEnum), value);

            throw new ArgumentException("entry is not RoleEnum");
        };
    }
}