using System;
using MySql.Data.MySqlClient;

namespace ConsoleApp1
{
    public class SanPhamManager
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== QUẢN LÝ SẢN PHẨM ===");
                Console.WriteLine("1. Xem danh sách sản phẩm");
                Console.WriteLine("2. Thêm sản phẩm");
                Console.WriteLine("3. Sửa sản phẩm");
                Console.WriteLine("4. Xóa sản phẩm");
                Console.WriteLine("5. Tìm sản phẩm theo phân loại");
                Console.WriteLine("6. Quay lại");
                Console.Write("Chọn chức năng (1-6): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": XemSanPham(); break;
                    case "2": ThemSanPham(); break;
                    case "3": SuaSanPham(); break;
                    case "4": XoaSanPham(); break;
                    case "5": TimSanPhamTheoDanhMuc(); break;
                    case "6": return;
                    default: Console.WriteLine("Lựa chọn không hợp lệ! Nhấn phím bất kỳ để tiếp tục..."); Console.ReadKey(); break;
                }
            }
        }

        private void XemSanPham()
        {
            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Ma_San_Pham, Ten_San_Pham, Gia, So_Luong_Ton_Kho FROM san_pham";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine("Danh sách sản phẩm:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Ma_San_Pham"]} - {reader["Ten_San_Pham"]} - Giá: {reader["Gia"]} - Tồn kho: {reader["So_Luong_Ton_Kho"]}");
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

        private void ThemSanPham()
        {
            Console.Write("Nhập tên sản phẩm: ");
            string ten = Console.ReadLine();
            Console.Write("Nhập giá: ");
            decimal gia = decimal.Parse(Console.ReadLine());
            Console.Write("Nhập số lượng tồn kho: ");
            int soLuong = int.Parse(Console.ReadLine());

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO san_pham (Ten_San_Pham, Gia, So_Luong_Ton_Kho) VALUES (@ten, @gia, @soLuong)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ten", ten);
                    cmd.Parameters.AddWithValue("@gia", gia);
                    cmd.Parameters.AddWithValue("@soLuong", soLuong);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Thêm sản phẩm thành công! Nhấn phím bất kỳ để tiếp tục...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void SuaSanPham()
        {
            Console.Write("Nhập mã sản phẩm cần sửa: ");
            int ma = int.Parse(Console.ReadLine());
            Console.Write("Nhập tên mới: ");
            string ten = Console.ReadLine();
            Console.Write("Nhập giá mới: ");
            decimal gia = decimal.Parse(Console.ReadLine());

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE san_pham SET Ten_San_Pham = @ten, Gia = @gia WHERE Ma_San_Pham = @ma";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ten", ten);
                    cmd.Parameters.AddWithValue("@gia", gia);
                    cmd.Parameters.AddWithValue("@ma", ma);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Sửa sản phẩm thành công!");
                    else
                        Console.WriteLine("Không tìm thấy sản phẩm!");
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

        private void XoaSanPham()
        {
            Console.Write("Nhập mã sản phẩm cần xóa: ");
            int ma = int.Parse(Console.ReadLine());

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM san_pham WHERE Ma_San_Pham = @ma";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ma", ma);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Xóa sản phẩm thành công!");
                    else
                        Console.WriteLine("Không tìm thấy sản phẩm!");
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

        private void TimSanPhamTheoDanhMuc()
        {
            while (true) // Vòng lặp để tiếp tục tìm kiếm
            {
                Console.Clear();
                using (MySqlConnection conn = dbHelper.GetConnection())
                {
                    try
                    {
                        conn.Open();
                        // Hiển thị danh sách danh mục
                        string queryDanhMuc = "SELECT Ma_Danh_Muc, Ten_Danh_Muc FROM danh_muc";
                        MySqlCommand cmdDanhMuc = new MySqlCommand(queryDanhMuc, conn);
                        MySqlDataReader readerDanhMuc = cmdDanhMuc.ExecuteReader();

                        Console.WriteLine("Danh sách danh mục (Nhập 0 để thoát):");
                        while (readerDanhMuc.Read())
                        {
                            Console.WriteLine($"{readerDanhMuc["Ma_Danh_Muc"]} - {readerDanhMuc["Ten_Danh_Muc"]}");
                        }
                        readerDanhMuc.Close();

                        // Nhập mã danh mục
                        Console.Write("Nhập mã danh mục để tìm sản phẩm: ");
                        string input = Console.ReadLine();

                        // Kiểm tra nếu nhập 0 thì thoát
                        if (input == "0")
                        {
                            Console.WriteLine("Thoát tìm kiếm.");
                            Console.ReadKey();
                            return; // Thoát khỏi hàm, quay lại menu chính
                        }

                        // Kiểm tra đầu vào hợp lệ
                        int maDanhMuc;
                        if (!int.TryParse(input, out maDanhMuc))
                        {
                            Console.WriteLine("Mã danh mục không hợp lệ! Nhấn phím bất kỳ để tiếp tục...");
                            Console.ReadKey();
                            continue; // Quay lại đầu vòng lặp để nhập lại
                        }

                        // Truy vấn sản phẩm theo danh mục
                        string query = @"
                            SELECT s.Ma_San_Pham, s.Ten_San_Pham, s.Gia, s.So_Luong_Ton_Kho, d.Ten_Danh_Muc 
                            FROM san_pham s
                            LEFT JOIN danh_muc d ON s.Ma_Danh_Muc = d.Ma_Danh_Muc
                            WHERE s.Ma_Danh_Muc = @maDanhMuc";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@maDanhMuc", maDanhMuc);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        Console.WriteLine($"Sản phẩm thuộc danh mục '{maDanhMuc}':");
                        bool hasResults = false;
                        while (reader.Read())
                        {
                            hasResults = true;
                            Console.WriteLine($"{reader["Ma_San_Pham"]} - {reader["Ten_San_Pham"]} - Giá: {reader["Gia"]} - Tồn kho: {reader["So_Luong_Ton_Kho"]} - Danh mục: {reader["Ten_Danh_Muc"]}");
                        }
                        if (!hasResults)
                        {
                            Console.WriteLine("Không tìm thấy sản phẩm nào trong danh mục này!");
                        }
                        Console.WriteLine("Nhấn phím bất kỳ để tiếp tục tìm kiếm...");
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
}