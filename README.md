# Notika — Yapay Zekâ Destekli E-Posta ve Verimlilik Platformu

<p align="center">
  <strong>E-posta yönetimi, yapay zekâ, görev takibi ve günlük dijital servisleri tek bir platformda birleştiren ASP.NET Core MVC uygulaması.</strong>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4?style=flat-square&logo=dotnet" alt="ASP.NET Core MVC" />
  <img src="https://img.shields.io/badge/Entity_Framework-Core-512BD4?style=flat-square" alt="Entity Framework Core" />
  <img src="https://img.shields.io/badge/SQL_Server-Database-CC2927?style=flat-square&logo=microsoftsqlserver" alt="SQL Server" />
  <img src="https://img.shields.io/badge/OpenAI-AI_Integration-412991?style=flat-square&logo=openai" alt="OpenAI" />
  <img src="https://img.shields.io/badge/RapidAPI-External_APIs-0055DA?style=flat-square&logo=rapid" alt="RapidAPI" />
  <img src="https://img.shields.io/badge/MailKit-Email_Service-0A66C2?style=flat-square" alt="MailKit" />
</p>

## 📌 Proje Hakkında

Notika, klasik bir e-posta uygulamasının temel özelliklerini yapay zekâ destekli araçlar, görev yönetimi, kişiselleştirilmiş dashboard ve farklı dış servis entegrasyonlarıyla bir araya getiren kapsamlı bir web uygulamasıdır.

Kullanıcılar e-postalarını yönetebilir, yapay zekâ ile yanıt oluşturabilir, gelen mesajların spam riskini analiz edebilir, görevlerini takip edebilir ve haber, finans, çeviri ve hava durumu servislerine tek bir uygulama üzerinden ulaşabilir.

## ✨ Öne Çıkan Özellikler

### 📧 E-posta Yönetimi

- Gelen ve gönderilen mesajları görüntüleme
- Yeni e-posta oluşturma
- Mesaj yanıtlama
- İleti dizileri üzerinden konuşma geçmişini takip etme
- Mesajları taslak olarak kaydetme
- Mesajları yıldızlama
- Mesajları silme ve silinen mesajları yönetme
- Spam mesajları ayrı alanda görüntüleme
- Mesajları kategorilere ayırma
- Okundu ve okunmadı durumlarını yönetme
- Yapay zekâ destekli e-posta yanıtı oluşturma

### 🤖 Yapay Zekâ Entegrasyonları

- **Akıllı e-posta yanıtı:** Mesaj içeriğine uygun yanıt önerileri oluşturulur.
- **Spam ve güvenilirlik analizi:** Gelen mesajlar yapay zekâ tarafından analiz edilerek spam skoru ve değerlendirme sonucu oluşturulur.
- **AI Destek Merkezi:** Kullanıcılar uygulama içerisinde bulunan yapay zekâ asistanıyla konuşarak farklı konularda yardım alabilir.

### ✅ Görev Yönetimi

Google Görevler benzeri hazırlanan görev modülüyle kullanıcılar:

- Yeni görev oluşturabilir.
- Görevlere tarih ve önem bilgisi ekleyebilir.
- Alt görevler tanımlayabilir.
- Görevleri tamamlandı olarak işaretleyebilir.
- Görevleri yıldızlayabilir veya silebilir.
- Bugünkü görevlerini görüntüleyebilir.
- Yaklaşan görevlerini takip edebilir.
- Süresi geçmiş görevlerini görüntüleyebilir.

### 📊 Kişiselleştirilmiş Dashboard

Her kullanıcı için özel olarak oluşturulan dashboard ekranında:

- Özet bilgi kartları
- Dinamik widget’lar
- Mesaj istatistikleri
- Görev istatistikleri
- Bildirim istatistikleri
- Pasta grafikler
- Bar grafikler
- Son aktiviteler
- Özet tablolar

yer almaktadır.

### 📰 Haberler

Haber içerikleri RapidAPI üzerinden dinamik olarak alınmaktadır. Kullanıcılar güncel haberleri farklı kategoriler altında uygulama içerisinden takip edebilir.

### 🌐 Çeviri

RapidAPI entegrasyonu kullanılarak farklı diller arasında çeviri yapılabilir.

