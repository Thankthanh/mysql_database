using System;
using MySql.Data.MySqlClient;

namespace ConsoleApp1
{
    public class KhachHangManager
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== QUẢN LÝ KHÁCH HÀNG ===");
                Console.WriteLine("1. Xem danh sách khách hàng");
                Console.WriteLine("2. Thêm khách hàng");
                Console.WriteLine("3. Sửa khách hàng");
                Console.WriteLine("4. Xóa khách hàng");
                Console.WriteLine("5. Quay lại");
                Console.Write("Chọn chức năng (1-5): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": XemKhachHang(); break;
                    case "2": ThemKhachHang(); break;
                    case "3": SuaKhachHang(); break;
                    case "4": XoaKhachHang(); break;
                    case "5": return;
                    default: Console.WriteLine("Lựa chọn không hợp lệ! Nhấn phím bất kỳ để tiếp tục..."); Console.ReadKey(); break;
                }
            }
        }

        private void XemKhachHang()
        {
            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Ma_Khach_Hang, Ten_Khach_Hang, So_Dien_Thoai, Email FROM khach_hang";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine("Danh sách khách hàng:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Ma_Khach_Hang"]} - {reader["Ten_Khach_Hang"]} - SĐT: {reader["So_Dien_Thoai"]} - Email: {reader["Email"]}");
                    }
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void ThemKhachHang()
        {
            Console.Write("Nhập tên khách hàng: ");
            string ten = Console.ReadLine();
            Console.Write("Nhập số điện thoại: ");
            string sdt = Console.ReadLine();
            Console.Write("Nhập email: ");
            string email = Console.ReadLine();

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO khach_hang (Ten_Khach_Hang, So_Dien_Thoai, Email) VALUES (@ten, @sdt, @email)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ten", ten);
                    cmd.Parameters.AddWithValue("@sdt", sdt);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Thêm khách hàng thành công!");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void SuaKhachHang()
        {
            Console.Write("Nhập mã khách hàng cần sửa: ");
            int ma = int.Parse(Console.ReadLine());
            Console.Write("Nhập tên mới: ");
            string ten = Console.ReadLine();

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE khach_hang SET Ten_Khach_Hang = @ten WHERE Ma_Khach_Hang = @ma";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ten", ten);
                    cmd.Parameters.AddWithValue("@ma", ma);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Sửa thành công!" : "Không tìm thấy khách hàng!");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void XoaKhachHang()
        {
            Console.Write("Nhập mã khách hàng cần xóa: ");
            int ma = int.Parse(Console.ReadLine());

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM khach_hang WHERE Ma_Khach_Hang = @ma";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ma", ma);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Xóa thành công!" : "Không tìm thấy khách hàng!");
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