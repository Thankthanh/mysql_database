using System;
using MySql.Data.MySqlClient;

namespace ConsoleApp1
{
    public class DonHangManager
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== QUẢN LÝ ĐƠN HÀNG ===");
                Console.WriteLine("1. Xem danh sách đơn hàng");
                Console.WriteLine("2. Tạo đơn hàng");
                Console.WriteLine("3. Cập nhật trạng thái đơn hàng");
                Console.WriteLine("4. Xóa đơn hàng");
                Console.WriteLine("5. Quay lại");
                Console.Write("Chọn chức năng (1-5): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": XemDonHang(); break;
                    case "2": TaoDonHang(); break;
                    case "3": CapNhatTrangThai(); break;
                    case "4": XoaDonHang(); break;
                    case "5": return;
                    default: Console.WriteLine("Lựa chọn không hợp lệ! Nhấn phím bất kỳ để tiếp tục..."); Console.ReadKey(); break;
                }
            }
        }

        private void XemDonHang()
        {
            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Ma_Don_Hang, Ma_Khach_Hang, Ngay_Dat_Hang, Tong_Tien, Trang_Thai FROM don_hang";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine("Danh sách đơn hàng:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Ma_Don_Hang"]} - Khách: {reader["Ma_Khach_Hang"]} - Ngày: {reader["Ngay_Dat_Hang"]} - Tổng: {reader["Tong_Tien"]} - Trạng thái: {reader["Trang_Thai"]}");
                    }
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void TaoDonHang()
        {
            Console.Write("Nhập mã khách hàng: ");
            int maKH = int.Parse(Console.ReadLine());
            Console.Write("Nhập mã nhân viên: ");
            int maNV = int.Parse(Console.ReadLine());
            Console.Write("Nhập ngày đặt hàng (YYYY-MM-DD): ");
            string ngayDat = Console.ReadLine();

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Thêm đơn hàng vào bảng don_hang
                    string queryDonHang = "INSERT INTO don_hang (Ma_Khach_Hang, Ngay_Dat_Hang, Tong_Tien, Ma_Nhan_Vien, Trang_Thai) VALUES (@maKH, @ngayDat, 0, @maNV, 'Đang xử lý'); SELECT LAST_INSERT_ID();";
                    MySqlCommand cmdDonHang = new MySqlCommand(queryDonHang, conn);
                    cmdDonHang.Parameters.AddWithValue("@maKH", maKH);
                    cmdDonHang.Parameters.AddWithValue("@ngayDat", ngayDat);
                    cmdDonHang.Parameters.AddWithValue("@maNV", maNV);
                    int maDonHang = Convert.ToInt32(cmdDonHang.ExecuteScalar()); // Lấy mã đơn hàng vừa thêm

                    // Thêm chi tiết đơn hàng
                    Console.WriteLine("Nhập danh sách sản phẩm cho đơn hàng (nhập mã và số lượng, để trống mã để kết thúc):");
                    while (true)
                    {
                        Console.Write("Mã sản phẩm: ");
                        string maSPInput = Console.ReadLine();
                        if (string.IsNullOrEmpty(maSPInput)) break; // Thoát nếu không nhập mã

                        int maSP = int.Parse(maSPInput);
                        Console.Write("Số lượng: ");
                        int soLuong = int.Parse(Console.ReadLine());

                        // Lấy giá sản phẩm từ san_pham
                        string queryGia = "SELECT Gia FROM san_pham WHERE Ma_San_Pham = @maSP";
                        MySqlCommand cmdGia = new MySqlCommand(queryGia, conn);
                        cmdGia.Parameters.AddWithValue("@maSP", maSP);
                        object giaResult = cmdGia.ExecuteScalar();
                        if (giaResult == null)
                        {
                            Console.WriteLine("Sản phẩm không tồn tại!");
                            continue;
                        }
                        decimal gia = Convert.ToDecimal(giaResult);

                        // Kiểm tra khuyến mãi áp dụng cho sản phẩm
                        string queryKM = @"
                    SELECT km.Giam_Gia 
                    FROM khuyen_mai km
                    JOIN khuyen_mai_san_pham kmsp ON km.Ma_Khuyen_Mai = kmsp.Ma_Khuyen_Mai
                    WHERE kmsp.Ma_San_Pham = @maSP 
                    AND @ngayDat BETWEEN km.Ngay_Bat_Dau AND km.Ngay_Ket_Thuc";
                        MySqlCommand cmdKM = new MySqlCommand(queryKM, conn);
                        cmdKM.Parameters.AddWithValue("@maSP", maSP);
                        cmdKM.Parameters.AddWithValue("@ngayDat", ngayDat);
                        object giamGiaObj = cmdKM.ExecuteScalar();
                        decimal giamGia = giamGiaObj != null && !DBNull.Value.Equals(giamGiaObj) ? Convert.ToDecimal(giamGiaObj) : 0;

                        // Tính Tieu_Khoan với khuyến mãi
                        decimal tieuKhoan = gia * soLuong * (1 - giamGia / 100);
                        Console.WriteLine($"Áp dụng giảm giá {giamGia}% - Tiểu khoản: {tieuKhoan}");

                        // Thêm vào chi_tiet_don_hang
                        string queryChiTiet = "INSERT INTO chi_tiet_don_hang (Ma_Don_Hang, Ma_San_Pham, So_Luong, Tieu_Khoan) VALUES (@maDH, @maSP, @soLuong, @tieuKhoan)";
                        MySqlCommand cmdChiTiet = new MySqlCommand(queryChiTiet, conn);
                        cmdChiTiet.Parameters.AddWithValue("@maDH", maDonHang);
                        cmdChiTiet.Parameters.AddWithValue("@maSP", maSP);
                        cmdChiTiet.Parameters.AddWithValue("@soLuong", soLuong);
                        cmdChiTiet.Parameters.AddWithValue("@tieuKhoan", tieuKhoan);
                        cmdChiTiet.ExecuteNonQuery();
                    }

                    Console.WriteLine("Tạo đơn hàng thành công!");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void CapNhatTrangThai()
        {
            Console.Write("Nhập mã đơn hàng cần cập nhật: ");
            int ma = int.Parse(Console.ReadLine());
            Console.Write("Nhập trạng thái mới (Đang xử lý/Đã giao): ");
            string trangThai = Console.ReadLine();

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE don_hang SET Trang_Thai = @trangThai WHERE Ma_Don_Hang = @ma";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@trangThai", trangThai);
                    cmd.Parameters.AddWithValue("@ma", ma);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Cập nhật trạng thái thành công!");
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy đơn hàng!");
                    }
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }
        private void XoaDonHang()
        {
            Console.Write("Nhập mã đơn hàng cần xóa: ");
            int ma = int.Parse(Console.ReadLine());

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM don_hang WHERE Ma_Don_Hang = @ma";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ma", ma);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Xóa đơn hàng thành công! (Tồn kho đã được cập nhật qua trigger)");
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy đơn hàng!");
                    }
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }
    }
}