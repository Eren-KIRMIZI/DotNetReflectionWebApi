# ASP.NET Core Web API Demo Uygulaması

Bu bölümde, Web API içerisinde model doğrulama ve temel CRUD işlemleri başarıyla uygulanmıştır. Uygulama, bir sonraki aşamada eklenecek olan Middleware, Filter ve Reflection tabanlı sistem bileşenleri için sağlam bir temel oluşturmuştur.

## 4) Product DTO ve Model Validation

Bu aşamada, Web API projesinde kullanılacak olan ProductDto modeli oluşturulmuştur. DTO (Data Transfer Object), istemci ile sunucu arasında taşınan veriyi temsil eden sınıflardır ve doğrudan domain modelinin dışa açılmasını engeller.

### 4.1 ProductDto Oluşturulma Amacı

ProductDto sınıfı aşağıdaki amaçlarla oluşturulmuştur:

- API üzerinden gönderilen ürün verilerini temsil etmek
- Model doğrulama (validation) kurallarını merkezi olarak tanımlamak
- Controller katmanında veri kontrolünü kolaylaştırmak

### 4.2 Data Annotation ile Model Validation

ProductDto sınıfında Data Annotation attribute'ları kullanarak doğrulama kuralları tanımlanmıştır.

**Kullanılan attribute'lar:**

- **[Required]** - Ürün adının boş geçilememesini sağlar
- **[StringLength]** - Ürün adının minimum ve maksimum uzunluğunu sınırlar
- **[Range]** - Ürün fiyatının belirli bir aralıkta olmasını zorunlu kılar

Bu doğrulamalar sayesinde, hatalı veriler controller seviyesine ulaşmadan yakalanmaktadır.

Bu yaklaşım, validation logic'in controller'dan ayrılmasını sağlar ve kodun okunabilirliğini arttırır.

## 5) Products Controller ve CRUD işlemleri

Bu aşamada, ürünler üzerinde temel CRUD (Create, Read, Delete) işlemlerini gerçekleştiren ProductsController oluşturulmuştur.

Controller, REST prensiplerine uygun şekilde tasarlanmıştır.

### 5.1 ProductsController Yapısı

Controller aşağıdaki attribute'lar ile yapılandırılmıştır:

- **[ApiController]** - Otomatik model doğrulama ve HTTP 400 hatalarının yönetilmesini sağlar
- **[Route("api/products")]** - Controller'a ait endpoint'lerin temel URL yolunu belirler

Bu yapı sayesinde, ürün işlemleri api/products yolu üzerinden erişilebilir hale gelmiştir.

### 5.2 Kullanılan HTTP Metotları

Controller içerisinde aşağıdaki HTTP metotları uygulanmıştır:

**GET: Tüm Ürünleri Listeleme**
```
GET /api/products
```
Sistemde bulunan tüm ürünleri döndürür.

**GET - ID'ye göre ürün getirme**
```
GET /api/products/{id}
```
Belirtilen ID'ye sahip ürün bulunamazsa 404 Not Found döner.

**POST – Yeni Ürün Ekleme**
```
POST /api/products
```
Bu endpoint'te:
- ProductDto modeli kullanılmıştır
- ModelState.IsValid kontrolü ile validation sağlanmıştır
- Hatalı veri gönderildiğinde 400 Bad Request dönmektedir

Bu işlem, Data Annotation validation'ın başarıyla çalıştığını göstermektedir.

**DELETE - Ürün Silme**
```
DELETE /api/products/{id}
```
Belirtilen ürün bulunmazsa 404 Not Found, başarılı silme işleminde 204 No Content döndürülmektedir.

### 5.3 Geçici Veri Yönetimi

Ürünler, örnekleme amacıyla statik bir liste içerisinde tutulmuştur:
```csharp
private static readonly List<ProductDto> _products = new();
```

Bu yaklaşım:

Veritabanı kullanılmadan API davranışının test edilmesini amaçlamaktadır.

## 6) Swagger ile API testleri

Uygulama çalıştırıldığında Swagger arayüzü üzerinden API endpoint'leri test edilmiştir.

**Yapılan testler:**

- Boş ürün listesi sorgulama
- Hatalı fiyat ile ürün ekleme (validation hatası)
- Geçerli veri ile ürün ekleme
- ID'ye göre ürün getirme
- Ürün silme

Swagger testleri sonucunda, tüm endpoint'lerin doğru çalıştığı gözlemlenmiştir.

**Kazanımlar:**

