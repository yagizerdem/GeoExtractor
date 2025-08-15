# GeoExtractor

🌊 **GPR (Ground Penetrating Radar) Veri İşleme ve Görselleştirme Uygulaması**

## Proje Hakkında

GeoExtractor, Dokuz Eylül Üniversitesi Donanım Stajı kapsamında (30.01.2025 - 22.02.2025) C# ile geliştirilmiş olan GPR veri analizi ve görselleştirme uygulamasıdır. Bu uygulama, Ground Penetrating Radar cihazlarından elde edilen verilerin işlenmesi, analiz edilmesi ve görselleştirilmesi amacıyla tasarlanmıştır.

## ✨ Ana Özellikler

- 📡 **GPR Veri İşleme**: Ground Penetrating Radar verilerinin otomatik işlenmesi
- 🖼️ **Görselleştirme**: GPR verilerinin 2D/3D görsel analizi
- 💾 **Veri Yönetimi**: SQLite veritabanı ile hızlı veri saklama
- 📊 **Analiz Araçları**: R tabanlı gelişmiş sinyal işleme algoritmaları
- 🗂️ **Dosya Sistemi**: Görsel çıktıların dosya sisteminde organizasyonu
- 🔄 **Gerçek Zamanlı İşleme**: Arka plan R script entegrasyonu

## 🏗️ Sistem Mimarisi

```
GeoExtractor/
├── GeoExtractor/                    # Ana C# projesi
│   ├── Forms/                       # Windows Forms arayüzleri
│   │   ├── MainForm.cs              # Ana uygulama formu
│   │   ├── DataVisualization.cs     # GPR veri görselleştirme
│   │   └── SettingsForm.cs          # Ayarlar formu
│   ├── Models/                      # Veri modelleri
│   │   ├── GPRData.cs               # GPR veri yapısı
│   │   └── ProcessingResult.cs      # İşlem sonuç modeli
│   ├── Services/                    # İş mantığı servisleri
│   │   ├── GPRProcessor.cs          # GPR veri işleme servisi
│   │   ├── DatabaseService.cs       # SQLite veritabanı servisi
│   │   └── RIntegration.cs          # R runtime entegrasyonu
│   ├── Utils/                       # Yardımcı sınıflar
│   │   ├── FileManager.cs           # Dosya işlemleri
│   │   └── ImageProcessor.cs        # Görsel işleme
│   ├── Resources/                   # Kaynaklar
│   └── App.config                   # Uygulama konfigürasyonu
├── Scripts/                         # R script dosyaları
│   ├── gpr_processing.R             # Ana GPR işleme scripti
│   ├── signal_analysis.R            # Sinyal analiz fonksiyonları
│   └── visualization.R              # Görselleştirme scripti
├── R Portable/                      # Taşınabilir R Runtime
│   ├── App/                         # R uygulaması
│   ├── Data/                        # R veri dosyaları
│   └── Other/                       # R destekleyici dosyaları
└── README.md                        # Bu dosya
```

## 🔧 R Portable Entegrasyonu

GPR veri işleme için **R Portable** kullanılmaktadır:

- **Sinyal İşleme**: R'ın güçlü matematik kütüphaneleri ile GPR sinyallerinin filtrelenmesi
- **İstatistiksel Analiz**: Anomali tespiti ve veri kalitesi analizi
- **Process Entegrasyonu**: C# Process sınıfı ile seamless R script çalıştırma
- **Taşınabilirlik**: Kurulum gerektirmeyen portable R runtime
- **Performans**: Optimize edilmiş GPR analiz algoritmaları

⚠️ **Not**: R Portable klasörü oldukça büyük boyuttadır (~500MB+) ve GPR analizi için özel olarak konfigüre edilmiş R paketlerini içermektedir.

## 🚀 Kurulum ve Çalıştırma

### Sistem Gereksinimleri

- Windows 10/11 (64-bit)
- .NET Framework 4.7.2 veya üzeri
- En az 8GB RAM (büyük GPR dosyaları için)
- 2GB boş disk alanı
- SQLite desteği

### Kurulum

1. **Repository'yi klonlayın**
   ```bash
   git clone https://github.com/yagizerdem/GeoExtractor.git
   cd GeoExtractor
   ```

2. **Visual Studio'da açın**
   - `GeoExtractor.sln` dosyasını açın
   - NuGet paketlerinin restore edilmesini bekleyin

3. **Veritabanını hazırlayın**
   - SQLite veritabanı otomatik olarak `Database/` klasöründe oluşturulur

4. **Build ve çalıştır**
   - Solution'ı build edin (F6)
   - Uygulamayı başlatın (F5)

## 🛠️ Teknik Detaylar

### Ana Teknolojiler

- **C# .NET Framework**: Ana uygulama geliştirme platformu
- **Windows Forms**: Kullanıcı arayüzü
- **SQLite**: GPR verilerinin yerel veritabanında saklanması
- **System.Diagnostics.Process**: R script entegrasyonu
- **System.IO**: Görsel dosyaların yönetimi
- **R Language**: GPR sinyal işleme ve analiz

