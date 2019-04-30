## Özet
Asp.net Mvc ile hazırlanmış blog uygulamasıdır. Uygulamada 3 farklı kullanıcı türü bulunmaktadır




Kullanıcı Türleri :
 - Standart Üye : Sistemdeki blogları inceleyebilir, beğenebilir ve yorum yapabilir.
 - Yazar : Standart üyenin özelliklerine sahiptir. Ayrıca sisteme kendi bloğunu eklebilir.
 - Yönetici: Sistemdeki tüm yetkilere sahiptir. Yeni kullanıcı ve blog ekleyebilir. Mevcut kullanıcıları ve blogları düzenleyebilmektedir.

## Kullanılan Teknolojiler
- Asp.Net MVC
- Entity Framework
- MSSQL

Uygulama katmanlı mimari kullanılarak geliştirilmiştir. Business, Common, Core, DataAccess, Entities, WebApp katmanları bulunmaktadır. Veritabanı oluşumu Entity Framework Code First ile proje çalıştığında ayağa kalmaktadır. Web kısmında görsellik Bootstrap ve jQuery ile sağlanmıştır.

Projenin çalışır halinin görsellerine [Screenshots](https://github.com/tekinali/nwBlog/tree/master/Screenshots) klasörü altından ulaşılabilir
