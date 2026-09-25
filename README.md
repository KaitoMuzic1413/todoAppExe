TodoCS

Ứng dụng Todo được viết bằng C# / .NET 10.

📦 Dành cho người dùng Windows

Sau khi build, thư mục gửi cho người dùng Windows:

bin/Release/net10.0/win-x64/publish/

Người dùng chỉ cần nhấn đúp vào file:

bin/Release/net10.0/win-x64/publish/todoCS.exe

Không cần cài .NET vì ứng dụng được build theo chế độ self-contained.

---

🛠️ Dành cho người phát triển

Build bản Windows

dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

File sau khi build:

bin/Release/net10.0/win-x64/publish/

Build bản Linux

dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true

File sau khi build:

bin/Release/net10.0/linux-x64/publish/

---

🚀 Build và cập nhật nhanh bằng Linux

1. Tạo file "build.sh"

nano build.sh

Dán nội dung sau:

#!/bin/bash

echo "Đang build file cho Windows..."
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

echo "Đang build file cho Linux..."
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true

echo ""
echo "Build hoàn tất!"
echo "File Windows: bin/Release/net*/win-x64/publish/"
echo "File Linux:   bin/Release/net*/linux-x64/publish/"

2. Cấp quyền thực thi

chmod +x build.sh

3. Cập nhật code và build

Mỗi khi sửa code xong, chạy:

./build.sh

Script sẽ tự động build cả:

- 🪟 Windows: "win-x64"
- 🐧 Linux: "linux-x64"

📁 Cấu trúc file sau khi build

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