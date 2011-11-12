﻿/*
    Copyright (C) 2011 de4dot@gmail.com

    This file is part of de4dot.

    de4dot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    de4dot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with de4dot.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using AssemblyData;

namespace AssemblyServer {
	public static class Start {
		public static int main(string[] args) {
			if (args.Length != 2)
				Environment.Exit(1);
			var channelName = args[0];
			var uri = args[1];

			var service = new AssemblyService();
			startServer(service, channelName, uri);
			service.waitExit();
			return 0;
		}

		static void startServer(AssemblyService service, string name, string uri) {
			var props = new Hashtable();
			props["portName"] = name;
			var provider = new BinaryServerFormatterSinkProvider();
			provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
			var channel = new IpcServerChannel(props, provider);
			ChannelServices.RegisterChannel(channel, false);
			RemotingServices.Marshal(service, uri);
		}
	}
}
