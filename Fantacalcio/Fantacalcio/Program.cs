using System;
using System.Collections.Generic;
using System.IO;

namespace Fantacalcio
{
    class Calciatore
    {
        string nome;
        string squadra;
        public Calciatore(string nome, string nomeSquadra)
        {
            this.nome = nome;
            this.squadra = nomeSquadra;
        }

        public string toString()
        {
            return "";
        }
    }

    class Squadra
    {
        string nome;
        string percorso;
        public int punti;
        public Squadra(string squadra)
        {
            this.nome = squadra;
            percorso = AppDomain.CurrentDomain.BaseDirectory + $"{nome}.txt";
            punti = 0;
        }

        public string toString()
        {
            StreamReader squadra = new StreamReader(percorso);
            string giocatori = squadra.ReadToEnd();
            return $"{giocatori}";
        }
    }
    class Fanta
    {
        List<string> nomi = new List<string>();
        public Fanta(List<string> nomiSq)
        {
            this.nomi = nomiSq;
        }

        public string toString()
        {
            string s = "";
            foreach (string i in nomi)
            {
                s += $"{i}\n";
            }
            return s;
        }
        static void Main(string[] args)
        {
            bool valido;
            int nSquadre;
            string nomeSq;
            string nomeCa;
            string percorso;
            string percorsoFileSq = AppDomain.CurrentDomain.BaseDirectory + "Squadre.txt";
            string imp = AppDomain.CurrentDomain.BaseDirectory + "status.txt";
            int prezzo;
            List<string> nomiRose = new List<string>();
            Squadra squadra;
            if (!File.Exists(imp))
            {
                do
                {
                    Console.WriteLine("Inserisci quante squadre ci sono nel campionato (numero pari tra 2 e 8)");
                    string sq = Convert.ToString(Console.ReadLine());
                    valido = int.TryParse(sq, out nSquadre);
                    Console.WriteLine("");
                    Console.Clear();
                } while (!valido || nSquadre > 9 || nSquadre < 1 || nSquadre % 2 != 0);

                for (int i = 0; i < nSquadre; i++)
                {
                    do
                    {
                        Console.WriteLine($"Inserisci il nome della {i + 1}° squadra");
                        nomeSq = Convert.ToString(Console.ReadLine());
                        Console.WriteLine("");
                    } while (nomeSq == "" || nomiRose.Contains(nomeSq));
                    StreamWriter fileNomi = new StreamWriter(percorsoFileSq, true);
                    fileNomi.WriteLine($"{nomeSq}");
                    fileNomi.Close();
                    percorso = AppDomain.CurrentDomain.BaseDirectory + $"{nomeSq}.txt";
                    nomiRose.Add(nomeSq);
                    StreamWriter fileSq = new StreamWriter(percorso, true);
                    for (int j = 0; j < 2; j++)
                    {
                        do
                        {
                            Console.WriteLine($"Inserisci il nome del {j + 1}° giocatore della {nomiRose[i]}");
                            nomeCa = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("");
                            do
                            {
                                Console.WriteLine($"Inserisci il prezzo d'acquisto di {nomeCa} della squadra {nomiRose[i]}");
                                string p = Convert.ToString(Console.ReadLine());
                                valido = int.TryParse(p, out prezzo);
                                Console.WriteLine("");
                            } while (!valido || prezzo < 1);
                            Console.Clear();
                        } while (nomeCa == "");
                        fileSq.WriteLine($"Nome: {nomeCa}, Prezzo d'acquisto: {prezzo}");
                    }
                    fileSq.Close();

                }

            }
            if (File.Exists(imp))
            {
                foreach (string squadre in File.ReadLines(percorsoFileSq))
                {
                    nomiRose.Add(squadre);
                }
            }
            StreamWriter impostazione = new StreamWriter(imp);
            impostazione.Close();
            Fanta fanta = new Fanta(nomiRose);
            for (int i = 0; i < nomiRose.Count; i++)
            {
                squadra = new Squadra(nomiRose[i].ToString());
                Console.WriteLine($"I giocatori della squadra {nomiRose[i]} sono:\n{squadra.toString()}Punti: {squadra.punti}\n");
            }

            Console.ReadKey();
        }
    }
}
