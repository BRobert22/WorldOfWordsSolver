using System;
using System.Collections.Generic;
using System.Linq;
using EasyConsoleCore;
using WeCantSpell.Hunspell;

namespace WorldOfWordsSolver {
    class Program {
        static void Main(string[] args)
        {

            Letras = new List<string>();
            PalavrasResult = new List<string>();

            MenuPrincipal();

            MenuGetLetras();

            MenuGerarCombinacoes();

            var teste = 0;

        }

        private static void MenuGetLetras()
        {

            Console.WriteLine("Informe as letras (separadas por virgula)");
            var str = Console.ReadLine();
            Letras = new List<string>();
            foreach (var letra in str.Split(','))
                Letras.Add(letra.Trim());

            MenuPrincipal();
        }

        private static  IEnumerable<int> constructSetFromBits(int i)
        {
            for (int n = 0; i != 0; i /= 2, n++)
            {
                if ((i & 1) != 0)
                    yield return n;
            }
        }

        private static IEnumerable<List<string>> produceEnumeration()
        {
            for (int i = 0; i < (1 << Letras.Count); i++)
            {
                yield return
                    constructSetFromBits(i).Select(n => Letras[n]).ToList();
            }
        }

        public static void MenuGerarCombinacoes()
        {
            var result = produceEnumeration().ToList();
            var olstEnumeratuon = new List<string>();
            foreach (var olst in result)
            {
                MensagemExecutanto(0);
                var palavra = "";
                foreach (var letra in olst)
                    palavra += letra;

                if (palavra.Length > 2)
                    olstEnumeratuon.Add(palavra);
            }

            double i = 0;
            foreach (var str in olstEnumeratuon)
            {
                i++;
                var percentage = (i / olstEnumeratuon.Count) * 100;
                MensagemExecutanto((int)percentage);
                int n = str.Length;
                permute(str, 0, n - 1);
            }

            MenuPrincipal();
        }

        public static void MenuListarCombinacoes()
        {
            foreach (var palavra in PalavrasResult)
                Console.WriteLine(palavra);   

            Console.ReadLine();
            MenuPrincipal();
        }

        public static List<string> Letras { get; set; }
        public static List<string> PalavrasJaUsadas { get; set; }
        public static List<string> PalavrasResult { get; set; }

        public static void MenuHeader()
        {
            Console.Clear();

            var strLetras = String.Join(' ', Letras);

            Console.WriteLine($"Letras: {strLetras}");
            if (PalavrasResult.Count > 0)
                Console.WriteLine($"Palavras geradas: {PalavrasResult.Count} Palavras");
        }

        public static void MenuPrincipal()
        {
            MenuHeader();
            var menu = new Menu()
                .Add("Informar letras", MenuGetLetras)
                .Add("Gerar Palavras", MenuGerarCombinacoes)
                .Add("Listar Palavras", MenuListarCombinacoes)
                .Add("Filtrar por tamanho", MenuFiltroTamanho)
                .Add("Filtrar letra na posição x", MenuFiltroLetraConhecida)
                .Add("Filtrar por palavras validas", MenuFiltroPalavrasValidas)
                ;
            

            menu.Display();
        }

        private static void MenuFiltroPalavrasValidas()
        {
            var i = 0.0;
            var total = PalavrasResult.Count;
            foreach (var palavra in PalavrasResult.ToList())
            {
                i++;
                var percentage = (i / total) * 100;
                MensagemExecutanto((int)percentage);

                if (!VerificaPalavra(palavra))
                    PalavrasResult.Remove(palavra);
            }
        }

        private static void MenuFiltroLetraConhecida()
        {
            Console.Clear();
            Console.WriteLine("Informe a posição : ");
            var posicao = Console.ReadLine();
            if (int.TryParse(posicao, out int result))
            {
                Console.WriteLine("Informe a letra: ");
                var letra = Console.ReadLine();

                var valorOriginal = PalavrasResult.Count;
                PalavrasResult.RemoveAll(x => x.Substring(result - 1, 1) != letra);
                Console.WriteLine($"{valorOriginal - PalavrasResult.Count} valores removidos!");
            }
            else
            {
                Console.WriteLine("Valor Invalido!");
            }

            Console.ReadLine();

           
        }

        private static void MenuFiltroTamanho()
        {
            Console.Clear();
            Console.WriteLine("Informe o tamanho que deseja manter: ");
            var valor = Console.ReadLine();
            if (int.TryParse(valor, out int result))
            {
                var valorOriginal = PalavrasResult.Count;

                PalavrasResult.RemoveAll(x => x.Length != result);
                Console.WriteLine($"{valorOriginal - PalavrasResult.Count} valores removidos!");
            }
            else
            {
                Console.WriteLine("Valor Invalido!");
            }

            Console.ReadLine();
            MenuPrincipal();
        }

        private static void permute(String str,
            int l, int r)
        {
            if (l == r)
            {
                if (str.Length > 2)
                {
                    if (!PalavrasResult.Any(x => x.Equals(str)))
                        PalavrasResult.Add(str);
                }

                    
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    str = swap(str, l, i);
                    permute(str, l + 1, r);
                    str = swap(str, l, i);
                }
            }
        }

        public static String swap(String a,
            int i, int j)
        {
            char temp;
            char[] charArray = a.ToCharArray();
            temp = charArray[i];
            charArray[i] = charArray[j];
            charArray[j] = temp;
            string s = new string(charArray);
            return s;
        }

        public static void MensagemExecutanto(int i)
        {
            Console.Clear();
            Console.WriteLine($"Executando {i}%");
        }

        public static bool VerificaPalavra(string palavra)
        {
            var dictionary = WordList.CreateFromFiles(@"Files\\pt_BR.dic");
            //bool notOk = dictionary.Check("teh");
            //var suggestions = dictionary.Suggest("teh");
            bool ok = dictionary.Check(palavra);
            return ok;
            //TXTextControl.Proofing.TXSpell spell = new TXTextControl.Proofing.TXSpell();

            //using (Hunspell hunspell = new Hunspell("Files\\pt_br.aff", "Files\\pt_br.dic"))
            //{
            //    return hunspell.Spell(palavra);
            //    Console.WriteLine("Hunspell - Spell Checking Functions");
            //    Console.WriteLine("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯");

            //    Console.WriteLine("Check if the word 'Recommendation' is spelled correct");
            //    bool correct = hunspell.Spell("Recommendation");
            //    Console.WriteLine("Recommendation is spelled " +
            //                      (correct ? "correct" : "not correct"));

            //    Console.WriteLine("");
            //    Console.WriteLine("Make suggestions for the word 'Recommendatio'");
            //    List<string> suggestions = hunspell.Suggest("Recommendatio");
            //    Console.WriteLine("There are " +
            //                      suggestions.Count.ToString() + " suggestions");
            //    foreach (string suggestion in suggestions)
            //    {
            //        Console.WriteLine("Suggestion is: " + suggestion);
            //    }
            //}
        }

    }

}
