# TodoCS

Ứng dụng Todo được viết bằng C# / .NET 10.

## Dành cho người dùng Windows

Thư mục gửi cho người dùng Windows:

```text
bin/Release/net10.0/win-x64/publish/
```

Người dùng chỉ cần nhấn đúp vào file:

```text
bin/Release/net10.0/win-x64/publish/todoCS.exe
```

## Dành cho người dùng Linux

Thư mục gửi cho người dùng Linux:

```text
bin/Release/net10.0/win-x64/publish/
```

Chạy lệnh này trg terminal tại thư mục publish:

```text
chmod -x todoCS
./todoCS
```


## Dành cho người phát triển

### Update app trên Windows

```text
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

### Update app bản Linux

```text
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
```


## Build và Update app nhanh bằng Linux

1. Tạo file "build.sh"

```text
nano build.sh
```

Dán nội dung sau:

```text
#!/bin/bash

echo "Đang build file cho Windows..."
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

echo "Đang build file cho Linux..."
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true

echo ""
echo "Build hoàn tất!"
echo "File Windows: bin/Release/net*/win-x64/publish/"
echo "File Linux:   bin/Release/net*/linux-x64/publish/"
```

2. Cấp quyền thực thi

```text
chmod +x build.sh
```

3. Cập nhật code và build

Mỗi khi sửa code xong, chạy:

```text
./build.sh
```

Script sẽ build cả:

- Windows: "win-x64"
- Linux: "linux-x64"

## Cấu trúc file sau khi build

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