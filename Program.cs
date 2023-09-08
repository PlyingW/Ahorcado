using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System;
using System.Collections.Generic;

class Program
{
    static Random random = new Random();

    static async System.Threading.Tasks.Task Main(string[] args)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://pokeapi.co/api/v2/pokemon?limit=100000&offset=0")
        };

        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            string body = await response.Content.ReadAsStringAsync();
            JObject jsonObject = JsonConvert.DeserializeObject<JObject>(body);

            JArray resultsArray = (JArray)jsonObject["results"];

            List<string> pokemonNames = new List<string>();

            foreach (JObject result in resultsArray)
            {
                string nombre = (string)result["name"];
                pokemonNames.Add(nombre);
            }

            string selectedName = pokemonNames[random.Next(pokemonNames.Count)];
            string nompoke = new string('_', selectedName.Length);
            int Derrota = 4;
            List<char> Nombrespoke = new List<char>();

            Console.WriteLine("Bienvenido al ahorcado de Pokémon");
            Console.WriteLine("¿Quién es este Pokémon? " + nompoke);

            while (Derrota > 0)
            {
                Console.Write("Ingresa una letra: ");
                

                char letra = Console.ReadLine()[0];

                if (Nombrespoke.Contains(letra))
                {
                    Console.WriteLine("La letra ya ha sido usada");
                    continue;
                }

                Nombrespoke.Add(letra);

                if (selectedName.Contains(letra.ToString()))
                {
                    char[] NuevoNombre = nompoke.ToCharArray();

                    for (int i = 0; i < selectedName.Length; i++)
                    {
                        if (selectedName[i] == letra)
                        {
                            NuevoNombre[i] = letra;
                        }
                    }

                    nompoke = new string(NuevoNombre);
                }
                else
                {
                    Derrota--;
                    Console.WriteLine($"La letra '{letra}' no está en el nombre. Intentos restantes: {Derrota}");
                }

                Console.WriteLine("Nombre del Pokémon: " + nompoke);

                if (nompoke == selectedName)
                {
                   
                    Console.WriteLine("EEEEEEEEEEESSSSSS: " + selectedName);
                    await MostrarEstadisticasPokemon(selectedName);
                    break;
                }
            }

            if (nompoke != selectedName)
            {
                Console.WriteLine("El Pokémon era: " + selectedName);
            }
        }
    }

    static async System.Threading.Tasks.Task MostrarEstadisticasPokemon(string nombrePokemon)
    {
        var Llamar = new HttpClient();
        var Valores = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://pokeapi.co/api/v2/pokemon/{nombrePokemon}")
        };

        using (var stadisticas = await Llamar.SendAsync(Valores))
        {
            stadisticas.EnsureSuccessStatusCode();
            string body = await stadisticas.Content.ReadAsStringAsync();
            JObject pokemonStats = JsonConvert.DeserializeObject<JObject>(body);

            Console.WriteLine("Estadísticas del Pokémon:");
            JArray Stats = (JArray)pokemonStats["stats"];
            foreach (JObject stat in Stats)
            {
                string statName = (string)stat["stat"]["name"];
                int baseStat = (int)stat["base_stat"];
                Console.WriteLine($"{statName}: {baseStat}");
            }
        }
    }
}
