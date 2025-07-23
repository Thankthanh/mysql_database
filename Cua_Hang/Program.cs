using System;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            SanPhamManager sanPhamManager = new SanPhamManager();
            DonHangManager donHangManager = new DonHangManager();
            NhapHangManager nhapHangManager = new NhapHangManager();
            KhachHangManager khachHangManager = new KhachHangManager();
            NhanVienManager nhanVienManager = new NhanVienManager();
            KhuyenMaiManager khuyenMaiManager = new KhuyenMaiManager();
            BaoCaoManager baoCaoManager = new BaoCaoManager();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== QUẢN LÝ SHOP MỸ PHẨM ===");
                Console.WriteLine("1. Quản lý sản phẩm");
                Console.WriteLine("2. Quản lý đơn hàng");
                Console.WriteLine("3. Quản lý nhập hàng");
                Console.WriteLine("4. Quản lý khách hàng");
                Console.WriteLine("5. Quản lý nhân viên");
                Console.WriteLine("6. Quản lý khuyến mãi");
                Console.WriteLine("7. Báo cáo và thống kê");
                Console.WriteLine("8. Thoát");
                Console.Write("Chọn chức năng (1-8): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": sanPhamManager.ShowMenu(); break;
                    case "2": donHangManager.ShowMenu(); break;
                    case "3": nhapHangManager.ShowMenu(); break;
                    case "4": khachHangManager.ShowMenu(); break;
                    case "5": nhanVienManager.ShowMenu(); break;
                    case "6": khuyenMaiManager.ShowMenu(); break;
                    case "7": baoCaoManager.ShowMenu(); break;
                    case "8": return;
                    default: Console.WriteLine("Lựa chọn không hợp lệ! Nhấn phím bất kỳ để tiếp tục..."); Console.ReadKey(); break;
                }
            }
        }
    }
}