using System;
using MySql.Data.MySqlClient;

namespace ConsoleApp1
{
    public class NhapHangManager
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== QUẢN LÝ NHẬP HÀNG ===");
                Console.WriteLine("1. Xem danh sách nhập hàng");
                Console.WriteLine("2. Tạo phiếu nhập hàng");
                Console.WriteLine("3. Sửa phiếu nhập hàng");
                Console.WriteLine("4. Xóa phiếu nhập hàng");
                Console.WriteLine("5. Quay lại");
                Console.Write("Chọn chức năng (1-5): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": XemNhapHang(); break;
                    case "2": TaoNhapHang(); break;
                    case "3": SuaNhapHang(); break;
                    case "4": XoaNhapHang(); break;
                    case "5": return;
                    default: Console.WriteLine("Lựa chọn không hợp lệ! Nhấn phím bất kỳ để tiếp tục..."); Console.ReadKey(); break;
                }
            }
        }

        private void XemNhapHang()
        {
            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Ma_Nhap_Hang, Ma_Nha_Cung_Cap, Ngay_Nhap_Hang, Tong_Chi_Phi FROM nhap_hang";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine("Danh sách nhập hàng:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Ma_Nhap_Hang"]} - NCC: {reader["Ma_Nha_Cung_Cap"]} - Ngày: {reader["Ngay_Nhap_Hang"]} - Tổng: {reader["Tong_Chi_Phi"]}");
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

        private void TaoNhapHang()
        {
            Console.Write("Nhập mã nhà cung cấp: ");
            int maNCC = int.Parse(Console.ReadLine());
            Console.Write("Nhập mã nhân viên: ");
            int maNV = int.Parse(Console.ReadLine());
            Console.Write("Nhập ngày nhập hàng (YYYY-MM-DD): ");
            string ngayNhap = Console.ReadLine();

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO nhap_hang (Ma_Nha_Cung_Cap, Ngay_Nhap_Hang, Tong_Chi_Phi, Ma_Nhan_Vien) VALUES (@maNCC, @ngayNhap, 0, @maNV)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maNCC", maNCC);
                    cmd.Parameters.AddWithValue("@ngayNhap", ngayNhap);
                    cmd.Parameters.AddWithValue("@maNV", maNV);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Tạo phiếu nhập hàng thành công! (Thêm chi tiết nhập hàng qua trigger tự động cập nhật tổng chi phí)");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void SuaNhapHang()
        {
            Console.Write("Nhập mã phiếu nhập cần sửa: ");
            int ma = int.Parse(Console.ReadLine());
            Console.Write("Nhập mã nhà cung cấp mới: ");
            int maNCC = int.Parse(Console.ReadLine());

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE nhap_hang SET Ma_Nha_Cung_Cap = @maNCC WHERE Ma_Nhap_Hang = @ma";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maNCC", maNCC);
                    cmd.Parameters.AddWithValue("@ma", ma);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Sửa thành công!" : "Không tìm thấy phiếu nhập!");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void XoaNhapHang()
        {
            Console.Write("Nhập mã phiếu nhập cần xóa: ");
            int ma = int.Parse(Console.ReadLine());

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM nhap_hang WHERE Ma_Nhap_Hang = @ma";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ma", ma);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Xóa thành công!" : "Không tìm thấy phiếu nhập!");
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