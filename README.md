# OnlineShopZitro

یک پروژه فروشگاه آنلاین با .NET Core که شامل مدیریت سبد خرید، پرداخت و اتصال به Redis و RabbitMQ برای کش و صف‌بندی پیام‌ها است.


## Features

- مدیریت سبد خرید (Basket)
- اتصال به Redis برای کش
- اتصال به RabbitMQ برای صف‌بندی پیام‌ها
- APIهای RESTful
- Clean Architecture با لایه‌بندی Domain / Application / Infrastructure / Presentation
- CORS فعال برای توسعه
- Swagger / OpenAPI برای مستندسازی API


## Prerequisites

- .NET 9 SDK
- Docker (برای Redis و RabbitMQ)
- Redis (Port 6379)
- RabbitMQ (Port 5672 و پنل مدیریتی 15672)

اجرای Redis و RabbitMQ
اجرای پروژه با dotnet run


## ساختار پروژه (Project Structure)
- توضیح مختصر از لایه‌ها:
## Project Structure

- **Domain**: موجودیت‌ها و قوانین بیزینس
- **Application**: سرویس‌ها و Use Caseها
- **Infrastructure**: پیاده‌سازی Repository، Redis، RabbitMQ
- **Presentation**: API، Controllers، ViewModels



تنظیمات connetionString
 "Redis": {
  "ConnectionString": "localhost:6379,abortConnect=false,defaultDatabase=0"
},
"RabbitMQ": {
  "Host": "localhost",
  "Port": 5672,
  "Username": "guest",
  "Password": "guest",
  "QueueName": "payment-queue"
}



نکات برای اجرا
- CORS برای توسعه فعال است
- Redis و RabbitMQ باید قبل از اجرای پروژه روشن باشند
- Mapping بین DTO و Domain انجام می‌شود

