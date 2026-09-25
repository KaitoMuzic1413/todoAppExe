# Todo App (.NET C#)

Hướng dẫn chi tiết cách chạy ứng dụng ở chế độ phát triển (Development) và cách đóng gói, bàn giao ứng dụng cho người dùng cuối trên các hệ điều hành khác nhau.

---

## 1. Yêu cầu hệ thống

* **Người phát triển (Developer):** Cần cài đặt **.NET 10.0 SDK** để chạy mã nguồn và publish ứng dụng.
* **Người dùng cuối (End-User):**
  * Do ứng dụng được publish dạng `--self-contained false` (phụ thuộc vào runtime), người dùng cần cài đặt sẵn **.NET 10.0 Runtime** trên máy.

---

## 2. Dành cho Lập trình viên (Chạy Source Code)

Nếu bạn muốn chạy ứng dụng trực tiếp từ mã nguồn:

```bash
# Di chuyển tới thư mục dự án
cd /home/redhat/Documents/workPlace/todoCS

# Chạy ứng dụng
dotnet run