### Veri Akışı

```
GPR Ham Veri (JSON) → C# Uygulama → R Script İşleme → 
SQLite Veritabanı + Görsel Dosyalar → Kullanıcı Arayüzü
```

## 📊 Temel Sınıf Yapıları

### GPR Veri Modeli

```csharp
public class GPRData
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public double[] Samples { get; set; }           // GPR sinyal örnekleri
    public double Frequency { get; set; }           // Frekans bilgisi
    public string Location { get; set; }            // Konum bilgisi
    public string ImagePath { get; set; }           // İşlenmiş görsel yolu
    public ProcessingStatus Status { get; set; }    // İşlem durumu
}

public enum ProcessingStatus
{
    Raw,        // Ham veri
    Processing, // İşleniyor
    Completed,  // İşlem tamamlandı
    Error       // Hata oluştu
}
```

### R Entegrasyon Servisi

```csharp
public class RIntegration
{
    private readonly string rExecutablePath;
    
    public ProcessingResult ExecuteGPRProcessing(GPRData data)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = rExecutablePath,
                Arguments = $"--slave --args {data.Id} {data.ToJson()}",
                RedirectStandardOutput = true,
                UseShellExecute = false
            }
        };
        
        // R script çalıştırma ve sonuç alma
        return ProcessResult(process);
    }
}
```

### Veritabanı Servisi

```csharp
public class DatabaseService
{
    private readonly string connectionString;
    
    public void SaveGPRData(GPRData data);
    public GPRData GetGPRData(int id);
    public List<GPRData> GetAllGPRData();
    public void UpdateProcessingStatus(int id, ProcessingStatus status);
}
```

## 🗄️ Veri Saklama

### SQLite Veritabanı Şeması

```sql
CREATE TABLE GPRData (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Timestamp DATETIME NOT NULL,
    RawDataJson TEXT NOT NULL,           -- JSON formatında ham GPR verisi
    Frequency REAL,
    Location TEXT,
    ImagePath TEXT,                      -- İşlenmiş görsel dosya yolu
    ProcessingStatus INTEGER DEFAULT 0,   -- İşlem durumu
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_timestamp ON GPRData(Timestamp);
CREATE INDEX idx_status ON GPRData(ProcessingStatus);
```

### Dosya Sistemi Organizasyonu

```
Images/
├── processed/              # İşlenmiş GPR görselleri
│   ├── 2025-01-30/        # Tarih bazlı klasörleme
│   │   ├── gpr_001.png
│   │   └── gpr_002.png
├── thumbnails/            # Küçük önizleme resimleri
└── exports/               # Export edilmiş dosyalar
```

## 🧪 Test ve Doğrulama

### Örnek GPR Veri Formatı (JSON)

```json
{
  "timestamp": "2025-02-15T10:30:00Z",
  "location": "Test Site A",
  "frequency": 400.0,
  "samples": [0.125, 0.230, 0.156, ...],
  "metadata": {
    "antenna_separation": "0.4m",
    "sampling_rate": 1024,
    "duration": "10.5s"
  }
}
```

## 📈 Performans ve Optimizasyon

- **Async Processing**: Büyük GPR dosyaları için asenkron işleme
- **Memory Management**: Büyük veri setleri için bellek optimizasyonu  
- **Database Indexing**: Hızlı veri erişimi için indexleme
- **Image Caching**: Görsel önbelleğe alma sistemi

## 🎓 Staj Projesi Detayları

**Dokuz Eylül Üniversitesi Donanım Stajı**
- **Staj Dönemi**: 30 Ocak 2025 - 22 Şubat 2025
- **Proje Kapsamı**: GPR veri analiz sistemi geliştirme
- **Teknolojiler**: C# .NET, SQLite, R Statistical Computing
- **Uygulama Türü**: Windows masaüstü uygulaması
- **Veri İşleme**: Gerçek zamanlı GPR sinyal analizi

## 🔧 Sorun Giderme

### Yaygın Sorunlar

1. **R Script Hatası**: 
   - R Portable path kontrolü
   - Script dosyalarının varlığını kontrol edin

2. **SQLite Bağlantı Hatası**:
   - Database klasör izinlerini kontrol edin
   - Veritabanı dosyasının varlığını doğrulayın

3. **Görsel İşleme Hatası**:
   - Images klasörü yazma izinlerini kontrol edin
   - Disk alanı kontrolü

## 👨‍💻 Geliştirici

**Yağız Erdem**
- GitHub: [@yagizerdem](https://github.com/yagizerdem)
- Proje: GPR Veri Analiz Sistemi
- Kurum: Dokuz Eylül Üniversitesi

## 📞 İletişim

Teknik sorular ve öneriler için:
- GitHub Issues
- Pull Request'ler kabul edilmektedir

---

**Not**: Bu GPR veri analiz uygulaması eğitim amaçlı geliştirilmiş olup, jeofizik araştırmalar için temel bir platform sağlamaktadır.
