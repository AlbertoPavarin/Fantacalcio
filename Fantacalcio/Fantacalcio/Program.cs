using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Fantacalcio
{
    class Calciatore
    {
        // attributi della classe Calciatore
        string nome;
        string squadra;
        int prezzo;

        // Costruttore della classe Calciatore
        public Calciatore(string nome, string nomeSquadra, int prezzo)
        {
            this.nome = nome;
            this.squadra = nomeSquadra;
            this.prezzo = prezzo;
        }

        // Metodo per stampare il calciatore con i suoi attributi
        public string toString()
        {
            return $"Il calciatore {this.nome} della squadra {this.squadra} è stato acquistato a {this.prezzo}";
        }
    }

    class Squadra
    {
        // Attributi della classe Squadra
        string nome;
        string percorso = AppDomain.CurrentDomain.BaseDirectory + "Squadre";
        string percorsoSq;
        public int punti { get; set; }
        public double giornata { get; set; }
        public int golG { get; set; }

        // Costruttore della classe Squadra
        public Squadra(string squadra)
        {
            this.nome = squadra;
            // Creato il percorso per il file della squadra
            var dir = Directory.CreateDirectory(percorso);
            percorsoSq = Path.Combine(dir.ToString(), $"{nome}.txt");
            punti = 0;
            giornata = 66;
            golG = 0;
        }

        public void calcoloGiornata(int gol, int autogol, int assist, int gialli, int rossi)
        {
            giornata += (gol * 3) - (autogol * 3) + assist - (gialli * 0.5) - rossi;
            double punteggioG = giornata;
            do
            {
                punteggioG -= 6;
                golG++;
            } while (punteggioG >= 66);
        }

        // Metodo per stampare i giocatori della squadra
        public string getPuntiGiornata()
        {
            StreamReader squadra = new StreamReader(percorsoSq);
            return $"I punti della giornata della squadra {nome} sono: {giornata}\nGol:{golG}";
        }

        public string toString()
        {
            return $"{nome}";
        }
    }

    class Partita
    {
        // attributi della classe Partita
        Squadra sq1, sq2;
        string percorsoSquadre;
        string dir;
        

        //Costruttore della classe Partita
        public Partita(Squadra squadra1, Squadra squadra2)
        {
            this.sq1 = squadra1;
            this.sq2 = squadra2;
            dir = AppDomain.CurrentDomain.BaseDirectory + "partite";
            var partite = Directory.CreateDirectory(dir); // Crea una nuova directory
            percorsoSquadre = Path.Combine(partite.ToString(), $"{sq1.toString()}_vs_{sq2.toString()}.txt"); // percorso in cui vengono salvati i file delle partite, cioè nella cartella partite
        }

        // Metodo per determinare il vincitore della partita
        public string vincitore()
        {
            if (sq1.golG > sq2.golG) // se i gol della squadra 1 son maggiori di quella della squadra 2 allora il vincitore è la squadra 1
            {
                sq1.punti += 3;
                return $"Il vincitore della partita {sq1.toString()} vs {sq2.toString()} è {sq1.toString()}" +
                       $"{sq1.toString()}: {sq1.golG} gol\n" +
                       $"{sq2.toString()}: {sq2.golG} gol\n\n"; 
            }
            else if (sq1.golG < sq2.golG) // se i gol della squadra 1 son minori di quella della squadra 2 allora il vincitore è la squadra 2
            {
                sq2.punti += 3;
                return $"Il vincitore della partita {sq1.toString()} vs {sq2.toString()} è {sq2.toString()}\n" + 
                       $"{sq1.toString()}: {sq1.golG} gol\n" +
                       $"{sq2.toString()}: {sq2.golG} gol\n\n";
            }
            else
            {
                sq1.punti++;
                sq2.punti++;
                return $"La partita tra {sq1.toString()} e {sq2.toString()} è finita in parità\n" +
                       $"{sq1.toString()}: {sq1.golG} gol\n" +
                       $"{sq2.toString()}: {sq2.golG} gol\n\n"; 
            }
        }

        // Metodo per stampare le due squadre che si affronteranno
        public string toString()
        {
            StreamWriter partite = new StreamWriter(percorsoSquadre); // Viene create una istanza della classe StreamWriter
            partite.WriteLine($"{sq1.toString()}\n{sq2.toString()}"); // Viene scritto nel file della partita le squadre che si scontrano
            partite.Close(); // Viene chiuso il file
            return $"{sq1.toString()} vs {sq2.toString()}";
        }
    }
    class Fanta
    {
        // Attributi per la classe Fanta
        List<Squadra> nomi = new List<Squadra>();
        List<int> partite = new List<int>();
        public List<Partita> nomiSqPar = new List<Partita>();
        int rnd1, rnd2;

        // Costruttore per la classe Fanta
        public Fanta(List<Squadra> nomiSq)
        {
            this.nomi = nomiSq;
        }

        // Metodo per stampare i nomi delle varie squadre
        public string toString()
        {
            string s = "";
            foreach (Squadra i in nomi)
            {
                s += $"{i}\n";
            }
            return s;
        }

        // Metodo per la creazione della partita
        public string CreazionePartita()
        {
            Random randomSq = new Random(); // Viene creata un istanza della classe Random
            do
            {
                rnd1 = randomSq.Next(0, nomi.Count); // Viene generato il primo numero random
                do
                {
                    rnd2 = randomSq.Next(0, nomi.Count); // Viene generato il secondo numero random
                } while (rnd2 == rnd1); // Viene generato finchè rnd2 è uguale a rnd1, dato che i due numeri randomici non possono essere uguali dato che altrimenti una squadra si andrebbe a scontrare con se stessa
            } while (partite.Contains(rnd1) | partite.Contains(rnd2)); // Vengono generati inoltre entrambi i numeri finchè essi sono contenuti nella lista partite, dato che una squadra potrebbe scontrarsi con due squadre allo stesso tempo
            partite.Add(rnd1); // Viene aggiunto il primo numero randomico alla lista partite
            partite.Add(rnd2); // Viene aggiunto il secondo numero randomico alla lista partite
            Partita partita = new Partita(nomi[rnd1], nomi[rnd2]); // Viene creata una istanza della classe Partita
            nomiSqPar.Add(partita);
            return $"{partita.toString()}"; // ritorna la parita grazie al metodo toString della classe Partita
        }
        public Calciatore calciatore;
        static void Main(string[] args)
        {
            // Creazione variabili
            bool valido;
            int gol, autogol, gialli, rossi, assist;
            int nSquadre;
            string nomeSq;
            string nomeCa;
            string percorso, percorsoSq;
            string percorsoFileSq = AppDomain.CurrentDomain.BaseDirectory + "Squadre.txt"; // Viene creato il percorso in cui si trova il file contenente i nomi di tutte le squadre
            string imp = AppDomain.CurrentDomain.BaseDirectory + "status.txt"; // Viene creato il percorso che definisce se c'è un fantacalcio in corso
            int prezzo;
            int partiteFatte = 0;
            List<string> nomiRose = new List<string>();
            List<int> partite = new List<int>();
            List<Squadra> squadre = new List<Squadra>();

            string rispostaNuovoFanta;
            string rispostaNuovaPartita;

        Repeat: // Etichetta per il goto
            if (!File.Exists(imp)) // Se il file status non esiste significa che il programma è stato lanciato per la prima volta o che è stato riavviato il Fantacalcio
            {
                do
                {
                    Console.WriteLine("Inserisci quante squadre ci sono nel campionato (numero pari tra 2 e 8)");
                    string sq = Convert.ToString(Console.ReadLine()); // Viene letto il valore inserito da console
                    valido = int.TryParse(sq, out nSquadre); // True = int, False = non int
                    Console.WriteLine("");
                    Console.Clear(); // Viene pulita la schermata
                } while (!valido || nSquadre > 9 || nSquadre < 1 || nSquadre % 2 != 0); // Viene chiesto quante sono le squadre da inserire finchè il numero inserito è un int, pari, maggiore di 1 e minore di 9

                for (int i = 0; i < nSquadre; i++) // Viene iterato per tutte le squadre che devono essere inserite
                {
                    do
                    {
                        Console.WriteLine($"Inserisci il nome della {i + 1}° squadra");
                        nomeSq = Convert.ToString(Console.ReadLine()); // Viene letto il valore in input
                        Console.WriteLine("");
                    } while (nomeSq == "" || nomiRose.Contains(nomeSq)); // Viene chiesto il nome delle squadre finchè il nome non è vuoto o diverso dal nome di un'altra squadra           
                    StreamWriter fileNomi = new StreamWriter(percorsoFileSq, true); // Viene creata una nuova istanza della classe StreamWriter
                    fileNomi.WriteLine($"{nomeSq}"); // Vengono scritti i nomi delle squadre su un file chiamato Squadre
                    fileNomi.Close(); // Viene chiuso il file dove vengono salvati i nomi delle squadre
                    nomiRose.Add(nomeSq); // Vengono aggiunti i nomi delle squadre alla lista nomiRose
                    percorso = AppDomain.CurrentDomain.BaseDirectory + "Squadre"; // Viene creato un file per ogni squadra
                    var dir = Directory.CreateDirectory(percorso);
                    percorsoSq = Path.Combine(dir.ToString(), $"{nomiRose[i]}.txt");
                    StreamWriter fileSq = new StreamWriter(percorsoSq, true); // Viene creata una nuova istanza della classe StreamWriter che servirà per creare un file per ogni squadra
                    for (int j = 0; j < 1; j++) // Viene iterato per tutti i giocatori della squadra
                    {
                        do
                        {
                            Console.WriteLine($"Inserisci il nome del {j + 1}° giocatore della {nomiRose[i]}");
                            nomeCa = Convert.ToString(Console.ReadLine()); // Vengono letti i nomi dei giocatori
                            Console.WriteLine("");
                        } while (nomeCa == ""); // Viene chiesto di inserire il nome del calciatore finchè esso non è vuoto
                        do
                        {
                            Console.WriteLine($"Inserisci il prezzo d'acquisto di {nomeCa} della squadra {nomiRose[i]}");
                            string p = Convert.ToString(Console.ReadLine()); // Viene letto il prezzo d'acquisto dei giocatori
                            valido = int.TryParse(p, out prezzo);
                            Console.WriteLine("");
                        } while (!valido || prezzo < 1); // Viene chiesto il prezzo finchè esso non è non è un numero double maggiore di 0
                        Console.Clear(); // Viene pulita la schermata
                        Calciatore calciatore = new Calciatore(nomeCa, nomiRose[i], prezzo); // Viene creata una nuova istanza della classe Calciatore
                        fileSq.WriteLine($"Nome: {nomeCa}, Prezzo d'acquisto: {prezzo}"); // Viene scritto nel file della singola squadra il nome del giocatore con il relativo prezzo
                    }
                    fileSq.Close(); // Viene chiuso il file della squadra
                }

            }
            if (File.Exists(imp)) // Se il file che indica se un fantacalcio è già avviato esiste allora
            {
                foreach (string sqs in File.ReadLines(percorsoFileSq))
                {
                    nomiRose.Add(sqs); // Vengono inserite le squadre lette dal file che contiene tutti i nomi delle squadre dentro la lista nomiRose
                }
            }
            //Se quel file non esiste
            StreamWriter impostazione = new StreamWriter(imp); // Viene creato indicando quindi che è in corso un Fanta
            impostazione.Close(); // Viene chiuso il file di impostazione 
            for (int i = 0; i < nomiRose.Count; i++)
            {
                Squadra squadra = new Squadra(nomiRose[i]); // Viene creata una nuova istanza della classe Squadra
                squadre.Add(squadra);
                percorso = AppDomain.CurrentDomain.BaseDirectory + "Squadre";
                var dir = Directory.CreateDirectory(percorso);
                percorsoSq = Path.Combine(dir.ToString(), $"{nomiRose[i]}.txt");
                StreamWriter fileSq = new StreamWriter(percorsoSq, true);
                fileSq.WriteLine($"\nPunti: {squadre[i].punti}");
                fileSq.Close();
            }
            Fanta fanta = new Fanta(squadre); // Viene creato un Fanta con tutte le squadre
        ContinuoFanta:
            Console.WriteLine("----------------------\n" +
                              "|Squadra   |   Punti:|\n" +
                              "----------------------\n");
            for (int i = 0; i < nomiRose.Count; i++) // Viene iterato per tutte le squadre
            {                
                Console.WriteLine($"{squadre[i].toString()}   |   {squadre[i].punti}\n"); // Vengon stampati il nome della squadra e i relativi punti          
            }

            do
            {
                Console.WriteLine("Iniziare una nuova partita?");
                rispostaNuovaPartita = Convert.ToString(Console.ReadLine()); // Viene letta la risposta che è stata data
                Console.WriteLine("");
            } while (rispostaNuovaPartita.ToUpper() != "NO" && rispostaNuovaPartita.ToUpper() != "N" && rispostaNuovaPartita.ToUpper() != "S" && rispostaNuovaPartita.ToUpper() != "SI" && rispostaNuovaPartita.ToUpper() != "SÌ"); // Viene chiesto se l'utente desidera generare una nuova giornata finchè la risposta data non soddisfa le richieste del while

            if (rispostaNuovaPartita.ToUpper() == "SI" || rispostaNuovaPartita.ToUpper() == "SÌ" || rispostaNuovaPartita.ToUpper() == "S") // Se la ripsosta è si
            {
                Console.Clear();
                Console.WriteLine("Le partite sono:");
                for (int i = 0; i < nomiRose.Count / 2; i++) // Viene iterato per le partite da generare
                {
                    Console.WriteLine($"{fanta.CreazionePartita()}"); // Viene stampata a schermo le varie partite create attraverso il metodo CreazionePartita della classe Fanta                 
                }
                Console.WriteLine();

                for (int j = 0; j < nomiRose.Count; j++) // Viene iterato per tutte le squadre
                {                  
                    do
                    {
                        Console.WriteLine($"Inserisci i gol della squadra {nomiRose[j]}");
                        string b = Convert.ToString(Console.ReadLine());
                        valido = int.TryParse(b, out gol);
                        Console.WriteLine("");
                    } while (!valido | gol < 0);
                    
                    do
                    {
                        Console.WriteLine($"Inserisci gli autogol della squadra {nomiRose[j]}");
                        string b = Convert.ToString(Console.ReadLine());
                        valido = int.TryParse(b, out autogol);
                        Console.WriteLine("");
                    } while (!valido | autogol < 0);

                    do
                    {
                        Console.WriteLine($"Inserisci gli assist della squadra {nomiRose[j]}");
                        string b = Convert.ToString(Console.ReadLine());
                        valido = int.TryParse(b, out assist);
                        Console.WriteLine("");
                    } while (!valido | assist < 0);

                    do
                    {
                        Console.WriteLine($"Inserisci i gialli della squadra {nomiRose[j]}");
                        string b = Convert.ToString(Console.ReadLine());
                        valido = int.TryParse(b, out gialli);
                        Console.WriteLine("");
                    } while (!valido | gialli < 0);

                    do
                    {
                        Console.WriteLine($"Inserisci i rossi della squadra {nomiRose[j]}");
                        string b = Convert.ToString(Console.ReadLine());
                        valido = int.TryParse(b, out rossi);
                        Console.WriteLine("");
                    } while (!valido | rossi < 0);
                    squadre[j].calcoloGiornata(gol, autogol, assist, gialli, rossi);
                    Console.WriteLine($"{squadre[j].getPuntiGiornata()}");
                    Console.WriteLine("\n\nPremi un tasto per continuare");
                    Console.ReadKey();
                    Console.Clear();
                }

                for (int i = 0; i < nomiRose.Count / 2; i++)
                {
                    Console.WriteLine($"{fanta.nomiSqPar[i].vincitore()}");
                }
                partiteFatte++;
                impostazione = new StreamWriter(imp);
                impostazione.WriteLine($"Partite fatte: {partiteFatte}");
                impostazione.Close();
                Console.WriteLine("\n\nPremi un tasto per continuare");
                Console.ReadKey();
                Console.Clear();
                string[] filePartite = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "partite", "*.txt");
                foreach (string file in filePartite)
                {
                    File.Delete(file);
                }
                goto ContinuoFanta;
            }
            else // Se la risposta è no allora il programma terminerà
            {
                Console.WriteLine("Uscita");
                for (int i = 0; i < 10; i++)
                {
                    Console.Write("# ");
                    Thread.Sleep(200); // Viene stampata una serie di #
                }
                Environment.Exit(0); // Serve per uscire dal programma
            }

            if (partiteFatte == 5)
            {
                do
                {
                    Console.WriteLine("Iniziare un altro fantacalcio?");
                    rispostaNuovoFanta = Convert.ToString(Console.ReadLine()); // Viene letta la risposta data in input
                } while (rispostaNuovoFanta.ToUpper() != "NO" && rispostaNuovoFanta.ToUpper() != "N" && rispostaNuovoFanta.ToUpper() != "S" && rispostaNuovoFanta.ToUpper() != "SI" && rispostaNuovoFanta.ToUpper() != "SÌ"); // Viene chiesto se l'utente desidera generare un nuovo fantacalcio finchè la risposta data non soddisfa le richieste del while

                if (rispostaNuovoFanta.ToUpper() == "SI" || rispostaNuovoFanta.ToUpper() == "SÌ" || rispostaNuovoFanta.ToUpper() == "S") // Se la risposta è èsi
                {
                    try
                    {
                        string[] fileSq = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.txt"); // Vengono salvati in un array tutti i file contenuti nella directory
                        foreach (string f in fileSq) // Per ogni file nell'array fileSq, che contiene tutti i file
                        {
                            File.Delete(f); // Vengono eliminati tutti i file
                            for (int i = 0; i < nomiRose.Count; i++)
                            {
                                nomiRose.RemoveAt(i); // Vengono eliminate tutte le squadre dall'array nomiRose
                            }
                        }
                    }
                    catch // Se non riesce dà un errore
                    {
                        Console.WriteLine("Errore");
                    }
                    Console.Clear(); // Viene pulita la console
                    goto Repeat; // Viene ricominciato un altro fantacalcio andando alla etticherra Repeat
                }
                else // Se la risposta è no allora il programma termina
                {
                    Console.WriteLine("Uscita");
                    for (int i = 0; i < 10; i++)
                    {
                        Console.Write("# ");
                        Thread.Sleep(200);
                    }
                    Environment.Exit(0);
                } //Da implementare alla fine del fanta
            }
            Console.ReadKey();
        }
    }
}
