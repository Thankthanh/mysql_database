using System;
using MySql.Data.MySqlClient;

namespace ConsoleApp1
{
    public class KhuyenMaiManager
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== QUẢN LÝ KHUYẾN MÃI ===");
                Console.WriteLine("1. Xem danh sách khuyến mãi");
                Console.WriteLine("2. Thêm khuyến mãi");
                Console.WriteLine("3. Sửa khuyến mãi");
                Console.WriteLine("4. Xóa khuyến mãi");
                Console.WriteLine("5. Quay lại");
                Console.Write("Chọn chức năng (1-5): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": XemKhuyenMai(); break;
                    case "2": ThemKhuyenMai(); break;
                    case "3": SuaKhuyenMai(); break;
                    case "4": XoaKhuyenMai(); break;
                    case "5": return;
                    default: Console.WriteLine("Lựa chọn không hợp lệ! Nhấn phím bất kỳ để tiếp tục..."); Console.ReadKey(); break;
                }
            }
        }

        private void XemKhuyenMai()
        {
            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    km.Ma_Khuyen_Mai, 
                    km.Ten_Khuyen_Mai, 
                    km.Giam_Gia, 
                    km.Ngay_Bat_Dau, 
                    km.Ngay_Ket_Thuc, 
                    sp.Ma_San_Pham, 
                    sp.Ten_San_Pham
                FROM khuyen_mai km
                LEFT JOIN khuyen_mai_san_pham kmsp ON km.Ma_Khuyen_Mai = kmsp.Ma_Khuyen_Mai
                LEFT JOIN san_pham sp ON kmsp.Ma_San_Pham = sp.Ma_San_Pham";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine("Danh sách khuyến mãi và sản phẩm áp dụng:");
                    string currentKhuyenMai = ""; // Biến để theo dõi khuyến mãi hiện tại

                    while (reader.Read())
                    {
                        string maKhuyenMai = reader["Ma_Khuyen_Mai"].ToString();

                        // Chỉ hiển thị thông tin khuyến mãi một lần khi thay đổi mã khuyến mãi
                        if (currentKhuyenMai != maKhuyenMai)
                        {
                            Console.WriteLine($"\nMã KM: {reader["Ma_Khuyen_Mai"]} - Tên: {reader["Ten_Khuyen_Mai"]} - Giảm: {reader["Giam_Gia"]} - Từ: {reader["Ngay_Bat_Dau"]} - Đến: {reader["Ngay_Ket_Thuc"]}");
                            Console.WriteLine("Sản phẩm áp dụng:");
                            currentKhuyenMai = maKhuyenMai;
                        }

                        // Hiển thị sản phẩm áp dụng (nếu có)
                        if (!reader.IsDBNull(reader.GetOrdinal("Ma_San_Pham")))
                        {
                            Console.WriteLine($"- {reader["Ma_San_Pham"]} - {reader["Ten_San_Pham"]}");
                        }
                    }
                    Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void ThemKhuyenMai()
        {
            Console.Write("Nhập tên khuyến mãi: ");
            string ten = Console.ReadLine();
            Console.Write("Nhập phần trăm giảm giá (hoặc để trống nếu không có): ");
            string giamGiaInput = Console.ReadLine();
            decimal? giamGia = string.IsNullOrEmpty(giamGiaInput) ? (decimal?)null : decimal.Parse(giamGiaInput);
            Console.Write("Nhập ngày bắt đầu (YYYY-MM-DD): ");
            string ngayBD = Console.ReadLine();
            Console.Write("Nhập ngày kết thúc (YYYY-MM-DD): ");
            string ngayKT = Console.ReadLine();

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    // Thêm khuyến mãi vào bảng khuyen_mai
                    string query = "INSERT INTO khuyen_mai (Ten_Khuyen_Mai, Giam_Gia, Ngay_Bat_Dau, Ngay_Ket_Thuc) VALUES (@ten, @giamGia, @ngayBD, @ngayKT); SELECT LAST_INSERT_ID();";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ten", ten);
                    cmd.Parameters.AddWithValue("@giamGia", giamGia.HasValue ? giamGia : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ngayBD", ngayBD);
                    cmd.Parameters.AddWithValue("@ngayKT", ngayKT);
                    int maKhuyenMai = Convert.ToInt32(cmd.ExecuteScalar()); // Lấy mã khuyến mãi vừa thêm

                    // Thêm sản phẩm áp dụng
                    Console.WriteLine("Nhập danh sách sản phẩm áp dụng (nhập mã sản phẩm, để trống để kết thúc):");
                    while (true)
                    {
                        Console.Write("Mã sản phẩm: ");
                        string maSPInput = Console.ReadLine();
                        if (string.IsNullOrEmpty(maSPInput)) break; // Thoát nếu không nhập mã

                        int maSP = int.Parse(maSPInput);
                        string insertSPQuery = "INSERT INTO khuyen_mai_san_pham (Ma_Khuyen_Mai, Ma_San_Pham) VALUES (@maKM, @maSP)";
                        MySqlCommand cmdSP = new MySqlCommand(insertSPQuery, conn);
                        cmdSP.Parameters.AddWithValue("@maKM", maKhuyenMai);
                        cmdSP.Parameters.AddWithValue("@maSP", maSP);
                        cmdSP.ExecuteNonQuery();
                        Console.WriteLine($"Đã thêm sản phẩm {maSP} vào khuyến mãi!");
                    }

                    Console.WriteLine("Thêm khuyến mãi và sản phẩm áp dụng thành công!");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void SuaKhuyenMai()
        {
            Console.Write("Nhập mã khuyến mãi cần sửa: ");
            int ma = int.Parse(Console.ReadLine());
            Console.Write("Nhập tên mới: ");
            string ten = Console.ReadLine();

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    // Cập nhật tên khuyến mãi
                    string query = "UPDATE khuyen_mai SET Ten_Khuyen_Mai = @ten WHERE Ma_Khuyen_Mai = @ma";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ten", ten);
                    cmd.Parameters.AddWithValue("@ma", ma);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        Console.WriteLine("Không tìm thấy khuyến mãi!");
                        Console.ReadKey();
                        return;
                    }

                    // Hỏi xem có muốn cập nhật danh sách sản phẩm không
                    Console.Write("Bạn có muốn cập nhật danh sách sản phẩm áp dụng không? (y/n): ");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        // Xóa toàn bộ sản phẩm cũ của khuyến mãi này
                        string deleteQuery = "DELETE FROM khuyen_mai_san_pham WHERE Ma_Khuyen_Mai = @ma";
                        MySqlCommand cmdDelete = new MySqlCommand(deleteQuery, conn);
                        cmdDelete.Parameters.AddWithValue("@ma", ma);
                        cmdDelete.ExecuteNonQuery();

                        // Thêm danh sách sản phẩm mới
                        Console.WriteLine("Nhập danh sách sản phẩm áp dụng (nhập mã sản phẩm, để trống để kết thúc):");
                        while (true)
                        {
                            Console.Write("Mã sản phẩm: ");
                            string maSPInput = Console.ReadLine();
                            if (string.IsNullOrEmpty(maSPInput)) break;

                            int maSP = int.Parse(maSPInput);
                            string insertSPQuery = "INSERT INTO khuyen_mai_san_pham (Ma_Khuyen_Mai, Ma_San_Pham) VALUES (@maKM, @maSP)";
                            MySqlCommand cmdSP = new MySqlCommand(insertSPQuery, conn);
                            cmdSP.Parameters.AddWithValue("@maKM", ma);
                            cmdSP.Parameters.AddWithValue("@maSP", maSP);
                            cmdSP.ExecuteNonQuery();
                            Console.WriteLine($"Đã thêm sản phẩm {maSP} vào khuyến mãi!");
                        }
                    }

                    Console.WriteLine("Sửa khuyến mãi thành công!");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void XoaKhuyenMai()
        {
            Console.Write("Nhập mã khuyến mãi cần xóa: ");
            int ma = int.Parse(Console.ReadLine());

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM khuyen_mai WHERE Ma_Khuyen_Mai = @ma";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ma", ma);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Xóa thành công!" : "Không tìm thấy khuyến mãi!");
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