- Çeviri geçmişi kaydedilebilir.
- Daha önce yapılan çeviriler görüntülenebilir.
- Önemli çeviriler kaydedilebilir.
- Kaydedilen çevirilere daha sonra tekrar ulaşılabilir.

### 💰 Finans

Finans modülünde ağırlıklı olarak CollectAPI servisleri kullanılmıştır.

Kullanıcılar:

- Güncel döviz kurlarını
- Para birimi dönüşümlerini
- Hisse senedi verilerini
- Piyasa özetini
- Takip listesini
- Finansal grafik ve tabloları

uygulama içerisinden görüntüleyebilir.

### 🌤️ Konuma Göre Hava Durumu

Tarayıcı konum izni sayesinde kullanıcının bulunduğu konum belirlenir ve ilgili bölgenin güncel hava durumu gösterilir.

Konum bilgisi tarayıcı hafızasında saklanarak uygulamanın farklı sayfalarında aynı konuma ait hava durumu verilerinin kullanılması sağlanır.

Gösterilen hava durumu bilgileri:

- Güncel sıcaklık
- Hissedilen sıcaklık
- Nem oranı
- Rüzgâr hızı
- Hava durumu açıklaması
- Konuma ait şehir bilgisi

### 🔔 Bildirim Sistemi

Uygulama için özel bir bildirim servisi hazırlanmıştır.

Aşağıdaki işlemler sonrasında kullanıcıya otomatik bildirim oluşturulur:

- Profil bilgilerinin güncellenmesi
- Şifre değiştirme işlemleri
- Güvenlik işlemleri
- Görev hareketleri
- Kullanıcı hesabıyla ilgili önemli işlemler
- Sistem tarafından gerçekleştirilen işlemler

Bildirim türleri `enum` yapısıyla düzenli ve genişletilebilir şekilde yönetilmektedir.

### 🔐 Kullanıcı ve Güvenlik İşlemleri

- ASP.NET Core Identity tabanlı kullanıcı yönetimi
- Kullanıcı kayıt işlemleri
- Kullanıcı giriş işlemleri
- Kullanıcıya özel veri erişimi
- Profil bilgilerini görüntüleme
- Profil bilgilerini güncelleme
- Şifre değiştirme
- Güvenlik işlemleri
- E-posta ile aktivasyon kodu gönderimi
- MailKit ile aktivasyon e-postası oluşturma
- Aktivasyon koduyla kullanıcı hesabını doğrulama
- Aktivasyon tamamlanmadığında doğrulanmamış kullanıcı kaydını sistemden temizleme

## 🔗 Kullanılan Dış Servisler

| Servis | Kullanım Alanı |
|---|---|
| OpenAI | E-posta yanıtlama, spam analizi ve AI destek asistanı |
| RapidAPI | Haberler, çeviri ve hava durumu servisleri |
| CollectAPI | Döviz, para birimi dönüşümü ve piyasa verileri |
| MailKit | Aktivasyon kodu ve sistem e-postaları |


## 🛠️ Kullanılan Teknolojiler

- ASP.NET Core MVC
- C#
- Razor View
- ViewComponent
- Entity Framework Core
- Microsoft SQL Server
- ASP.NET Core Identity
- Dependency Injection
- LINQ
- OpenAI API
- RapidAPI
- CollectAPI
- MailKit
- HttpClient
- JavaScript
- Fetch API
- Geolocation API
- HTML
- CSS
- Tailwind CSS
- Grafik ve veri görselleştirme araçları

## 🏗️ Proje Yapısı

```text
IdentityEmailApp
├── Controllers
├── DTOs
├── Entities
├── Services
│   ├── Abstract
│   └── Concrete
├── ViewComponents
├── Views
├── Data
├── Migrations
└── wwwroot
```

Uygulamada dış servis çağrıları ve iş kuralları servis katmanında tutulmuştur. Sayfalarda tekrar kullanılan arayüz bölümleri ViewComponent yapısıyla ayrıştırılmıştır.

## 📸 Ekran Görüntüleri

Görseller `docs/screenshots` klasörü içerisinde tutulmaktadır.

### Giriş, Kayıt ve Aktivasyon

<p align="center">
  
