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

Gửi **nguyên thư mục** publish này cho người dùng Linux (không gửi riêng file `todoCS`):

```text
bin/Release/net10.0/linux-x64/publish/
```

Máy đích cần Linux **x86-64** và một phiên desktop có giao diện đồ họa (Avalonia dùng X11 mặc định). Trên Ubuntu/Debian, cài thư viện hệ thống nếu máy chưa có:

```bash
sudo apt update
sudo apt install -y libx11-6 libice6 libsm6 libfontconfig1
```

Sau khi chép và giải nén nguyên thư mục publish, mở Terminal tại đó và chạy:

```bash
bash run-todoCS.sh
```

> Bản Linux là **self-contained**, không cần cài .NET. Máy vẫn cần các thư viện Linux và môi trường desktop ở trên. Nếu nhấp đúp không mở được, chạy lệnh trong Terminal để xem lỗi.

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
dotnet publish -c Release -r linux-x64 --self-contained true
```

Gửi **toàn bộ thư mục** sau khi build để giữ các native assets Avalonia đi kèm:

```text
bin/Release/net10.0/linux-x64/publish/
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
dotnet publish -c Release -r linux-x64 --self-contained true

echo ""
echo "Build hoàn tất!"
echo "File Windows: bin/Release/net10.0/win-x64/publish/"
echo "File Linux:   bin/Release/net10.0/linux-x64/publish/"
cat > bin/Release/net10.0/linux-x64/publish/run-todoCS.sh <<'EOF'
#!/usr/bin/env bash
set -e
cd "$(dirname "$(readlink -f "$0")")"
chmod +x ./todoCS
exec ./todoCS "$@"
EOF
chmod +x bin/Release/net10.0/linux-x64/publish/run-todoCS.sh
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
