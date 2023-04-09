using System;

namespace dotnet_lab5_src.RandomGenerators;

public static class RandomClientGenerator
{
    private const int MinClients = 5;
    private const int MaxClients = 7;
    private static string[] _namesDictionary = new string[]
    {
        "James",
        "Caitlyn",
        "Hank",
        "Mary",
        "Vadim",
        "Dionis",
        "Kris",
        "Xardas",
        "Jacob",
        "Walter"
    };

    private static string[] _surnamesDictionary = new string[]
    {
        "Smith",
        "Black",
        "White",
        "King",
        "Breadly",
        "Salamanca",
        "Jeager",
        "Hayakava",
        "Gray",
        "Dobkin"
    };

    private static string GetRandomClientName()
    {
        var name = _namesDictionary[Random.Shared.Next(_namesDictionary.Length)];
        var surname = _surnamesDictionary[Random.Shared.Next(_surnamesDictionary.Length)];

        return $"{name} {surname}";
    }

    public static string[] GetRandomClients()
    {
        int amount = Random.Shared.Next(MinClients,MaxClients);

        var clients = new string[amount];
        for(int i = 0; i < clients.Length; i++)
        {
            clients[i] = GetRandomClientName();
        }

        return clients;
    }
}