<img width="1825" height="962" alt="register" src="https://github.com/user-attachments/assets/bf22d4e6-fb48-423f-b965-928ee1e33914" />
  <img width="1850" height="962" alt="login" src="https://github.com/user-attachments/assets/41aef688-0dce-4a89-9f88-84c910be8786" />
<img width="1780" height="892" alt="1" src="https://github.com/user-attachments/assets/82fdc8f1-d33f-43ae-afb6-cd2868bbef50" />
<img width="1797" height="958" alt="Ekran Alıntısı" src="https://github.com/user-attachments/assets/3e465845-6868-41ff-84ff-2dbac2dab76e" />


</p>

### Kullanıcı Dashboard’u

<p align="center">
 <img width="1839" height="945" alt="5" src="https://github.com/user-attachments/assets/2b523094-cb49-4e7d-8ad7-0062ec161da9" />
<img width="1851" height="968" alt="4" src="https://github.com/user-attachments/assets/1bc9312e-fb36-4fc0-98c6-bb5fbb9fb7d0" />
<img width="1841" height="956" alt="3" src="https://github.com/user-attachments/assets/ad2d5318-9174-4549-acf6-ff72ab51db6c" />
<img width="1838" height="946" alt="2" src="https://github.com/user-attachments/assets/7abf70f2-8f58-46e3-9bf4-46ba276d326c" />
<img width="1845" height="969" alt="1" src="https://github.com/user-attachments/assets/e8429375-2731-436d-90d4-7ccb87c833f6" />

</p>

### Gelen Kutusu, Mesaj Detayı, Bildiirmler  

<table>
  <tr>
    <td width="50%">
<img width="1536" height="966" alt="2" src="https://github.com/user-attachments/assets/9f667475-2d3b-4e39-8111-52efda7c2b66" />
 <img width="1436" height="832" alt="9" src="https://github.com/user-attachments/assets/50d55982-c22d-46db-a4f9-dd5f698241f0" />
  
</td>
    <td width="50%">
  
<img width="1401" height="888" alt="6" src="https://github.com/user-attachments/assets/32cc7f1a-11d5-424d-8b4d-e77c30eb2e31" />
<img width="1453" height="910" alt="5" src="https://github.com/user-attachments/assets/e4900819-feec-4e61-b811-8e8ab61b5de4" />
<img width="1457" height="923" alt="4" src="https://github.com/user-attachments/assets/0c856217-a2d6-40df-9fc4-4f9bf96e157b" />
<img width="1517" height="912" alt="3" src="https://github.com/user-attachments/assets/56881b85-ab5d-4993-9a00-9f834cc5503c" />

</td>
  </tr>
</table>

### AI E-posta Yanıtı ve Spam Analizi

<table>
  <tr>
    <td width="50%">
    <img width="1198" height="939" alt="11" src="https://github.com/user-attachments/assets/19a0bca6-fd56-4da8-8ead-d069f1639ec5" />

</td>
<td width="50%">
<img width="1029" height="970" alt="10" src="https://github.com/user-attachments/assets/e6ffda50-c0d9-4b3f-8ad8-979ac092419c" />

</td>
  </tr>
</table>

### Görevler ve Bildirimler

<table>
  <tr>
    <td width="50%">
     <img width="1559" height="902" alt="1" src="https://github.com/user-attachments/assets/6f68db71-a25c-452e-b76f-a5535770fcba" />

</td>
    <td width="50%">
<img width="1846" height="797" alt="yeni görev oluştur" src="https://github.com/user-attachments/assets/6e71160a-3795-4ccc-a432-046867b71de2" />
<img width="1838" height="770" alt="6" src="https://github.com/user-attachments/assets/f180bb2f-e4bd-4682-9fd3-2bbab0c8561e" />
<img width="1839" height="769" alt="5" src="https://github.com/user-attachments/assets/7e07c185-a6d9-436b-9ca5-b3e25bb32d1c" />
<img width="1837" height="958" alt="4" src="https://github.com/user-attachments/assets/08c8cd92-7689-47f1-a710-da2f358255e6" />
<img width="1848" height="966" alt="3" src="https://github.com/user-attachments/assets/2e2d9654-7be2-4416-bc69-744e8f4ebdb8" />
<img width="1828" height="789" alt="2" src="https://github.com/user-attachments/assets/f43349dd-bbe6-43a4-9d9f-83803a9c6952" />
<img width="1841" height="974" alt="1" src="https://github.com/user-attachments/assets/e1bd9674-3e18-4ade-a038-9968d441e1b4" />