- DTO kullanımı - Veri transferi güvenli hale getirildi
- Model validation - Hatalı veri girişleri engellendi
- RESTful API - HTTP metotları doğru şekilde kullanıldı
- Swagger test - API davranışı doğrulandı

## 7) Custom Middleware ve Filter Kullanımı

Bu aşamada, ASP.NET Core Web API uygulamasının request–response pipeline yapısı genişletilmiştir.

Amaç; gelen isteklerin, çalışan action'ların ve oluşabilecek hataların merkezi ve kontrol edilebilir bir şekilde yönetilmesini sağlamaktır.

Bu kapsamda:

- Custom Middleware
- Action Filter
- Exception Filter

kullanılmıştır.

### 7.1) Custom Middleware – Request / Response Loglama

Middleware'ler, ASP.NET Core uygulamasında HTTP isteklerinin geçtiği pipeline içerisinde çalışan bileşenlerdir.

Her istek ve yanıt, controller'a ulaşmadan önce ve sonra middleware'lerden geçer.

**Middleware Kullanım Amacı**

Bu projede oluşturulan middleware'in amaçları:

- Gelen HTTP isteğinin metot ve path bilgisini loglamak
- İstek işlendikten sonra response status code'u yakalamak
- Pipeline'ın çalışma sırasını gözlemlemek

**RequestResponseLoggingMiddleware**

Oluşturulan RequestResponseLoggingMiddleware sınıfı, her istek için aşağıdaki bilgileri konsola yazdırmaktadır:

- HTTP Method (GET, POST, DELETE)
- Request Path
- Zaman Bilgisi
- Response Status Code

Bu middleware sayesinde, uygulamaya gelen her isteğin uçtan uca takibi yapılabilmektedir.

### 7.2) Action Filter - Action Çalışma Süresi Ölçümü

Filter'lar, controller action'larının öncesinde ve sonrasında çalışan bileşenlerdir.

Bu projede, action'ların çalışma süresini ölçmek için bir Action Filter kullanılmıştır.

**ActionTimingFilter**

ActionTimingFilter, IActionFilter arayüzünü implemente etmektedir.

Çalışma mantığı:

- OnActionExecuting → Action başlamadan önce çalışır
- OnActionExecuted → Action tamamlandıktan sonra çalışır

Bu iki nokta arasındaki süre ölçülerek, action'ın kaç milisaniyede çalıştığı konsola yazdırılmaktadır.

Bu yaklaşım, performans ölçümü ve debugging açısından oldukça faydalıdır.

### 7.3) Exception Filter - Global Hata Yönetimi

Uygulamalarda oluşabilecek beklenmeyen hataların her controller'da ayrı ayrı yönetilmesi, kod tekrarına yol açar. Bu nedenle projede Global Exception Filter kullanılmıştır.

GlobalExceptionFilter, IExceptionFilter arayüzünü implemente eder ve uygulama genelinde oluşan hataları yakalar.

Bu filter:

- Exception oluştuğunda devreye girer
- Konsola log basar
- Kullanıcıya kontrollü ve anlamlı bir hata mesajı döndürür
- HTTP 500 (Internal Server Error) status code üretir

Bu sayede:

- Uygulama çökmez
- Hatalar merkezi bir noktadan yönetilir
- Güvenli hata mesajı sağlanır

## 8) Program.cs ve Pipeline Konfigürasyonu

Bu aşamada, oluşturulan middleware ve filter'lar uygulamanın pipeline'ına entegre edilmiştir.

### 8.1 Controller Servislerine Filter Ekleme

Program.cs içerisinde AddControllers metodu özelleştirilerek:

- ActionTimingFilter
- GlobalExceptionFilter

global olarak tanımlanmıştır.

Bu sayede:

- Tüm controller ve action'lar için otomatik olarak çalışırlar
- Tek tek attribute eklemeye gerek kalmaz

### 8.2 Middleware'in Pipeline'a Dahil Edilmesi

RequestResponseLoggingMiddleware, UseMiddleware metodu ile pipeline'a eklenmiştir.

Pipeline sırası şu şekildedir:

1) HTTPS yönlendirme
2) Custom Middleware (Request / Response Loglama)
3) Authorization
4) Controller mapping

Middleware sırası, uygulamanın davranışını doğrudan etkilediği için özellikle bilinçli şekilde konumlandırılmıştır.

## 9) Reflection Tabanlı Metadata Endpoint

Bu aşamada, Web API uygulamasının kendi yapısını çalışma zamanında analiz edebilen bir endpoint geliştirilmiştir.

Bu endpoint, .NET'in Reflection yetenekleri kullanılarak oluşturulmuştur.

