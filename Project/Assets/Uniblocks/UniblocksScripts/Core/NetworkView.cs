using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Uniblocks
{
    public class NetworkView : MonoBehaviour
    {
        internal void RPC(string v, NetworkPlayer player, int chunkx, int chunky, int chunkz, byte[] dataBytes)
        {
            throw new NotImplementedException();
        }

        internal void RPC(string v, NetworkPlayer player, NetworkPlayer sender, int x, int y, int z, int chunkx, int chunky, int chunkz, int data)
        {
            throw new NotImplementedException();
        }

        internal void RPC(string v, RPCMode server, int player, int x, int y, int z)
        {
            throw new NotImplementedException();
        }

        internal void RPC(string v, RPCMode server, int player, int range)
        {
            throw new NotImplementedException();
        }

        internal void RPC(string v, RPCMode server, NetworkPlayer player, int x, int y, int z, int chunkx, int chunky, int chunkz, int data)
        {
            throw new NotImplementedException();
        }

        internal void RPC(string v, RPCMode server, NetworkPlayer player, int x, int y, int z)
        {
            throw new NotImplementedException();
        }

        internal void RPC(string v, RPCMode server, NetworkPlayer player, int range)
        {
            throw new NotImplementedException();
        }
    }
}
