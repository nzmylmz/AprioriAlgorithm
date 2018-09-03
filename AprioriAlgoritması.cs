using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace urunler
{
    class urunSinifi {
        public string urunIsmi1, urunIsmi2;
        public int tekrarSayisi = 0, musteriindeks=0;
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<urunSinifi> ciftler = new List<urunSinifi>();

            string[] urunler = { "Bal", "Çay", "Ekmek", "Makarna", "Peynir", "Simit", "Tereyağı", "Reçel", "Yumurta", "Zeytin" };
            Console.WriteLine("\t\t\t\t APRIORI ALGORİTMASI\n\t\t\t\t---------------------\n");
            string[][] musteri = musteriRastgeleAlisverisi(urunler);//oluşturulan ürün listesinden müşterilere
            //rastgele ürün dağıtımı yapılıyor ve müşteriler jagged dizisine yükleniyor.
            musteriRastgeleAlisverisListesiYazdir(musteri);//müşteri alışveriş listesi ekrana yazdırılıyor.
            musteriAlisverisEsleri(musteri, ciftler);//müşteri ürün çiftleri tespit ediliyor.
            Console.Write("\n Tkr  Ürün 1\tÜrün 2\n -----------------------\n");

            for (int i = 0; i < ciftler.Count; i++)//Ürün çiftlerini ve tekrar sayılarını ekrana yazdırır
            {
                Console.Write(" "+ciftler[i].tekrarSayisi + "--> " + ciftler[i].urunIsmi1);
                Console.CursorLeft = 16;
                Console.WriteLine(ciftler[i].urunIsmi2);
            }
            frekansTablosu(musteri,urunler);//ürünlerin müşteri alışverişlerin de kaç kere tekrar ettiğini bulur
            urunOnerimiz(musteri,ciftler);
        }
        public static string[][] musteriRastgeleAlisverisi(string[] urunler)
        {
            Random rnd = new Random();
            string[][] musteri = new string[5][];
            for (int i = 0; i < 5; i++)
            {
                int[] kontrolDizisi = { 10, 10, 10, 10, 10 };//rastgele ürün yerleştirirken ürün tekrarını engellmek için depolamada kullanılıyor
                int R = rnd.Next(1, 6);
                musteri[i] = new string[R];
                for (int j = 0; j < R; j++)
                {

                    int T = rnd.Next(0, 10);
                    if (kontrolDizisi.Contains(T))//ürün tekrarını engellemek için şartılı yapı oluşturuldu. 
                    { j--; continue; }
                    musteri[i][j] = urunler[T];
                    kontrolDizisi[j] = T;//Ürün tekrarını önlemek için ürünlerin indexlerini depoluyor.
                }
            }
            return musteri;
        }
        //müşterilere rastgele alışveriş sepeti oluşturuluyor
        public static void musteriRastgeleAlisverisListesiYazdir(string[][] musteri) {
            Console.WriteLine(" Müşteri Alışveriş Listesi\n --------------------------");
            for (int i = 0; i < musteri.GetLength(0); i++)
            {
                Console.Write(" {0}. Müşteri --> ", i + 1);
                for (int j = 0; j < musteri[i].GetLength(0); j++)
                {
                    char c = ',';
                    if (j == musteri[i].GetLength(0) - 1)
                        c = '.';
                    Console.Write(musteri[i][j] + c + " ");
                }
                Console.WriteLine();
            }
        }
        //oluşturulan rastgele sepetler iligili müşteriyle birlikte listelenir
        public static void musteriAlisverisEsleri(string[][] musteri, List<urunSinifi> ciftler)
        {
            for (int i = 0; i < musteri.GetLength(0); i++)
            {
                for (int t = 0; t < musteri[i].GetLength(0); t++)
                    for (int j = 1 + t; j < musteri[i].GetLength(0); j++)
                        tekrarSayisi(i,musteri[i][t], musteri[i][j], ciftler);
                        //Ürün çiftlerinin tekrarının tespiti için tekrarSayisi methodunu her ürün için çağırır.
            }
        }
        //müşteri alışveriş listesinden çift ürünler elde edilir.
        public static void tekrarSayisi(int mSira, string u1, string u2, List<urunSinifi> ciftler)
        {
            if (kayitAra(u1, u2, ciftler))
            {
                for (int i = 0; i < ciftler.Count; i++)
                {
                    if (((u1 == ciftler[i].urunIsmi1) || (u1 == ciftler[i].urunIsmi2)) && ((u2 == ciftler[i].urunIsmi1) || (u2 == ciftler[i].urunIsmi2)))
                    {
                        ciftler[i].tekrarSayisi++;
                    }
                }
            }
            else
                kayitOlustur(mSira,u1, u2, ciftler);
             }
        //listedeki çift ürünlerin tekrar sayıları tespit edilir
        public static bool kayitAra(string u1, string u2, List<urunSinifi> ciftler)
        {
            for (int i = 0; i < ciftler.Count; i++)
                if (((u1 == ciftler[i].urunIsmi1) || (u1 == ciftler[i].urunIsmi2)) && ((u2 == ciftler[i].urunIsmi1) || (u2 == ciftler[i].urunIsmi2)))
                    return true;
            return false;
        }
        //Ürün çiftleri listesinde var olan ürünler tespit edilir.
        public static void kayitOlustur(int sira, string u1,string u2, List<urunSinifi> ciftler)
        {
            urunSinifi kayit = new urunSinifi();

            kayit.urunIsmi1 = u1.ToString();
            kayit.urunIsmi2 = u2.ToString();
            kayit.tekrarSayisi++;
            kayit.musteriindeks = sira;
            ciftler.Add(kayit);
        }
        //Listede yer almayan ilk kez oluşturulacak ürün çiftleri için 
        public static void frekansTablosu(string[][] musteri, string[] urunler)
        {
            Console.Write("\n    Frekans Tablosu\n ---------------------\n");
            for (int u = 0; u < urunler.Length; u++)
            {
                int adet = 0;
                for (int x = 0; x < musteri.GetLongLength(0); x++)
                    for (int y = 0; y < musteri[x].GetLength(0); y++)
                        if (musteri[x][y] == urunler[u])
                            adet++;

                Console.Write(" {0}. {1}", u + 1, urunler[u]);
                Console.CursorLeft = 13;
                Console.WriteLine(" --> {0}", adet);
            }
        }
        //ürünlerin müşteriler tarafından kaç kez satın alındığını listeler
        public static void urunOnerimiz(string[][] musteri, List<urunSinifi> ciftler)
        {
            ArrayList OneriList = new ArrayList();
            double guvenSart = 0.0;
            int indeks=0;
            string d = "d";
            while (d == "d")
            {
                Console.WriteLine("\n İki Farklı Ürün Giriniz...\n --------------------------\n");
                Console.Write(" Birinci Ürününüzü Giriniz : ");
                string urun1 = Console.ReadLine();
                Console.Write("\n İkinci Ürününüzü Giriniz : ");
                string urun2 = Console.ReadLine();
                Console.WriteLine("\n Ürün öneri(leri)miz\n ----------------------------");
                for (int i = 0; i < ciftler.Count; i++)
                {
                    if (((urun1.ToLower() == ciftler[i].urunIsmi1.ToLower()) || (urun1.ToLower() == ciftler[i].urunIsmi2.ToLower())) && ((urun2.ToLower() == ciftler[i].urunIsmi1.ToLower()) || (urun2.ToLower() == ciftler[i].urunIsmi2.ToLower())))
                    {
                        OneriList = oneriAra(urun1, urun2, musteri);
                        indeks= i;
                        guvenSart = (double)(ciftler[i].tekrarSayisi) / 2;//güven aralığı en az %50 olması için tekrarın en az yarısı olmalıdır
                        byte counter = 5;
                        for (int o = 0; o < OneriList.Count; o++)
                        {
                            for (int e = o + 1; e < OneriList.Count; e++)
                            {
                                if (OneriList[o] == OneriList[e])
                                {
                                    counter++;
                                }
                            }
                            if (counter < guvenSart)//güven aralığı altındaki ürünler listeden kaldırılır
                            {
                                OneriList.RemoveAt(o);
                            }
                        }
                    }
                }
                for (int o = 0; o < OneriList.Count; o++)
                {//tekrar sayısı 4 olduğu durumlarda OneriListte kalan tekrar eden ürünleri silmek için yazıldı.
                    for (int e = o + 1; e < OneriList.Count; e++)
                    {
                        if (OneriList[o] == OneriList[e])
                        {
                            OneriList.RemoveAt(e);
                        }
                    }
                }
                if (OneriList.Count == 0)
                    Console.WriteLine(" Ürün eşleşmesi bulunamadı...");
                for (int s = 0; s < OneriList.Count; s++)
                {
                    if (s < 3)//3 tane öneri sunmak için
                    {
                        Console.WriteLine(" {0} - {1}",s+1,OneriList[s].ToString());                       
                    }
                }
                Console.Write("\n 1 - Aynı liste ile devam etmek için 1'e\n 2 - Yeni liste ile devam etmek için 2'ye\n 3 - Çıkmak için herhangi bir tuşa basınız\n");
                d = Console.ReadKey().KeyChar.ToString();
                switch (d)
                {
                    case "1": d = "d"; OneriList.Clear(); break;
                    case "2": 
                        Console.Clear();
                        string[] a = {" "};
                        Main(a);
                        break;
                }
            }
        }
        //yeni müşteriler için ürün teklifleri yapılır
        public static ArrayList oneriAra(string u1,string u2,string[][]musteri)
        {
            ArrayList l1 = new ArrayList();
            for (int i = 0; i < musteri.GetLength(0); i++)//ürün eşlerinin geçtiği indexleri l1 arraylistine yükler
            {
                for (int t = 0; t < musteri[i].GetLength(0); t++)
                    if (u1.ToLower() == musteri[i][t].ToLower())
                    {
                        for (int x = 0; x < musteri[i].GetLength(0); x++)
                            if (u2.ToLower() == musteri[i][x].ToLower())
                                if(!l1.Contains(i)) l1.Add(i);
                    }
            }
            ArrayList list = new ArrayList();
            foreach (int i in l1)//l1 arraylist'teki indexlerdeki verileri list arraylist'ine yükler
                    for (int y = 0; y < musteri[i].GetLongLength(0); y++)
                        if (musteri[i][y].ToLower() != u1.ToLower())
                            if (musteri[i][y].ToLower() != u2.ToLower())
                            {
                            string urun = musteri[i][y];
                            list.Add(urun.ToString());
                            }
            return list;
        }
       //ürün teklifleri için ürünler gözden geçirilir ve uygun ürünler urunOneri methoduna gönderilir.
    }
}
