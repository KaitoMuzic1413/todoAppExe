#!/usr/bin/env bash
set -euo pipefail

echo "Đang build file cho Windows..."
dotnet publish todoCS.csproj -c Release -r win-x64 --self-contained true \
  -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

echo "Đang build file cho Linux..."
# Giữ nguyên cả thư mục publish để không làm thiếu native assets của Avalonia.
dotnet publish todoCS.csproj -c Release -r linux-x64 --self-contained true

linux_publish="bin/Release/net10.0/linux-x64/publish"
chmod +x "$linux_publish/todoCS"
cat > "$linux_publish/run-todoCS.sh" <<'EOF'
#!/usr/bin/env bash
set -e
cd "$(dirname "$(readlink -f "$0")")"
chmod +x ./todoCS
exec ./todoCS "$@"
EOF
chmod +x "$linux_publish/run-todoCS.sh"

echo "Windows: bin/Release/net10.0/win-x64/publish/"
echo "Linux:   bin/Release/net10.0/linux-x64/publish/ (gửi nguyên thư mục)"
