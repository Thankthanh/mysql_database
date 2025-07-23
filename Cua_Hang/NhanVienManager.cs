using System;
using MySql.Data.MySqlClient;

namespace ConsoleApp1
{
    public class NhanVienManager
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== QUẢN LÝ NHÂN VIÊN == ");
                Console.WriteLine("1. Xem danh sách nhân viên");
                Console.WriteLine("2. Thêm nhân viên");
                Console.WriteLine("3. Sửa nhân viên");
                Console.WriteLine("4. Xóa nhân viên");
                Console.WriteLine("5. Quay lại");
                Console.Write("Chọn chức năng (1-5): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": XemNhanVien(); break;
                    case "2": ThemNhanVien(); break;
                    case "3": SuaNhanVien(); break;
                    case "4": XoaNhanVien(); break;
                    case "5": return;
                    default: Console.WriteLine("Lựa chọn không hợp lệ! Nhấn phím bất kỳ để tiếp tục..."); Console.ReadKey(); break;
                }
            }
        }

        private void XemNhanVien()
        {
            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Ma_Nhan_Vien, Ten_Nhan_Vien, Chuc_Vu, Luong FROM nhan_vien";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine("Danh sách nhân viên:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Ma_Nhan_Vien"]} - {reader["Ten_Nhan_Vien"]} - Chức vụ: {reader["Chuc_Vu"]} - Lương: {reader["Luong"]}");
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

        private void ThemNhanVien()
        {
            Console.Write("Nhập tên nhân viên: ");
            string ten = Console.ReadLine();
            Console.Write("Nhập chức vụ: ");
            string chucVu = Console.ReadLine();
            Console.Write("Nhập lương: ");
            decimal luong = decimal.Parse(Console.ReadLine());

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO nhan_vien (Ten_Nhan_Vien, Chuc_Vu, Luong) VALUES (@ten, @chucVu, @luong)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ten", ten);
                    cmd.Parameters.AddWithValue("@chucVu", chucVu);
                    cmd.Parameters.AddWithValue("@luong", luong);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Thêm nhân viên thành công!");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void SuaNhanVien()
        {
            Console.Write("Nhập mã nhân viên cần sửa: ");
            int ma = int.Parse(Console.ReadLine());
            Console.Write("Nhập chức vụ mới: ");
            string chucVu = Console.ReadLine();

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE nhan_vien SET Chuc_Vu = @chucVu WHERE Ma_Nhan_Vien = @ma";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@chucVu", chucVu);
                    cmd.Parameters.AddWithValue("@ma", ma);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Sửa thành công!" : "Không tìm thấy nhân viên!");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void XoaNhanVien()
        {
            Console.Write("Nhập mã nhân viên cần xóa: ");
            int ma = int.Parse(Console.ReadLine());

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM nhan_vien WHERE Ma_Nhan_Vien = @ma";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ma", ma);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Xóa thành công!" : "Không tìm thấy nhân viên!");
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