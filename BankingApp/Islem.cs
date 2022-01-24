using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApp
{
    class Islem
    {
        public int IslemNo { get; set; }
        public string HesapNo { get; set; }
        public DateTime IslemTarihi { get; set; }
        public IslemTuru IslemTuru { get; set; }
        public string IslemAciklamasi { get; set; }
        public double Miktar { get; set; }
        public double OncekiBakiye { get; set; }
        public double SonrakiBakiye { get; set; }

        public Islem HesapOlusturIslem(List<Islem> islemSayisi, List<Hesap> bankaHesaplari)
        {
            Islem islem = new Islem();
            
            // Islem Listesindeki kayıt sayısının bir fazlası 
            islem.IslemNo = islemSayisi.Count + 1;

            islem.HesapNo = bankaHesaplari[bankaHesaplari.Count - 1].HesapNo;

            islem.IslemTarihi = bankaHesaplari[bankaHesaplari.Count - 1].OlusturulmaTarihi;

            islem.IslemTuru = IslemTuru.HesapOlusturma;

            islem.IslemAciklamasi = "Hesap Oluşturuldu...";

            islem.Miktar = bankaHesaplari[bankaHesaplari.Count - 1].Bakiye;

            islem.OncekiBakiye = 0;

            islem.SonrakiBakiye = islem.Miktar + islem.OncekiBakiye;
            
            return islem;
        }
    }

    enum IslemTuru
    {
        HesapOlusturma = 1,
        ParaYatirma = 2,
        ParaCekme = 3,
        Cekilis = 4
        
    }
}