**Geliştirilen endpoint:**
```
GET /api/system/attribute-map
```

Bu endpoint sayesinde API, kendi controller ve action yapılarını dinamik olarak inceleyebilmekte ve metadata bilgisini JSON formatında dış dünyaya sunabilmektedir.

### 9.1 Reflection Kullanım Amacı

Reflection, bir uygulamanın çalışma zamanında kendi tip bilgilerine erişmesini sağlar.

Bu projede Reflection aşağıdaki amaçlarla kullanılmıştır:

- Uygulama içerisindeki tüm controller sınıflarını tespit etmek
- Controller'lara ait action metotlarını bulmak
- Action'lar üzerinde kullanılan HTTP attribute'larını (HttpGet, HttpPost, vb.) okumak
- Bu bilgileri yapılandırılmış bir JSON çıktısı olarak sunmak

Bu yaklaşım, statik kodlamaya ihtiyaç duymadan kendini tanımlayan (self-descriptive) bir API yapısı oluşturur.

### 9.2 SystemController Tasarımı

Bu amaç doğrultusunda SystemController isimli bir controller oluşturulmuştur.

Controller, api/system route'u altında konumlandırılmıştır.

İlgili endpoint:
```
GET /api/system/attribute-map
```

Bu endpoint, yalnızca sistemin iç yapısını analiz etmeye yönelik olup, iş mantığından bağımsızdır.

### 9.3 Controller ve Action Tespiti

Reflection işlemi sırasında:

- Assembly.GetExecutingAssembly() kullanılarak çalışan assembly alınmıştır
- ControllerBase sınıfından türeyen tüm concrete (soyut olmayan) sınıflar tespit edilmiştir
- Her controller içerisindeki public instance action metotları listelenmiştir

Bu sayede uygulama, hangi controller'lara ve hangi action'lara sahip olduğunu runtime'da öğrenebilmektedir.

### 9.4 Attribute Analizi

Her action metodu üzerinde:

- GetCustomAttributes() metodu kullanılarak attribute'lar okunmuştur
- HttpGet, HttpPost, HttpDelete gibi HTTP attribute'ları filtrelenmiştir
- Attribute isimleri metadata çıktısına eklenmiştir

Bu işlem, endpoint'lerin hangi HTTP metotlarıyla erişilebilir olduğunu otomatik olarak ortaya çıkarmaktadır.

### 9.5 JSON Metadata Çıktısı

Endpoint çağrıldığında, aşağıdaki bilgileri içeren bir JSON yapı döndürülmektedir:

- Controller adı
- Action adı
- Action üzerinde kullanılan HTTP attribute'ları

Bu yapı, API'nin güncel durumunu yansıtan dinamik bir harita niteliğindedir.

<img width="1366" height="714" alt="cikti1" src="https://github.com/user-attachments/assets/75a49755-621f-4fdc-a9bb-665a1277916a" />


<img width="1297" height="640" alt="cikti2" src="https://github.com/user-attachments/assets/fa1af701-bc13-40ba-b4d2-63d31f022cc6" />


<img width="150" height="92" alt="cikti3" src="https://github.com/user-attachments/assets/1fafc7e5-5f90-4187-b80e-0c7a485bf1db" />


<img width="1281" height="632" alt="Cikti4" src="https://github.com/user-attachments/assets/783a23ff-c846-4579-b29e-8540d23754ca" />


<img width="1301" height="555" alt="cikti5" src="https://github.com/user-attachments/assets/2acd1add-c30f-4f21-908b-2cdf3fd939ff" />


<img width="1289" height="398" alt="Cikti6" src="https://github.com/user-attachments/assets/62449aa1-a847-4b55-8889-d5c6d1f9f865" />


<img width="1301" height="563" alt="Cikti7" src="https://github.com/user-attachments/assets/544d49a5-d6b7-440b-b425-6080ebb83d0a" />


<img width="1313" height="193" alt="Cikti8" src="https://github.com/user-attachments/assets/849ea9fa-fd92-4048-9d8a-6e46f5975ad6" />


<img width="1306" height="220" alt="Cikti9" src="https://github.com/user-attachments/assets/30af6428-8a25-400e-b486-780d3887c95d" />


<img width="843" height="57" alt="Cikti10" src="https://github.com/user-attachments/assets/a179c82f-97dc-4eca-82a6-5fc1cf2e7409" />


<img width="1302" height="394" alt="Cikti11" src="https://github.com/user-attachments/assets/806353f2-1866-42f7-bade-60c7abb6d233" />
