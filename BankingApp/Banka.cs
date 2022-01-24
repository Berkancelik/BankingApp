using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApp
{
    class Banka
    {
        public Banka()
        {
            List<Hesap> bankaHesaplari = new List<Hesap>();
            List<Islem> IslemGecmisi = new List<Islem>();

            ArayuzGoster arayuzGoster = new ArayuzGoster();
            arayuzGoster.InitializeUI();
            Console.Write("Yapmak İstediğiniz İşlemi Yazınız (1 - 7) : ");
            string islemDegeri = Console.ReadLine();

            Hesap hesap = new Hesap();
            Islem islem = new Islem();
            string hesapNo;


            do
            {
                switch (islemDegeri)
                {
                    case "1":
                        // Hesap Açma İşlemi
                        bankaHesaplari.Add(hesap.HesapAc());
                        IslemGecmisi.Add(islem.HesapOlusturIslem(IslemGecmisi, bankaHesaplari));
                        IslemDekontuGoster(IslemGecmisi);
                        HesapGoster(bankaHesaplari);
                        Console.WriteLine("\n");
                        arayuzGoster.InitializeUI();
                        Console.Write("Yapmak İstediğiniz İşlemi Yazınız (1 - 7) : ");
                        break;
                    case "2":
                        // Para Yatırma
                        Console.Write("Hesap No Giriniz : ");
                        hesapNo = Console.ReadLine();
                        ParaYatir(hesapNo, bankaHesaplari, IslemGecmisi);
                        arayuzGoster.InitializeUI();
                        Console.Write("Yapmak İstediğiniz İşlemi Yazınız (1 - 7) : ");
                        break;
                    case "3":
                        // Para Çekme
                        Console.Write("Hesap No Giriniz : ");
                        hesapNo = Console.ReadLine();
                        ParaCek(hesapNo, bankaHesaplari, IslemGecmisi);
                        arayuzGoster.InitializeUI();
                        Console.Write("Yapmak İstediğiniz İşlemi Yazınız (1 - 7) : ");
                        break;
                    case "4":
                        // Hesap Listesi
                        HesapListesi(bankaHesaplari);
                        arayuzGoster.InitializeUI();
                        Console.Write("Yapmak İstediğiniz İşlemi Yazınız (1 - 7) : ");
                        break;
                    case "5":
                        // Hesap Durumu
                        Console.Write("Hesap No Giriniz : ");
                        hesapNo = Console.ReadLine();
                        // Hesabın zamana bağlı kar tutarını ve güncel bakiyesini hesaplar
                        hesap.KarTutari(HesapNoKontrol(hesapNo, bankaHesaplari), DateTime.Today, hesapNo, bankaHesaplari, IslemGecmisi);

                        // Hesabın son durumunu görüntüler
                        hesap.HesapDurum(HesapNoKontrol(hesapNo, bankaHesaplari), hesapNo, bankaHesaplari);
                        Console.WriteLine("\n");
                        arayuzGoster.InitializeUI();
                        Console.Write("Yapmak İstediğiniz İşlemi Yazınız (1 - 10) : ");
                        break;
                    case "6":
                        // Hesap İşlem Geçmişini Listele
                        Console.Write("Hesap No Giriniz : ");
                        HesapIslemKayıtlarıListele(Console.ReadLine(), bankaHesaplari, IslemGecmisi);
                        arayuzGoster.InitializeUI();
                        Console.Write("Yapmak İstediğiniz İşlemi Yazınız (1 - 10) : ");
                        break;
                    case "7":
                        // Çekiliş İşlemi
                        double cekilisDegeri = CekilisDegeriOnay();
                        Cekilis(cekilisDegeri, bankaHesaplari, IslemGecmisi);
                        arayuzGoster.InitializeUI();
                        Console.Write("Yapmak İstediğiniz İşlemi Yazınız (1 - 10) : ");
                        break;
                    case "q":
                        break;
                    default:
                        Console.WriteLine("\n\n");
                        Console.WriteLine("**********************");
                        Console.WriteLine("Geçersiz İşlem Değeri!");
                        Console.WriteLine("**********************");
                        Console.WriteLine("\n\n");
                        arayuzGoster.InitializeUI();
                        Console.Write("Yapmak İstediğiniz İşlemi Yazınız (1 - 10) : ");
                        break;
                }
                islemDegeri = Console.ReadLine();

            } while (islemDegeri != "q");
        }


        public void ParaYatir(string hesapNo, List<Hesap> bankaHesaplari, List<Islem> islemListesi)
        {
            // hesap doğru mu?
            if (HesapNoKontrol(hesapNo, bankaHesaplari))
            {
                // İlgili banka hesabını bul
                int result = bankaHesaplari.FindIndex(bankaHesabi => bankaHesabi.HesapNo == hesapNo);

                // hesap sınıfının örneğini alıyoruz.
                Hesap hesap = new Hesap();

                // Yeni işlem oluştur.
                Islem islem = new Islem();

                islem.IslemNo = islemListesi.Count + 1;

                islem.HesapNo = hesapNo;

                // Geçerli bir tarih girildi mi?
                string geciciTarihString = "";
                DateTime geciciTarihDeger;
                do
                {
                    Console.Write("Tarih Giriniz (YIL,AY,GÜN) ya da (GÜN/AY/YIL) : ");
                    geciciTarihString = Console.ReadLine();

                } while (!DateTime.TryParse(geciciTarihString, out geciciTarihDeger) || (geciciTarihDeger < islemListesi.FindLast(item => item.HesapNo == hesapNo).IslemTarihi));
                islem.IslemTarihi = geciciTarihDeger;
                Console.WriteLine("\n");

                // Anaparaya eklenecek faiz gelirinin hesaplanması
                hesap.KarTutari(true, islem.IslemTarihi, hesapNo, bankaHesaplari, islemListesi);

                // İşlem türü tanımlanıyor
                islem.IslemTuru = IslemTuru.ParaYatirma;

                Console.WriteLine("\n");
                Console.WriteLine("****************************************");
                Console.WriteLine($"Banka Hesap Bakiyeniz : {bankaHesaplari[result].Bakiye}");
                Console.WriteLine("****************************************");
                Console.WriteLine("\n");

                // Yüklencek miktar belirleniyor
                double yuklenecekBakiye = 0;
                do
                {
                    Console.Write("Yüklenecek Miktarı Giriniz : ");
                    yuklenecekBakiye = Convert.ToDouble(Console.ReadLine());

                } while (yuklenecekBakiye <= 0);

                // İşlem Miktar (+)
                islem.Miktar = yuklenecekBakiye;

                // İşlem Açıklaması
                islem.IslemAciklamasi = $"Banka Hesabına {islem.Miktar} TL Yatırıldı...";

                Console.WriteLine("\n");

                // Banka hesabına aktarılacak paradan önceki para miktarı
                islem.OncekiBakiye = bankaHesaplari[result].Bakiye;

                // İlgili hesabın son durumu
                islem.SonrakiBakiye = Math.Round((islem.OncekiBakiye + islem.Miktar), 2, MidpointRounding.ToZero);

                // Banka hesabınının bakiyesini güncelle.
                bankaHesaplari[result].Bakiye = (double)islem.SonrakiBakiye;

                // Çekiliş miktarı
                bankaHesaplari[result].CekilisHakki += bankaHesaplari[result].HesapTuruDeger == HesapTuru.Cari ? 0 : (int)islem.Miktar / 1000;

                // Oluşturulan işlemi işlem listesine ekliyoruz.
                islemListesi.Add(islem);

                Console.WriteLine("\n");
                Console.WriteLine("*********************");
                Console.WriteLine("Hesaba Para Aktarıldı");
                Console.WriteLine("*********************");
                Console.WriteLine("\n");

                // İşlem Dekontunu Göster
                IslemDekontuGoster(islemListesi);
            }
            else
            {
                // Hesap doğru değilse hesap bulunamadı
                Console.WriteLine("\n");
                Console.WriteLine("*********************************************");
                Console.WriteLine("Para Yatırılacak Hesap Sistemde Bulunamadı...");
                Console.WriteLine("*********************************************");
                Console.WriteLine("\n");
            }
        }

        public void ParaCek(string hesapNo, List<Hesap> bankaHesaplari, List<Islem> islemListesi)
        {
            // hesap doğru mu?
            if (HesapNoKontrol(hesapNo, bankaHesaplari))
            {
                // İlgili banka hesabını bul
                int result = bankaHesaplari.FindIndex(bankaHesabi => bankaHesabi.HesapNo == hesapNo);

                // hesap sınıfının örneğini alıyoruz.
                Hesap hesap = new Hesap();

                // yeni bir işlem oluşturuyoruz
                Islem islem = new Islem();

                // İşlem Tarihi
                string geciciTarihString = "";
                DateTime geciciTarihDeger;
                do
                {
                    Console.Write("Tarih Giriniz (YIL,AY,GÜN) : ");
                    geciciTarihString = Console.ReadLine();

                } while (!DateTime.TryParse(geciciTarihString, out geciciTarihDeger) || (geciciTarihDeger < islemListesi.FindLast(item => item.HesapNo == hesapNo).IslemTarihi));
                islem.IslemTarihi = geciciTarihDeger;
                Console.WriteLine("\n");

                // Anaparaya eklenecek faiz gelirinin hesaplanması
                hesap.KarTutari(true, islem.IslemTarihi, hesapNo, bankaHesaplari, islemListesi);

                Console.WriteLine("\n");
                Console.WriteLine("****************************************");
                Console.WriteLine($"Banka Hesap Bakiyeniz : {bankaHesaplari[result].Bakiye}");
                Console.WriteLine("****************************************");
                Console.WriteLine("\n");

                double geciciCekilecekMiktar = 0;
                do
                {
                    Console.Write("Banka Hesabından Çekmek İstediğiniz Miktar : ");
                    geciciCekilecekMiktar = Convert.ToDouble(Console.ReadLine());

                } while (geciciCekilecekMiktar < 0);

                // Çekilecek miktar hesapta var mı?
                if (bankaHesaplari[result].Bakiye >= geciciCekilecekMiktar)
                {
                    // işlem No
                    islem.IslemNo = islemListesi.Count + 1;

                    // Hesap No
                    islem.HesapNo = hesapNo;

                    // İşlem Türünü belirtiyoruz
                    islem.IslemTuru = IslemTuru.ParaCekme;

                    // İşlem Miktarı (-)
                    islem.Miktar = geciciCekilecekMiktar;

                    // İşlem Açıklaması
                    islem.IslemAciklamasi = $"Banka Hesabından {islem.Miktar} TL Çekildi...";

                    // Para Çekiminden Önceki Bakiye
                    islem.OncekiBakiye = bankaHesaplari[result].Bakiye;

                    // Para Çekiminden Sonraki Bakiye
                    islem.SonrakiBakiye = Math.Round((bankaHesaplari[result].Bakiye - islem.Miktar), 2 ,MidpointRounding.ToZero);

                    // Banka hesabınının bakiyesini güncelle.
                    bankaHesaplari[result].Bakiye = islem.SonrakiBakiye;

                    // Çekiliş Miktarı
                    bankaHesaplari[result].CekilisHakki += bankaHesaplari[result].HesapTuruDeger == HesapTuru.Cari ? 0 : (int)islem.Miktar / 1000;

                    // Oluşturulan işlemi işlem listesine ekliyoruz.
                    islemListesi.Add(islem);

                    Console.WriteLine("\n");
                    Console.WriteLine("************************");
                    Console.WriteLine("Hesabtan Para Çekildi...");
                    Console.WriteLine("************************");
                    Console.WriteLine("\n");

                    // İşlem Dekontunu Göster
                    IslemDekontuGoster(islemListesi);

                }
                else
                {
                    Console.WriteLine("\n");
                    Console.WriteLine("*********************************************");
                    Console.WriteLine("\tYetersiz Bakiye...");
                    Console.WriteLine("*********************************************");
                    Console.WriteLine("\n");
                }

            }
            else
            {
                // Hesap doğru değilse hesap bulunamadı
                Console.WriteLine("\n");
                Console.WriteLine("*********************************************");
                Console.WriteLine("Para Çekilecek Hesap Sistemde Bulunamadı...");
                Console.WriteLine("*********************************************");
                Console.WriteLine("\n");
            }

        }

        public void HesapListesi(List<Hesap> bankaHesaplari)
        {
            if (bankaHesaplari.Count > 0)
            {
                foreach (Hesap bankaHesabi in bankaHesaplari)
                {
                    Console.WriteLine("\n");
                    Console.WriteLine("********************************************");
                    Console.WriteLine($"Hesap Numarası : {bankaHesabi.HesapNo}");
                    Console.WriteLine($"İsim : {bankaHesabi.Isim}");
                    Console.WriteLine($"Soyisim : {bankaHesabi.Soyisim}");
                    Console.WriteLine($"Banka Hesap Türü : {bankaHesabi.HesapTuruDeger}");
                    Console.WriteLine($"Hesap Oluşturulma Tarihi : {bankaHesabi.OlusturulmaTarihi}");
                    Console.WriteLine($"Bakiye : {(double)bankaHesabi.Bakiye} TL");
                    Console.WriteLine($"Çekiliş Hakkı : {bankaHesabi.CekilisHakki}");
                    Console.WriteLine("********************************************");
                }
            }
            else
            {
                Console.WriteLine("\n");
                Console.WriteLine("******************************");
                Console.WriteLine("Sistemde Hesap Bulunmamaktadır");
                Console.WriteLine("******************************");
            }

        }

        public void HesapIslemKayıtlarıListele(string hesapNo, List<Hesap> bankaHesaplari, List<Islem> islemListesi)
        {
            // hesap var mı?
            if (HesapNoKontrol(hesapNo, bankaHesaplari))
            {
                int result = bankaHesaplari.FindIndex(bankaHesabi => bankaHesabi.HesapNo == hesapNo);
                foreach (var islem in islemListesi)
                {
                    if (islem.HesapNo == bankaHesaplari[result].HesapNo)
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine("********************* İŞLEM DEKONTU *********************");
                        Console.WriteLine($"İşlem No : {islem.IslemNo}");
                        Console.WriteLine($"Hesap No : {islem.HesapNo}");
                        Console.WriteLine($"İşlem Tarihi : {islem.IslemTarihi}");
                        Console.WriteLine($"İşlem Türü : {islem.IslemTuru}");
                        Console.WriteLine($"Miktar (TL) : {(double)islem.Miktar} TL");
                        Console.WriteLine($"Önceki Bakiye (TL) : {(double)islem.OncekiBakiye} TL");
                        Console.WriteLine($"Sonraki Bakiye (TL) : {(double)islem.SonrakiBakiye} TL");
                        Console.WriteLine("********************* İŞLEM DEKONTU *********************");
                    }
                }
            }
            else
            {
                // Hesap doğru değilse hesap bulunamadı
                Console.WriteLine("\n");
                Console.WriteLine("**********************************************");
                Console.WriteLine("İşlem Geçmişi Listelenecek Hesap Bulunamadı...");
                Console.WriteLine("**********************************************");
                Console.WriteLine("\n");
            }
        }

        public void Cekilis(double cekilisDegeri, List<Hesap> bankaHesaplari, List<Islem> islemListesi)
        {
            // Eğer sistemde kayıtlı hesap yoksa çekiliş yapma
            if (bankaHesaplari.Count > 0)
            {
                // Sistemde kayıtlı kullanıcı var ancak çekiliş hakkı olmayan hesaplarsa çekiliş olmaz
                List<Hesap> cekilisHesaplari = new List<Hesap>();
                foreach (var bankaHesabi in bankaHesaplari)
                {
                    if (bankaHesabi.CekilisHakki > 0)
                    {
                        cekilisHesaplari.Add(bankaHesabi);
                        // Çekilişe dahil olan hesapların çekiliş haklarını 1 azaltıyoruz.
                        bankaHesabi.CekilisHakki -= 1;
                    }
                }

                // Sistemde çekiliş hakkı bulunan hesaplar varsa
                if (cekilisHesaplari.Count > 0)
                {
                    Hesap hesap = new Hesap();

                    Random random = new Random();
                    int cekilisHesabiIndex = random.Next(1, cekilisHesaplari.Count + 1) - 1; // Çekilişi kazanan hesabın indexi

                    // Yeni bir işlem oluşturuyoruz.
                    Islem islem = new Islem();

                    // İşlem no atıyoruz
                    islem.IslemNo = islemListesi.Count + 1;

                    // Çekilişi Kazanan Hesabın Numarasını atıyoruz
                    islem.HesapNo = cekilisHesaplari[cekilisHesabiIndex].HesapNo;

                    // Anaparaya eklenecek faiz gelirinin hesaplanması
                    hesap.KarTutari(true, DateTime.Today, islem.HesapNo, bankaHesaplari, islemListesi);

                    // İşlem tarihini atıyoruz
                    islem.IslemTarihi = DateTime.Today;

                    // İşlem türünü belirtiyoruz
                    islem.IslemTuru = IslemTuru.Cekilis;

                    // islem açıklamasını belirtiyoruz.
                    islem.IslemAciklamasi = $"Bankamızın gerçekleştirdiği çekiliş sonucu {islem.HesapNo} nolu hesaba {cekilisDegeri} TL yatırıldı.";

                    // Kaç TL değerinde çekiliş gerçekleştirdiğimizi beliritiyoruz.
                    islem.Miktar = cekilisDegeri;

                    // Çekilişi kazanan banka hesabının hesap numarasını banka hesapları listesinden çekiyoruz.
                    int result = bankaHesaplari.FindIndex(bankaHesabi => bankaHesabi.HesapNo == cekilisHesaplari[cekilisHesabiIndex].HesapNo);

                    // Hesaba çekiliş değeri aktarılmadan önceki bakiye
                    islem.OncekiBakiye = bankaHesaplari[result].Bakiye;

                    // Çekilişte belirtilen miktarı banka hesabına aktarıyoruz.
                    islem.SonrakiBakiye = islem.OncekiBakiye + islem.Miktar;

                    // Oluşturulan işlemi, işlem listesine ekliyoruz.
                    islemListesi.Add(islem);

                    // Banka hesap bakiyesini güncelliyoruz
                    bankaHesaplari[result].Bakiye = islem.SonrakiBakiye;

                    hesap.HesapDurum(HesapNoKontrol(islem.HesapNo, bankaHesaplari), islem.HesapNo, bankaHesaplari);

                    // Çekilişi kazanan hesabı göster
                    IslemDekontuGoster(islemListesi);

                }
                else
                {
                    Console.WriteLine("\n");
                    Console.WriteLine("************************************************");
                    Console.WriteLine("Sistemde Bulunan Hesapların Çekiliş Hakkı Yoktur");
                    Console.WriteLine("************************************************");
                    Console.WriteLine("\n");
                }

            }
            else
            {
                Console.WriteLine("\n");
                Console.WriteLine("************************************************************");
                Console.WriteLine("Sistemde Çekilişe Katılacak Kayıtlı Hesap Bulunmamaktadır...");
                Console.WriteLine("************************************************************");
                Console.WriteLine("\n");
            }



        }

        // İlave Fonksiyonlar
        public bool HesapNoKontrol(string hesapNo, List<Hesap> bankaHesaplari)
        {
            {
                int result = bankaHesaplari.FindIndex(bankaHesabi => bankaHesabi.HesapNo == hesapNo);
                if (result != -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void HesapGoster(List<Hesap> bankaHesaplari)
        {
            for (int i = bankaHesaplari.Count; i > 0; i--)
            {
                Console.WriteLine("\n");
                Console.WriteLine("********************* HESAP CÜZDANI *********************");
                Console.WriteLine($"Hesap Numarası : {bankaHesaplari[i - 1].HesapNo}");
                Console.WriteLine($"İsim : {bankaHesaplari[i - 1].Isim}");
                Console.WriteLine($"Soyisim : {bankaHesaplari[i - 1].Soyisim}");
                Console.WriteLine($"Banka Hesap Türü : {bankaHesaplari[i - 1].HesapTuruDeger}");
                Console.WriteLine($"Hesap Oluşturulma Tarihi : {bankaHesaplari[i - 1].OlusturulmaTarihi}");
                Console.WriteLine($"Hesaba Aktarılan Miktar : {(double)bankaHesaplari[i - 1].Bakiye} TL");
                Console.WriteLine($"Çekiliş Hakkı : {bankaHesaplari[i - 1].CekilisHakki}");
                Console.WriteLine("********************* HESAP CÜZDANI *********************");
                break;
            }
        }

        public void IslemDekontuGoster(List<Islem> islemGecmisi)
        {
            int dekontIndex = islemGecmisi.Count - 1;
            Console.WriteLine("\n");
            Console.WriteLine("********************* İŞLEM DEKONTU *********************");
            Console.WriteLine($"İşlem No : {islemGecmisi[dekontIndex].IslemNo}");
            Console.WriteLine($"Hesap No : {islemGecmisi[dekontIndex].HesapNo}");
            Console.WriteLine($"İşlem Tarihi : {islemGecmisi[dekontIndex].IslemTarihi}");
            Console.WriteLine($"İşlem Türü : {islemGecmisi[dekontIndex].IslemTuru}");
            Console.WriteLine($"İşlem Açıklaması : {islemGecmisi[dekontIndex].IslemAciklamasi}");
            Console.WriteLine($"Miktar (TL) : {(double)islemGecmisi[dekontIndex].Miktar} TL");
            Console.WriteLine($"Önceki Bakiye (TL) : {(double)islemGecmisi[dekontIndex].OncekiBakiye} TL");
            Console.WriteLine($"Sonraki Bakiye (TL) : {(double)islemGecmisi[dekontIndex].SonrakiBakiye} TL");
            Console.WriteLine("********************* İŞLEM DEKONTU *********************");
        }

        public double CekilisDegeriOnay()
        {
            // Çekiliş değerini belirle
            double geciciCekilisDeger = 0;
            do
            {
                Console.Write("Çekiliş Kaç TL Değerinde Olacak : ");
                geciciCekilisDeger = Convert.ToDouble(Console.ReadLine());
            } while (geciciCekilisDeger == 0 || geciciCekilisDeger < 0);

            return geciciCekilisDeger;

        }
    }
}
