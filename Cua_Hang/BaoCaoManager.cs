using System;
using MySql.Data.MySqlClient;

namespace ConsoleApp1
{
    public class BaoCaoManager
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== BÁO CÁO VÀ THỐNG KÊ ===");
                Console.WriteLine("1. Thống kê doanh thu theo tháng");
                Console.WriteLine("2. Thống kê tồn kho");
                Console.WriteLine("3. Thống kê nhập hàng theo tháng");
                Console.WriteLine("4. Tìm nhân viên bán giỏi nhất tháng");
                Console.WriteLine("5. 5 sản phẩm bán chạy nhất tháng");
                Console.WriteLine("6. Quay lại");
                Console.Write("Chọn chức năng (1-6): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ThongKeDoanhThu(); break;
                    case "2": ThongKeTonKho(); break;
                    case "3": ThongKeNhapHang(); break;
                    case "4": NhanVienBanGioiNhat(); break;
                    case "5": SanPhamBanChay(); break;
                    case "6": return;
                    default: Console.WriteLine("Lựa chọn không hợp lệ! Nhấn phím bất kỳ để tiếp tục..."); Console.ReadKey(); break;
                }
            }
        }

        private void ThongKeDoanhThu()
        {
            Console.Write("Nhập tháng (MM): ");
            string thang = Console.ReadLine();
            Console.Write("Nhập năm (YYYY): ");
            string nam = Console.ReadLine();

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT SUM(Tong_Tien) as DoanhThu FROM don_hang WHERE MONTH(Ngay_Dat_Hang) = @thang AND YEAR(Ngay_Dat_Hang) = @nam";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@thang", thang);
                    cmd.Parameters.AddWithValue("@nam", nam);
                    object result = cmd.ExecuteScalar();
                    decimal doanhThu = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                    Console.WriteLine($"Doanh thu tháng {thang}/{nam}: {doanhThu}");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void ThongKeTonKho()
        {
            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Ma_San_Pham, Ten_San_Pham, So_Luong_Ton_Kho FROM san_pham WHERE So_Luong_Ton_Kho < 10";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine("Sản phẩm sắp hết hàng (tồn kho < 10):");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Ma_San_Pham"]} - {reader["Ten_San_Pham"]} - Tồn kho: {reader["So_Luong_Ton_Kho"]}");
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

        private void ThongKeNhapHang()
        {
            Console.Write("Nhập tháng (MM): ");
            string thang = Console.ReadLine();
            Console.Write("Nhập năm (YYYY): ");
            string nam = Console.ReadLine();

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT SUM(Tong_Chi_Phi) as ChiPhi FROM nhap_hang WHERE MONTH(Ngay_Nhap_Hang) = @thang AND YEAR(Ngay_Nhap_Hang) = @nam";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@thang", thang);
                    cmd.Parameters.AddWithValue("@nam", nam);
                    object result = cmd.ExecuteScalar();
                    decimal chiPhi = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                    Console.WriteLine($"Chi phí nhập hàng tháng {thang}/{nam}: {chiPhi}");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void NhanVienBanGioiNhat()
        {
            Console.Write("Nhập tháng (MM): ");
            string thang = Console.ReadLine();
            Console.Write("Nhập năm (YYYY): ");
            string nam = Console.ReadLine();

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT nv.Ma_Nhan_Vien, nv.Ten_Nhan_Vien, SUM(dh.Tong_Tien) as TongDoanhThu
                        FROM don_hang dh
                        JOIN nhan_vien nv ON dh.Ma_Nhan_Vien = nv.Ma_Nhan_Vien
                        WHERE MONTH(dh.Ngay_Dat_Hang) = @thang 
                          AND YEAR(dh.Ngay_Dat_Hang) = @nam 
                          AND dh.Trang_Thai = 'Đã giao'
                        GROUP BY nv.Ma_Nhan_Vien, nv.Ten_Nhan_Vien
                        ORDER BY TongDoanhThu DESC
                        LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@thang", thang);
                    cmd.Parameters.AddWithValue("@nam", nam);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Console.WriteLine($"Nhân viên bán giỏi nhất tháng {thang}/{nam}:");
                        Console.WriteLine($"{reader["Ma_Nhan_Vien"]} - {reader["Ten_Nhan_Vien"]} - Tổng doanh thu: {reader["TongDoanhThu"]}");
                    }
                    else
                    {
                        Console.WriteLine($"Không tìm thấy nhân viên nào có đơn hàng đã xử lý trong tháng {thang}/{nam}.");
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

        private void SanPhamBanChay()
        {
            Console.Write("Nhập tháng (MM): ");
            string thang = Console.ReadLine();
            Console.Write("Nhập năm (YYYY): ");
            string nam = Console.ReadLine();

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT sp.Ma_San_Pham, sp.Ten_San_Pham, SUM(ct.So_Luong) as TongSoLuong
                        FROM chi_tiet_don_hang ct
                        JOIN san_pham sp ON ct.Ma_San_Pham = sp.Ma_San_Pham
                        JOIN don_hang dh ON ct.Ma_Don_Hang = dh.Ma_Don_Hang
                        WHERE MONTH(dh.Ngay_Dat_Hang) = @thang 
                          AND YEAR(dh.Ngay_Dat_Hang) = @nam
                        GROUP BY sp.Ma_San_Pham, sp.Ten_San_Pham
                        ORDER BY TongSoLuong DESC
                        LIMIT 5";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@thang", thang);
                    cmd.Parameters.AddWithValue("@nam", nam);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine($"5 sản phẩm bán chạy nhất tháng {thang}/{nam}:");
                    int rank = 1;
                    while (reader.Read())
                    {
                        Console.WriteLine($"{rank}. {reader["Ma_San_Pham"]} - {reader["Ten_San_Pham"]} - Số lượng bán: {reader["TongSoLuong"]}");
                        rank++;
                    }
                    if (rank == 1)
                    {
                        Console.WriteLine("Không có sản phẩm nào được bán trong tháng này.");
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
    }
}