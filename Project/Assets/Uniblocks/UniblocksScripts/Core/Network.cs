using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Uniblocks
{
    public enum RPCMode
    {
        None,
        Server,
    }

    public class Network : MonoBehaviour
    {
        internal static bool isClient;
        internal static NetworkPlayer player;
        internal static IEnumerable<NetworkPlayer> connections;

        public static bool isServer { get; internal set; }

        internal static void Connect(string serverIP, int port, string serverPassword)
        {
            throw new NotImplementedException();
        }

        internal static void InitializeServer(int maxConnections, int port, bool useNat)
        {
            throw new NotImplementedException();
        }

        internal static void Instantiate(GameObject uniblocksNetworkPrefab, Vector3 position, Quaternion rotation, int v)
        {
            throw new NotImplementedException();
        }
    }
}
