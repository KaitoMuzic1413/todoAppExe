# TodoCS

Ứng dụng Todo được viết bằng **C# / .NET 10**.

## Dành cho người dùng Windows

Thư mục gửi cho người dùng Windows:

```text
bin/Release/net10.0/win-x64/publish/
```

Người dùng chỉ cần nhấn đúp vào file:

```text
todoCS.exe
```

> Bản Windows được build dưới dạng **self-contained**, vì vậy người dùng không cần cài .NET để chạy ứng dụng.

## Dành cho người dùng Linux

Thư mục gửi cho người dùng Linux:

```text
bin/Release/net10.0/linux-x64/publish/
```

Mở Terminal tại thư mục `publish` và chạy:

```bash
chmod +x todoCS
./todoCS
```

> Bản Linux được build dưới dạng **self-contained**, vì vậy người dùng không cần cài .NET để chạy ứng dụng.

---

## Dành cho người phát triển

### Cần cài trước khi clone/chạy project

* **Git**: dùng để clone repository
* **.NET SDK 10**: dùng để restore, build và chạy project
* **Visual Studio / VS Code**: dùng để chỉnh sửa code (không bắt buộc)

### Kiểm tra Git

```bash
git --version
```

### Kiểm tra .NET

```bash
dotnet --version
```

---

## Clone repository

```bash
git clone https://github.com/KaitoMuzic1413/todoAppExe.git
cd todoAppExe
```

## Restore dependencies

```bash
dotnet restore
```

> Các thư viện NuGet cần thiết sẽ được .NET tự động tải về thông qua file `.csproj`. Không cần tải từng thư viện thủ công.

## Chạy project

```bash
dotnet run
```

## Build project

```bash
dotnet build
```

## Build bản Release

```bash
dotnet build -c Release
```

---

## Update app trên Windows

Build bản Windows:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

File sau khi build:

```text
bin/Release/net10.0/win-x64/publish/todoCS.exe
```

---

## Update app bản Linux

Build bản Linux:

```bash
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
```

File sau khi build:

```text
bin/Release/net10.0/linux-x64/publish/todoCS
```

---

## Build và Update app nhanh bằng Linux

### 1. Tạo file `build.sh`

```bash
nano build.sh
```

Dán nội dung:

```bash
#!/bin/bash

echo "Đang build file cho Windows..."
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

echo "Đang build file cho Linux..."
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true

echo ""
echo "Build hoàn tất!"
echo "File Windows: bin/Release/net10.0/win-x64/publish/"
echo "File Linux:   bin/Release/net10.0/linux-x64/publish/"
```

### 2. Cấp quyền thực thi

```bash
chmod +x build.sh
```

### 3. Cập nhật code và build

Mỗi khi sửa code xong, chạy:

```bash
./build.sh
```

Script sẽ build cả:

* Windows: `win-x64`
* Linux: `linux-x64`

---

## Cấu trúc file sau khi build

```text
bin/
└── Release/
    └── net10.0/
        ├── win-x64/
        │   └── publish/
        │       └── todoCS.exe
        │
        └── linux-x64/
            └── publish/
                └── todoCS
```

## Lưu ý

Các thư mục/file như `bin/`, `obj/` và những file được khai báo trong `.gitignore` sẽ không được đưa lên GitHub.

Khi clone project, các file build cần thiết sẽ được .NET tạo lại thông qua các lệnh `dotnet restore`, `dotnet build` hoặc `dotnet publish`.
