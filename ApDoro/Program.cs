using System;
using System.IO;
using Microsoft.Win32;

namespace ApDoro
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("ApDoro 登录背景修改工具启动...");
                
                // 1. 修改注册表启用自定义登录背景
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\Background", 
                    true))
                {
                    if (key == null)
                    {
                        Console.WriteLine("创建注册表项...");
                        using (var newKey = Registry.LocalMachine.CreateSubKey(
                            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\Background"))
                        {
                            newKey.SetValue("OEMBackground", 1, RegistryValueKind.DWord);
                        }
                    }
                    else
                    {
                        Console.WriteLine("更新注册表值...");
                        key.SetValue("OEMBackground", 1, RegistryValueKind.DWord);
                    }
                }

                // 2. 准备目标路径
                string system32Path = Environment.GetFolderPath(Environment.SpecialFolder.System);
                string targetPath = Path.Combine(system32Path, "oobe\\info\\backgrounds");
                
                Console.WriteLine($"目标目录: {targetPath}");
                
                // 3. 创建目录(如果不存在)
                if (!Directory.Exists(targetPath))
                {
                    Console.WriteLine("创建背景目录...");
                    Directory.CreateDirectory(targetPath);
                }

                // 4. 复制图片到目标位置
                string sourceImagePath = "ApDoroBackground.jpg"; 
                string targetImagePath = Path.Combine(targetPath, "backgroundDefault.jpg");
                
                Console.WriteLine($"源图片: {sourceImagePath}");
                
                if (!File.Exists(sourceImagePath))
                {
                    Console.WriteLine("错误: 未找到背景图片文件 'ApDoroBackground.jpg'");
                    Console.WriteLine("请将背景图片放在程序同目录下并命名为 ApDoroBackground.jpg");
                    Console.WriteLine("支持格式: JPG/JPEG (推荐分辨率: 1920x1080)");
                    return;
                }
                
                File.Copy(sourceImagePath, targetImagePath, true);
                Console.WriteLine($"已复制背景图片到: {targetImagePath}");

                Console.WriteLine("\n操作成功完成!");
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("ApDoro 提示: 登录界面背景已成功替换");
                Console.WriteLine("请重启电脑或注销账户查看效果");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("错误: 权限不足，请以管理员身份运行此程序");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ApDoro 遇到错误: {ex.Message}");
            }
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
