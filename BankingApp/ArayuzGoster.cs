using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApp
{
    class ArayuzGoster
    {
        public void InitializeUI()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-> Hesap Açma İşlemleri ( 1 ) \n");
            string[] hesapTurleri = new string[] {
                "-> Kısa Vadeli Hesap Açma",
                "-> Uzun Vadeli Hesap Açma",
                "-> Özel Hesap Açma",
                "-> Cari Hesap Açma"
            };
            foreach (var hesapTuru in hesapTurleri)
            {
                Console.WriteLine($"\t{hesapTuru}\n");
            }

            string[] digerHesapIslemleri = new string[] {
                "-> Para Yatırma İşlemi ( 2 )",
                "-> Para Çekme İşlemi ( 3 )",
                "-> Hesap Listesi ( 4 )",
                "-> Hesap Durumu ( 5 )",
                "-> Hesap İşlem Kayıtları ( 6 )",
                "-> Çekiliş ( 7 )",
                $"-> Çıkış için \"q\" giriniz"
            };

            foreach (var digerHesapIslemi in digerHesapIslemleri)
            {
                Console.WriteLine($"{digerHesapIslemi}\n");
            }

        }
    }
}
