using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Fantacalcio
{
    class Calciatore
    {
        string nome;
        string squadra;
        int prezzo;
        public Calciatore(string nome, string nomeSquadra, int prezzo)
        {
            this.nome = nome;
            this.squadra = nomeSquadra;
            this.prezzo = prezzo;
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

    class Partita
    {
        string sq1, sq2;
        public Partita(string squadra1, string squadra2)
        {
            this.sq1 = squadra1;
            this.sq2 = squadra2;
        }

        public string toString()
        {
            return $"{sq1} vs {sq2}";
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
        public Squadra squadra;
        public Calciatore calciatore;
        public Partita partita;
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
            int rnd1, rnd2;
            string sqs;
            List<string> nomiRose = new List<string>();
            List<int> partite = new List<int>();

            string rispostaNuovoFanta;
            string rispostaNuovaPartita;
        Repeat:
            if (!File.Exists(imp)) // Se il file status non esiste significa che il programma è stato lanciato per la prima volta o che è stato riavviato il Fantacalcio
            {
                do
                {
                    Console.WriteLine("Inserisci quante squadre ci sono nel campionato (numero pari tra 2 e 8)");
                    string sq = Convert.ToString(Console.ReadLine());
                    valido = int.TryParse(sq, out nSquadre);
                    Console.WriteLine("");
                    Console.Clear();
                } while (!valido || nSquadre > 9 || nSquadre < 1 || nSquadre % 2 != 0); // Viene chiesto quante sono le squadre da inserire e controlla se il numero inserito è un int, pari, maggiore di 1 e minore di 9
                
                for (int i = 0; i < nSquadre; i++)
                {
                    do
                    {
                        Console.WriteLine($"Inserisci il nome della {i + 1}° squadra");
                        nomeSq = Convert.ToString(Console.ReadLine());
                        Console.WriteLine("");
                    } while (nomeSq == "" || nomiRose.Contains(nomeSq)); // Viene chiesto il nome delle squadre finchè il nome non è vuoto o uguale al nome di un'altra squadra           
                    StreamWriter fileNomi = new StreamWriter(percorsoFileSq, true);
                    fileNomi.WriteLine($"{nomeSq}"); // Vengono scritti i nomi delle squadre su un file chiamato Squadre
                    fileNomi.Close();
                    percorso = AppDomain.CurrentDomain.BaseDirectory + $"{nomeSq}.txt"; // Viene creato un file per ogni squadra
                    nomiRose.Add(nomeSq); // Vengono aggiunti i nomi delle squadre alla lista nomiRose
                    StreamWriter fileSq = new StreamWriter(percorso, true);
                    for (int j = 0; j < 1; j++)
                    {
                        do
                        {
                            Console.WriteLine($"Inserisci il nome del {j + 1}° giocatore della {nomiRose[i]}");
                            nomeCa = Convert.ToString(Console.ReadLine()); // Vengono chiesti i nomi dei giocatori
                            Console.WriteLine("");                          
                            do
                            {
                                Console.WriteLine($"Inserisci il prezzo d'acquisto di {nomeCa} della squadra {nomiRose[i]}");
                                string p = Convert.ToString(Console.ReadLine());
                                valido = int.TryParse(p, out prezzo);
                                Console.WriteLine("");
                            } while (!valido || prezzo < 1);
                            Console.Clear();
                            Calciatore calciatore = new Calciatore(nomeCa, nomiRose[i], prezzo);
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
            Console.WriteLine("----------------------\n" +
                              "|Squadra   |   Punti:|\n" +
                              "----------------------\n");
            for (int i = 0; i < nomiRose.Count; i++)
            {
                Squadra squadra = new Squadra(nomiRose[i].ToString());
                Console.WriteLine($"{nomiRose[i]}   |   {squadra.punti}\n");
            }

            do
            {
                Console.WriteLine("Iniziare una nuova partita?");
                rispostaNuovaPartita = Convert.ToString(Console.ReadLine());
                Console.WriteLine("");
            } while (rispostaNuovaPartita.ToUpper() != "NO" && rispostaNuovaPartita.ToUpper() != "SI" && rispostaNuovaPartita.ToUpper() != "SÌ");

            if (rispostaNuovaPartita.ToUpper() == "SI" || rispostaNuovaPartita.ToUpper() == "SÌ")
            {
                Random randomSq = new Random();
                for (int i = 0; i < nomiRose.Count / 2; i++)
                {
                    do {
                        rnd1 = randomSq.Next(1, nomiRose.Count);
                        do
                        {
                            rnd2 = randomSq.Next(1, nomiRose.Count + 1);
                        } while (rnd2 == rnd1);
                    } while (partite.Contains(rnd1) || partite.Contains(rnd2));
                    partite.Add(rnd1);
                    partite.Add(rnd2);
                    Partita partita = new Partita(nomiRose[rnd1 - 1], nomiRose[rnd2 - 1]);
                    rnd1 = 0;
                    rnd2 = 0;
                    Console.WriteLine($"{partita.toString()}");
                }
            }
            else
            {
                Console.WriteLine("OK\n");
            }

            do
            {
                Console.WriteLine("Iniziare un altro fantacalcio?");
                rispostaNuovoFanta = Convert.ToString(Console.ReadLine());
            } while (rispostaNuovoFanta.ToUpper() != "NO" && rispostaNuovoFanta.ToUpper() != "SI" && rispostaNuovoFanta.ToUpper() != "SÌ");

            if (rispostaNuovoFanta.ToUpper() == "SI" || rispostaNuovoFanta.ToUpper() == "SÌ")
            {
                try
                {
                    string[] fileSq = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.txt");
                    foreach (string f in fileSq)
                    {
                        File.Delete(f);
                        for (int i = 0; i < nomiRose.Count; i++)
                        {
                            nomiRose.RemoveAt(i);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Errore");
                }
                Console.Clear();
                goto Repeat;
            }
            else
            {
                Console.WriteLine("Uscita");
                for (int i = 0; i < 10; i++)
                {
                    Console.Write("# ");
                    Thread.Sleep(200);
                }
                Environment.Exit(0);
            } //Da implementare alla fine del fanta
            Console.ReadKey();
        }
    }
}