</td>
  </tr>
</table>

### AI Destek Merkezi ve Çeviri

<table>
  <tr>
    <td width="50%">
      <img width="1801" height="893" alt="4" src="https://github.com/user-attachments/assets/8063ec71-4835-48ff-8ffc-7ee0af3757b6" />
<img width="1782" height="910" alt="1" src="https://github.com/user-attachments/assets/3913203a-a95d-4075-b1e7-6bbdc8b63890" />

</td>
    <td width="50%">
    
<img width="1819" height="949" alt="3" src="https://github.com/user-attachments/assets/959784ac-0e18-4ae9-b9e2-83b2f8822b8b" />
<img width="1814" height="964" alt="2" src="https://github.com/user-attachments/assets/2a59f370-8d07-4593-b2bf-d4a0aeacbfbc" />
<img width="1817" height="937" alt="1" src="https://github.com/user-attachments/assets/6b0ac3c0-84fd-43b7-b8ca-eb87596a4d1b" />

    </td>
  </tr>
</table>

### Haberler ve Finans

<table>
  <tr>
    <td width="50%">
    <img width="1778" height="960" alt="7" src="https://github.com/user-attachments/assets/68fc1320-d727-4ee9-97ed-f39600558f81" />
<img width="1786" height="951" alt="6" src="https://github.com/user-attachments/assets/95050bce-bedf-4db1-b3b3-bac86eab102d" />
<img width="1811" height="938" alt="5" src="https://github.com/user-attachments/assets/86237308-b426-4436-93a3-5b7df3808903" />
<img width="1798" height="949" alt="4" src="https://github.com/user-attachments/assets/137e4829-521e-4dc9-b4ad-c2ba896c39c4" />
<img width="1771" height="951" alt="3" src="https://github.com/user-attachments/assets/cbb7c6be-9223-4bcc-a9b7-24b54ec2ab93" />
<img width="665" height="912" alt="2" src="https://github.com/user-attachments/assets/43c3b068-ddd8-4651-a238-d89ce420fe9f" />

</td>
    <td width="50%">
   <img width="982" height="969" alt="1" src="https://github.com/user-attachments/assets/282f9519-a991-4baa-abfd-d5957c584b49" />

    </td>
  </tr>
</table>

### Profil

<table>
  <tr>
    <td width="50%">
  <img width="1169" height="868" alt="8" src="https://github.com/user-attachments/assets/b69e5bcf-b15f-452c-bce6-9f1274c9f577" />

</td>
    <td width="50%">
 
<img width="1146" height="968" alt="13" src="https://github.com/user-attachments/assets/b5554f41-a013-46ba-86a0-6cdeaf652bd6" />

</td>
  </tr>
</table>

## ⚙️ Kurulum

Projeyi bilgisayarınıza klonlayın:

```bash
git clone https://github.com/merveearp/IdentityEmailApp.git
cd IdentityEmailApp
```

Gerekli NuGet paketlerini yükleyin:

```bash
dotnet restore
```

Veritabanı bağlantı adresini ve API anahtarlarını kendi ortamınıza göre yapılandırın.

Ardından veritabanını oluşturun:

```bash
dotnet ef database update
```

Uygulamayı çalıştırın:

```bash
dotnet run
```

> API anahtarlarını, bağlantı bilgilerini ve e-posta parolalarını doğrudan kaynak kodda veya GitHub deposunda paylaşmayın. User Secrets ya da ortam değişkenleri kullanın.

## 👩‍💻 Geliştirici

**Merve Arpacıoğlu Türk**

- GitHub: [github.com/merveearp](https://github.com/merveearp)


---

Bu proje; ASP.NET Core MVC, yapay zekâ entegrasyonları, dış API kullanımı, kimlik doğrulama, bildirim sistemi, görev yönetimi ve veri görselleştirme konularında kapsamlı bir uygulama geliştirme çalışması olarak hazırlanmıştır.
